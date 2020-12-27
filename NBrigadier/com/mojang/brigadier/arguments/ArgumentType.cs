using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public interface ArgumentType<T>
	{
// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: T parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		T parse(StringReader reader);

		virtual System.Func<Suggestions> listSuggestions<S>(com.mojang.brigadier.context.CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.empty();
		}

		virtual ICollection<string> Examples
		{
			get
			{
				return CollectionsHelper.EmptyList();
			}
		}
	}

}