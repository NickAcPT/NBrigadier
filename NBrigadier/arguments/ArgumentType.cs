// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Suggestion;

namespace NBrigadier.Arguments
{
    public interface IArgumentType<T>
    {
        T Parse(StringReader reader);

        Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        IList<string> GetExamples()
        {
            return new List<string>();
        }
    }
}