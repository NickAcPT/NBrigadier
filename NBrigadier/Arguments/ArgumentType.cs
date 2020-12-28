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


	public interface ArgumentType<T>
	{
// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: T parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		T parse(StringReader reader);

		virtual System.Func<Suggestions> listSuggestions<S>(CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.empty();
		}

		virtual ICollection<string> Examples
		{
			get
			{
				return CollectionsHelper.EmptyList<string>();
			}
		}
	}

}