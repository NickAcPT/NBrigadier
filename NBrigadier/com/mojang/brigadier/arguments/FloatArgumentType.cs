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

	public class FloatArgumentType : ArgumentType<float>
	{
		private static ICollection<string> EXAMPLES = CollectionsHelper.AsList("0", "1.2", ".5", "-1", "-.5", "-1234.56");

		private float minimum;
		private float maximum;

		private FloatArgumentType(float minimum, float maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static FloatArgumentType floatArg()
		{
			return floatArg(-float.MaxValue);
		}

		public static FloatArgumentType floatArg(float min)
		{
			return floatArg(min, float.MaxValue);
		}

		public static FloatArgumentType floatArg(float min, float max)
		{
			return new FloatArgumentType(min, max);
		}

		public static float getFloat<T1>(com.mojang.brigadier.context.CommandContext<T1> context, string name)
		{
			return context.getArgument(name, typeof(float));
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

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<float> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual float parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 float result = reader.readFloat();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.floatTooLow().createWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.floatTooHigh().createWithContext(reader, result, maximum);
			}
			return result;
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