using System.Collections.Generic;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
	using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public class RootCommandNode<S> : CommandNode<S>
	{
		public RootCommandNode() : base(null, c => true, null, s => new List<S> {s.Source}, false)
		{
		}

		public override string Name
		{
			get
			{
				return "";
			}
		}

		public override string UsageText
		{
			get
			{
				return "";
			}
		}

		public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
		{
		}

		public override System.Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
		{
			return Suggestions.Empty();
		}

		public override bool IsValidInput(string input)
		{
			return false;
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is RootCommandNode<S>))
			{
				return false;
			}
			return base.Equals(o);
		}

		public override ArgumentBuilder<S, T> CreateBuilder<T>()
		{
			throw new System.InvalidOperationException("Cannot convert root into a builder");
		}

		protected internal override string SortedKey
		{
			get
			{
				return "";
			}
		}

		public override ICollection<string> Examples
		{
			get
			{
				return new List<string>();
			}
		}

		public override string ToString()
		{
			return "<root>";
		}
	}

}