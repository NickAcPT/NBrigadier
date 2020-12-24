// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class Dynamic2CommandExceptionType : CommandExceptionType
    {
        public delegate Message Function(object a, object b);

        private readonly Function function;

        public Dynamic2CommandExceptionType(Function function)
        {
            this.function = function;
        }

        public virtual CommandSyntaxException Create(object a, object b)
        {
            return new(this, function(a, b));
        }

        public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object a, object b)
        {
            return new(this, function(a, b), reader.String, reader.Cursor);
        }
    }
}