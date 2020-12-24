using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class LongArgumentType : IArgumentType<long>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "123", "-123"};
        private readonly long _maximum;

        private readonly long _minimum;

        private LongArgumentType(long minimum, long maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public virtual long Minimum => _minimum;

        public virtual long Maximum => _maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public long Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadLong();
            if (result < _minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BuiltInExceptions.LongTooLow()
                    .CreateWithContext(reader, result, _minimum);
            }

            if (result > _maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BuiltInExceptions.LongTooHigh()
                    .CreateWithContext(reader, result, _maximum);
            }

            return result;
        }

        public Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public IList<string> GetExamples()
        {
            return new List<string>();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_maximum.GetHashCode() * 397) ^ _minimum.GetHashCode();
            }
        }

        protected bool Equals(LongArgumentType other)
        {
            return _maximum == other._maximum && _minimum == other._minimum;
        }

        public static bool operator ==(LongArgumentType left, LongArgumentType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LongArgumentType left, LongArgumentType right)
        {
            return !Equals(left, right);
        }

        public static LongArgumentType LongArg()
        {
            return LongArg(long.MinValue);
        }

        public static LongArgumentType LongArg(long min)
        {
            return LongArg(min, long.MaxValue);
        }

        public static LongArgumentType LongArg(long min, long max)
        {
            return new(min, max);
        }

        public static long GetLong<T1>(CommandContext<T1> context, string name)
        {
            return context.GetArgument<long>(name, typeof(long));
        }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != GetType()) return false;
            return Equals((LongArgumentType) o);
        }


        public override string ToString()
        {
            if (_minimum == long.MinValue && _maximum == long.MaxValue)
                return "longArg()";
            if (_maximum == long.MaxValue)
                return "longArg(" + _minimum + ")";
            return "longArg(" + _minimum + ", " + _maximum + ")";
        }
    }
}