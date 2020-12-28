﻿using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.CommandSuggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
	using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class RootCommandNode<TS> : CommandNode<TS>, IRootCommandNode
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
		public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
		{
		}

		public override System.Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
		}

        protected internal override bool IsValidInput(string input)
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
		public override IArgumentBuilder<TS> CreateBuilder()
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