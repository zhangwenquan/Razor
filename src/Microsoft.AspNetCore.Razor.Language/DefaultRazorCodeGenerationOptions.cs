﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNetCore.Razor.Language
{
    internal class DefaultRazorCodeGenerationOptions : RazorCodeGenerationOptions
    {
        public DefaultRazorCodeGenerationOptions(bool indentWithTabs, int indentSize, bool designTime)
        {
            IndentWithTabs = indentWithTabs;
            IndentSize = indentSize;
            DesignTime = designTime;
        }

        public override bool DesignTime { get; }

        public override bool IndentWithTabs { get; }

        public override int IndentSize { get; }
    }
}
