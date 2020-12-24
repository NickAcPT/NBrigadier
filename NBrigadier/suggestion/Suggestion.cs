using System;
using System.Text;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public class Suggestion : IComparable<Suggestion>
    {
        protected bool Equals(Suggestion other)
        {
            return Equals(_range, other._range) && _text == other._text && Equals(_tooltip, other._tooltip);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_range, _text, _tooltip);
        }

        public static bool operator ==(Suggestion left, Suggestion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Suggestion left, Suggestion right)
        {
            return !Equals(left, right);
        }

        private readonly StringRange _range;
        private readonly string _text;
        private readonly IMessage _tooltip;

        public Suggestion(StringRange range, string text) : this(range, text, null)
        {
        }

        public Suggestion(StringRange range, string text, IMessage tooltip)
        {
            this._range = range;
            this._text = text;
            this._tooltip = tooltip;
        }

        public virtual StringRange Range => _range;

        public virtual string Text => _text;

        public virtual IMessage Tooltip => _tooltip;

        public int CompareTo(Suggestion o)
        {
            return string.Compare(_text, o._text, StringComparison.Ordinal);
        }

        public virtual string Apply(string input)
        {
            if (_range.Start == 0 && _range.End == input.Length) return _text;
            var result = new StringBuilder();
            if (_range.Start > 0) result.Append(input.Substring(0, _range.Start));
            result.Append(_text);
            if (_range.End < input.Length) result.Append(input.Substring(_range.End));
            return result.ToString();
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is Suggestion)) return false;
            var that = (Suggestion) o;
            return Equals(_range, that._range) && Equals(_text, that._text) && Equals(_tooltip, that._tooltip);
        }

        public override string ToString()
        {
            return "Suggestion{" + "range=" + _range + ", text='" + _text + '\'' + ", tooltip='" + _tooltip + '\'' + '}';
        }

        public virtual int CompareToIgnoreCase(Suggestion b)
        {
            return string.Compare(_text, b._text, StringComparison.OrdinalIgnoreCase);
        }

        public virtual Suggestion Expand(string command, StringRange range)
        {
            if (range.Equals(this._range)) return this;
            var result = new StringBuilder();
            if (range.Start < this._range.Start) result.Append(command.SubstringSpecial(range.Start, this._range.Start));
            result.Append(_text);
            if (range.End > this._range.End) result.Append(command.SubstringSpecial(this._range.End, range.End));
            return new Suggestion(range, result.ToString(), _tooltip);
        }
    }
}