using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public class ParseResults<S>
    {
        public ParseResults(CommandContextBuilder<S> context, ImmutableStringReader reader,
            IDictionary<CommandNode<S>, CommandSyntaxException> exceptions)
        {
            this.Context = context;
            this.Reader = reader;
            this.Exceptions = exceptions;
        }

        public ParseResults(CommandContextBuilder<S> context) : this(context, new StringReader(""),
            new Dictionary<CommandNode<S>, CommandSyntaxException>())
        {
        }

        public virtual CommandContextBuilder<S> Context { get; }

        public virtual ImmutableStringReader Reader { get; }

        public virtual IDictionary<CommandNode<S>, CommandSyntaxException> Exceptions { get; }
    }
}