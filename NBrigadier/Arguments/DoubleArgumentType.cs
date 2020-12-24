using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
    using CommandSyntaxException = CommandSyntaxException;


	public class DoubleArgumentType : ArgumentType<double>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string> {"0", "1.2", ".5", "-1", "-.5", "-1234.56"};

		private readonly double minimum;
		private readonly double maximum;

		private DoubleArgumentType(double minimum, double maximum)
		{
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public static DoubleArgumentType DoubleArg()
		{
			return DoubleArg(-double.MaxValue);
		}

		public static DoubleArgumentType DoubleArg(double min)
		{
			return DoubleArg(min, double.MaxValue);
		}

		public static DoubleArgumentType DoubleArg(double min, double max)
		{
			return new DoubleArgumentType(min, max);
		}

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

		public double Parse(StringReader reader)
		{
			int start = reader.Cursor;
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