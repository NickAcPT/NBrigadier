using System;
using System.Collections.Generic;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.CommandSuggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    using StringReader = StringReader;
    using StringRange = StringRange;
	using CommandSyntaxException = CommandSyntaxException;
	using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class LiteralCommandNode<TS> : CommandNode<TS>, ILiteralCommandNode
	{
		private string _literal;

		public LiteralCommandNode(string literal, Command<TS> command, System.Predicate<TS> requirement, CommandNode<TS> redirect, RedirectModifier<TS> modifier, bool forks) : base(command, requirement, redirect, modifier, forks)
		{
			this._literal = literal;
		}

		public virtual string Literal
		{
			get
			{
				return _literal;
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
		public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
		{
			 int start = reader.Cursor;
			 int end = Parse(reader);
			if (end > -1)
			{
				contextBuilder.WithNode(this, StringRange.Between(start, end));
				return;
			}

			throw CommandSyntaxException.builtInExceptions.LiteralIncorrect().CreateWithContext(reader, Literal);
		}

		private int Parse(StringReader reader)
		{
			 int start = reader.Cursor;
			if (reader.CanRead(Literal.Length))
			{
				 int end = start + Literal.Length;
				if (reader.String.Substring(start, end - start).Equals(Literal))
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

		public override System.Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
		{
			if (Literal.ToLower().StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal))
			{
				return builder.Suggest(Literal).BuildFuture();
			}
			else
			{
				return Suggestions.Empty();
			}
		}

        protected internal override bool IsValidInput(string input)
		{
			return Parse(new StringReader(input)) > -1;
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

		public override IArgumentBuilder<TS> CreateBuilder()
		{
			 LiteralArgumentBuilder<TS> builder = LiteralArgumentBuilder<TS>.Literal(this.Literal);
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