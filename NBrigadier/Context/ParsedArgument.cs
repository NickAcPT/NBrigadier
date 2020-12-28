using NBrigadier.Generics;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{

	public class ParsedArgument<S, T> : IParsedArgument
	{
		private StringRange range;
		private T result;

		public ParsedArgument(int start, int end, T result)
		{
			this.range = StringRange.between(start, end);
			this.result = result;
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

        public object ResultObject => Result;

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
			if (!(o is IParsedArgument))
			{
				return false;
			}
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: ParsedArgument<?, ?> that = (ParsedArgument<?, ?>) o;
			 ParsedArgument<object, object> that = (ParsedArgument<object, object>) o;
			return ObjectsHelper.Equals(range, that.range) && ObjectsHelper.Equals(result, that.result);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.hash(range, result);
		}
	}

}