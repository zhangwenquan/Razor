// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Razor.Evolution;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.Test.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.Extensions.DependencyModel;
using Xunit;

namespace Microsoft.AspNetCore.Mvc.Razor.Extensions
{
    public class CodeGenerationTestBase<TTest> : IntegrationTestBase
    {
        public CodeGenerationTestBase()
            : base(typeof(TTest))
        {
        }

        private static Assembly _assembly = typeof(TTest).GetTypeInfo().Assembly;

        protected void RunRuntimeTest(IEnumerable<TagHelperDescriptor> descriptors = null)
        {
            // Arrange
            var engine = CreateRazorEngine(
                descriptors,
                false,
                new List<IRazorEngineFeature> { GetMetadataReferenceFeature() });
            var document = CreateCodeDocument();  
            
            // Act
            engine.Process(document);

            // Assert
            var cSharpDocument = document.GetCSharpDocument();

            Assert.Empty(cSharpDocument.Diagnostics);

            AssertIRMatchesBaseline(document.GetIRDocument());
            AssertCSharpDocumentMatchesBaseline(cSharpDocument);
        }

        protected void RunDesignTimeTest(IEnumerable<TagHelperDescriptor> descriptors = null)
        {
            // Arrange
            var engine = CreateRazorEngine(
                descriptors,
                true,
                new List<IRazorEngineFeature> { GetMetadataReferenceFeature() });
            var document = CreateCodeDocument();

            // Act
            engine.Process(document);

            // Assert
            AssertIRMatchesBaseline(document.GetIRDocument());
            AssertDesignTimeDocumentMatchBaseline(document);
        }

        private static IRazorEngineFeature GetMetadataReferenceFeature()
        {
            var currentAssembly = typeof(TTest).GetTypeInfo().Assembly;
            var dependencyContext = DependencyContext.Load(currentAssembly);

            var references = dependencyContext.CompileLibraries.SelectMany(l => l.ResolveReferencePaths())
                .Select(assemblyPath => MetadataReference.CreateFromFile(assemblyPath))
                .ToList<MetadataReference>();

            var syntaxTree = CreateTagHelperSyntaxTree();
            var compilation = CSharpCompilation.Create("Microsoft.AspNetCore.Mvc.Razor", syntaxTree, references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var stream = new MemoryStream();
            var compilationResult = compilation.Emit(stream, options: new EmitOptions());
            stream.Position = 0;

            Assert.True(compilationResult.Success);

            references.Add(MetadataReference.CreateFromStream(stream));

            var feature = new DefaultMetadataReferenceFeature()
            {
                References = references,
            };

            return feature;
        }

        private static IEnumerable<SyntaxTree> CreateTagHelperSyntaxTree()
        {
            var text = $@"
            public class UrlResolutionTagHelper : {typeof(TagHelper).FullName}
            {{

            }}";

            return new SyntaxTree[] { CSharpSyntaxTree.ParseText(text) };
        }
    }
}
