using System;
using System.Collections.Generic;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Generics;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public class RootCommandNode<TS> : CommandNode<TS>, IRootCommandNode
    {
        public RootCommandNode() : base(null, c => true, null, s => CollectionsHelper.SingletonList(s.Source), false)
        {
        }

        public override string Name => "";

        public override string UsageText => "";

        protected internal override string SortedKey => "";

        public override ICollection<string> Examples => CollectionsHelper.EmptyList<string>();

        public override void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder)
        {
        }

        public override Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        protected internal override bool IsValidInput(string input)
        {
            return false;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is IRootCommandNode)) return false;
            return base.Equals(o);
        }

        public override IArgumentBuilder<TS> CreateBuilder()
        {
            throw new InvalidOperationException("Cannot convert root into a builder");
        }

        public override string ToString()
        {
            return "<root>";
        }
    }
}