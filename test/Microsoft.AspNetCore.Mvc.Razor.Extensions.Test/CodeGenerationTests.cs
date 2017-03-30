// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.Evolution;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions
{
    public class CodeGenerationTests : CodeGenerationTestBase<CodeGenerationTests>
    {
        #region Runtime
        [Fact]
        public void RazorEngine_Basic_Runtime()
        {
            RunRuntimeTest("Basic");
        }

        [Fact]
        public void RazorEngine_ViewImports_Runtime()
        {
            RunRuntimeTest("_ViewImports");
        }

        [Fact]
        public void RazorEngine_Inject_Runtime()
        {
            RunRuntimeTest("Inject");
        }

        [Fact]
        public void RazorEngine_InjectWithModel_Runtime()
        {
            RunRuntimeTest("InjectWithModel");
        }

        [Fact]
        public void RazorEngine_InjectWithSemicolon_Runtime()
        {
            RunRuntimeTest("InjectWithSemicolon");
        }

        [Fact]
        public void RazorEngine_Model_Runtime()
        {
            RunRuntimeTest("Model");
        }

        [Fact]
        public void RazorEngine_ModelExpressionTagHelper_Runtime()
        {
            RunRuntimeTest("ModelExpressionTagHelper");
        }

        [Fact]
        public void RazorEngine_RazorPages_Runtime()
        {
            RunRuntimeTest("RazorPages", BuildDivDescriptors());
        }

        [Fact]
        public void RazorEngine_RazorPagesWithoutModel_Runtime()
        {
            RunRuntimeTest("RazorPagesWithoutModel", BuildDivDescriptors());
        }
        #endregion

        #region DesignTime
        [Fact]
        public void RazorEngine_Basic_DesignTime()
        {
            RunDesignTimeTest("Basic");
        }

        [Fact]
        public void RazorEngine_ViewImports_DesignTime()
        {
            RunDesignTimeTest("_ViewImports");
        }

        [Fact]
        public void RazorEngine_Inject_DesignTime()
        {
            RunDesignTimeTest("Inject");
        }

        [Fact]
        public void RazorEngine_InjectWithModel_DesignTime()
        {
            RunDesignTimeTest("InjectWithModel");
        }

        [Fact]
        public void RazorEngine_InjectWithSemicolon_DesignTime()
        {
            RunDesignTimeTest("InjectWithSemicolon");
        }

        [Fact]
        public void RazorEngine_Model_DesignTime()
        {
            RunDesignTimeTest("Model");
        }

        [Fact]
        public void RazorEngine_MultipleModels_DesignTime()
        {
            RunDesignTimeTest("MultipleModels");
        }

        [Fact]
        public void RazorEngine_ModelExpressionTagHelper_DesignTime()
        {
            RunDesignTimeTest("ModelExpressionTagHelper");
        }

        [Fact]
        public void RazorEngine_RazorPages_DesignTime()
        {
            RunDesignTimeTest("RazorPages", BuildDivDescriptors());
        }

        [Fact]
        public void RazorEngine_RazorPagesWithoutModel_DesignTime()
        {
            RunDesignTimeTest("RazorPagesWithoutModel", BuildDivDescriptors());
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