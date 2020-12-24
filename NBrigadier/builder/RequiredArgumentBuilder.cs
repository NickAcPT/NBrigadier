// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Arguments;
using NBrigadier.Suggestion;
using NBrigadier.Tree;

namespace NBrigadier.Builder
{
    public class RequiredArgumentBuilder<TS, T> : ArgumentBuilder<TS, RequiredArgumentBuilder<TS, T>>
    {
        private SuggestionProvider<TS> _suggestionsProvider;

        private RequiredArgumentBuilder(string name, IArgumentType<T> type)
        {
            Name = name;
            Type = type;
        }

        public virtual SuggestionProvider<TS> SuggestionsProvider => _suggestionsProvider;

        protected internal override RequiredArgumentBuilder<TS, T> This => this;

        public virtual IArgumentType<T> Type { get; }

        public virtual string Name { get; }

        public static RequiredArgumentBuilder<TS, T> Argument(string name, IArgumentType<T> type)
        {
            return new(name, type);
        }

        public virtual RequiredArgumentBuilder<TS, T> Suggests(SuggestionProvider<TS> provider)
        {
            _suggestionsProvider = provider;
            return This;
        }

        public override CommandNode<TS> Build()
        {
            var result = new ArgumentCommandNode<TS, T>(Name, Type, Command, Requirement, Redirect, RedirectModifier,
                Fork, SuggestionsProvider);

            foreach (var argument in Arguments) result.AddChild(argument);

            return result;
        }
    }
}