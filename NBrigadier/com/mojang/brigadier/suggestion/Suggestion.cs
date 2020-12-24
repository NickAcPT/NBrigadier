using System;
using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using Message = com.mojang.brigadier.Message;
	using StringRange = com.mojang.brigadier.context.StringRange;

	public class Suggestion : IComparable<Suggestion>
	{
		private readonly StringRange range;
		private readonly string text;
		private readonly Message tooltip;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public Suggestion(final com.mojang.brigadier.context.StringRange range, final String text)
		public Suggestion(StringRange range, string text) : this(range, text, null)
		{
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public Suggestion(final com.mojang.brigadier.context.StringRange range, final String text, final com.mojang.brigadier.Message tooltip)
		public Suggestion(StringRange range, string text, Message tooltip)
		{
			this.range = range;
			this.text = text;
			this.tooltip = tooltip;
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public virtual string Text
		{
			get
			{
				return text;
			}
		}

		public virtual Message Tooltip
		{
			get
			{
				return tooltip;
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public String apply(final String input)
		public virtual string Apply(string input)
		{
			if (range.Start == 0 && range.End == input.Length)
			{
				return text;
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringBuilder result = new StringBuilder();
			StringBuilder result = new StringBuilder();
			if (range.Start > 0)
			{
				result.Append(input.Substring(0, range.Start));
			}
			result.Append(text);
			if (range.End < input.Length)
			{
				result.Append(input.Substring(range.End));
			}
			return result.ToString();
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Suggestion that = (Suggestion) o;
			Suggestion that = (Suggestion) o;
			return object.Equals(range, that.range) && object.Equals(text, that.text) && object.Equals(tooltip, that.tooltip);
		}

		public override string ToString()
		{
			return "Suggestion{" + "range=" + range + ", text='" + text + '\'' + ", tooltip='" + tooltip + '\'' + '}';
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public int compareTo(final Suggestion o)
		public int CompareTo(Suggestion o)
		{
			return String.Compare(text, o.text, StringComparison.Ordinal);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public int compareToIgnoreCase(final Suggestion b)
		public virtual int CompareToIgnoreCase(Suggestion b)
		{
			return string.Compare(text, b.text, StringComparison.OrdinalIgnoreCase);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public Suggestion expand(final String command, final com.mojang.brigadier.context.StringRange range)
		public virtual Suggestion Expand(string command, StringRange range)
		{
			if (range.Equals(this.range))
			{
				return this;
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringBuilder result = new StringBuilder();
			StringBuilder result = new StringBuilder();
			if (range.Start < this.range.Start)
			{
				result.Append(StringHelper.SubstringSpecial(command, range.Start, this.range.Start));
			}
			result.Append(text);
			if (range.End > this.range.End)
			{
				result.Append(StringHelper.SubstringSpecial(command, this.range.End, range.End));
			}
			return new Suggestion(range, result.ToString(), tooltip);
		}
	}

}