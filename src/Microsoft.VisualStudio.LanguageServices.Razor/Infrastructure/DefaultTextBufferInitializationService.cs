// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure
{
    // This is the *bridge* between the VS/ITextBuffer/global-MEF and the Roslyn/SourceTextContainer/workspace-MEF
    // worlds. Each document that is open will have a an associated listener that will dispatch to the Initialize
    // and Uninitialize methods to create all of the needed state.
    [Export(typeof(ITextBufferInitializationService))]
    internal class DefaultTextBufferInitializationService : ITextBufferInitializationService
    {
        public void Initialize(ITextBuffer razorBuffer, ITextBuffer cSharpBuffer, Workspace workspace)
        {
            var services = workspace.Services.GetLanguageServices(RazorLanguage.Name);

            var factory = services.GetRequiredService<RazorEditorWorkerFactory>();

            // We track the document's identity based on the CSharp projection of the Razor code.
            // The Razor code itself isn't part of the workspace.
            var documentId = workspace.GetDocumentIdInCurrentContext(cSharpBuffer.AsTextContainer());
            var worker = factory.Create(documentId, razorBuffer.AsTextContainer());

            razorBuffer.Properties.AddProperty(typeof(RazorEditorWorker), worker);

            if (razorBuffer.Properties.TryGetProperty<EventSubscriptions>(typeof(EventSubscriptions), out var subscriptions))
            {
                for (var i = 0; i < subscriptions.Items.Count; i++)
                {
                    subscriptions.Items[i].OnWorkerInitialized(worker);
                }
            }
        }

        public void Unitialize(ITextBuffer razorBuffer)
        {
            var worker = razorBuffer.Properties.GetProperty<RazorEditorWorker>(typeof(RazorEditorWorker));

            if (razorBuffer.Properties.TryGetProperty<EventSubscriptions>(typeof(EventSubscriptions), out var subscriptions))
            {
                for (var i = 0; i < subscriptions.Items.Count; i++)
                {
                    subscriptions.Items[i].OnWorkerUninitialized(worker);
                }
            }

            (worker as IDisposable)?.Dispose();
        }
    }
}
