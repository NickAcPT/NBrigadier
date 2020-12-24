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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public LiteralCommandNode(final String literal, final com.mojang.brigadier.Command<S> command, final java.util.function.Predicate<S> requirement, final CommandNode<S> redirect, final com.mojang.brigadier.RedirectModifier<S> modifier, final boolean forks)
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: @Override public void parse(final com.mojang.brigadier.StringReader reader, final com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = reader.getCursor();
			int start = reader.Cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int end = parse(reader);
			int end = Parse(reader);
			if (end > -1)
			{
				contextBuilder.WithNode(this, StringRange.Between(start, end));
				return;
			}

			throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.LiteralIncorrect().CreateWithContext(reader, literal);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private int parse(final com.mojang.brigadier.StringReader reader)
		private int Parse(StringReader reader)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = reader.getCursor();
			int start = reader.Cursor;
			if (reader.CanRead(literal.Length))
			{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int end = start + literal.length();
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(final com.mojang.brigadier.context.CommandContext<S> context, final com.mojang.brigadier.suggestion.SuggestionsBuilder builder)
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

        //WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean isValidInput(final String input)
		public override bool IsValidInput(string input)
		{
			return Parse(new StringReader(input)) > -1;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LiteralCommandNode that = (LiteralCommandNode) o;
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.builder.LiteralArgumentBuilder<S> builder = com.mojang.brigadier.builder.LiteralArgumentBuilder.literal(this.literal);
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