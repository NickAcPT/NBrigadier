using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
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
	using StringRange = com.mojang.brigadier.context.StringRange;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public class LiteralCommandNode<S> : CommandNode<S>, ILiteralCommandNode
	{
		private string literal;

		public LiteralCommandNode(string literal, Command<S> command, System.Predicate<S> requirement, CommandNode<S> redirect, RedirectModifier<S> modifier, bool forks) : base(command, requirement, redirect, modifier, forks)
		{
			this.Literal = literal;
		}

		public virtual string Literal
		{
			get
			{
				return Literal;
			}
		}

		public override string Name
		{
			get
			{
				return Literal;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public override void parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
			 int start = reader.Cursor;
			 int end = parse(reader);
			if (end > -1)
			{
				contextBuilder.withNode(this, StringRange.between(start, end));
				return;
			}

			throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.literalIncorrect().createWithContext(reader, Literal);
		}

		private int parse(StringReader reader)
		{
			 int start = reader.Cursor;
			if (reader.canRead(Literal.Length))
			{
				 int end = start + Literal.Length;
				if (reader.String.Substring(start, end - start).Equals(Literal))
				{
					reader.Cursor = end;
					if (!reader.canRead() || reader.peek() == ' ')
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

		public override System.Func<Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, SuggestionsBuilder builder)
		{
			if (Literal.ToLower().StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				return builder.suggest(Literal).buildFuture();
			}
			else
			{
				return Suggestions.empty();
			}
		}

		public override bool isValidInput(string input)
		{
			return parse(new StringReader(input)) > -1;
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is ILiteralCommandNode))
			{
				return false;
			}

			 ILiteralCommandNode that = (ILiteralCommandNode) o;

			if (!Literal.Equals(that.Literal))
			{
				return false;
			}
			return base.Equals(o);
		}

		public override string UsageText
		{
			get
			{
				return Literal;
			}
		}

		public override int GetHashCode()
		{
			int result = Literal.GetHashCode();
			result = 31 * result + base.GetHashCode();
			return result;
		}

		public override LiteralArgumentBuilder<S> createBuilder()
		{
			 LiteralArgumentBuilder<S> builder = LiteralArgumentBuilder.literal(this.Literal);
			builder.requires(Requirement);
			builder.forward(Redirect, RedirectModifier, Fork);
			if (Command != null)
			{
				builder.executes(Command);
			}
			return builder;
		}

		protected internal override string SortedKey
		{
			get
			{
				return Literal;
			}
		}

		public override ICollection<string> Examples
		{
			get
			{
				return CollectionsHelper.SingletonList(Literal);
			}
		}

		public override string ToString()
		{
			return "<literal " + Literal + ">";
		}
	}

}