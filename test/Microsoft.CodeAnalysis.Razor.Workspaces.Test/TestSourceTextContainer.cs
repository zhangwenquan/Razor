// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace Microsoft.CodeAnalysis.Text
{
    public class TestSourceTextContainer : SourceTextContainer
    {
        public override event EventHandler<TextChangeEventArgs> TextChanged;

        private readonly DocumentId _documentId;
        private readonly Workspace _workspace;

        public TestSourceTextContainer(Workspace workspace, DocumentId documentId)
        {
            _workspace = workspace;
            _documentId = documentId;
        }

        public override SourceText CurrentText => Document.GetTextAsync().GetAwaiter().GetResult();


        public TextDocument Document => _workspace.CurrentSolution.GetDocument(_documentId);

        public void UpdateText(SourceText text)
        {
            var original = CurrentText;

            var solution = _workspace.CurrentSolution.WithDocumentText(_documentId, text);
            Assert.True(_workspace.TryApplyChanges(solution));

            var handler = TextChanged;
            if (handler != null)
            {
                handler(this, new TextChangeEventArgs(original, CurrentText));
            }
        }
    }
}
