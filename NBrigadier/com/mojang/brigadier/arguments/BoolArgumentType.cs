using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public class BoolArgumentType : ArgumentType<bool>
	{
		private static ICollection<string> EXAMPLES = CollectionsHelper.AsList("true", "false");

		private BoolArgumentType()
		{
		}

		public static BoolArgumentType @bool()
		{
			return new BoolArgumentType();
		}

		public static bool getBool<T1>(com.mojang.brigadier.context.CommandContext<T1> context, string name)
		{
			return context.getArgument(name, typeof(Boolean));
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<bool> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual bool parse(StringReader reader)
		{
			return reader.readBoolean();
		}

		public virtual System.Func<Suggestions> listSuggestions<S>(com.mojang.brigadier.context.CommandContext<S> context, SuggestionsBuilder builder)
		{
			if ("true".StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				builder.suggest("true");
			}
			if ("false".StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				builder.suggest("false");
			}
			return builder.buildFuture();
		}

		public virtual ICollection<string> Examples
		{
			get
			{
				return EXAMPLES;
			}
		}
	}

}