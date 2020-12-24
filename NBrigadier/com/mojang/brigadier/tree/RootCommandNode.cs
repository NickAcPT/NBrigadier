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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void parse(final com.mojang.brigadier.StringReader reader, final com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(final com.mojang.brigadier.context.CommandContext<S> context, final com.mojang.brigadier.suggestion.SuggestionsBuilder builder)
		public override System.Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean isValidInput(final String input)
		public override bool IsValidInput(string input)
		{
			return false;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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

//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: @Override public com.mojang.brigadier.builder.ArgumentBuilder<S, object> createBuilder()
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