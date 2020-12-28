using NBrigadier.Generics;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{

	public class ParsedArgument<TS, T> : IParsedArgument
	{
		private StringRange _range;
		private T _result;

		public ParsedArgument(int start, int end, T result)
		{
			this._range = StringRange.Between(start, end);
			this._result = result;
		}

		public virtual StringRange Range
		{
			get
			{
				return _range;
			}
		}

        public object ResultObject => Result;

        public virtual T Result
		{
			get
			{
				return _result;
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
			return ObjectsHelper.Equals(_range, that._range) && ObjectsHelper.Equals(_result, that._result);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.Hash(_range, _result);
		}
	}

}