using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
    using CommandSyntaxException = CommandSyntaxException;


	public class IntegerArgumentType : ArgumentType<int>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "123", "-123"};

		private readonly int minimum;
		private readonly int maximum;

		private IntegerArgumentType(int minimum, int maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
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

		public int Parse(StringReader reader)
		{
			int start = reader.Cursor;
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