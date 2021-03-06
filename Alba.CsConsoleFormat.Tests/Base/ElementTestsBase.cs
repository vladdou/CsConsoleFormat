﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;

namespace Alba.CsConsoleFormat.Tests
{
    public class ElementTestsBase
    {
        protected const string XamlNS = "xmlns='urn:alba:cs-console-format' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'";

        private char _fillChar = 'a';

        protected static void RenderOn1x1([NotNull] BlockElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            element.GenerateVisualTree();
            element.Measure(new Size(1, 1));
            element.Arrange(new Rect(1, 1, 1, 1));
            element.Render(new ConsoleBuffer(1));
        }

        protected static string GetRenderedText([NotNull] Element element, int consoleWidth)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            var doc = element as Document ?? new Document { Children = { element } };
            string text = ConsoleRenderer.RenderDocumentToText(doc, new TextRenderTarget(),
                new Rect(0, 0, consoleWidth, Size.Infinity));
            return text.Length > 0 ? text.Remove(text.Length - 2) : text;
        }

        protected static string GetRenderedText([NotNull] ConsoleBuffer buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            var target = new TextRenderTarget();
            target.Render(buffer);
            string text = target.OutputText;
            return text.Length > 0 ? text.Remove(text.Length - 2) : text;
        }

        protected Div CreateRectDiv(int width, int height)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++)
                    sb.Append(_fillChar++);
                sb.AppendLine();
            }
            int newLineLength = Environment.NewLine.Length;
            sb.Remove(sb.Length - newLineLength, newLineLength);

            return new Div {
                TextWrap = TextWrap.NoWrap,
                Width = width,
                Height = height,
                Align = Align.Left,
                VerticalAlign = VerticalAlign.Top,
                Children = {
                    new Span(sb.ToString())
                }
            };
        }

        protected class Fill : BlockElement
        {
            [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Write-only properties are not okay.")]
            public char Char { get; set; }

            public override void Render(ConsoleBuffer buffer)
            {
                base.Render(buffer);
                buffer.FillForegroundRectangle(new Rect(RenderSize), null, Char);
            }
        }

        protected sealed class FillAlphabet : Fill
        {
            public int AlphaWidth { get; set; }
            public int AlphaHeight { get; set; }

            public FillAlphabet()
            {
                Char = '-';
            }

            protected override Size MeasureOverride(Size availableSize) => new Size(AlphaWidth, AlphaHeight);

            public override void Render(ConsoleBuffer buffer)
            {
                base.Render(buffer);
                char nextChar = 'a';
                for (int y = 0; y < AlphaHeight; y++)
                    for (int x = 0; x < AlphaWidth; x++)
                        buffer.DrawString(x, y, null, new string(nextChar++, 1));
            }
        }
    }
}