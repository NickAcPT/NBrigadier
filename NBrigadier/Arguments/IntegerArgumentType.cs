using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
	using CommandSyntaxException = CommandSyntaxException;

	public class IntegerArgumentType : IArgumentType<int>
	{
		private static ICollection<string> _examples = CollectionsHelper.AsList("0", "123", "-123");

		private int _minimum;
		private int _maximum;

		private IntegerArgumentType(int minimum, int maximum)
		{
			this._minimum = minimum;
			this._maximum = maximum;
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
			return new IntegerArgumentType(min, max);
		}

		public static int GetInteger<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<int>(name, typeof(int));
		}

		public virtual int Minimum
		{
			get
			{
				return _minimum;
			}
		}

		public virtual int Maximum
		{
			get
			{
				return _maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<int> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int Parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 int result = reader.ReadInt();
			if (result < _minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.IntegerTooLow().CreateWithContext(reader, result, _minimum);
			}
			if (result > _maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.builtInExceptions.IntegerTooHigh().CreateWithContext(reader, result, _maximum);
			}
			return result;
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is IntegerArgumentType))
			{
				return false;
			}

			 IntegerArgumentType that = (IntegerArgumentType) o;
			return _maximum == that._maximum && _minimum == that._minimum;
		}

		public override int GetHashCode()
		{
			return 31 * _minimum + _maximum;
		}

		public override string ToString()
		{
			if (_minimum == int.MinValue && _maximum == int.MaxValue)
			{
				return "integer()";
			}
			else if (_maximum == int.MaxValue)
			{
				return "integer(" + _minimum + ")";
			}
			else
			{
				return "integer(" + _minimum + ", " + _maximum + ")";
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