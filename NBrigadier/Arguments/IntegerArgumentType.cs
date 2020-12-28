using System;
using System.Collections.Generic;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class IntegerArgumentType : IArgumentType<int>
    {
        private static readonly ICollection<string> _examples = CollectionsHelper.AsList("0", "123", "-123");
        private readonly int _maximum;

        private readonly int _minimum;

        private IntegerArgumentType(int minimum, int maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public virtual int Minimum => _minimum;

        public virtual int Maximum => _maximum;

                public virtual int Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadInt();
            if (result < _minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.builtInExceptions.IntegerTooLow()
                    .CreateWithContext(reader, result, _minimum);
            }

            if (result > _maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.builtInExceptions.IntegerTooHigh()
                    .CreateWithContext(reader, result, _maximum);
            }

            return result;
        }

        public Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public virtual ICollection<string> Examples => _examples;

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
            return _maximum == that._maximum && _minimum == that._minimum;
        }

        public override int GetHashCode()
        {
            return 31 * _minimum + _maximum;
        }

        public override string ToString()
        {
            if (_minimum == int.MinValue && _maximum == int.MaxValue)
                return "integer()";
            if (_maximum == int.MaxValue)
                return "integer(" + _minimum + ")";
            return "integer(" + _minimum + ", " + _maximum + ")";
        }
    }
}