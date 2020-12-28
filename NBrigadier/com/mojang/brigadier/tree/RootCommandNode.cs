using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;
using NBrigadier.Generics;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.tree
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.builder;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public class RootCommandNode<S> : CommandNode<S>, IRootCommandNode
	{
		public RootCommandNode() : base(null, c => true, null, s => CollectionsHelper.SingletonList(s.Source), false)
		{
		}

		public override string Name
		{
			get
			{
				return "";
			}
		}

		public override string UsageText
		{
			get
			{
				return "";
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public override void parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
		}

		public override System.Func<Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.empty();
		}

        protected internal override bool isValidInput(string input)
		{
			return false;
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is IRootCommandNode))
			{
				return false;
			}
			return base.Equals(o);
		}

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: @Override public com.mojang.brigadier.builder.ArgumentBuilder<S, ?> createBuilder()
		public override IArgumentBuilder<S> createBuilder()
		{
			throw new System.InvalidOperationException("Cannot convert root into a builder");
		}

		protected internal override string SortedKey
		{
			get
			{
				return "";
			}
		}

		public override ICollection<string> Examples
		{
			get
			{
				return CollectionsHelper.EmptyList<string>();
			}
		}

		public override string ToString()
		{
			return "<root>";
		}
	}

}