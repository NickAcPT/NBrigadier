using System;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class StringRange
	{
		private int _start;
		private int _end;

		public StringRange(int start, int end)
		{
			this._start = start;
			this._end = end;
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
				return _start;
			}
		}

		public virtual int End
		{
			get
			{
				return _end;
			}
		}

		public virtual string Get(IMmutableStringReader reader)
		{
			return reader.String.Substring(_start, _end - _start);
		}

		public virtual string Get(string @string)
		{
			return @string.Substring(_start, _end - _start);
		}

		public virtual bool Empty
		{
			get
			{
				return _start == _end;
			}
		}

		public virtual int Length
		{
			get
			{
				return _end - _start;
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
			return _start == that._start && _end == that._end;
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.Hash(_start, _end);
		}

		public override string ToString()
		{
			return "StringRange{" + "start=" + _start + ", end=" + _end + '}';
		}
	}

}