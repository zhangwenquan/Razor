// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Microsoft.AspNetCore.Mvc.Razor.Extensions;
using Microsoft.AspNetCore.Razor.Evolution;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.AspNetCore.Razor.Test.Common
{
    public class CodeGenerationTestBase<TTest>
    {
        private static Assembly _assembly = typeof(TTest).GetTypeInfo().Assembly;

        protected void RunRuntimeTest(string testName, IEnumerable<TagHelperDescriptor> descriptors = null)
        {
            // Arrange
            var inputFile = "TestFiles/Input/" + testName + ".cshtml";
            var outputFile = "TestFiles/Output/Runtime/" + testName + ".cs";
            var expectedCode = ResourceFile.ReadResource(_assembly, outputFile, sourceFile: false);

            var engine = RazorEngine.Create(b =>
            {
                RazorExtensions.Register(b);

                b.Features.Add(GetMetadataReferenceFeature());

                if (descriptors != null)
                {
                    b.AddTagHelpers(descriptors);
                }
                else
                {
                    b.Features.Add(new DefaultTagHelperFeature());
                }

            });

            var inputContent = ResourceFile.ReadResource(_assembly, inputFile, sourceFile: true);
            var item = new TestRazorProjectItem("/" + inputFile) { Content = inputContent, };
            var project = new TestRazorProject(new List<RazorProjectItem>()
            {
                item,
            });

            var razorTemplateEngine = new MvcRazorTemplateEngine(engine, project);
            razorTemplateEngine.Options.ImportsFileName = "_ViewImports.cshtml";
            var codeDocument = razorTemplateEngine.CreateCodeDocument(item);
            codeDocument.Items["SuppressUniqueIds"] = "test";
            codeDocument.Items["NewLineString"] = "\r\n";

            // Act
            var csharpDocument = razorTemplateEngine.GenerateCode(codeDocument);

            // Assert
            Assert.Empty(csharpDocument.Diagnostics);

#if GENERATE_BASELINES
            ResourceFile.UpdateFile(_assembly, outputFile, expectedCode, csharpDocument.GeneratedCode);
#else
            Assert.Equal(expectedCode, csharpDocument.GeneratedCode, ignoreLineEndingDifferences: true);
#endif
        }

        private void RunDesignTimeTest(string testName, IEnumerable<TagHelperDescriptor> descriptors = null)
        {
            // Arrange
            var inputFile = "TestFiles/Input/" + testName + ".cshtml";
            var outputFile = "TestFiles/Output/DesignTime/" + testName + ".cs";
            var expectedCode = ResourceFile.ReadResource(_assembly, outputFile, sourceFile: false);

            var lineMappingOutputFile = "TestFiles/Output/DesignTime/" + testName + ".mappings.txt";
            var expectedMappings = ResourceFile.ReadResource(_assembly, lineMappingOutputFile, sourceFile: false);

            var engine = RazorEngine.CreateDesignTime(b =>
            {
                RazorExtensions.Register(b);

                b.Features.Add(GetMetadataReferenceFeature());

                if (descriptors != null)
                {
                    b.AddTagHelpers(descriptors);
                }
                else
                {
                    b.Features.Add(new DefaultTagHelperFeature());
                }
            });

            var inputContent = ResourceFile.ReadResource(_assembly, inputFile, sourceFile: true);
            var item = new TestRazorProjectItem("/" + inputFile) { Content = inputContent, };
            var project = new TestRazorProject(new List<RazorProjectItem>()
            {
                item,
            });

            var razorTemplateEngine = new MvcRazorTemplateEngine(engine, project);
            razorTemplateEngine.Options.ImportsFileName = "_ViewImports.cshtml";
            var codeDocument = razorTemplateEngine.CreateCodeDocument(item);
            codeDocument.Items["SuppressUniqueIds"] = "test";
            codeDocument.Items["NewLineString"] = "\r\n";

            // Act
            var csharpDocument = razorTemplateEngine.GenerateCode(codeDocument);

            // Assert
            Assert.Empty(csharpDocument.Diagnostics);

            var serializedMappings = LineMappingsSerializer.Serialize(csharpDocument, codeDocument.Source);

#if GENERATE_BASELINES
            ResourceFile.UpdateFile(_assembly, outputFile, expectedCode, csharpDocument.GeneratedCode);
            ResourceFile.UpdateFile(_assembly, lineMappingOutputFile, expectedMappings, serializedMappings);
#else
            Assert.Equal(expectedCode, csharpDocument.GeneratedCode, ignoreLineEndingDifferences: true);
            Assert.Equal(expectedMappings, serializedMappings, ignoreLineEndingDifferences: true);
#endif
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
