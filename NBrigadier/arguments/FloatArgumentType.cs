using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class FloatArgumentType : ArgumentType<float>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string>
            {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

        private readonly float maximum;

        private readonly float minimum;

        private FloatArgumentType(float minimum, float maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public virtual float Minimum => minimum;

        public virtual float Maximum => maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public float Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadFloat();
            if (result < minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.FloatTooLow()
                    .CreateWithContext(reader, result, minimum);
            }

            if (result > maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.FloatTooHigh()
                    .CreateWithContext(reader, result, maximum);
            }

            return result;
        }

        public static FloatArgumentType FloatArg()
        {
            return FloatArg(-float.MaxValue);
        }

        public static FloatArgumentType FloatArg(float min)
        {
            return FloatArg(min, float.MaxValue);
        }

        public static FloatArgumentType FloatArg(float min, float max)
        {
            return new(min, max);
        }

        public static float GetFloat<T1>(CommandContext<T1> context, string name)
        {
            return context.GetArgument<float>(name, typeof(float));
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is FloatArgumentType)) return false;

            var that = (FloatArgumentType) o;
            return maximum == that.maximum && minimum == that.minimum;
        }

        public override int GetHashCode()
        {
            return (int) (31 * minimum + maximum);
        }

        public override string ToString()
        {
            if (minimum == -float.MaxValue && maximum == float.MaxValue)
                return "float()";
            if (maximum == float.MaxValue)
                return "float(" + minimum + ")";
            return "float(" + minimum + ", " + maximum + ")";
        }
    }
}