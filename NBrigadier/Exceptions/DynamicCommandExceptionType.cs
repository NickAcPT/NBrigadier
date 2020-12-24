// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace NBrigadier.Exceptions
{
    public class DynamicCommandExceptionType : ICommandExceptionType
    {
        private readonly Func<object, IMessage> _function;

        public DynamicCommandExceptionType(Func<object, IMessage> function)
        {
            _function = function;
        }

        public virtual CommandSyntaxException Create(object arg)
        {
            return new(this, _function(arg));
        }

        public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, object arg)
        {
            return new(this, _function(arg), reader.String, reader.Cursor);
        }
    }
}