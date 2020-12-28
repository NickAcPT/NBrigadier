using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
	using CommandSyntaxException = CommandSyntaxException;

	public class FloatArgumentType : IArgumentType<float>
	{
		private static ICollection<string> _examples = CollectionsHelper.AsList("0", "1.2", ".5", "-1", "-.5", "-1234.56");

		private float _minimum;
		private float _maximum;

		private FloatArgumentType(float minimum, float maximum)
		{
			this._minimum = minimum;
			this._maximum = maximum;
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
			return new FloatArgumentType(min, max);
		}

		public static float GetFloat<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<float>(name, typeof(float));
		}

		public virtual float Minimum
		{
			get
			{
				return _minimum;
			}
		}

		public virtual float Maximum
		{
			get
			{
				return _maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<float> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual float Parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 float result = reader.ReadFloat();
			if (result < _minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.FloatTooLow().CreateWithContext(reader, result, _minimum);
			}
			if (result > _maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.FloatTooHigh().CreateWithContext(reader, result, _maximum);
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
			if (!(o is FloatArgumentType))
			{
				return false;
			}

			 FloatArgumentType that = (FloatArgumentType) o;
			return _maximum == that._maximum && _minimum == that._minimum;
		}

		public override int GetHashCode()
		{
			return (int)(31 * _minimum + _maximum);
		}

		public override string ToString()
		{
			if (_minimum == -float.MaxValue && _maximum == float.MaxValue)
			{
				return "float()";
			}
			else if (_maximum == float.MaxValue)
			{
				return "float(" + _minimum + ")";
			}
			else
			{
				return "float(" + _minimum + ", " + _maximum + ")";
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