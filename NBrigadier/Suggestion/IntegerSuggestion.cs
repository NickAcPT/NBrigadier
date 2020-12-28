using System;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
	using Message = Message;
	using StringRange = StringRange;

	public class IntegerSuggestion : Suggestion
	{
		private int value;

		public IntegerSuggestion(StringRange range, int value) : this(range, value, null)
		{
		}

		public IntegerSuggestion(StringRange range, int value, Message tooltip) : base(range, Convert.ToString(value), tooltip)
		{
			this.value = value;
		}

		public virtual int Value
		{
			get
			{
				return value;
			}
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is IntegerSuggestion))
			{
				return false;
			}
			 IntegerSuggestion that = (IntegerSuggestion) o;
			return value == that.value && base.Equals(o);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.hash(base.GetHashCode(), value);
		}

		public override string ToString()
		{
			return "IntegerSuggestion{" + "value=" + value + ", range=" + Range + ", text='" + Text + '\'' + ", tooltip='" + Tooltip + '\'' + '}';
		}

		public override int CompareTo(Suggestion o)
		{
			if (o is IntegerSuggestion)
			{
				return (value).CompareTo(((IntegerSuggestion) o).value);
			}
			return base.CompareTo(o);
		}

		public override int compareToIgnoreCase(Suggestion b)
		{
			return CompareTo(b);
		}
	}

}