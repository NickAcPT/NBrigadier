using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;


	public class IntegerArgumentType : ArgumentType<int>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "123", "-123"};

		private readonly int minimum;
		private readonly int maximum;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private IntegerArgumentType(final int minimum, final int maximum)
		private IntegerArgumentType(int minimum, int maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static IntegerArgumentType Integer()
		{
			return Integer(int.MinValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static IntegerArgumentType integer(final int min)
		public static IntegerArgumentType Integer(int min)
		{
			return Integer(min, int.MaxValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static IntegerArgumentType integer(final int min, final int max)
		public static IntegerArgumentType Integer(int min, int max)
		{
			return new IntegerArgumentType(min, max);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static int getInteger(final com.mojang.brigadier.context.CommandContext<?> context, final String name)
		public static int GetInteger<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<int>(name, typeof(int));
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public System.Nullable<int> parse(final com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public int Parse(StringReader reader)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = reader.getCursor();
			int start = reader.Cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int result = reader.readInt();
			int result = reader.ReadInt();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.IntegerTooLow().CreateWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.IntegerTooHigh().CreateWithContext(reader, result, maximum);
			}
			return result;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final IntegerArgumentType that = (IntegerArgumentType) o;
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