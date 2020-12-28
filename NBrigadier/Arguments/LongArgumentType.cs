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

	public class LongArgumentType : ArgumentType<long>
	{
		private static ICollection<string> EXAMPLES = CollectionsHelper.AsList("0", "123", "-123");

		private long minimum;
		private long maximum;

		private LongArgumentType(long minimum, long maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static LongArgumentType longArg()
		{
			return longArg(long.MinValue);
		}

		public static LongArgumentType longArg(long min)
		{
			return longArg(min, long.MaxValue);
		}

		public static LongArgumentType longArg(long min, long max)
		{
			return new LongArgumentType(min, max);
		}

		public static long getLong<T1>(com.mojang.brigadier.context.CommandContext<T1> context, string name)
		{
			return context.getArgument<long>(name, typeof(long));
		}

		public virtual long Minimum
		{
			get
			{
				return minimum;
			}
		}

		public virtual long Maximum
		{
			get
			{
				return maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<long> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual long parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 long result = reader.readLong();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.longTooLow().createWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.longTooHigh().createWithContext(reader, result, maximum);
			}
			return result;
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
			return maximum == that.maximum && minimum == that.minimum;
		}

		public override int GetHashCode()
		{
			return 31 * ObjectsHelper.hash(minimum, maximum);
		}

		public override string ToString()
		{
			if (minimum == long.MinValue && maximum == long.MaxValue)
			{
				return "longArg()";
			}
			else if (maximum == long.MaxValue)
			{
				return "longArg(" + minimum + ")";
			}
			else
			{
				return "longArg(" + minimum + ", " + maximum + ")";
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