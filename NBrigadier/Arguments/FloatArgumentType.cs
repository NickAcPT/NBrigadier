using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class FloatArgumentType : IArgumentType<float>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string>
            {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

        private readonly float _maximum;

        private readonly float _minimum;

        private FloatArgumentType(float minimum, float maximum)
        {
            this._minimum = minimum;
            this._maximum = maximum;
        }

        public virtual float Minimum => _minimum;

        public virtual float Maximum => _maximum;

        public virtual ICollection<string> Examples => EXAMPLES;

        public float Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadFloat();
            if (result < _minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.builtInExceptions.FloatTooLow()
                    .CreateWithContext(reader, result, _minimum);
            }

            if (result > _maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.builtInExceptions.FloatTooHigh()
                    .CreateWithContext(reader, result, _maximum);
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
            return _maximum == that._maximum && _minimum == that._minimum;
        }

        public override int GetHashCode()
        {
            return (int) (31 * _minimum + _maximum);
        }

        public override string ToString()
        {
            if (_minimum == -float.MaxValue && _maximum == float.MaxValue)
                return "float()";
            if (_maximum == float.MaxValue)
                return "float(" + _minimum + ")";
            return "float(" + _minimum + ", " + _maximum + ")";
        }
    }
}