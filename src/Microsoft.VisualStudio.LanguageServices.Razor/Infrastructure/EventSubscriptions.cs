// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure
{
    internal class EventSubscriptions
    {
        public IList<IRazorEditorWorkerEvents> Items { get; } = new List<IRazorEditorWorkerEvents>();
    }
}
