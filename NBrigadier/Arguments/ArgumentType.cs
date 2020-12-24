// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public interface ArgumentType<T>
	{
		T Parse(StringReader reader);

		System.Func<Suggestions> ListSuggestions<S>(CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
		}

		IList<String> GetExamples()
		{
			return new List<string>();
		}
	}

}