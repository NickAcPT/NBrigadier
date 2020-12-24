using System;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static boolean getBool(final com.mojang.brigadier.context.CommandContext<?> context, final String name)
		public static bool GetBool<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<bool>(name, typeof(Boolean));
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public System.Nullable<bool> parse(final com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public bool Parse(StringReader reader)
		{
			return reader.ReadBoolean();
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public <S> java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(final com.mojang.brigadier.context.CommandContext<S> context, final com.mojang.brigadier.suggestion.SuggestionsBuilder builder)
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