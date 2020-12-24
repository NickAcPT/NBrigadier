﻿using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class DoubleArgumentType : IArgumentType<double>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string>
            {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

        private readonly double _maximum;

        private readonly double _minimum;

        private DoubleArgumentType(double minimum, double maximum)
        {
            this._minimum = minimum;
            this._maximum = maximum;
        }

        public virtual double Minimum => _minimum;

        public virtual double Maximum => _maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public double Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadDouble();
            if (result < _minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BuiltInExceptions.DoubleTooLow()
                    .CreateWithContext(reader, result, _minimum);
            }

            if (result > _maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.BuiltInExceptions.DoubleTooHigh()
                    .CreateWithContext(reader, result, _maximum);
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
            return _maximum == that._maximum && _minimum == that._minimum;
        }

        public override int GetHashCode()
        {
            return (int) (31 * _minimum + _maximum);
        }

        public override string ToString()
        {
            if (_minimum == -double.MaxValue && _maximum == double.MaxValue)
                return "double()";
            if (_maximum == double.MaxValue)
                return "double(" + _minimum + ")";
            return "double(" + _minimum + ", " + _maximum + ")";
        }
    }
}