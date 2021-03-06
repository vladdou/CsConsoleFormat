﻿namespace Alba.CsConsoleFormat
{
    public class Column : Element
    {
        public GridLength Width { get; set; }
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int Index { get; internal set; }
        public int ActualWidth { get; internal set; }

        public Column()
        {
            Width = GridLength.Auto;
            MinWidth = 0;
            MaxWidth = Size.Infinity;
        }

        protected override bool CanHaveChildren => false;

        public override string ToString() => base.ToString() + $" Width={Width}({MinWidth},{ActualWidth},{(MaxWidth == Size.Infinity ? "Inf" : MaxWidth + "")})";
    }
}