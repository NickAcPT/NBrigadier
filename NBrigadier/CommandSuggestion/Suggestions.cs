using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    public class Suggestions
    {
        private static readonly Suggestions _empty = new(StringRange.At(0), new List<Suggestion>());

        private readonly StringRange _range;
        private readonly IList<Suggestion> _suggestions;

        public Suggestions(StringRange range, IList<Suggestion> suggestions)
        {
            _range = range;
            _suggestions = suggestions;
        }

        public virtual StringRange Range => _range;

        public virtual IList<Suggestion> List => _suggestions;

        public virtual bool IsEmpty => _suggestions.Count == 0;

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is Suggestions)) return false;
            var that = (Suggestions) o;
            return ObjectsHelper.Equals(_range, that._range) && ObjectsHelper.Equals(_suggestions, that._suggestions);
        }

        public override int GetHashCode()
        {
            return ObjectsHelper.Hash(_range, _suggestions);
        }

        public override string ToString()
        {
            return "Suggestions{" + "range=" + _range + ", suggestions=" + _suggestions + '}';
        }

        public static Func<Suggestions> Empty()
        {
            return () => _empty;
        }

        public static Suggestions Merge(string command, ICollection<Suggestions> input)
        {
            if (input.Count == 0)
                return _empty;
            if (input.Count == 1) return input.First();

            ISet<Suggestion> texts = new HashSet<Suggestion>();
            foreach (var suggestions in input)
                // ORIGINAL LINE: texts.addAll(suggestions.getList());
                CollectionsHelper.AddAll(texts, suggestions.List);
            return Create(command, texts.OrderBy(c => c.Text).ToList());
        }

        public static Suggestions Create(string command, ICollection<Suggestion> suggestions)
        {
            if (suggestions.Count == 0) return _empty;
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
            IList<Suggestion> sorted = texts.Sort((a, b) => a.CompareToIgnoreCase(b)).ToList();
            return new Suggestions(range, sorted);
        }
    }
}