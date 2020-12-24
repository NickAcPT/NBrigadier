// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{

	public class ParsedArgument<S, T>
	{
		private readonly StringRange range;
		private readonly T result;

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
			var that = o as ParsedArgument<S, T>;
			return object.Equals(range, that.range) && object.Equals(result, that.result);
		}
    }

}