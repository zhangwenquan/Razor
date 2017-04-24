// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.Editor
{
    public abstract class RazorEditorWorker
    {
        public abstract event EventHandler Updated;

        public abstract State Current { get; }

        public abstract RazorTemplateEngine TemplateEngine { get; }

        public abstract Workspace Workspace { get; }

        public abstract Task ParseAsync();

        public abstract Task ParseAsync(TextChange change);

        public abstract bool TryParse(TextChange change);

        public abstract class State
        {
            public abstract SourceText Text { get; }

            public abstract RazorSyntaxTree SyntaxTree { get; }

            public abstract RazorCSharpDocument CSharpDocument { get; }
        }
    }
}
