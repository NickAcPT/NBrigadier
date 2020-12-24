using System;
using System.Collections.Generic;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class LiteralCommandNode<TS> : CommandNode<TS>
    {
        private readonly string _literal;

        public LiteralCommandNode(string literal, Command<TS> command, Predicate<TS> requirement, CommandNode<TS> redirect,
            RedirectModifier<TS> modifier, bool forks) : base(command, requirement, redirect, modifier, forks)
        {
            this._literal = literal;
        }

        public virtual string Literal => _literal;

        public override string Name => _literal;

        public override string UsageText => _literal;

        protected internal override string SortedKey => _literal;

        public override ICollection<string> Examples => new List<string> {_literal};

        public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
        {
            var start = reader.Cursor;
            var end = Parse(reader);
            if (end > -1)
            {
                contextBuilder.WithNode(this, StringRange.Between(start, end));
                return;
            }

            throw CommandSyntaxException.builtInExceptions.LiteralIncorrect().CreateWithContext(reader, _literal);
        }

        private int Parse(StringReader reader)
        {
            var start = reader.Cursor;
            if (reader.CanRead(_literal.Length))
            {
                var end = start + _literal.Length;
                if (reader.String.Substring(start, end - start).Equals(_literal))
                {
                    reader.Cursor = end;
                    if (!reader.CanRead() || reader.Peek() == ' ')
                        return end;
                    reader.Cursor = start;
                }
            }

            return -1;
        }

        public override Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            if (_literal.ToLower().StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
                return builder.Suggest(_literal).BuildFuture();
            return Suggestions.Empty();
        }

        public override ArgumentBuilder<TS, T> CreateBuilder<T>()
        {
            return CreateLiteralBuilder() as ArgumentBuilder<TS, T>;
        }

        public override bool IsValidInput(string input)
        {
            return Parse(new StringReader(input)) > -1;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is LiteralCommandNode<TS>)) return false;

            var that = (LiteralCommandNode<TS>) o;

            if (!_literal.Equals(that._literal)) return false;
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            var result = _literal.GetHashCode();
            result = 31 * result + base.GetHashCode();
            return result;
        }

        public LiteralArgumentBuilder<TS> CreateLiteralBuilder()
        {
            var builder = LiteralArgumentBuilder<TS>.LiteralBuilder<TS>(_literal);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            if (Command != null) builder.Executes(Command);
            return builder;
        }

        public override string ToString()
        {
            return "<literal " + _literal + ">";
        }
    }
}