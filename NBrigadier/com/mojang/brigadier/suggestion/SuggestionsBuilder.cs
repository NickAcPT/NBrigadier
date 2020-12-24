using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using Message = com.mojang.brigadier.Message;
	using StringRange = com.mojang.brigadier.context.StringRange;


	public class SuggestionsBuilder
	{
		private readonly string input;
		private readonly int start;
		private readonly string remaining;
		private readonly IList<Suggestion> result = new List<Suggestion>();

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder(final String input, final int start)
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

		public virtual Suggestions Build()
		{
			return Suggestions.Create(input, result);
		}

		public virtual System.Func<Suggestions> BuildFuture()
		{
			return () => Build();
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder suggest(final String text)
		public virtual SuggestionsBuilder Suggest(string text)
		{
			if (text.Equals(remaining))
			{
				return this;
			}
			result.Add(new Suggestion(StringRange.Between(start, input.Length), text));
			return this;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder suggest(final String text, final com.mojang.brigadier.Message tooltip)
		public virtual SuggestionsBuilder Suggest(string text, Message tooltip)
		{
			if (text.Equals(remaining))
			{
				return this;
			}
			result.Add(new Suggestion(StringRange.Between(start, input.Length), text, tooltip));
			return this;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder suggest(final int value)
		public virtual SuggestionsBuilder Suggest(int value)
		{
			result.Add(new IntegerSuggestion(StringRange.Between(start, input.Length), value));
			return this;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder suggest(final int value, final com.mojang.brigadier.Message tooltip)
		public virtual SuggestionsBuilder Suggest(int value, Message tooltip)
		{
			result.Add(new IntegerSuggestion(StringRange.Between(start, input.Length), value, tooltip));
			return this;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder add(final SuggestionsBuilder other)
		public virtual SuggestionsBuilder Add(SuggestionsBuilder other)
		{
			((List<Suggestion>)result).AddRange(other.result);
			return this;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionsBuilder createOffset(final int start)
		public virtual SuggestionsBuilder CreateOffset(int start)
		{
			return new SuggestionsBuilder(input, start);
		}

		public virtual SuggestionsBuilder Restart()
		{
			return new SuggestionsBuilder(input, start);
		}
	}

}