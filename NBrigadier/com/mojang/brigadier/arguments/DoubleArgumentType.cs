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

	public class DoubleArgumentType : ArgumentType<double>
	{
		private static ICollection<string> EXAMPLES = CollectionsHelper.AsList("0", "1.2", ".5", "-1", "-.5", "-1234.56");

		private double minimum;
		private double maximum;

		private DoubleArgumentType(double minimum, double maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static DoubleArgumentType doubleArg()
		{
			return doubleArg(-double.MaxValue);
		}

		public static DoubleArgumentType doubleArg(double min)
		{
			return doubleArg(min, double.MaxValue);
		}

		public static DoubleArgumentType doubleArg(double min, double max)
		{
			return new DoubleArgumentType(min, max);
		}

		public static double getDouble<T1>(com.mojang.brigadier.context.CommandContext<T1> context, string name)
		{
			return context.getArgument(name, typeof(double));
		}

		public virtual double Minimum
		{
			get
			{
				return minimum;
			}
		}

		public virtual double Maximum
		{
			get
			{
				return maximum;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<double> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual double parse(StringReader reader)
		{
			 int start = reader.Cursor;
			 double result = reader.readDouble();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.doubleTooLow().createWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.doubleTooHigh().createWithContext(reader, result, maximum);
			}
			return result;
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
			return maximum == that.maximum && minimum == that.minimum;
		}

		public override int GetHashCode()
		{
			return (int)(31 * minimum + maximum);
		}

		public override string ToString()
		{
			if (minimum == -double.MaxValue && maximum == double.MaxValue)
			{
				return "double()";
			}
			else if (maximum == double.MaxValue)
			{
				return "double(" + minimum + ")";
			}
			else
			{
				return "double(" + minimum + ", " + maximum + ")";
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