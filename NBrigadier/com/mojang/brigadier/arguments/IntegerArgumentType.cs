using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public class IntegerArgumentType : ArgumentType<int>
	{
		private static ICollection<string> EXAMPLES = CollectionsHelper.AsList("0", "123", "-123");

		private int minimum;
		private int maximum;

		private IntegerArgumentType(int minimum, int maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static IntegerArgumentType integer()
		{
			return integer(int.MinValue);
		}

		public static IntegerArgumentType integer(int min)
		{
			return integer(min, int.MaxValue);
		}

		public static IntegerArgumentType integer(int min, int max)
		{
			return new IntegerArgumentType(min, max);
		}

		public static int getInteger<T1>(com.mojang.brigadier.context.CommandContext<T1> context, string name)
		{
			return context.getArgument(name, typeof(int));
		}

		public virtual int Minimum
		{
			get
			{
				return minimum;
			}
		}

		public virtual int Maximum
		{
			get
			{
				return maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<int> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 int result = reader.readInt();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.integerTooLow().createWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.integerTooHigh().createWithContext(reader, result, maximum);
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
			return maximum == that.maximum && minimum == that.minimum;
		}

		public override int GetHashCode()
		{
			return 31 * minimum + maximum;
		}

		public override string ToString()
		{
			if (minimum == int.MinValue && maximum == int.MaxValue)
			{
				return "integer()";
			}
			else if (maximum == int.MaxValue)
			{
				return "integer(" + minimum + ")";
			}
			else
			{
				return "integer(" + minimum + ", " + maximum + ")";
			}
		}

		public virtual ICollection<string> Examples
		{
			get
			{
				return EXAMPLES;
			}
		}
	}

}