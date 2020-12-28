using NBrigadier.Arguments;
using NBrigadier.Suggestion;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public class RequiredArgumentBuilder<S, T> : ArgumentBuilder<S, RequiredArgumentBuilder<S, T>>
	{
		private string name;
		private ArgumentType<T> type;
		private SuggestionProvider<S> suggestionsProvider = null;

		private RequiredArgumentBuilder(string name, ArgumentType<T> type)
		{
			this.name = name;
			this.type = type;
		}

		public static RequiredArgumentBuilder<S, T> argument(string name, ArgumentType<T> type)
		{
			return new RequiredArgumentBuilder<S, T>(name, type);
		}

		public virtual RequiredArgumentBuilder<S, T> suggests(SuggestionProvider<S> provider)
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

		public override CommandNode<S> build()
		{
			 ArgumentCommandNode<S, T> result = new ArgumentCommandNode<S, T>(Name, Type, Command, Requirement, Redirect, RedirectModifier, Fork, SuggestionsProvider);

			foreach (CommandNode<S> argument in Arguments)
			{
				result.addChild(argument);
			}

			return result;
		}
	}

}