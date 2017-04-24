// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Editor;

namespace Microsoft.VisualStudio.LanguageServices.Razor
{
    public interface IRazorEditorWorkerEvents
    {
        void OnWorkerInitialized(RazorEditorWorker worker);

        void OnWorkerUninitialized(RazorEditorWorker worker);
    }
}
