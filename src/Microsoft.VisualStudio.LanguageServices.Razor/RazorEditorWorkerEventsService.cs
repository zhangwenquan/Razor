// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor
{
    public abstract class RazorEditorWorkerEventsService
    {
        public abstract RazorEditorWorker GetWorker(ITextBuffer razorBuffer);

        public abstract void AdviseWorkerEvents(ITextBuffer razorBuffer, IRazorEditorWorkerEvents events);

        public abstract void UnadviseWorkerEvents(ITextBuffer razorBuffer, IRazorEditorWorkerEvents events);
    }
}
