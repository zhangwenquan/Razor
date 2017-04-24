// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Projection;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure
{
    internal class TextBufferStateListener
    {
        private readonly ITextBufferInitializationService _initializer;

        private ITextBuffer _cSharpBuffer;
        private WorkspaceRegistration _registration;
        private Workspace _workspace;

        public TextBufferStateListener(ITextBufferInitializationService initializer, IBufferGraph graph, ITextBuffer razorBuffer)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException(nameof(initializer));
            }

            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (razorBuffer == null)
            {
                throw new ArgumentNullException(nameof(razorBuffer));
            }

            _initializer = initializer;

            Graph = graph;
            RazorBuffer = razorBuffer;

            // This will usually return null. Generally the CSharp buffer is initialized well after
            // the Razor buffer, so when we get here it hasn't been created yet.
            CSharpBuffer = GetCSharpBuffer(Graph, RazorBuffer);

            Graph.GraphBuffersChanged += Graph_GraphBuffersChanged;
        }

        public IBufferGraph Graph { get; }

        public ITextBuffer RazorBuffer { get; }

        public ITextBuffer CSharpBuffer
        {
            get
            {
                return _cSharpBuffer;
            }
            private set
            {
                if (!object.ReferenceEquals(_cSharpBuffer, value))
                {
                    _cSharpBuffer = value;

                    if (_cSharpBuffer != null)
                    {
                        Registration = Workspace.GetWorkspaceRegistration(_cSharpBuffer.AsTextContainer());
                    }
                }
            }
        }

        public WorkspaceRegistration Registration
        {
            get
            {
                return _registration;
            }
            private set
            {
                if (!object.ReferenceEquals(_registration, value))
                {
                    if (_registration != null)
                    {
                        _registration.WorkspaceChanged -= Registration_WorkspaceChanged;
                        Workspace = null;
                    }

                    _registration = value;

                    if (_registration != null)
                    {
                        _registration.WorkspaceChanged += Registration_WorkspaceChanged;
                        Workspace = _registration.Workspace;
                    }
                }
            }
        }

        public Workspace Workspace
        {
            get
            {
                return _workspace;
            }
            private set
            {
                if (!object.ReferenceEquals(_workspace, value))
                {
                    if (_workspace != null)
                    {
                        _workspace.DocumentActiveContextChanged -= Workspace_DocumentActiveContextChanged;

                        _initializer.Unitialize(RazorBuffer);
                    }

                    _workspace = value;

                    if (_workspace != null)
                    {
                        _initializer.Initialize(RazorBuffer, CSharpBuffer, _workspace);

                        _workspace.DocumentActiveContextChanged += Workspace_DocumentActiveContextChanged;
                    }
                }
            }
        }

        private static ITextBuffer GetCSharpBuffer(IBufferGraph graph, ITextBuffer razorBuffer)
        {
            var cSharpBuffers = graph.GetTextBuffers(b => b.ContentType.TypeName == "CSharp");
            for (var i = 0; i < cSharpBuffers.Count; i++)
            {
                // The buffer that we're looking for is the C# projection OVER the Razor code.
                var cSharpBuffer = cSharpBuffers[i] as IProjectionBufferBase;
                if (cSharpBuffer != null && cSharpBuffer.SourceBuffers.Contains(razorBuffer))
                {
                    return cSharpBuffer;
                }
            }

            return null;
        }

        private void Graph_GraphBuffersChanged(object sender, GraphBuffersChangedEventArgs e)
        {
            Debug.Assert(sender == Graph);

            // Ignore the event args, and let's just update the CSharp buffer if we need to.
            CSharpBuffer = GetCSharpBuffer(Graph, RazorBuffer);
        }

        private void Registration_WorkspaceChanged(object sender, EventArgs e)
        {
            Debug.Assert(_registration == sender);

            // There's no data on the event args, just use the registration.
            Workspace = _registration.Workspace;
        }

        private void Workspace_DocumentActiveContextChanged(object sender, DocumentActiveContextChangedEventArgs e)
        {
        }
    }
}
