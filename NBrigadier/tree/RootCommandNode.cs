using System;
using System.Collections.Generic;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class RootCommandNode<TS> : CommandNode<TS>
    {
        public RootCommandNode() : base(null, c => true, null, s => new List<TS> {s.Source}, false)
        {
        }

        public override string Name => "";

        public override string UsageText => "";

        protected internal override string SortedKey => "";

        public override ICollection<string> Examples => new List<string>();

        public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
        {
        }

        public override Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public override bool IsValidInput(string input)
        {
            return false;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is RootCommandNode<TS>)) return false;
            return base.Equals(o);
        }

        public override ArgumentBuilder<TS, T> CreateBuilder<T>()
        {
            throw new InvalidOperationException("Cannot convert root into a builder");
        }

        public override string ToString()
        {
            return "<root>";
        }
    }
}