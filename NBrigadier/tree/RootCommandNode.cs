using System;
using System.Collections.Generic;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class RootCommandNode<S> : CommandNode<S>
    {
        public RootCommandNode() : base(null, c => true, null, s => new List<S> {s.Source}, false)
        {
        }

        public override string Name => "";

        public override string UsageText => "";

        protected internal override string SortedKey => "";

        public override ICollection<string> Examples => new List<string>();

        public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
        {
        }

        public override Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
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
            if (!(o is RootCommandNode<S>)) return false;
            return base.Equals(o);
        }

        public override ArgumentBuilder<S, T> CreateBuilder<T>()
        {
            throw new InvalidOperationException("Cannot convert root into a builder");
        }

        public override string ToString()
        {
            return "<root>";
        }
    }
}