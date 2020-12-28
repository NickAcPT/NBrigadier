using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.CommandSuggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
	using CommandSyntaxException = CommandSyntaxException;

	public class LongArgumentType : IArgumentType<long>
	{
		private static ICollection<string> _examples = CollectionsHelper.AsList("0", "123", "-123");

		private long _minimum;
		private long _maximum;

		private LongArgumentType(long minimum, long maximum)
		{
			this._minimum = minimum;
			this._maximum = maximum;
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
			return new LongArgumentType(min, max);
		}

		public static long GetLong<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<long>(name, typeof(long));
		}

		public virtual long Minimum
		{
			get
			{
				return _minimum;
			}
		}

		public virtual long Maximum
		{
			get
			{
				return _maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<long> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual long Parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 long result = reader.ReadLong();
			if (result < _minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.LongTooLow().CreateWithContext(reader, result, _minimum);
			}
			if (result > _maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.LongTooHigh().CreateWithContext(reader, result, _maximum);
			}
			return result;
		}

        public Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is LongArgumentType))
			{
				return false;
			}

			 LongArgumentType that = (LongArgumentType) o;
			return _maximum == that._maximum && _minimum == that._minimum;
		}

		public override int GetHashCode()
		{
			return 31 * ObjectsHelper.Hash(_minimum, _maximum);
		}

		public override string ToString()
		{
			if (_minimum == long.MinValue && _maximum == long.MaxValue)
			{
				return "longArg()";
			}
			else if (_maximum == long.MaxValue)
			{
				return "longArg(" + _minimum + ")";
			}
			else
			{
				return "longArg(" + _minimum + ", " + _maximum + ")";
			}
		}

		public virtual ICollection<string> Examples
		{
			get
			{
				return _examples;
			}
		}
	}

}