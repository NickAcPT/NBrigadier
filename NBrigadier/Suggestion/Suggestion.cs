using System;
using System.Text;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public class Suggestion : IComparable<Suggestion>
    {
        private readonly StringRange range;
        private readonly string text;
        private readonly Message tooltip;

        public Suggestion(StringRange range, string text) : this(range, text, null)
        {
        }

        public Suggestion(StringRange range, string text, Message tooltip)
        {
            this.range = range;
            this.text = text;
            this.tooltip = tooltip;
        }

        public virtual StringRange Range => range;

        public virtual string Text => text;

        public virtual Message Tooltip => tooltip;

        public int CompareTo(Suggestion o)
        {
            return string.Compare(text, o.text, StringComparison.Ordinal);
        }

        public virtual string Apply(string input)
        {
            if (range.Start == 0 && range.End == input.Length) return text;
            var result = new StringBuilder();
            if (range.Start > 0) result.Append(input.Substring(0, range.Start));
            result.Append(text);
            if (range.End < input.Length) result.Append(input.Substring(range.End));
            return result.ToString();
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is Suggestion)) return false;
            var that = (Suggestion) o;
            return Equals(range, that.range) && Equals(text, that.text) && Equals(tooltip, that.tooltip);
        }

        public override string ToString()
        {
            return "Suggestion{" + "range=" + range + ", text='" + text + '\'' + ", tooltip='" + tooltip + '\'' + '}';
        }

        public virtual int CompareToIgnoreCase(Suggestion b)
        {
            return string.Compare(text, b.text, StringComparison.OrdinalIgnoreCase);
        }

        public virtual Suggestion Expand(string command, StringRange range)
        {
            if (range.Equals(this.range)) return this;
            var result = new StringBuilder();
            if (range.Start < this.range.Start) result.Append(command.SubstringSpecial(range.Start, this.range.Start));
            result.Append(text);
            if (range.End > this.range.End) result.Append(command.SubstringSpecial(this.range.End, range.End));
            return new Suggestion(range, result.ToString(), tooltip);
        }
    }
}