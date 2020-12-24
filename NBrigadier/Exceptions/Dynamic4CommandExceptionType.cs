// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class Dynamic4CommandExceptionType : ICommandExceptionType
    {
        private readonly IFunction _function;

        public Dynamic4CommandExceptionType(IFunction function)
        {
            this._function = function;
        }

        public virtual CommandSyntaxException Create(object a, object b, object c, object d)
        {
            return new(this, _function.Apply(a, b, c, d));
        }

        public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, object a, object b,
            object c, object d)
        {
            return new(this, _function.Apply(a, b, c, d), reader.String, reader.Cursor);
        }

        public interface IFunction
        {
            IMessage Apply(object a, object b, object c, object d);
        }
    }
}