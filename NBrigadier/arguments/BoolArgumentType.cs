using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class BoolArgumentType : ArgumentType<bool>
    {
        private static readonly ICollection<string> EXAMPLES = new List<string> {"true", "false"};

        private BoolArgumentType()
        {
        }

        public virtual ICollection<string> Examples => EXAMPLES;

        public bool Parse(StringReader reader)
        {
            return reader.ReadBoolean();
        }

        public Func<Suggestions> ListSuggestions<S>(CommandContext<S> context, SuggestionsBuilder builder)
        {
            if ("true".StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal)) builder.Suggest("true");
            if ("false".StartsWith(builder.Remaining.ToLower(), StringComparison.Ordinal)) builder.Suggest("false");
            return builder.BuildFuture();
        }

        public static BoolArgumentType Bool()
        {
            return new();
        }

        public static bool GetBool<T1>(CommandContext<T1> context, string name)
        {
            return context.GetArgument<bool>(name, typeof(bool));
        }
    }
}