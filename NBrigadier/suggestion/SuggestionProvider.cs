// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public delegate System.Func<Suggestions> SuggestionProvider<S>(CommandContext<S> context, SuggestionsBuilder builder);

}