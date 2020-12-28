using System;
using System.Collections.Generic;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class ArgumentCommandNode<TS, T> : CommandNode<TS>, IArgumentCommandNode<TS>
    {
        private static readonly string _usageArgumentOpen = "<";
        private static readonly string _usageArgumentClose = ">";
        private readonly SuggestionProvider<TS> _customSuggestions;

        private readonly string _name;
        private readonly IArgumentType<T> _type;

        public ArgumentCommandNode(string name, IArgumentType<T> type, Command<TS> command, Predicate<TS> requirement,
            CommandNode<TS> redirect, RedirectModifier<TS> modifier, bool forks,
            SuggestionProvider<TS> customSuggestions) : base(command, requirement, redirect, modifier, forks)
        {
            _name = name;
            _type = type;
            _customSuggestions = customSuggestions;
        }

        public virtual IArgumentType<T> Type => _type;

        public override string Name => _name;

        public override string UsageText => _usageArgumentOpen + _name + _usageArgumentClose;

        protected internal override string SortedKey => _name;

        public override ICollection<string> Examples => _type.Examples;

        public virtual SuggestionProvider<TS> CustomSuggestions => _customSuggestions;

        // WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
        public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
        {
            var start = reader.Cursor;
            var result = _type.Parse(reader);
            var parsed = new ParsedArgument<TS, T>(start, reader.Cursor, result);

            contextBuilder.WithArgument<T>(_name, parsed);
            contextBuilder.WithNode(this, parsed.Range);
        }

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public java.util.concurrent.CompletableFuture<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
        public override Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            if (_customSuggestions == null)
                return _type.ListSuggestions(context, builder);
            return _customSuggestions(context, builder);
        }

        public override IArgumentBuilder<TS> CreateBuilder()
        {
            var builder = RequiredArgumentBuilder<TS, T>.Argument(_name, _type);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            builder.Suggests(_customSuggestions);
            if (Command != null) builder.Executes(Command);
            return builder;
        }

        protected internal override bool IsValidInput(string input)
        {
            try
            {
                var reader = new StringReader(input);
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
            if (this == o) return true;
            if (!(o is ArgumentCommandNode<TS, T>)) return false;

            var that = (ArgumentCommandNode<TS, T>) o;

            if (!_name.Equals(that._name)) return false;
            if (!_type.Equals(that._type)) return false;
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            var result = _name.GetHashCode();
            result = 31 * result + _type.GetHashCode();
            return result;
        }

        public override string ToString()
        {
            return "<argument " + _name + ":" + _type + ">";
        }
    }
}