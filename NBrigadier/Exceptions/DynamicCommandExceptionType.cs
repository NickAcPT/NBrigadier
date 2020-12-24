// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;

namespace NBrigadier.Exceptions
{
    public class DynamicCommandExceptionType : CommandExceptionType
    {
        private readonly Func<object, Message> function;

        public DynamicCommandExceptionType(Func<object, Message> function)
        {
            this.function = function;
        }

        public virtual CommandSyntaxException Create(object arg)
        {
            return new(this, function(arg));
        }

        public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object arg)
        {
            return new(this, function(arg), reader.String, reader.Cursor);
        }
    }
}