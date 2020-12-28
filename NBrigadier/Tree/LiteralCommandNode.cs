using System;
using System.Collections.Generic;
using NBrigadier.Builder;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class LiteralCommandNode<TS> : CommandNode<TS>, ILiteralCommandNode
    {
        public LiteralCommandNode(string literal, Command<TS> command, Predicate<TS> requirement,
            CommandNode<TS> redirect, RedirectModifier<TS> modifier, bool forks) : base(command, requirement, redirect,
            modifier, forks)
        {
            Literal = literal;
        }

        public override string Name => Literal;

        public override string UsageText => Literal;

        protected internal override string SortedKey => Literal;

        public override ICollection<string> Examples => CollectionsHelper.SingletonList(Literal);

        public virtual string Literal { get; }

        public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
        {
            var start = reader.Cursor;
            var end = Parse(reader);
            if (end > -1)
            {
                contextBuilder.WithNode(this, StringRange.Between(start, end));
                return;
            }

            throw CommandSyntaxException.builtInExceptions.LiteralIncorrect().CreateWithContext(reader, Literal);
        }

        private int Parse(StringReader reader)
        {
            var start = reader.Cursor;
            if (reader.CanRead(Literal.Length))
            {
                var end = start + Literal.Length;
                if (reader.String.Substring(start, end - start).Equals(Literal))
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
            if (Literal.ToLower().StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
                return builder.Suggest(Literal).BuildFuture();
            return Suggestions.Empty();
        }

        protected internal override bool IsValidInput(string input)
        {
            return Parse(new StringReader(input)) > -1;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is ILiteralCommandNode)) return false;

            var that = (ILiteralCommandNode) o;

            if (!Literal.Equals(that.Literal)) return false;
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            var result = Literal.GetHashCode();
            result = 31 * result + base.GetHashCode();
            return result;
        }

        public override IArgumentBuilder<TS> CreateBuilder()
        {
            var builder = LiteralArgumentBuilder<TS>.Literal(Literal);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            if (Command != null) builder.Executes(Command);
            return builder;
        }

        public override string ToString()
        {
            return "<literal " + Literal + ">";
        }
    }
}