using System;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using Message = com.mojang.brigadier.Message;
	using StringRange = com.mojang.brigadier.context.StringRange;

	public class IntegerSuggestion : Suggestion
	{
		private int value;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public IntegerSuggestion(final com.mojang.brigadier.context.StringRange range, final int value)
		public IntegerSuggestion(StringRange range, int value) : this(range, value, null)
		{
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public IntegerSuggestion(final com.mojang.brigadier.context.StringRange range, final int value, final com.mojang.brigadier.Message tooltip)
		public IntegerSuggestion(StringRange range, int value, Message tooltip) : base(range, Convert.ToString(value), tooltip)
		{
			this.value = value;
		}

		public virtual int Value
		{
			get
			{
				return value;
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is IntegerSuggestion))
			{
				return false;
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final IntegerSuggestion that = (IntegerSuggestion) o;
			IntegerSuggestion that = (IntegerSuggestion) o;
			return value == that.value && base.Equals(o);
		}

		public override string ToString()
		{
			return "IntegerSuggestion{" + "value=" + value + ", range=" + Range + ", text='" + Text + '\'' + ", tooltip='" + Tooltip + '\'' + '}';
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public int compareTo(final Suggestion o)
		public int CompareTo(Suggestion o)
		{
			if (o is IntegerSuggestion)
			{
				return value.CompareTo(((IntegerSuggestion) o).value);
			}
			return base.CompareTo(o);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public int compareToIgnoreCase(final Suggestion b)
		public override int CompareToIgnoreCase(Suggestion b)
		{
			return CompareTo(b);
		}
	}

}