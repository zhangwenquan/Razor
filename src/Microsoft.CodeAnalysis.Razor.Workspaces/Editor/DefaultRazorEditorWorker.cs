// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;
using Span = Microsoft.AspNetCore.Razor.Language.Legacy.Span;
using PartialParseResult = Microsoft.AspNetCore.Razor.Language.Legacy.PartialParseResult;

namespace Microsoft.CodeAnalysis.Razor.Editor
{
    [DebuggerDisplay("{DebuggerToString(),nq}")]
    internal class DefaultRazorEditorWorker : RazorEditorWorker
    {
        private readonly ThreadDispatcher _dispatcher;
        private readonly Workspace _workspace;
        private readonly RazorTemplateEngine _templateEngine;
        private readonly string _filePath;
        private readonly DocumentId _documentId;
        private readonly SourceTextContainer _container;

        private readonly List<WorkItem> _workQueue;
        private readonly object _workQueueLock;
        private readonly ManualResetEventSlim _workStarted;

        private PrivateState _current;

        public override event EventHandler Updated;

        public DefaultRazorEditorWorker(
            ThreadDispatcher dispatcher,
            Workspace workspace,
            RazorTemplateEngine templateEngine,
            string filePath,
            DocumentId documentId,
            SourceTextContainer container)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            if (workspace == null)
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            if (templateEngine == null)
            {
                throw new ArgumentNullException(nameof(templateEngine));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _dispatcher = dispatcher;
            _workspace = workspace;
            _templateEngine = templateEngine;
            _filePath = filePath;
            _documentId = documentId;
            _container = container;

            _current = new PrivateState(null, null, null, null, PartialParseResult.Accepted);
            _workQueueLock = new object();
            _workQueue = new List<WorkItem>();
            _workStarted = new ManualResetEventSlim(initialState: false);
        }

        public override State Current => _current;

        public override RazorTemplateEngine TemplateEngine => _templateEngine;

        public override Workspace Workspace => _workspace;

        public override Task ParseAsync()
        {
            _dispatcher.AssertForegroundThread();

            // Since we don't have an edit, we can't partial parse, just enqueue a reparse.
            var workItem = new WorkItem(_container.CurrentText);

            Enqueue(workItem);

            return workItem.Task;
        }

        public override Task ParseAsync(TextChange change)
        {
            if (change == null)
            {
                throw new ArgumentNullException(nameof(change));
            }

            _dispatcher.AssertForegroundThread();

            var item = new WorkItem(_container.CurrentText, change);
            if (TrySynchronousParse(item))
            {
                return Task.CompletedTask;
            }

            Enqueue(item);

            return Task.CompletedTask;
        }

        public override bool TryParse(TextChange change)
        {
            if (change == null)
            {
                throw new ArgumentNullException(nameof(change));
            }

            _dispatcher.AssertForegroundThread();

            var item = new WorkItem(_container.CurrentText, change);
            if (TrySynchronousParse(item))
            {
                return true;
            }

            Enqueue(item);
            return false;
        }

        private bool TrySynchronousParse(WorkItem item)
        {
            _dispatcher.AssertForegroundThread();

            if (_workStarted.IsSet)
            {
                // We have a pending background task, we can't do a partial parse.
                return false;
            }

            var syntaxTree = _current.SyntaxTree;
            if (syntaxTree == null)
            {
                // There's no existing syntax tree, we can't do a partial parse.
                return false;
            }

            // We only expect to be called synchronously with a single change.
            Debug.Assert(item.Changes.Length == 1);

            var owner = _current.Owner;
            var change = item.Changes.Single().AsSourceChange();

            var result = PartialParseResult.Rejected;

            // Try the last change owner
            if (owner != null && owner.EditHandler.OwnsChange(owner, change))
            {
                var editResult = owner.EditHandler.ApplyChange(owner, change);
                result = editResult.Result;
                if ((result & PartialParseResult.Rejected) != PartialParseResult.Rejected)
                {
                    owner.ReplaceWith(editResult.EditedSpan);

                    Complete(item, syntaxTree, _current.CSharpDocument, owner, result);
                    return true;
                }
            }

            // Locate the span responsible for this change
            owner = syntaxTree.Root.LocateOwner(change);

            if ((_current.Result & PartialParseResult.Provisional) == PartialParseResult.Provisional)
            {
                // Last change owner couldn't accept this, so we must do a full reparse
                result = PartialParseResult.Rejected;
            }
            else if (owner != null)
            {
                var editResult = owner.EditHandler.ApplyChange(owner, change);
                result = editResult.Result;
                if ((result & PartialParseResult.Rejected) != PartialParseResult.Rejected)
                {
                    owner.ReplaceWith(editResult.EditedSpan);

                    Complete(item, syntaxTree, _current.CSharpDocument, owner, result);
                    return true;
                }
            }

            return false;
        }

        private void Complete(WorkItem workItem, RazorSyntaxTree syntaxTree, RazorCSharpDocument cSharpDocument, Span owner, PartialParseResult result)
        {
            _dispatcher.AssertForegroundThread();

            _current = new PrivateState(workItem.Snapshot, syntaxTree, _current.CSharpDocument, owner, result);

            var handler = Updated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

            workItem.Complete();
        }

        private void Enqueue(WorkItem workItem)
        {
            _dispatcher.AssertForegroundThread();

            // The protocol for enqueueing items as is follows.
            //
            // With the lock held:
            //      Add the work item
            //      If work started event is set, there is already a pending 'task' to process the work items
            //      If work started event is NOT set, create a 'task' to process the work items
            //          Create background task
            //          Set the work started event 
            //
            // This protocol ensures that we don't create a flood of parsing tasks when we get repeated
            // text changes.
            //
            // It's OK to queue more work even when the background task has taken its parcel of work already.
            // The background task will queue another background task if there is pending work when it finishes.
            // Think about it this way... the work started event is always set if the queue has work to process.

            // We only ever start background tasks from the main thread so there's no possible race.  
            lock (_workQueueLock)
            {
                _workQueue.Add(workItem);

                if (!_workStarted.IsSet)
                {
                    CreateBackgroundTask();

                    _workStarted.Set();
                }
            }
        }

        private void CreateBackgroundTask()
        {
            _dispatcher.AssertForegroundThread();

            Task.Factory
                .StartNew<BackgroundParseResult>(RunBackgroundParse, CancellationToken.None, TaskCreationOptions.None, _dispatcher.BackgroundScheduler)
                .ContinueWith(CompleteBackgroundParse, _dispatcher.ForegroundScheduler);
        }

        private BackgroundParseResult RunBackgroundParse()
        {
            _dispatcher.AssertBackgroundThread();

            WorkItem item = null;
            lock (_workQueueLock)
            {
                Debug.Assert(_workQueue.Count > 0);

                // A work item requires its own batch if someone is waiting on its Task. We want to find the first item
                // in the queue that has someone waiting and process that.
                // 
                // If none of the items have a dance partner then we'll just take the last one.
                var i = 0;
                for (; i < _workQueue.Count; i++)
                {
                    item = _workQueue[i];
                    if (item.RequiresOwnBatch)
                    {
                        break;
                    }
                }

                _workQueue.RemoveRange(0, i);
            }

            Debug.Assert(item != null);

            var imports = _templateEngine.GetImports(_filePath);
            var sourceDocument = item.Snapshot.AsSourceDocument(_filePath);
            var codeDocument = RazorCodeDocument.Create(sourceDocument, imports);

            var cSharpDocument = _templateEngine.GenerateCode(codeDocument);
            return new BackgroundParseResult(item, codeDocument.GetSyntaxTree(), cSharpDocument);
        }

        private void CompleteBackgroundParse(Task<BackgroundParseResult> task)
        {
            _dispatcher.AssertForegroundThread();

            if (task.IsFaulted)
            {
                // Rethrow the exception on the main thread.
                throw task.Exception.InnerException;
            }

            var result = task.Result;
            Complete(result.Item, result.SyntaxTree, result.CSharpDocument, owner: null, result: default(PartialParseResult));

            lock (_workQueueLock)
            {
                Debug.Assert(_workStarted.IsSet);

                if (_workQueue.Count == 0)
                {
                    // Nothing left to process.
                    _workStarted.Reset();
                }
                else
                {
                    // There's more work, start another background task.
                    CreateBackgroundTask();
                }
            }
        }

        private string DebuggerToString()
        {
            return _workStarted.IsSet ? "Running" : "Idle";
        }

        private class PrivateState : State
        {
            private readonly SourceText _text;
            private readonly RazorSyntaxTree _syntaxTree;
            private readonly RazorCSharpDocument _cSharpDocument;
            private readonly Span _owner;
            private readonly PartialParseResult _result;

            public PrivateState(
                SourceText text,
                RazorSyntaxTree syntaxTree,
                RazorCSharpDocument cSharpDocument,
                Span owner,
                PartialParseResult result)
            {
                _text = text;
                _syntaxTree = syntaxTree;
                _cSharpDocument = cSharpDocument;
                _owner = owner;
                _result = result;
            }

            public override SourceText Text => _text;

            public override RazorSyntaxTree SyntaxTree => _syntaxTree;

            public override RazorCSharpDocument CSharpDocument => _cSharpDocument;

            public Span Owner => _owner;

            public PartialParseResult Result => _result;
        }

        private class WorkItem
        {
            private TaskCompletionSource<object> _taskCompletionSource;

            public WorkItem(SourceText snapshot)
            {
                Snapshot = snapshot;

                Changes = Array.Empty<TextChange>();
            }

            public WorkItem(SourceText snapshot, TextChange change)
            {
                Snapshot = snapshot;
                Changes = new[] { change };
            }

            public WorkItem(SourceText snapshot, TextChange[] changes)
            {
                Snapshot = snapshot;
                Changes = changes;
            }

            public TextChange[] Changes { get; }

            public bool RequiresOwnBatch => _taskCompletionSource != null;

            public SourceText Snapshot { get; }

            public Task Task
            {
                get
                {
                    if (_taskCompletionSource == null)
                    {
                        _taskCompletionSource = new TaskCompletionSource<object>();
                    }

                    return _taskCompletionSource.Task;
                }
            }

            public void Complete()
            {
                if (_taskCompletionSource == null)
                {
                    return;
                }

                _taskCompletionSource.SetResult(null);
            }
        }

        private class BackgroundParseResult
        {
            private readonly WorkItem _item;
            private readonly RazorSyntaxTree _syntaxTree;
            private readonly RazorCSharpDocument _cSharpDocument;

            public BackgroundParseResult(WorkItem item, RazorSyntaxTree syntaxTree, RazorCSharpDocument cSharpDocument)
            {
                _item = item;
                _syntaxTree = syntaxTree;
                _cSharpDocument = cSharpDocument;
            }

            public WorkItem Item => _item;

            public RazorSyntaxTree SyntaxTree => _syntaxTree;

            public RazorCSharpDocument CSharpDocument => _cSharpDocument;
        }
    }
}
