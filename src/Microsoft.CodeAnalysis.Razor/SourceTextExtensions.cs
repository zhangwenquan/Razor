// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor
{
    public static class SourceTextExtensions
    {
        public static RazorSourceDocument AsSourceDocument(this SourceText sourceText)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            return new SourceTextSourceDocument(sourceText, fileName: null);
        }

        public static RazorSourceDocument AsSourceDocument(this SourceText sourceText, string fileName)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            return new SourceTextSourceDocument(sourceText, fileName);
        }
    }
}
