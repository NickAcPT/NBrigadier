using System;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class StringRange
    {
        private readonly int end;
        private readonly int start;

        public StringRange(int start, int end)
        {
            this.start = start;
            this.end = end;
        }

        public virtual int Start => start;

        public virtual int End => end;

        public virtual bool Empty => start == end;

        public virtual int Length => end - start;

        public static StringRange At(int pos)
        {
            return new(pos, pos);
        }

        public static StringRange Between(int start, int end)
        {
            return new(start, end);
        }

        public static StringRange Encompassing(StringRange a, StringRange b)
        {
            return new(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));
        }

        public virtual string Get(ImmutableStringReader reader)
        {
            return reader.String.Substring(start, end - start);
        }

        public virtual string Get(string @string)
        {
            return @string.Substring(start, end - start);
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is StringRange)) return false;
            var that = (StringRange) o;
            return start == that.start && end == that.end;
        }

        public override string ToString()
        {
            return "StringRange{" + "start=" + start + ", end=" + end + '}';
        }
    }
}