// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace NBrigadier.Context
{
    public class ParsedArgument<TS, T>
    {
        protected bool Equals(ParsedArgument<TS, T> other)
        {
            return Equals(_range, other._range) && EqualityComparer<T>.Default.Equals(_result, other._result);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_range, _result);
        }

        public static bool operator ==(ParsedArgument<TS, T> left, ParsedArgument<TS, T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ParsedArgument<TS, T> left, ParsedArgument<TS, T> right)
        {
            return !Equals(left, right);
        }

        private readonly StringRange _range;
        private readonly T _result;

        public ParsedArgument(int start, int end, T result)
        {
            _range = StringRange.Between(start, end);
            this._result = result;
        }

        public virtual StringRange Range => _range;

        public virtual T Result => _result;

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is ParsedArgument<TS, T>)) return false;
            var that = o as ParsedArgument<TS, T>;
            return Equals(_range, that._range) && Equals(_result, that._result);
        }
    }
}