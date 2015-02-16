﻿using System.Linq;
using Alba.CsConsoleFormat.Framework.Text;

namespace Alba.CsConsoleFormat
{
    public class Span : InlineElement
    {
        public string Text { get; set; }

        public Span (string text)
        {
            Text = text;
        }

        public Span () : this(null)
        {}

        public override string GeneratedText
        {
            get { return Text ?? string.Concat(VisualChildren.Cast<InlineElement>().Select(c => c.GeneratedText)); }
        }

        public override void GenerateSequence (IInlineSequence sequence)
        {
            if (Color != null)
                sequence.PushColor(Color.Value);
            if (BgColor != null)
                sequence.PushBgColor(BgColor.Value);

            if (Text != null) {
                sequence.AppendText(Text);
            }
            else {
                foreach (InlineElement child in VisualChildren.Cast<InlineElement>())
                    child.GenerateSequence(sequence);
            }

            if (BgColor != null)
                sequence.PopFormatting();
            if (Color != null)
                sequence.PopFormatting();
        }

        public override string ToString ()
        {
            return base.ToString() + " Text=\"{0}\"".Fmt(Text);
        }
    }
}