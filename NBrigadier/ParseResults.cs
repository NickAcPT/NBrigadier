using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    using CommandSyntaxException = CommandSyntaxException;


    public class ParseResults<S>
	{
		private readonly CommandContextBuilder<S> context;
		private readonly IDictionary<CommandNode<S>, CommandSyntaxException> exceptions;
		private readonly ImmutableStringReader reader;

		public ParseResults(CommandContextBuilder<S> context, ImmutableStringReader reader, IDictionary<CommandNode<S>, CommandSyntaxException> exceptions)
		{
			this.context = context;
			this.reader = reader;
			this.exceptions = exceptions;
		}

		public ParseResults(CommandContextBuilder<S> context) : this(context, new StringReader(""), new Dictionary<CommandNode<S>, CommandSyntaxException>())
		{
		}

		public virtual CommandContextBuilder<S> Context
		{
			get
			{
				return context;
			}
		}

		public virtual ImmutableStringReader Reader
		{
			get
			{
				return reader;
			}
		}

		public virtual IDictionary<CommandNode<S>, CommandSyntaxException> Exceptions
		{
			get
			{
				return exceptions;
			}
		}
	}

}