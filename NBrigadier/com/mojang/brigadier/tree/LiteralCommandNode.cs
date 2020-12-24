using System;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.tree
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.builder;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using StringRange = com.mojang.brigadier.context.StringRange;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public class LiteralCommandNode<S> : CommandNode<S>
	{
		private readonly string literal;

		public LiteralCommandNode(string literal, Command<S> command, System.Predicate<S> requirement, CommandNode<S> redirect, RedirectModifier<S> modifier, bool forks) : base(command, requirement, redirect, modifier, forks)
		{
			this.literal = literal;
		}

		public virtual string Literal
		{
			get
			{
				return literal;
			}
		}

		public override string Name
		{
			get
			{
				return literal;
			}
		}

		public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
			int start = reader.Cursor;
			int end = Parse(reader);
			if (end > -1)
			{
				contextBuilder.WithNode(this, StringRange.Between(start, end));
				return;
			}

			throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LiteralIncorrect().CreateWithContext(reader, literal);
		}

		private int Parse(StringReader reader)
		{
			int start = reader.Cursor;
			if (reader.CanRead(literal.Length))
			{
				int end = start + literal.Length;
				if (reader.String.Substring(start, end - start).Equals(literal))
				{
					reader.Cursor = end;
					if (!reader.CanRead() || reader.Peek() == ' ')
					{
						return end;
					}
					else
					{
						reader.Cursor = start;
					}
				}
			}
			return -1;
		}

		public override System.Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
		{
			if (literal.ToLower().StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				return builder.Suggest(literal).BuildFuture();
			}
			else
			{
				return Suggestions.Empty();
			}
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
			if (this == o)
			{
				return true;
			}
			if (!(o is LiteralCommandNode<S>))
			{
				return false;
			}

			LiteralCommandNode<S> that = (LiteralCommandNode<S>) o;

			if (!literal.Equals(that.literal))
			{
				return false;
			}
			return base.Equals(o);
		}

		public override string UsageText
		{
			get
			{
				return literal;
			}
		}

		public override int GetHashCode()
		{
			int result = literal.GetHashCode();
			result = 31 * result + base.GetHashCode();
			return result;
		}

		public LiteralArgumentBuilder<S> CreateLiteralBuilder()
		{
			LiteralArgumentBuilder<S> builder = LiteralArgumentBuilder<S>.LiteralBuilder<S>(this.literal);
			builder.Requires(Requirement);
			builder.Forward(Redirect, RedirectModifier, Fork);
			if (Command != null)
			{
				builder.Executes(Command);
			}
			return builder;
		}

		protected internal override string SortedKey
		{
			get
			{
				return literal;
			}
		}

		public override ICollection<string> Examples
		{
			get
			{
				return new List<string> {literal};
			}
		}

		public override string ToString()
		{
			return "<literal " + literal + ">";
		}
	}

}