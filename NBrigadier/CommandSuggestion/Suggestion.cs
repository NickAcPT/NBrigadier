using System;
using System.Text;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    using StringRange = StringRange;

	public class Suggestion : IComparable<Suggestion>
	{
		private StringRange _range;
		private string _text;
		private IMessage _tooltip;

		public Suggestion(StringRange range, string text) : this(range, text, null)
		{
		}

		public Suggestion(StringRange range, string text, IMessage tooltip)
		{
			this._range = range;
			this._text = text;
			this._tooltip = tooltip;
		}

		public virtual StringRange Range
		{
			get
			{
				return _range;
			}
		}

		public virtual string Text
		{
			get
			{
				return _text;
			}
		}

		public virtual IMessage Tooltip
		{
			get
			{
				return _tooltip;
			}
		}

		public virtual string Apply(string input)
		{
			if (_range.Start == 0 && _range.End == input.Length)
			{
				return _text;
			}
			 StringBuilder result = new StringBuilder();
			if (_range.Start > 0)
			{
				result.Append(input.Substring(0, _range.Start));
			}
			result.Append(_text);
			if (_range.End < input.Length)
			{
				result.Append(input.Substring(_range.End));
			}
			return result.ToString();
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is Suggestion))
			{
				return false;
			}
			 Suggestion that = (Suggestion) o;
			return ObjectsHelper.Equals(_range, that._range) && ObjectsHelper.Equals(_text, that._text) && ObjectsHelper.Equals(_tooltip, that._tooltip);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.Hash(_range, _text, _tooltip);
		}

		public override string ToString()
		{
			return "Suggestion{" + "range=" + _range + ", text='" + _text + '\'' + ", tooltip='" + _tooltip + '\'' + '}';
		}

		public virtual int CompareTo(Suggestion o)
		{
			return _text.CompareTo(o._text);
		}

		public virtual int CompareToIgnoreCase(Suggestion b)
		{
			return string.Compare(_text, b._text, StringComparison.OrdinalIgnoreCase);
		}

		public virtual Suggestion Expand(string command, StringRange range)
		{
			if (range.Equals(this._range))
			{
				return this;
			}
			 StringBuilder result = new StringBuilder();
			if (range.Start < this._range.Start)
			{
				result.Append(StringHelper.SubstringSpecial(command, range.Start, this._range.Start));
			}
			result.Append(_text);
			if (range.End > this._range.End)
			{
				result.Append(StringHelper.SubstringSpecial(command, this._range.End, range.End));
			}
			return new Suggestion(range, result.ToString(), _tooltip);
		}
	}

}