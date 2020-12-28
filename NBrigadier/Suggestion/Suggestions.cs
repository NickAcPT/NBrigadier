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
		private static Suggestions _empty = new Suggestions(StringRange.At(0), new List<Suggestion>());

		private StringRange _range;
		private IList<Suggestion> _suggestions;

		public Suggestions(StringRange range, IList<Suggestion> suggestions)
		{
			this._range = range;
			this._suggestions = suggestions;
		}

		public virtual StringRange Range
		{
			get
			{
				return _range;
			}
		}

		public virtual IList<Suggestion> List
		{
			get
			{
				return _suggestions;
			}
		}

		public virtual bool IsEmpty
		{
			get
			{
				return _suggestions.Count == 0;
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
			return ObjectsHelper.Equals(_range, that._range) && ObjectsHelper.Equals(_suggestions, that._suggestions);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.Hash(_range, _suggestions);
		}

		public override string ToString()
		{
			return "Suggestions{" + "range=" + _range + ", suggestions=" + _suggestions + '}';
		}

		public static System.Func<Suggestions> Empty()
		{
			return () => (_empty);
		}

		public static Suggestions Merge(string command, ICollection<Suggestions> input)
		{
			if (input.Count == 0)
			{
				return _empty;
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
			return Create(command, texts);
		}

		public static Suggestions Create(string command, ICollection<Suggestion> suggestions)
		{
			if (suggestions.Count == 0)
			{
				return _empty;
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
			 IList<Suggestion> sorted = new List<Suggestion>(texts);
			sorted.Sort((a, b) => a.CompareToIgnoreCase(b));
			return new Suggestions(range, sorted);
		}
	}

}