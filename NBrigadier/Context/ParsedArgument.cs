// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;

namespace NBrigadier.Context
{
    public class ParsedArgument<TS, T>
    {
        private readonly StringRange _range;
        private readonly T _result;

        public ParsedArgument(int start, int end, T result)
        {
            _range = StringRange.Between(start, end);
            _result = result;
        }

        public virtual StringRange Range => _range;

        public virtual T Result => _result;

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_range != null ? _range.GetHashCode() : 0) * 397) ^
                       EqualityComparer<T>.Default.GetHashCode(_result);
            }
        }

        protected bool Equals(ParsedArgument<TS, T> other)
        {
            return Equals(_range, other._range) && EqualityComparer<T>.Default.Equals(_result, other._result);
        }

        public static bool operator ==(ParsedArgument<TS, T> left, ParsedArgument<TS, T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ParsedArgument<TS, T> left, ParsedArgument<TS, T> right)
        {
            return !Equals(left, right);
        }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != GetType()) return false;
            return Equals((ParsedArgument<TS, T>) o);
        }
    }
}