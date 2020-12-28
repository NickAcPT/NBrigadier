using System;
using System.Text;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
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

        public virtual int CompareTo(Suggestion o)
        {
            return _text.CompareTo(o._text);
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
            return ObjectsHelper.Equals(_range, that._range) && ObjectsHelper.Equals(_text, that._text) &&
                   ObjectsHelper.Equals(_tooltip, that._tooltip);
        }

        public override int GetHashCode()
        {
            return ObjectsHelper.Hash(_range, _text, _tooltip);
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