using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public class Suggestions
    {
        private static readonly Suggestions EMPTY = new(StringRange.At(0), new List<Suggestion>());

        private readonly StringRange range;
        private readonly IList<Suggestion> suggestions;

        public Suggestions(StringRange range, IList<Suggestion> suggestions)
        {
            this.range = range;
            this.suggestions = suggestions;
        }

        public virtual StringRange Range => range;

        public virtual IList<Suggestion> List => suggestions;

        public virtual bool IsEmpty => suggestions.Count == 0;

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is Suggestions)) return false;
            var that = (Suggestions) o;
            return Equals(range, that.range) && Equals(suggestions, that.suggestions);
        }

        public override string ToString()
        {
            return "Suggestions{" + "range=" + range + ", suggestions=" + suggestions + '}';
        }

        public static Func<Suggestions> Empty()
        {
            return () => EMPTY;
        }

        public static Suggestions Merge(string command, ICollection<Suggestions> input)
        {
            if (input.Count == 0)
                return EMPTY;
            if (input.Count == 1) return input.First();

            var texts = new HashSet<Suggestion>();
            foreach (var suggestions in input)
            foreach (var suggestion in suggestions.List)
                texts.Add(suggestion);
            return Create(command, texts);
        }

        public static Suggestions Create(string command, ICollection<Suggestion> suggestions)
        {
            if (suggestions.Count == 0) return EMPTY;
            var start = int.MaxValue;
            var end = int.MinValue;
            foreach (var suggestion in suggestions)
            {
                start = Math.Min(suggestion.Range.Start, start);
                end = Math.Max(suggestion.Range.End, end);
            }

            var range = new StringRange(start, end);
            ISet<Suggestion> texts = new HashSet<Suggestion>();
            foreach (var suggestion in suggestions) texts.Add(suggestion.Expand(command, range));
            IList<Suggestion> sorted = texts
                .OrderBy(a => a, Comparer<Suggestion>.Create((a, b) => a.CompareToIgnoreCase(b))).ToList();
            return new Suggestions(range, sorted);
        }
    }
}