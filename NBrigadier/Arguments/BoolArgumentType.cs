using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class BoolArgumentType : IArgumentType<bool>
	{
		private static ICollection<string> _examples = CollectionsHelper.AsList("true", "false");

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

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public System.Nullable<bool> parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual bool Parse(StringReader reader)
		{
			return reader.ReadBoolean();
		}

		public virtual System.Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
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
				return _examples;
			}
		}
	}

}