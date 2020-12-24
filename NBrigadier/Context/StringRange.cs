﻿using System;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
	using ImmutableStringReader = ImmutableStringReader;

	public class StringRange
	{
		private readonly int start;
		private readonly int end;

		public StringRange(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		public static StringRange At(int pos)
		{
			return new StringRange(pos, pos);
		}

		public static StringRange Between(int start, int end)
		{
			return new StringRange(start, end);
		}

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

		public virtual string Get(ImmutableStringReader reader)
		{
			return reader.String.Substring(start, end - start);
		}

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
			StringRange that = (StringRange) o;
			return start == that.start && end == that.end;
		}

		public override string ToString()
		{
			return "StringRange{" + "start=" + start + ", end=" + end + '}';
		}
	}

}