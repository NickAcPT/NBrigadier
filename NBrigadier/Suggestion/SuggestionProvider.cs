// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using NBrigadier.Context;

namespace NBrigadier.Suggestion
{
    public delegate Func<Suggestions> SuggestionProvider<S>(CommandContext<S> context, SuggestionsBuilder builder);
}