using System;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class StringRange
    {
        private readonly int _end;
        private readonly int _start;

        public StringRange(int start, int end)
        {
            _start = start;
            _end = end;
        }

        public virtual int Start => _start;

        public virtual int End => _end;

        public virtual bool Empty => _start == _end;

        public virtual int Length => _end - _start;

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

        public virtual string Get(IMmutableStringReader reader)
        {
            return reader.String.Substring(_start, _end - _start);
        }

        public virtual string Get(string @string)
        {
            return @string.Substring(_start, _end - _start);
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is StringRange)) return false;
            var that = (StringRange) o;
            return _start == that._start && _end == that._end;
        }

        public override int GetHashCode()
        {
            return ObjectsHelper.Hash(_start, _end);
        }

        public override string ToString()
        {
            return "StringRange{" + "start=" + _start + ", end=" + _end + '}';
        }
    }
}