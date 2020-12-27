using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using Message = com.mojang.brigadier.Message;
	using StringRange = com.mojang.brigadier.context.StringRange;


	public class SuggestionsBuilder
	{
		private string input;
		private int start;
		private string remaining;
		private IList<Suggestion> result = new List<Suggestion>();

		public SuggestionsBuilder(string input, int start)
		{
			this.input = input;
			this.start = start;
			this.remaining = input.Substring(start);
		}

		public virtual string Input
		{
			get
			{
				return input;
			}
		}

		public virtual int Start
		{
			get
			{
				return start;
			}
		}

		public virtual string Remaining
		{
			get
			{
				return remaining;
			}
		}

		public virtual Suggestions build()
		{
			return Suggestions.create(input, result);
		}

		public virtual System.Func<Suggestions> buildFuture()
		{
			return () => (build());
		}

		public virtual SuggestionsBuilder suggest(string text)
		{
			if (text.Equals(remaining))
			{
				return this;
			}
			result.Add(new Suggestion(StringRange.between(start, input.Length), text));
			return this;
		}

		public virtual SuggestionsBuilder suggest(string text, Message tooltip)
		{
			if (text.Equals(remaining))
			{
				return this;
			}
			result.Add(new Suggestion(StringRange.between(start, input.Length), text, tooltip));
			return this;
		}

		public virtual SuggestionsBuilder suggest(int value)
		{
			result.Add(new IntegerSuggestion(StringRange.between(start, input.Length), value));
			return this;
		}

		public virtual SuggestionsBuilder suggest(int value, Message tooltip)
		{
			result.Add(new IntegerSuggestion(StringRange.between(start, input.Length), value, tooltip));
			return this;
		}

		public virtual SuggestionsBuilder add(SuggestionsBuilder other)
		{
			((List<Suggestion>)result).AddRange(other.result);
			return this;
		}

		public virtual SuggestionsBuilder createOffset(int start)
		{
			return new SuggestionsBuilder(input, start);
		}

		public virtual SuggestionsBuilder restart()
		{
			return new SuggestionsBuilder(input, start);
		}
	}

}