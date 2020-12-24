// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
    using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
    using Message = com.mojang.brigadier.Message;

    public class Dynamic2CommandExceptionType : CommandExceptionType
    {
        private readonly Function function;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public Dynamic2CommandExceptionType(final Function function)
        public Dynamic2CommandExceptionType(Function function)
        {
            this.function = function;
        }

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException create(final Object a, final Object b)
        public virtual CommandSyntaxException Create(object a, object b)
        {
            return new CommandSyntaxException(this, function(a, b));
        }

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException createWithContext(final com.mojang.brigadier.ImmutableStringReader reader, final Object a, final Object b)
        public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object a, object b)
        {
            return new CommandSyntaxException(this, function(a, b), reader.String, reader.Cursor);
        }

        public delegate Message Function(object a, object b);
    }
}