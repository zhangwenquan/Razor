// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Infrastructure
{
    // Handles initialization and de-initialization of state associated with a Razor ITextBuffer.
    internal interface ITextBufferInitializationService
    {
        void Initialize(ITextBuffer razorBuffer, ITextBuffer cSharpBuffer, Workspace workspace);

        void Unitialize(ITextBuffer razorBuffer);
    }
}
