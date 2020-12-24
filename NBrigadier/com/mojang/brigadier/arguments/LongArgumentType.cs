using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;


	public class LongArgumentType : ArgumentType<long>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "123", "-123"};

		private readonly long minimum;
		private readonly long maximum;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private LongArgumentType(final long minimum, final long maximum)
		private LongArgumentType(long minimum, long maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static LongArgumentType LongArg()
		{
			return LongArg(long.MinValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static LongArgumentType longArg(final long min)
		public static LongArgumentType LongArg(long min)
		{
			return LongArg(min, long.MaxValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static LongArgumentType longArg(final long min, final long max)
		public static LongArgumentType LongArg(long min, long max)
		{
			return new LongArgumentType(min, max);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static long getLong(final com.mojang.brigadier.context.CommandContext<?> context, final String name)
		public static long GetLong<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<long>(name, typeof(long));
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public System.Nullable<long> parse(final com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public long Parse(StringReader reader)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = reader.getCursor();
			int start = reader.Cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final long result = reader.readLong();
			long result = reader.ReadLong();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LongTooLow().CreateWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LongTooHigh().CreateWithContext(reader, result, maximum);
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
			if (!(o is LongArgumentType))
			{
				return false;
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LongArgumentType that = (LongArgumentType) o;
			LongArgumentType that = (LongArgumentType) o;
			return maximum == that.maximum && minimum == that.minimum;
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