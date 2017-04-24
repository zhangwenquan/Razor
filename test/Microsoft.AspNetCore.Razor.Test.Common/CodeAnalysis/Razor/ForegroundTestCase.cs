// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.CodeAnalysis.Razor
{
    [DebuggerDisplay(@"\{ class = {TestMethod.TestClass.Class.Name}, method = {TestMethod.Method.Name}, display = {DisplayName}, skip = {SkipReason} \}")]
    public class ForegroundTestCase : LongLivedMarshalByRefObject, IXunitTestCase
    {
        private IXunitTestCase _testCase;

        public ForegroundTestCase(IXunitTestCase testCase)
        {
            _testCase = testCase;
        }

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", error: true)]
        public ForegroundTestCase()
        {
        }

        public IMethodInfo Method => _testCase.Method;

        public Task<RunSummary> RunAsync(
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            object[] constructorArguments,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
        {
            var completionSource = new TaskCompletionSource<RunSummary>();
            var thread = new Thread(() =>
            {
                try
                {
                    var synchronizationContext = new SingleThreadedSynchronizationContext();
                    SynchronizationContext.SetSynchronizationContext(synchronizationContext);
                    
                    // This will 'start' the test method but return control here on the first await
                    // so we can process other work items.
                    var task = _testCase.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);

                    // When the test method completes, we can complete the sync context, which allows our call to Run()
                    // to return.
                    task.ContinueWith((t) =>
                    {
                        synchronizationContext.Complete();
                    });

                    // This will execute or block until Complete() is called.
                    synchronizationContext.Run();

                    // Report the result back to the Task we returned earlier.
                    CopyTaskResultFrom(completionSource, task);
                }
                catch (Exception e)
                {
                    completionSource.SetException(e);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return completionSource.Task;
        }

        public string DisplayName
        {
            get { return _testCase.DisplayName; }
        }

        public string SkipReason
        {
            get { return _testCase.SkipReason; }
        }

        public ISourceInformation SourceInformation
        {
            get { return _testCase.SourceInformation; }
            set { _testCase.SourceInformation = value; }
        }

        public ITestMethod TestMethod
        {
            get { return _testCase.TestMethod; }
        }

        public object[] TestMethodArguments
        {
            get { return _testCase.TestMethodArguments; }
        }

        public Dictionary<string, List<string>> Traits
        {
            get { return _testCase.Traits; }
        }

        public string UniqueID
        {
            get { return _testCase.UniqueID; }
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            _testCase = info.GetValue<IXunitTestCase>("InnerTestCase");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("InnerTestCase", _testCase);
        }

        private static void CopyTaskResultFrom<T>(TaskCompletionSource<T> completionSource, Task<T> task)
        {
            if (completionSource == null)
            {
                throw new ArgumentNullException("tcs");
            }

            if (task == null)
            {
                throw new ArgumentNullException("template");
            }

            if (!task.IsCompleted)
            {
                throw new ArgumentException("Task must be completed first.", nameof(task));
            }

            if (task.IsFaulted)
            {
                completionSource.SetException(task.Exception);
            }
            else if (task.IsCanceled)
            {
                completionSource.SetCanceled();
            }
            else
            {
                completionSource.SetResult(task.Result);
            }
        }
    }
}