// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class SimpleCommandExceptionType : ICommandExceptionType
    {
        private readonly IMessage _message;

        public SimpleCommandExceptionType(IMessage message)
        {
            _message = message;
        }

        public virtual CommandSyntaxException Create()
        {
            return new(this, _message);
        }

        public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader)
        {
            return new(this, _message, reader.String, reader.Cursor);
        }

        public override string ToString()
        {
            return _message.String;
        }
    }
}