﻿using System;
using System.Globalization;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.CommandSuggestion
{
    public class IntegerSuggestion : Suggestion
    {
        private readonly int _value;

        public IntegerSuggestion(StringRange range, int value) : this(range, value, null)
        {
        }

        public IntegerSuggestion(StringRange range, int value, IMessage tooltip) : base(range, Convert.ToString(value, CultureInfo.InvariantCulture),
            tooltip)
        {
            _value = value;
        }

        public virtual int Value => _value;

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is IntegerSuggestion)) return false;
            var that = (IntegerSuggestion) o;
            return _value == that._value && base.Equals(o);
        }

        public override int GetHashCode()
        {
            return ObjectsHelper.Hash(base.GetHashCode(), _value);
        }

        public override string ToString()
        {
            return "IntegerSuggestion{" + "value=" + _value + ", range=" + Range + ", text='" + Text + '\'' +
                   ", tooltip='" + Tooltip + '\'' + '}';
        }

        public override int CompareTo(Suggestion o)
        {
            if (o is IntegerSuggestion) return _value.CompareTo(((IntegerSuggestion) o)._value);
            return base.CompareTo(o);
        }

        public override int CompareToIgnoreCase(Suggestion b)
        {
            return CompareTo(b);
        }
    }
}