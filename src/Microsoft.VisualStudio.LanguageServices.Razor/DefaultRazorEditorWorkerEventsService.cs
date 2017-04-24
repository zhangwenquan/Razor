// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor
{
    [Export(typeof(RazorEditorWorkerEventsService))]
    internal class DefaultRazorEditorWorkerEventsService : RazorEditorWorkerEventsService
    {
        public override RazorEditorWorker GetWorker(ITextBuffer razorBuffer)
        {
            if (razorBuffer == null)
            {
                throw new ArgumentNullException(nameof(razorBuffer));
            }

            razorBuffer.Properties.TryGetProperty<RazorEditorWorker>(typeof(RazorEditorWorker), out var worker);
            return worker;
        }

        public override void AdviseWorkerEvents(ITextBuffer razorBuffer, IRazorEditorWorkerEvents events)
        {
            if (razorBuffer == null)
            {
                throw new ArgumentNullException(nameof(razorBuffer));
            }

            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }
            
            if (!razorBuffer.Properties.TryGetProperty<EventSubscriptions>(typeof(EventSubscriptions), out var subscriptions))
            {
                subscriptions = new EventSubscriptions();
                razorBuffer.Properties.AddProperty(typeof(EventSubscriptions), subscriptions);
            }

            subscriptions.Items.Add(events);
        }

        public override void UnadviseWorkerEvents(ITextBuffer razorBuffer, IRazorEditorWorkerEvents events)
        {
            if (razorBuffer == null)
            {
                throw new ArgumentNullException(nameof(razorBuffer));
            }

            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            if (razorBuffer.Properties.TryGetProperty<EventSubscriptions>(typeof(EventSubscriptions), out var subscriptions))
            {
                for (var i = 0; i < subscriptions.Items.Count; i++)
                {
                    if (object.ReferenceEquals(events, subscriptions.Items[i]))
                    {
                        subscriptions.Items.RemoveAt(i);
                        return;
                    }
                }
            }
        }
    }
}
