using System;
using System.Collections.Generic;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public interface IArgumentType<T>
    {
        ICollection<string> Examples { get; }

        T Parse(StringReader reader);

        Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder);
    }
}