using System;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    public delegate Func<Suggestions> SuggestionProvider<TS>(CommandContext<TS> context, SuggestionsBuilder builder);
}