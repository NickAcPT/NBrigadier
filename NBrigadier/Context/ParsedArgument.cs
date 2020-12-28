using NBrigadier.Generics;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class ParsedArgument<TS, T> : IParsedArgument
    {
        private readonly StringRange _range;
        private readonly T _result;

        public ParsedArgument(int start, int end, T result)
        {
            _range = StringRange.Between(start, end);
            _result = result;
        }

        public virtual T Result => _result;

        public virtual StringRange Range => _range;

        public object ResultObject => Result;

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is IParsedArgument)) return false;
            var that = (ParsedArgument<object, object>) o;
            return ObjectsHelper.Equals(_range, that._range) && ObjectsHelper.Equals(_result, that._result);
        }

        public override int GetHashCode()
        {
            return ObjectsHelper.Hash(_range, _result);
        }
    }
}