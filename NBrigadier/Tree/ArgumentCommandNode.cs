using System.Collections.Generic;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.CommandSuggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    using StringReader = StringReader;
    using CommandSyntaxException = CommandSyntaxException;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class ArgumentCommandNode<TS, T> : CommandNode<TS>, IArgumentCommandNode<TS>
	{
		private static string _usageArgumentOpen = "<";
		private static string _usageArgumentClose = ">";

		private string _name;
		private IArgumentType<T> _type;
		private SuggestionProvider<TS> _customSuggestions;

		public ArgumentCommandNode(string name, IArgumentType<T> type, Command<TS> command, System.Predicate<TS> requirement, CommandNode<TS> redirect, RedirectModifier<TS> modifier, bool forks, SuggestionProvider<TS> customSuggestions) : base(command, requirement, redirect, modifier, forks)
		{
			this._name = name;
			this._type = type;
			this._customSuggestions = customSuggestions;
		}

		public virtual IArgumentType<T> Type
		{
			get
			{
				return _type;
			}
		}

		public override string Name
		{
			get
			{
				return _name;
			}
		}

		public override string UsageText
		{
			get
			{
				return _usageArgumentOpen + _name + _usageArgumentClose;
			}
		}

		public virtual SuggestionProvider<TS> CustomSuggestions
		{
			get
			{
				return _customSuggestions;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
		{
			 int start = reader.Cursor;
			 T result = _type.Parse(reader);
			 ParsedArgument<TS, T> parsed = new ParsedArgument<TS, T>(start, reader.Cursor, result);

			contextBuilder.WithArgument<T>(_name, parsed);
			contextBuilder.WithNode(this, parsed.Range);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public java.util.concurrent.CompletableFuture<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public override System.Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
		{
			if (_customSuggestions == null)
			{
				return _type.ListSuggestions(context, builder);
			}
			else
			{
				return _customSuggestions(context, builder);
			}
		}

		public override IArgumentBuilder<TS> CreateBuilder()
		{
			 RequiredArgumentBuilder<TS, T> builder = RequiredArgumentBuilder<TS, T>.Argument(_name, _type);
			builder.Requires(Requirement);
			builder.Forward(Redirect, RedirectModifier, Fork);
			builder.Suggests(_customSuggestions);
			if (Command != null)
			{
				builder.Executes(Command);
			}
			return builder;
		}

        protected internal override bool IsValidInput(string input)
		{
			try
			{
				 StringReader reader = new StringReader(input);
				_type.Parse(reader);
				return !reader.CanRead() || reader.Peek() == ' ';
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
			if (!(o is ArgumentCommandNode<TS, T>))
			{
				return false;
			}

			 ArgumentCommandNode<TS, T> that = (ArgumentCommandNode<TS, T>) o;

			if (!_name.Equals(that._name))
			{
				return false;
			}
			if (!_type.Equals(that._type))
			{
				return false;
			}
			return base.Equals(o);
		}

		public override int GetHashCode()
		{
			int result = _name.GetHashCode();
			result = 31 * result + _type.GetHashCode();
			return result;
		}

		protected internal override string SortedKey
		{
			get
			{
				return _name;
			}
		}

		public override ICollection<string> Examples
		{
			get
			{
				return _type.Examples;
			}
		}

		public override string ToString()
		{
			return "<argument " + _name + ":" + _type + ">";
		}
	}

}