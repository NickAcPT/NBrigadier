using System.Collections.Generic;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    using StringReader = StringReader;
    using CommandSyntaxException = CommandSyntaxException;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class ArgumentCommandNode<S, T> : CommandNode<S>, IArgumentCommandNode<S>
	{
		private static string USAGE_ARGUMENT_OPEN = "<";
		private static string USAGE_ARGUMENT_CLOSE = ">";

		private string name;
		private ArgumentType<T> type;
		private SuggestionProvider<S> customSuggestions;

		public ArgumentCommandNode(string name, ArgumentType<T> type, Command<S> command, System.Predicate<S> requirement, CommandNode<S> redirect, RedirectModifier<S> modifier, bool forks, SuggestionProvider<S> customSuggestions) : base(command, requirement, redirect, modifier, forks)
		{
			this.name = name;
			this.type = type;
			this.customSuggestions = customSuggestions;
		}

		public virtual ArgumentType<T> Type
		{
			get
			{
				return type;
			}
		}

		public override string Name
		{
			get
			{
				return name;
			}
		}

		public override string UsageText
		{
			get
			{
				return USAGE_ARGUMENT_OPEN + name + USAGE_ARGUMENT_CLOSE;
			}
		}

		public virtual SuggestionProvider<S> CustomSuggestions
		{
			get
			{
				return customSuggestions;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public override void parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
			 int start = reader.Cursor;
			 T result = type.parse(reader);
			 ParsedArgument<S, T> parsed = new ParsedArgument<S, T>(start, reader.Cursor, result);

			contextBuilder.withArgument<T>(name, parsed);
			contextBuilder.withNode(this, parsed.Range);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public java.util.concurrent.CompletableFuture<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public override System.Func<Suggestions> listSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
		{
			if (customSuggestions == null)
			{
				return type.listSuggestions(context, builder);
			}
			else
			{
				return customSuggestions(context, builder);
			}
		}

		public override IArgumentBuilder<S> createBuilder()
		{
			 RequiredArgumentBuilder<S, T> builder = RequiredArgumentBuilder<S, T>.argument(name, type);
			builder.requires(Requirement);
			builder.forward(Redirect, RedirectModifier, Fork);
			builder.suggests(customSuggestions);
			if (Command != null)
			{
				builder.executes(Command);
			}
			return builder;
		}

        protected internal override bool isValidInput(string input)
		{
			try
			{
				 StringReader reader = new StringReader(input);
				type.parse(reader);
				return !reader.canRead() || reader.peek() == ' ';
			}
			catch (CommandSyntaxException)
			{
				return false;
			}
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is ArgumentCommandNode<S, T>))
			{
				return false;
			}

			 ArgumentCommandNode<S, T> that = (ArgumentCommandNode<S, T>) o;

			if (!name.Equals(that.name))
			{
				return false;
			}
			if (!type.Equals(that.type))
			{
				return false;
			}
			return base.Equals(o);
		}

		public override int GetHashCode()
		{
			int result = name.GetHashCode();
			result = 31 * result + type.GetHashCode();
			return result;
		}

		protected internal override string SortedKey
		{
			get
			{
				return name;
			}
		}

		public override ICollection<string> Examples
		{
			get
			{
				return type.Examples;
			}
		}

		public override string ToString()
		{
			return "<argument " + name + ":" + type + ">";
		}
	}

}