using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public delegate System.Func<Suggestions> SuggestionProvider<S>(CommandContext<S> context, SuggestionsBuilder builder);

}