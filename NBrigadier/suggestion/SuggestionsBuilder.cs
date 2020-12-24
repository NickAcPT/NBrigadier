using System;
using System.Collections.Generic;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public class SuggestionsBuilder
    {
        private readonly string input;
        private readonly string remaining;
        private readonly IList<Suggestion> result = new List<Suggestion>();
        private readonly int start;

        public SuggestionsBuilder(string input, int start)
        {
            this.input = input;
            this.start = start;
            remaining = input.Substring(start);
        }

        public virtual string Input => input;

        public virtual int Start => start;

        public virtual string Remaining => remaining;

        public virtual Suggestions Build()
        {
            return Suggestions.Create(input, result);
        }

        public virtual Func<Suggestions> BuildFuture()
        {
            return () => Build();
        }

        public virtual SuggestionsBuilder Suggest(string text)
        {
            if (text.Equals(remaining)) return this;
            result.Add(new Suggestion(StringRange.Between(start, input.Length), text));
            return this;
        }

        public virtual SuggestionsBuilder Suggest(string text, Message tooltip)
        {
            if (text.Equals(remaining)) return this;
            result.Add(new Suggestion(StringRange.Between(start, input.Length), text, tooltip));
            return this;
        }

        public virtual SuggestionsBuilder Suggest(int value)
        {
            result.Add(new IntegerSuggestion(StringRange.Between(start, input.Length), value));
            return this;
        }

        public virtual SuggestionsBuilder Suggest(int value, Message tooltip)
        {
            result.Add(new IntegerSuggestion(StringRange.Between(start, input.Length), value, tooltip));
            return this;
        }

        public virtual SuggestionsBuilder Add(SuggestionsBuilder other)
        {
            ((List<Suggestion>) result).AddRange(other.result);
            return this;
        }

        public virtual SuggestionsBuilder CreateOffset(int start)
        {
            return new(input, start);
        }

        public virtual SuggestionsBuilder Restart()
        {
            return new(input, start);
        }
    }
}