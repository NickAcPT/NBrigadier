using System;
using System.Text;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public class Suggestion : IComparable<Suggestion>
    {
        private readonly StringRange _range;
        private readonly string _text;
        private readonly IMessage _tooltip;

        public Suggestion(StringRange range, string text) : this(range, text, null)
        {
        }

        public Suggestion(StringRange range, string text, IMessage tooltip)
        {
            _range = range;
            _text = text;
            _tooltip = tooltip;
        }

        public virtual StringRange Range => _range;

        public virtual string Text => _text;

        public virtual IMessage Tooltip => _tooltip;

        public int CompareTo(Suggestion o)
        {
            return string.Compare(_text, o._text, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _range != null ? _range.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_text != null ? _text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_tooltip != null ? _tooltip.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(Suggestion other)
        {
            return Equals(_range, other._range) && _text == other._text && Equals(_tooltip, other._tooltip);
        }

        public static bool operator ==(Suggestion left, Suggestion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Suggestion left, Suggestion right)
        {
            return !Equals(left, right);
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
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != GetType()) return false;
            return Equals((Suggestion) o);
        }

        public override string ToString()
        {
            return "Suggestion{" + "range=" + _range + ", text='" + _text + '\'' + ", tooltip='" + _tooltip + '\'' +
                   '}';
        }

        public virtual int CompareToIgnoreCase(Suggestion b)
        {
            return string.Compare(_text, b._text, StringComparison.OrdinalIgnoreCase);
        }

        public virtual Suggestion Expand(string command, StringRange range)
        {
            if (range.Equals(_range)) return this;
            var result = new StringBuilder();
            if (range.Start < _range.Start) result.Append(command.SubstringSpecial(range.Start, _range.Start));
            result.Append(_text);
            if (range.End > _range.End) result.Append(command.SubstringSpecial(_range.End, range.End));
            return new Suggestion(range, result.ToString(), _tooltip);
        }
    }
}