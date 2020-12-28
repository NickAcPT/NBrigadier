using System;
using System.Collections.Generic;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    public class SuggestionsBuilder
    {
        private readonly string _input;
        private readonly string _remaining;
        private readonly IList<Suggestion> _result = new List<Suggestion>();
        private readonly int _start;

        public SuggestionsBuilder(string input, int start)
        {
            _input = input;
            _start = start;
            _remaining = input.Substring(start);
        }

        public virtual string Input => _input;

        public virtual int Start => _start;

        public virtual string Remaining => _remaining;

        public virtual Suggestions Build()
        {
            return Suggestions.Create(_input, _result);
        }

        public virtual Func<Suggestions> BuildFuture()
        {
            return () => Build();
        }

        public virtual SuggestionsBuilder Suggest(string text)
        {
            if (text.Equals(_remaining)) return this;
            _result.Add(new Suggestion(StringRange.Between(_start, _input.Length), text));
            return this;
        }

        public virtual SuggestionsBuilder Suggest(string text, IMessage tooltip)
        {
            if (text.Equals(_remaining)) return this;
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
            ((List<Suggestion>) _result).AddRange(other._result);
            return this;
        }

        public virtual SuggestionsBuilder CreateOffset(int start)
        {
            return new(_input, start);
        }

        public virtual SuggestionsBuilder Restart()
        {
            return new(_input, _start);
        }
    }
}