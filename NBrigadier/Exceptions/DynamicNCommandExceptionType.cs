﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class DynamicNCommandExceptionType : ICommandExceptionType
    {
        private readonly IFunction _function;

        public DynamicNCommandExceptionType(IFunction function)
        {
            _function = function;
        }

        public virtual CommandSyntaxException Create(object a, params object[] args)
        {
            return new(this, _function.Apply(args));
        }

        public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, params object[] args)
        {
            return new(this, _function.Apply(args), reader.String, reader.Cursor);
        }

        public interface IFunction
        {
            IMessage Apply(object[] args);
        }
    }
}