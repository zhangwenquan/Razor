// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.CodeAnalysis.Razor
{
    // Similar to https://github.com/xunit/samples.xunit/tree/master/STAExamples
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Microsoft.CodeAnalysis.Razor.ForegroundTestCaseDiscoverer", "Microsoft.AspNetCore.Razor.Test.Common")]
    public class ForegroundFactAttribute : FactAttribute
    {
    }
}
