using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
	using StringRange = StringRange;


	public class Suggestions
	{
		private static Suggestions EMPTY = new Suggestions(StringRange.at(0), new List<Suggestion>());

		private StringRange range;
		private IList<Suggestion> suggestions;

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

		public virtual bool Empty
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
			return ObjectsHelper.Equals(range, that.range) && ObjectsHelper.Equals(suggestions, that.suggestions);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.hash(range, suggestions);
		}

		public override string ToString()
		{
			return "Suggestions{" + "range=" + range + ", suggestions=" + suggestions + '}';
		}

		public static System.Func<Suggestions> empty()
		{
			return () => (EMPTY);
		}

		public static Suggestions merge(string command, ICollection<Suggestions> input)
		{
			if (input.Count == 0)
			{
				return EMPTY;
			}
			else if (input.Count == 1)
			{
				return input.First();
			}

			 ISet<Suggestion> texts = new HashSet<Suggestion>();
			foreach (Suggestions suggestions in input)
			{
// ORIGINAL LINE: texts.addAll(suggestions.getList());
				CollectionsHelper.AddAll(texts, suggestions.List);
			}
			return create(command, texts);
		}

		public static Suggestions create(string command, ICollection<Suggestion> suggestions)
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
				texts.Add(suggestion.expand(command, range));
			}
			 IList<Suggestion> sorted = new List<Suggestion>(texts);
			sorted.Sort((a, b) => a.compareToIgnoreCase(b));
			return new Suggestions(range, sorted);
		}
	}

}