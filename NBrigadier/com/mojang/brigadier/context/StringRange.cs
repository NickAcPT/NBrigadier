using System;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.context
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;

	public class StringRange
	{
		private readonly int start;
		private readonly int end;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public StringRange(final int start, final int end)
		public StringRange(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static StringRange at(final int pos)
		public static StringRange At(int pos)
		{
			return new StringRange(pos, pos);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static StringRange between(final int start, final int end)
		public static StringRange Between(int start, int end)
		{
			return new StringRange(start, end);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static StringRange encompassing(final StringRange a, final StringRange b)
		public static StringRange Encompassing(StringRange a, StringRange b)
		{
			return new StringRange(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));
		}

		public virtual int Start
		{
			get
			{
				return start;
			}
		}

		public virtual int End
		{
			get
			{
				return end;
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public String get(final com.mojang.brigadier.ImmutableStringReader reader)
		public virtual string Get(ImmutableStringReader reader)
		{
			return reader.String.Substring(start, end - start);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public String get(final String string)
		public virtual string Get(string @string)
		{
			return @string.Substring(start, end - start);
		}

		public virtual bool Empty
		{
			get
			{
				return start == end;
			}
		}

		public virtual int Length
		{
			get
			{
				return end - start;
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
			if (!(o is StringRange))
			{
				return false;
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringRange that = (StringRange) o;
			StringRange that = (StringRange) o;
			return start == that.start && end == that.end;
		}

		public override string ToString()
		{
			return "StringRange{" + "start=" + start + ", end=" + end + '}';
		}
	}

}