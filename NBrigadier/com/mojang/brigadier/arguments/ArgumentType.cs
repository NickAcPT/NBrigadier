// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public interface ArgumentType<T>
	{
//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: T parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		T Parse(StringReader reader);

//TODO TASK: There is no equivalent in C# to Java default interface methods unless the C# 2019 extended interface option is selected:
		System.Func<com.mojang.brigadier.suggestion.Suggestions> ListSuggestions<S>(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
		}

//TODO TASK: There is no equivalent in C# to Java default interface methods unless the C# 2019 extended interface option is selected:
		IList<String> GetExamples()
		{
			return new List<string>();
		}
	}

}