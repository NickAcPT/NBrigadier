﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Arguments;
using NBrigadier.Suggestion;
using NBrigadier.Tree;

namespace NBrigadier.Builder
{
    public class RequiredArgumentBuilder<S, T> : ArgumentBuilder<S, RequiredArgumentBuilder<S, T>>
	{
		private readonly string name;
		private readonly ArgumentType<T> type;
		private SuggestionProvider<S> suggestionsProvider = null;

		private RequiredArgumentBuilder(string name, ArgumentType<T> type)
		{
			this.name = name;
			this.type = type;
		}

		public static RequiredArgumentBuilder<S, T> Argument(string name, ArgumentType<T> type)
		{
			return new RequiredArgumentBuilder<S, T>(name, type);
		}

		public virtual RequiredArgumentBuilder<S, T> Suggests(SuggestionProvider<S> provider)
		{
			this.suggestionsProvider = provider;
			return This;
		}

		public virtual SuggestionProvider<S> SuggestionsProvider
		{
			get
			{
				return suggestionsProvider;
			}
		}

		protected internal override RequiredArgumentBuilder<S, T> This
		{
			get
			{
				return this;
			}
		}

		public virtual ArgumentType<T> Type
		{
			get
			{
				return type;
			}
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
		}

		public override CommandNode<S> Build()
		{
			ArgumentCommandNode<S, T> result = new ArgumentCommandNode<S, T>(Name, Type, Command, Requirement, Redirect, RedirectModifier, Fork, SuggestionsProvider);

			foreach (CommandNode<S> argument in Arguments)
			{
				result.AddChild(argument);
			}

			return result;
		}
	}

}