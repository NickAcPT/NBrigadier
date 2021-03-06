﻿using System;
using System.Collections.Generic;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class LongArgumentType : IArgumentType<long>
    {
        private static readonly ICollection<string> _examples = CollectionsHelper.AsList("0", "123", "-123");
        private readonly long _maximum;

        private readonly long _minimum;

        private LongArgumentType(long minimum, long maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }

        public virtual long Minimum => _minimum;

        public virtual long Maximum => _maximum;

        public virtual long Parse(StringReader reader)
        {
            var start = reader.Cursor;
            var result = reader.ReadLong();
            if (result < _minimum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.builtInExceptions.LongTooLow().CreateWithContext(reader, result, _minimum);
            }

            if (result > _maximum)
            {
                reader.Cursor = start;
                throw CommandSyntaxException.builtInExceptions.LongTooHigh()
                    .CreateWithContext(reader, result, _maximum);
            }

            return result;
        }

        public Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public virtual ICollection<string> Examples => _examples;

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

        public override int GetHashCode()
        {
            return 31 * ObjectsHelper.Hash(_minimum, _maximum);
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