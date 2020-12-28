using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


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

		public static bool getBool<T1>(CommandContext<T1> context, string name)
		{
			return context.getArgument<bool>(name, typeof(Boolean));
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<bool> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual bool parse(StringReader reader)
		{
			return reader.readBoolean();
		}

		public virtual System.Func<Suggestions> listSuggestions<S>(CommandContext<S> context, SuggestionsBuilder builder)
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