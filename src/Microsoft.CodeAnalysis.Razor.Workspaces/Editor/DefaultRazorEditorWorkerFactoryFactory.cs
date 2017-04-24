// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;

namespace Microsoft.CodeAnalysis.Razor.Editor
{
    [ExportLanguageServiceFactory(typeof(RazorEditorWorkerFactory), "Razor")]
    internal class DefaultRazorEditorWorkerFactoryFactory : ILanguageServiceFactory
    {
        public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
        {
            if (languageServices == null)
            {
                throw new ArgumentNullException(nameof(languageServices));
            }

            var workspace = languageServices.WorkspaceServices.Workspace;
            return new DefaultRazorEditorWorkerFactory(workspace);
        }
    }
}
