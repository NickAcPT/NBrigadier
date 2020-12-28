using System.Collections.Generic;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    using StringRange = StringRange;


	public class SuggestionsBuilder
	{
		private string _input;
		private int _start;
		private string _remaining;
		private IList<Suggestion> _result = new List<Suggestion>();

		public SuggestionsBuilder(string input, int start)
		{
			this._input = input;
			this._start = start;
			this._remaining = input.Substring(start);
		}

		public virtual string Input
		{
			get
			{
				return _input;
			}
		}

		public virtual int Start
		{
			get
			{
				return _start;
			}
		}

		public virtual string Remaining
		{
			get
			{
				return _remaining;
			}
		}

		public virtual Suggestions Build()
		{
			return Suggestions.Create(_input, _result);
		}

		public virtual System.Func<Suggestions> BuildFuture()
		{
			return () => (Build());
		}

		public virtual SuggestionsBuilder Suggest(string text)
		{
			if (text.Equals(_remaining))
			{
				return this;
			}
			_result.Add(new Suggestion(StringRange.Between(_start, _input.Length), text));
			return this;
		}

		public virtual SuggestionsBuilder Suggest(string text, IMessage tooltip)
		{
			if (text.Equals(_remaining))
			{
				return this;
			}
			_result.Add(new Suggestion(StringRange.Between(_start, _input.Length), text, tooltip));
			return this;
		}

		public virtual SuggestionsBuilder Suggest(int value)
		{
			_result.Add(new IntegerSuggestion(StringRange.Between(_start, _input.Length), value));
			return this;
		}

		public virtual SuggestionsBuilder Suggest(int value, IMessage tooltip)
		{
			_result.Add(new IntegerSuggestion(StringRange.Between(_start, _input.Length), value, tooltip));
			return this;
		}

		public virtual SuggestionsBuilder Add(SuggestionsBuilder other)
		{
			((List<Suggestion>)_result).AddRange(other._result);
			return this;
		}

		public virtual SuggestionsBuilder CreateOffset(int start)
		{
			return new SuggestionsBuilder(_input, start);
		}

		public virtual SuggestionsBuilder Restart()
		{
			return new SuggestionsBuilder(_input, _start);
		}
	}

}