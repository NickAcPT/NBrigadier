using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class BoolArgumentType : ArgumentType<bool>
	{
		private static readonly ICollection<string> EXAMPLES = new List<string>{"true", "false"};

		private BoolArgumentType()
		{
		}

		public static BoolArgumentType Bool()
		{
			return new BoolArgumentType();
		}

		public static bool GetBool<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<bool>(name, typeof(Boolean));
		}

		public bool Parse(StringReader reader)
		{
			return reader.ReadBoolean();
		}

		public System.Func<Suggestions> ListSuggestions<S>(CommandContext<S> context, SuggestionsBuilder builder)
		{
			if ("true".StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				builder.Suggest("true");
			}
			if ("false".StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				builder.Suggest("false");
			}
			return builder.BuildFuture();
		}

		public virtual ICollection<string> Examples
		{
			get
			{
				return EXAMPLES;
			}
		}
	}

}