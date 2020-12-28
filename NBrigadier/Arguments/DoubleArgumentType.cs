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

	public class DoubleArgumentType : IArgumentType<double>
	{
		private static ICollection<string> _examples = CollectionsHelper.AsList("0", "1.2", ".5", "-1", "-.5", "-1234.56");

		private double _minimum;
		private double _maximum;

		private DoubleArgumentType(double minimum, double maximum)
		{
			this._minimum = minimum;
			this._maximum = maximum;
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
			return new DoubleArgumentType(min, max);
		}

		public static double GetDouble<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<double>(name, typeof(double));
		}

		public virtual double Minimum
		{
			get
			{
				return _minimum;
			}
		}

		public virtual double Maximum
		{
			get
			{
				return _maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<double> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual double Parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 double result = reader.ReadDouble();
			if (result < _minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.DoubleTooLow().CreateWithContext(reader, result, _minimum);
			}
			if (result > _maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.DoubleTooHigh().CreateWithContext(reader, result, _maximum);
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
			if (!(o is DoubleArgumentType))
			{
				return false;
			}

			 DoubleArgumentType that = (DoubleArgumentType) o;
			return _maximum == that._maximum && _minimum == that._minimum;
		}

		public override int GetHashCode()
		{
			return (int)(31 * _minimum + _maximum);
		}

		public override string ToString()
		{
			if (_minimum == -double.MaxValue && _maximum == double.MaxValue)
			{
				return "double()";
			}
			else if (_maximum == double.MaxValue)
			{
				return "double(" + _minimum + ")";
			}
			else
			{
				return "double(" + _minimum + ", " + _maximum + ")";
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