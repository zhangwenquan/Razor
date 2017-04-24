// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Editor
{
    public abstract class RazorEditorWorkerFactory : ILanguageService
    {
        public abstract RazorEditorWorker Create(string filePath, DocumentId documentId, SourceTextContainer container);
    }
}
