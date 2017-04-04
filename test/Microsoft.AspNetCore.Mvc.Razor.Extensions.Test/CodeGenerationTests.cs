// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Evolution;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions
{
    public class CodeGenerationTests : CodeGenerationTestBase<CodeGenerationTests>
    {
        #region Runtime
        [Fact]
        public void Basic_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void _ViewImports_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void Inject_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void InjectWithModel_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void InjectWithSemicolon_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void Model_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void ModelExpressionTagHelper_Runtime()
        {
            RunRuntimeTest();
        }

        [Fact]
        public void RazorPages_Runtime()
        {
            RunRuntimeTest(BuildDivDescriptors());
        }

        [Fact]
        public void RazorPagesWithoutModel_Runtime()
        {
            RunRuntimeTest(BuildDivDescriptors());
        }
        #endregion

        #region DesignTime
        [Fact]
        public void Basic_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void _ViewImports_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void Inject_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void InjectWithModel_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void InjectWithSemicolon_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void Model_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void MultipleModels_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void ModelExpressionTagHelper_DesignTime()
        {
            RunDesignTimeTest();
        }

        [Fact]
        public void RazorPages_DesignTime()
        {
            RunDesignTimeTest(BuildDivDescriptors());
        }

        [Fact]
        public void RazorPagesWithoutModel_DesignTime()
        {
            RunDesignTimeTest(BuildDivDescriptors());
        }
        #endregion

        private static IEnumerable<TagHelperDescriptor> BuildDivDescriptors()
        {
            return new List<TagHelperDescriptor> {
                BuildDescriptor("div", "DivTagHelper", "TestAssembly"),
                BuildDescriptor("a", "UrlResolutionTagHelper", "Microsoft.AspNetCore.Mvc.Razor")
            };
        }

        private static TagHelperDescriptor BuildDescriptor(
            string tagName,
            string typeName,
            string assemblyName)
        {
            return ITagHelperDescriptorBuilder.Create(typeName, assemblyName)
                .TagMatchingRule(ruleBuilder => ruleBuilder.RequireTagName(tagName))
                .Build();
        }

    }
}