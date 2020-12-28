using NBrigadier.Arguments;
using NBrigadier.CommandSuggestion;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public class RequiredArgumentBuilder<TS, T> : ArgumentBuilder<TS, RequiredArgumentBuilder<TS, T>>
	{
		private string _name;
		private IArgumentType<T> _type;
		private SuggestionProvider<TS> _suggestionsProvider = null;

		private RequiredArgumentBuilder(string name, IArgumentType<T> type)
		{
			this._name = name;
			this._type = type;
		}

		public static RequiredArgumentBuilder<TS, T> Argument(string name, IArgumentType<T> type)
		{
			return new RequiredArgumentBuilder<TS, T>(name, type);
		}

		public virtual RequiredArgumentBuilder<TS, T> Suggests(SuggestionProvider<TS> provider)
		{
			this._suggestionsProvider = provider;
			return This;
		}

		public virtual SuggestionProvider<TS> SuggestionsProvider
		{
			get
			{
				return _suggestionsProvider;
			}
		}

		protected internal override RequiredArgumentBuilder<TS, T> This
		{
			get
			{
				return this;
			}
		}

		public virtual IArgumentType<T> Type
		{
			get
			{
				return _type;
			}
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public override CommandNode<TS> Build()
		{
			 ArgumentCommandNode<TS, T> result = new ArgumentCommandNode<TS, T>(Name, Type, Command, Requirement, RedirectTarget, RedirectModifier, HasFork, SuggestionsProvider);

			foreach (CommandNode<TS> argument in Arguments)
			{
				result.AddChild(argument);
			}

			return result;
		}
	}

}