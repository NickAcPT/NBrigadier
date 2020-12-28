using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using com.mojang.brigadier.tree;


	public class ParseResults<S>
	{
		private CommandContextBuilder<S> context;
		private IDictionary<CommandNode<S>, CommandSyntaxException> exceptions;
		private ImmutableStringReader reader;

		public ParseResults(CommandContextBuilder<S> context, ImmutableStringReader reader, IDictionary<CommandNode<S>, CommandSyntaxException> exceptions)
		{
			this.context = context;
			this.reader = reader;
			this.exceptions = exceptions;
		}

		public ParseResults(CommandContextBuilder<S> context) : this(context, new StringReader(""), CollectionsHelper.EmptyMap<CommandNode<S>, CommandSyntaxException>())
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