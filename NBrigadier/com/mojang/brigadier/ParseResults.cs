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
		private readonly CommandContextBuilder<S> context;
		private readonly IDictionary<CommandNode<S>, CommandSyntaxException> exceptions;
		private readonly ImmutableStringReader reader;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public ParseResults(final com.mojang.brigadier.context.CommandContextBuilder<S> context, final ImmutableStringReader reader, final java.util.Map<com.mojang.brigadier.tree.CommandNode<S>, com.mojang.brigadier.exceptions.CommandSyntaxException> exceptions)
		public ParseResults(CommandContextBuilder<S> context, ImmutableStringReader reader, IDictionary<CommandNode<S>, CommandSyntaxException> exceptions)
		{
			this.context = context;
			this.reader = reader;
			this.exceptions = exceptions;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public ParseResults(final com.mojang.brigadier.context.CommandContextBuilder<S> context)
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