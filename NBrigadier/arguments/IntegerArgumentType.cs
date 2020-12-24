using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class IntegerArgumentType : ArgumentType<int>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "123", "-123"};
        private readonly int maximum;

        private readonly int minimum;

        private IntegerArgumentType(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public virtual int Minimum => minimum;

        public virtual int Maximum => maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public int Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadInt();
            if (result < minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.IntegerTooLow()
                    .CreateWithContext(reader, result, minimum);
            }

            if (result > maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.IntegerTooHigh()
                    .CreateWithContext(reader, result, maximum);
            }

            return result;
        }

        public static IntegerArgumentType Integer()
        {
            return Integer(int.MinValue);
        }

        public static IntegerArgumentType Integer(int min)
        {
            return Integer(min, int.MaxValue);
        }

        public static IntegerArgumentType Integer(int min, int max)
        {
            return new(min, max);
        }

        public static int GetInteger<T1>(CommandContext<T1> context, string name)
        {
            return context.GetArgument<int>(name, typeof(int));
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is IntegerArgumentType)) return false;

            var that = (IntegerArgumentType) o;
            return maximum == that.maximum && minimum == that.minimum;
        }

        public override int GetHashCode()
        {
            return 31 * minimum + maximum;
        }

        public override string ToString()
        {
            if (minimum == int.MinValue && maximum == int.MaxValue)
                return "integer()";
            if (maximum == int.MaxValue)
                return "integer(" + minimum + ")";
            return "integer(" + minimum + ", " + maximum + ")";
        }
    }
}