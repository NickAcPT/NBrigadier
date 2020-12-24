using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

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
            this._minimum = minimum;
            this._maximum = maximum;
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
            if (this == o) return true;
            if (!(o is LongArgumentType)) return false;

            var that = (LongArgumentType) o;
            return _maximum == that._maximum && _minimum == that._minimum;
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