using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;


	public class FloatArgumentType : ArgumentType<float>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

		private readonly float minimum;
		private readonly float maximum;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private FloatArgumentType(final float minimum, final float maximum)
		private FloatArgumentType(float minimum, float maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static FloatArgumentType FloatArg()
		{
			return FloatArg(-float.MaxValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static FloatArgumentType floatArg(final float min)
		public static FloatArgumentType FloatArg(float min)
		{
			return FloatArg(min, float.MaxValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static FloatArgumentType floatArg(final float min, final float max)
		public static FloatArgumentType FloatArg(float min, float max)
		{
			return new FloatArgumentType(min, max);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static float getFloat(final com.mojang.brigadier.context.CommandContext<?> context, final String name)
		public static float GetFloat<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<float>(name, typeof(float));
		}

		public virtual float Minimum
		{
			get
			{
				return minimum;
			}
		}

		public virtual float Maximum
		{
			get
			{
				return maximum;
			}
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public System.Nullable<float> parse(final com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public float Parse(StringReader reader)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = reader.getCursor();
			int start = reader.Cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final float result = reader.readFloat();
			float result = reader.ReadFloat();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.FloatTooLow().CreateWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.FloatTooHigh().CreateWithContext(reader, result, maximum);
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
			if (!(o is FloatArgumentType))
			{
				return false;
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final FloatArgumentType that = (FloatArgumentType) o;
			FloatArgumentType that = (FloatArgumentType) o;
			return maximum == that.maximum && minimum == that.minimum;
		}

		public override int GetHashCode()
		{
			return (int)(31 * minimum + maximum);
		}

		public override string ToString()
		{
			if (minimum == -float.MaxValue && maximum == float.MaxValue)
			{
				return "float()";
			}
			else if (maximum == float.MaxValue)
			{
				return "float(" + minimum + ")";
			}
			else
			{
				return "float(" + minimum + ", " + maximum + ")";
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