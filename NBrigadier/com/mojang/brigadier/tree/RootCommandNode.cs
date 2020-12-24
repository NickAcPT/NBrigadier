using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.tree
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.builder;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public class RootCommandNode<S> : CommandNode<S>
	{
		public RootCommandNode() : base(null, c => true, null, s => new List<S> {s.Source}, false)
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

		public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
		}

		public override System.Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
		}

		public override bool IsValidInput(string input)
		{
			return false;
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is RootCommandNode<S>))
			{
				return false;
			}
			return base.Equals(o);
		}

		public override ArgumentBuilder<S, T> CreateBuilder<T>()
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
				return new List<string>();
			}
		}

		public override string ToString()
		{
			return "<root>";
		}
	}

}