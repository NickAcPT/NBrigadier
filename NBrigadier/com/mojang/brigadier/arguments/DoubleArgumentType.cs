using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;


	public class DoubleArgumentType : ArgumentType<double>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

		private readonly double minimum;
		private readonly double maximum;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private DoubleArgumentType(final double minimum, final double maximum)
		private DoubleArgumentType(double minimum, double maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static DoubleArgumentType DoubleArg()
		{
			return DoubleArg(-double.MaxValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static DoubleArgumentType doubleArg(final double min)
		public static DoubleArgumentType DoubleArg(double min)
		{
			return DoubleArg(min, double.MaxValue);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static DoubleArgumentType doubleArg(final double min, final double max)
		public static DoubleArgumentType DoubleArg(double min, double max)
		{
			return new DoubleArgumentType(min, max);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static double getDouble(final com.mojang.brigadier.context.CommandContext<?> context, final String name)
		public static double GetDouble<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<double>(name, typeof(double));
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public System.Nullable<double> parse(final com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public double Parse(StringReader reader)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = reader.getCursor();
			int start = reader.Cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double result = reader.readDouble();
			double result = reader.ReadDouble();
			if (result < minimum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DoubleTooLow().CreateWithContext(reader, result, minimum);
			}
			if (result > maximum)
			{
				reader.Cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DoubleTooHigh().CreateWithContext(reader, result, maximum);
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
			if (!(o is DoubleArgumentType))
			{
				return false;
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final DoubleArgumentType that = (DoubleArgumentType) o;
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