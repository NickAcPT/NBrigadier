// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.context
{

	public class ParsedArgument<S, T>
	{
		private readonly StringRange range;
		private readonly T result;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public ParsedArgument(final int start, final int end, final T result)
		public ParsedArgument(int start, int end, T result)
		{
			this.range = StringRange.Between(start, end);
			this.result = result;
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public virtual T Result
		{
			get
			{
				return result;
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is ParsedArgument<S, T>))
			{
				return false;
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParsedArgument<?, ?> that = (ParsedArgument<?, ?>) o;
//WARNING: Java wildcard generics have no direct equivalent in C#:
			var that = o as ParsedArgument<S, T>;
			return object.Equals(range, that.range) && object.Equals(result, that.result);
		}
    }

}