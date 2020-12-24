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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public Suggestions(final com.mojang.brigadier.context.StringRange range, final java.util.List<Suggestion> suggestions)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Suggestions that = (Suggestions) o;
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static Suggestions merge(final String command, final java.util.Collection<Suggestions> input)
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

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Set<Suggestion> texts = new java.util.HashSet<>();
            HashSet<Suggestion> texts = new HashSet<Suggestion>();
			foreach (Suggestions suggestions in input)
            {
                foreach (var suggestion in suggestions.List) texts.Add(suggestion);
            }
			return Create(command, texts);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static Suggestions create(final String command, final java.util.Collection<Suggestion> suggestions)
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.StringRange range = new com.mojang.brigadier.context.StringRange(start, end);
			StringRange range = new StringRange(start, end);
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Set<Suggestion> texts = new java.util.HashSet<>();
			ISet<Suggestion> texts = new HashSet<Suggestion>();
			foreach (Suggestion suggestion in suggestions)
			{
				texts.Add(suggestion.Expand(command, range));
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<Suggestion> sorted = new java.util.ArrayList<>(texts);
			IList<Suggestion> sorted = texts.OrderBy(a => a, Comparer<Suggestion>.Create((a, b) => a.CompareToIgnoreCase(b))).ToList();
			return new Suggestions(range, sorted);
		}
	}

}