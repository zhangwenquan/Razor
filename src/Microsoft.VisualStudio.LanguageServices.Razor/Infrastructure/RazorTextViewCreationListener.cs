// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure
{
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    [Export(typeof(IWpfTextViewCreationListener))]
    internal class RazorTextViewCreationListener : IWpfTextViewCreationListener
    {
        [Import]
        public ITextBufferInitializationService Initializer { get; set; }

        public void TextViewCreated(IWpfTextView textView)
        {
            if (textView == null)
            {
                throw new ArgumentNullException(nameof(textView));
            }

            var razorBuffer = textView.BufferGraph.GetTextBuffers(b => b.ContentType.TypeName == RazorLanguage.ContentType).FirstOrDefault();
            if (razorBuffer != null)
            {
                var listener = new TextBufferStateListener(Initializer, textView.BufferGraph, razorBuffer);
                razorBuffer.Properties.AddProperty(typeof(TextBufferStateListener), listener);
            }
        }
    }
}
