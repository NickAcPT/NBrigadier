// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class Dynamic3CommandExceptionType : ICommandExceptionType
    {
        private readonly IFunction _function;

        public Dynamic3CommandExceptionType(IFunction function)
        {
            _function = function;
        }

        public virtual CommandSyntaxException Create(object a, object b, object c)
        {
            return new(this, _function.Apply(a, b, c));
        }

        public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, object a, object b,
            object c)
        {
            return new(this, _function.Apply(a, b, c), reader.String, reader.Cursor);
        }

        public interface IFunction
        {
            IMessage Apply(object a, object b, object c);
        }
    }
}