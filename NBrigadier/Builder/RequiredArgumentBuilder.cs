// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Arguments;
using NBrigadier.Suggestion;
using NBrigadier.Tree;

namespace NBrigadier.Builder
{
    public class RequiredArgumentBuilder<S, T> : ArgumentBuilder<S, RequiredArgumentBuilder<S, T>>
    {
        private SuggestionProvider<S> suggestionsProvider;

        private RequiredArgumentBuilder(string name, ArgumentType<T> type)
        {
            this.Name = name;
            this.Type = type;
        }

        public virtual SuggestionProvider<S> SuggestionsProvider => suggestionsProvider;

        protected internal override RequiredArgumentBuilder<S, T> This => this;

        public virtual ArgumentType<T> Type { get; }

        public virtual string Name { get; }

        public static RequiredArgumentBuilder<S, T> Argument(string name, ArgumentType<T> type)
        {
            return new(name, type);
        }

        public virtual RequiredArgumentBuilder<S, T> Suggests(SuggestionProvider<S> provider)
        {
            suggestionsProvider = provider;
            return This;
        }

        public override CommandNode<S> Build()
        {
            var result = new ArgumentCommandNode<S, T>(Name, Type, Command, Requirement, Redirect, RedirectModifier,
                Fork, SuggestionsProvider);

            foreach (var argument in Arguments) result.AddChild(argument);

            return result;
        }
    }
}