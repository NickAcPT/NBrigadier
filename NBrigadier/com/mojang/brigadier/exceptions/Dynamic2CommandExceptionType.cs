﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
    using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
    using Message = com.mojang.brigadier.Message;

    public class Dynamic2CommandExceptionType : CommandExceptionType
    {
        private readonly Function function;

        public Dynamic2CommandExceptionType(Function function)
        {
            this.function = function;
        }

        public virtual CommandSyntaxException Create(object a, object b)
        {
            return new CommandSyntaxException(this, function(a, b));
        }

        public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object a, object b)
        {
            return new CommandSyntaxException(this, function(a, b), reader.String, reader.Cursor);
        }

        public delegate Message Function(object a, object b);
    }
}