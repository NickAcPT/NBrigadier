using System;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Suggestion
{
    public class IntegerSuggestion : Suggestion
    {
        private readonly int _value;

        public IntegerSuggestion(StringRange range, int value) : this(range, value, null)
        {
        }

        public IntegerSuggestion(StringRange range, int value, IMessage tooltip) : base(range, Convert.ToString(value),
            tooltip)
        {
            this._value = value;
        }

        public virtual int Value => _value;

        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != this.GetType()) return false;
            return Equals((IntegerSuggestion) o);
        }

        public override string ToString()
        {
            return "IntegerSuggestion{" + "value=" + _value + ", range=" + Range + ", text='" + Text + '\'' +
                   ", tooltip='" + Tooltip + '\'' + '}';
        }

        public new int CompareTo(Suggestion o)
        {
            if (o is IntegerSuggestion suggestion) return _value.CompareTo(suggestion._value);
            return base.CompareTo(o);
        }

        protected bool Equals(IntegerSuggestion other)
        {
            return base.Equals(other) && _value == other._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), _value);
        }

        public static bool operator ==(IntegerSuggestion left, IntegerSuggestion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IntegerSuggestion left, IntegerSuggestion right)
        {
            return !Equals(left, right);
        }

        public override int CompareToIgnoreCase(Suggestion b)
        {
            return CompareTo(b);
        }
    }
}