﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Xunit;
using static Microsoft.AspNetCore.Razor.Language.Intermediate.RazorIRAssert;

namespace Microsoft.AspNetCore.Razor.Language.Extensions
{
    public class InheritsDirectivePassTest
    {
        [Fact]
        public void Execute_SkipsDocumentWithNoClassNode()
        {
            // Arrange
            var engine = CreateEngine();
            var pass = new InheritsDirectivePass()
            {
                Engine = engine,
            };

            var sourceDocument = TestRazorSourceDocument.Create("@inherits Hello<World[]>");
            var codeDocument = RazorCodeDocument.Create(sourceDocument);

            var irDocument = new DocumentIRNode();
            irDocument.Children.Add(new DirectiveIRNode() { Descriptor = FunctionsDirective.Directive, });

            // Act
            pass.Execute(codeDocument, irDocument);

            // Assert
            Children(
                irDocument,
                node => Assert.IsType<DirectiveIRNode>(node));
        }

        [Fact]
        public void Execute_Inherits_SetsClassDeclarationBaseType()
        {
            // Arrange
            var engine = CreateEngine();
            var pass = new InheritsDirectivePass()
            {
                Engine = engine,
            };

            var content = "@inherits Hello<World[]>";
            var sourceDocument = TestRazorSourceDocument.Create(content);
            var codeDocument = RazorCodeDocument.Create(sourceDocument);

            var irDocument = Lower(codeDocument, engine);

            // Act
            pass.Execute(codeDocument, irDocument);

            // Assert
            Children(
                irDocument,
                node => Assert.IsType<ChecksumIRNode>(node),
                node => Assert.IsType<NamespaceDeclarationIRNode>(node));

            var @namespace = irDocument.Children[1];
            Children(
                @namespace,
                node => Assert.IsType<ClassDeclarationIRNode>(node));

            var @class = (ClassDeclarationIRNode)@namespace.Children[0];
            Assert.Equal("Hello<World[]>", @class.BaseType);
        }

        private static RazorEngine CreateEngine()
        {
            return RazorEngine.Create(b =>
            {
                InheritsDirective.Register(b);
            });
        }

        private static DocumentIRNode Lower(RazorCodeDocument codeDocument, RazorEngine engine)
        {
            for (var i = 0; i < engine.Phases.Count; i++)
            {
                var phase = engine.Phases[i];
                phase.Execute(codeDocument);

                if (phase is IRazorDocumentClassifierPhase)
                {
                    break;
                }
            }

            var irDocument = codeDocument.GetIRDocument();
            Assert.NotNull(irDocument);

            return irDocument;
        }
    }
}
