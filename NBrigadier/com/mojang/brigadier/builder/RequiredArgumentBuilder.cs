// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.builder
{
	using com.mojang.brigadier.arguments;
	using com.mojang.brigadier.suggestion;
	using com.mojang.brigadier.tree;
	using com.mojang.brigadier.tree;

	public class RequiredArgumentBuilder<S, T> : ArgumentBuilder<S, RequiredArgumentBuilder<S, T>>
	{
		private readonly string name;
		private readonly ArgumentType<T> type;
		private SuggestionProvider<S> suggestionsProvider = null;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private RequiredArgumentBuilder(final String name, final com.mojang.brigadier.arguments.ArgumentType<T> type)
		private RequiredArgumentBuilder(string name, ArgumentType<T> type)
		{
			this.name = name;
			this.type = type;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static <S, T> RequiredArgumentBuilder<S, T> argument(final String name, final com.mojang.brigadier.arguments.ArgumentType<T> type)
		public static RequiredArgumentBuilder<S, T> Argument(string name, ArgumentType<T> type)
		{
			return new RequiredArgumentBuilder<S, T>(name, type);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public RequiredArgumentBuilder<S, T> suggests(final com.mojang.brigadier.suggestion.SuggestionProvider<S> provider)
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.tree.ArgumentCommandNode<S, T> result = new com.mojang.brigadier.tree.ArgumentCommandNode<>(getName(), getType(), getCommand(), getRequirement(), getRedirect(), getRedirectModifier(), isFork(), getSuggestionsProvider());
			ArgumentCommandNode<S, T> result = new ArgumentCommandNode<S, T>(Name, Type, Command, Requirement, Redirect, RedirectModifier, Fork, SuggestionsProvider);

			foreach (CommandNode<S> argument in Arguments)
			{
				result.AddChild(argument);
			}

			return result;
		}
	}

}