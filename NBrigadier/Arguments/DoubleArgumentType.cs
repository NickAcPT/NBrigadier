using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class DoubleArgumentType : ArgumentType<double>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string>
            {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

        private readonly double maximum;

        private readonly double minimum;

        private DoubleArgumentType(double minimum, double maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }

        public virtual double Minimum => minimum;

        public virtual double Maximum => maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public double Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadDouble();
            if (result < minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DoubleTooLow()
                    .CreateWithContext(reader, result, minimum);
            }

            if (result > maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DoubleTooHigh()
                    .CreateWithContext(reader, result, maximum);
            }

            return result;
        }

        public static DoubleArgumentType DoubleArg()
        {
            return DoubleArg(-double.MaxValue);
        }

        public static DoubleArgumentType DoubleArg(double min)
        {
            return DoubleArg(min, double.MaxValue);
        }

        public static DoubleArgumentType DoubleArg(double min, double max)
        {
            return new(min, max);
        }

        public static double GetDouble<T1>(CommandContext<T1> context, string name)
        {
            return context.GetArgument<double>(name, typeof(double));
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is DoubleArgumentType)) return false;

            var that = (DoubleArgumentType) o;
            return maximum == that.maximum && minimum == that.minimum;
        }

        public override int GetHashCode()
        {
            return (int) (31 * minimum + maximum);
        }

        public override string ToString()
        {
            if (minimum == -double.MaxValue && maximum == double.MaxValue)
                return "double()";
            if (maximum == double.MaxValue)
                return "double(" + minimum + ")";
            return "double(" + minimum + ", " + maximum + ")";
        }
    }
}