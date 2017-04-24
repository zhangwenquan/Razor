// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Editor
{
    internal class DefaultRazorEditorWorkerFactory : RazorEditorWorkerFactory
    {
        private readonly Workspace _workspace;

        public DefaultRazorEditorWorkerFactory(Workspace workspace)
        {
            _workspace = workspace;
        }

        public override RazorEditorWorker Create(string filePath, DocumentId documentId, SourceTextContainer container)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (documentId == null)
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            
            return new DefaultRazorEditorWorker(new DefaultThreadDispatcher(), _workspace, null, filePath, documentId, container);
        }
    }
}
