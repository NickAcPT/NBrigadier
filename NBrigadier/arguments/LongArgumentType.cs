using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class LongArgumentType : ArgumentType<long>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "123", "-123"};
        private readonly long maximum;

        private readonly long minimum;

        private LongArgumentType(long minimum, long maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public virtual long Minimum => minimum;

        public virtual long Maximum => maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public long Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadLong();
            if (result < minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LongTooLow()
                    .CreateWithContext(reader, result, minimum);
            }

            if (result > maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LongTooHigh()
                    .CreateWithContext(reader, result, maximum);
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
            return maximum == that.maximum && minimum == that.minimum;
        }


        public override string ToString()
        {
            if (minimum == long.MinValue && maximum == long.MaxValue)
                return "longArg()";
            if (maximum == long.MaxValue)
                return "longArg(" + minimum + ")";
            return "longArg(" + minimum + ", " + maximum + ")";
        }
    }
}