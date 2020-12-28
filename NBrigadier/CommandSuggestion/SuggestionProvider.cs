using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    public delegate System.Func<Suggestions> SuggestionProvider<TS>(CommandContext<TS> context, SuggestionsBuilder builder);

}