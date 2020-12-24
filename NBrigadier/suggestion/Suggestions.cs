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
        protected bool Equals(Suggestions other)
        {
            return Equals(_range, other._range) && Equals(_suggestions, other._suggestions);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_range, _suggestions);
        }

        public static bool operator ==(Suggestions left, Suggestions right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Suggestions left, Suggestions right)
        {
            return !Equals(left, right);
        }

        private static readonly Suggestions EMPTY = new(StringRange.At(0), new List<Suggestion>());

        private readonly StringRange _range;
        private readonly IList<Suggestion> _suggestions;

        public Suggestions(StringRange range, IList<Suggestion> suggestions)
        {
            this._range = range;
            this._suggestions = suggestions;
        }

        public virtual StringRange Range => _range;

        public virtual IList<Suggestion> List => _suggestions;

        public virtual bool IsEmpty => _suggestions.Count == 0;

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is Suggestions)) return false;
            var that = (Suggestions) o;
            return Equals(_range, that._range) && Equals(_suggestions, that._suggestions);
        }

        public override string ToString()
        {
            return "Suggestions{" + "range=" + _range + ", suggestions=" + _suggestions + '}';
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