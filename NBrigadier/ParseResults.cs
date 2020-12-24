using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public class ParseResults<TS>
    {
        public ParseResults(CommandContextBuilder<TS> context, IMmutableStringReader reader,
            IDictionary<CommandNode<TS>, CommandSyntaxException> exceptions)
        {
            Context = context;
            Reader = reader;
            Exceptions = exceptions;
        }

        public ParseResults(CommandContextBuilder<TS> context) : this(context, new StringReader(""),
            new Dictionary<CommandNode<TS>, CommandSyntaxException>())
        {
        }

        public virtual CommandContextBuilder<TS> Context { get; }

        public virtual IMmutableStringReader Reader { get; }

        public virtual IDictionary<CommandNode<TS>, CommandSyntaxException> Exceptions { get; }
    }
}