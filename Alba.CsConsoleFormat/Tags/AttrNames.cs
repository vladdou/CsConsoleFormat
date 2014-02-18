﻿using System.Xml.Linq;
using Alba.CsConsoleFormat.Framework.Text;

namespace Alba.CsConsoleFormat
{
    internal static class AttrNames
    {
        public static readonly XName Align = GetName<ConAlignAttr>();
        public static readonly XName Background = GetName<ConBackgroundAttr>();
        public static readonly XName Foreground = GetName<ConForegroundAttr>();
        public static readonly XName VerticalAlign = GetName<ConVerticalAlignAttr>();

        private static XName GetName<TConTag> () where TConTag : XAttribute
        {
            return typeof(TConTag).Name.RemovePrefix("Con").RemovePostfix("Attr");
        }
    }
}