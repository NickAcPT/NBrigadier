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
    public class LiteralCommandNode<S> : CommandNode<S>
    {
        private readonly string literal;

        public LiteralCommandNode(string literal, Command<S> command, Predicate<S> requirement, CommandNode<S> redirect,
            RedirectModifier<S> modifier, bool forks) : base(command, requirement, redirect, modifier, forks)
        {
            this.literal = literal;
        }

        public virtual string Literal => literal;

        public override string Name => literal;

        public override string UsageText => literal;

        protected internal override string SortedKey => literal;

        public override ICollection<string> Examples => new List<string> {literal};

        public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
        {
            var start = reader.Cursor;
            var end = Parse(reader);
            if (end > -1)
            {
                contextBuilder.WithNode(this, StringRange.Between(start, end));
                return;
            }

            throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LiteralIncorrect().CreateWithContext(reader, literal);
        }

        private int Parse(StringReader reader)
        {
            var start = reader.Cursor;
            if (reader.CanRead(literal.Length))
            {
                var end = start + literal.Length;
                if (reader.String.Substring(start, end - start).Equals(literal))
                {
                    reader.Cursor = end;
                    if (!reader.CanRead() || reader.Peek() == ' ')
                        return end;
                    reader.Cursor = start;
                }
            }

            return -1;
        }

        public override Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
        {
            if (literal.ToLower().StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
                return builder.Suggest(literal).BuildFuture();
            return Suggestions.Empty();
        }

        public override ArgumentBuilder<S, T> CreateBuilder<T>()
        {
            return CreateLiteralBuilder() as ArgumentBuilder<S, T>;
        }

        public override bool IsValidInput(string input)
        {
            return Parse(new StringReader(input)) > -1;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is LiteralCommandNode<S>)) return false;

            var that = (LiteralCommandNode<S>) o;

            if (!literal.Equals(that.literal)) return false;
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            var result = literal.GetHashCode();
            result = 31 * result + base.GetHashCode();
            return result;
        }

        public LiteralArgumentBuilder<S> CreateLiteralBuilder()
        {
            var builder = LiteralArgumentBuilder<S>.LiteralBuilder<S>(literal);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            if (Command != null) builder.Executes(Command);
            return builder;
        }

        public override string ToString()
        {
            return "<literal " + literal + ">";
        }
    }
}