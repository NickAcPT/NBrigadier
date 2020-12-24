using System;
using System.Collections.Generic;
using System.Linq;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using StringRange = com.mojang.brigadier.context.StringRange;


	public class Suggestions
	{
		private static readonly Suggestions EMPTY = new Suggestions(StringRange.At(0), new List<Suggestion>());

		private readonly StringRange range;
		private readonly IList<Suggestion> suggestions;

		public Suggestions(StringRange range, IList<Suggestion> suggestions)
		{
			this.range = range;
			this.suggestions = suggestions;
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public virtual IList<Suggestion> List
		{
			get
			{
				return suggestions;
			}
		}

		public virtual bool IsEmpty
		{
			get
			{
				return suggestions.Count == 0;
			}
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is Suggestions))
			{
				return false;
			}
			Suggestions that = (Suggestions) o;
			return object.Equals(range, that.range) && object.Equals(suggestions, that.suggestions);
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
			{
				return EMPTY;
			}
			else if (input.Count == 1)
			{
				return input.First();
			}

            HashSet<Suggestion> texts = new HashSet<Suggestion>();
			foreach (Suggestions suggestions in input)
            {
                foreach (var suggestion in suggestions.List) texts.Add(suggestion);
            }
			return Create(command, texts);
		}

		public static Suggestions Create(string command, ICollection<Suggestion> suggestions)
		{
			if (suggestions.Count == 0)
			{
				return EMPTY;
			}
			int start = int.MaxValue;
			int end = int.MinValue;
			foreach (Suggestion suggestion in suggestions)
			{
				start = Math.Min(suggestion.Range.Start, start);
				end = Math.Max(suggestion.Range.End, end);
			}
			StringRange range = new StringRange(start, end);
			ISet<Suggestion> texts = new HashSet<Suggestion>();
			foreach (Suggestion suggestion in suggestions)
			{
				texts.Add(suggestion.Expand(command, range));
			}
			IList<Suggestion> sorted = texts.OrderBy(a => a, Comparer<Suggestion>.Create((a, b) => a.CompareToIgnoreCase(b))).ToList();
			return new Suggestions(range, sorted);
		}
	}

}