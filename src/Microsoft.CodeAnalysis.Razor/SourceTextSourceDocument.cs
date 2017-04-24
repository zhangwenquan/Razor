// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor
{
    internal class SourceTextSourceDocument : RazorSourceDocument
    {
        private readonly SourceText _sourceText;
        private readonly string _fileName;

        private SourceTextLineCollection _lines;

        public SourceTextSourceDocument(SourceText sourceText, string fileName)
        {
            if (sourceText == null)
            {
                throw new ArgumentNullException(nameof(sourceText));
            }

            _sourceText = sourceText;
            _fileName = fileName;
        }

        public override char this[int position] => _sourceText[position];

        public override Encoding Encoding => _sourceText.Encoding;

        public override string FileName => _fileName;

        public override int Length => _sourceText.Length;

        public override RazorSourceLineCollection Lines
        {
            get
            {
                if (_lines == null)
                {
                    _lines = new SourceTextLineCollection(_sourceText.Lines, _fileName);
                }

                return _lines;
            }
        }

        public override void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            _sourceText.CopyTo(sourceIndex, destination, destinationIndex, count);
        }

        public override byte[] GetChecksum()
        {
            return _sourceText.GetChecksum().ToArray();
        }

        private class SourceTextLineCollection : RazorSourceLineCollection
        {
            private readonly TextLineCollection _textLines;
            private readonly string _fileName;

            public SourceTextLineCollection(TextLineCollection textLines, string fileName)
            {
                if (textLines == null)
                {
                    throw new ArgumentNullException(nameof(textLines));
                }

                _textLines = textLines;
                _fileName = fileName;
            }

            public override int Count => _textLines.Count;

            public override int GetLineLength(int index)
            {
                return _textLines[index].Start - _textLines[index].End;
            }

            internal override SourceLocation GetLocation(int position)
            {
                var linePosition = _textLines.GetLinePosition(position);
                return new SourceLocation(_fileName, position, linePosition.Line, linePosition.Character);
            }
        }
    }
}
