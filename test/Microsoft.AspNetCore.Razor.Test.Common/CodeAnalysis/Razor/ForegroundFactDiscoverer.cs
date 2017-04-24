// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.CodeAnalysis.Razor
{
    public class ForegroundFactDiscoverer : IXunitTestCaseDiscoverer
    {
        readonly FactDiscoverer _factDiscoverer;

        public ForegroundFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            _factDiscoverer = new FactDiscoverer(diagnosticMessageSink);
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return 
                _factDiscoverer
                .Discover(discoveryOptions, testMethod, factAttribute)
                .Select(testCase => new ForegroundTestCase(testCase));
        }
    }
}