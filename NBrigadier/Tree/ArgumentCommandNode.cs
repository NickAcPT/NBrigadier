using System;
using System.Collections.Generic;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class ArgumentCommandNode<S, T> : CommandNode<S>, IArgumentCommandNode<S>
    {
        private const string USAGE_ARGUMENT_OPEN = "<";
        private const string USAGE_ARGUMENT_CLOSE = ">";
        private readonly SuggestionProvider<S> customSuggestions;

        private readonly string name;
        private readonly ArgumentType<T> type;

        public ArgumentCommandNode(string name, ArgumentType<T> type, Command<S> command, Predicate<S> requirement,
            CommandNode<S> redirect, RedirectModifier<S> modifier, bool forks, SuggestionProvider<S> customSuggestions)
            : base(command, requirement, redirect, modifier, forks)
        {
            this.name = name;
            this.type = type;
            this.customSuggestions = customSuggestions;
        }

        public virtual ArgumentType<T> Type => type;

        protected internal override string SortedKey => name;

        public override string Name => name;

        public override string UsageText => USAGE_ARGUMENT_OPEN + name + USAGE_ARGUMENT_CLOSE;

        public virtual SuggestionProvider<S> CustomSuggestions => customSuggestions;

        public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
        {
            var start = reader.Cursor;
            var result = type.Parse(reader);
            var parsed = new ParsedArgument<S, object>(start, reader.Cursor, result);

            contextBuilder.WithArgument(name, parsed);
            contextBuilder.WithNode(this, parsed.Range);
        }

        public override Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
        {
            if (customSuggestions == null)
                return type.ListSuggestions(context, builder);
            return customSuggestions(context, builder);
        }

        public override bool IsValidInput(string input)
        {
            try
            {
                var reader = new StringReader(input);
                type.Parse(reader);
                return !reader.CanRead() || reader.Peek() == ' ';
            }
            catch (CommandSyntaxException)
            {
                return false;
            }
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is ArgumentCommandNode<S, T>)) return false;

            var that = (ArgumentCommandNode<S, T>) o;

            if (!name.Equals(that.name)) return false;
            if (!type.Equals(that.type)) return false;
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            var result = name.GetHashCode();
            result = 31 * result + type.GetHashCode();
            return result;
        }

        public override ICollection<string> Examples => type.GetExamples();

        public override string ToString()
        {
            return "<argument " + name + ":" + type + ">";
        }

        public override ArgumentBuilder<S, T> CreateBuilder<T>()
        {
            return CreateRequiredArgumentBuilder<T>() as ArgumentBuilder<S, T>;
        }

        public RequiredArgumentBuilder<S, T> CreateRequiredArgumentBuilder<T>() where T : ArgumentBuilder<S, T>
        {
            var builder = RequiredArgumentBuilder<S, T>.Argument(name, type as ArgumentType<T>);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            builder.Suggests(customSuggestions);
            if (Command != null) builder.Executes(Command);
            return builder;
        }
    }
}