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


	public interface IArgumentType<T>
	{
// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: T parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		T Parse(StringReader reader);

		virtual System.Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
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