﻿using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System;
using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.suggestion
{
	using Message = com.mojang.brigadier.Message;
	using StringRange = com.mojang.brigadier.context.StringRange;

	public class Suggestion : IComparable<Suggestion>
	{
		private StringRange range;
		private string text;
		private Message tooltip;

		public Suggestion(StringRange range, string text) : this(range, text, null)
		{
		}

		public Suggestion(StringRange range, string text, Message tooltip)
		{
			this.range = range;
			this.text = text;
			this.tooltip = tooltip;
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public virtual string Text
		{
			get
			{
				return text;
			}
		}

		public virtual Message Tooltip
		{
			get
			{
				return tooltip;
			}
		}

		public virtual string apply(string input)
		{
			if (range.Start == 0 && range.End == input.Length)
			{
				return text;
			}
			 StringBuilder result = new StringBuilder();
			if (range.Start > 0)
			{
				result.Append(input.Substring(0, range.Start));
			}
			result.Append(text);
			if (range.End < input.Length)
			{
				result.Append(input.Substring(range.End));
			}
			return result.ToString();
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is Suggestion))
			{
				return false;
			}
			 Suggestion that = (Suggestion) o;
			return ObjectsHelper.Equals(range, that.range) && ObjectsHelper.Equals(text, that.text) && ObjectsHelper.Equals(tooltip, that.tooltip);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.hash(range, text, tooltip);
		}

		public override string ToString()
		{
			return "Suggestion{" + "range=" + range + ", text='" + text + '\'' + ", tooltip='" + tooltip + '\'' + '}';
		}

		public virtual int CompareTo(Suggestion o)
		{
			return text.CompareTo(o.text);
		}

		public virtual int compareToIgnoreCase(Suggestion b)
		{
			return string.Compare(text, b.text, StringComparison.OrdinalIgnoreCase);
		}

		public virtual Suggestion expand(string command, StringRange range)
		{
			if (range.Equals(this.range))
			{
				return this;
			}
			 StringBuilder result = new StringBuilder();
			if (range.Start < this.range.Start)
			{
				result.Append(StringHelper.SubstringSpecial(command, range.Start, this.range.Start));
			}
			result.Append(text);
			if (range.End > this.range.End)
			{
				result.Append(StringHelper.SubstringSpecial(command, this.range.End, range.End));
			}
			return new Suggestion(range, result.ToString(), tooltip);
		}
	}

}