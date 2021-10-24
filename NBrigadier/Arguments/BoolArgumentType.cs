using System;
using System.Collections.Generic;
using System.Globalization;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class BoolArgumentType : IArgumentType<bool>
    {
        private static readonly ICollection<string> _examples = CollectionsHelper.AsList("true", "false");

        private BoolArgumentType()
        {
        }

        public virtual bool Parse(StringReader reader)
        {
            return reader.ReadBoolean();
        }

        public virtual Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            if ("true".StartsWith(builder.Remaining.ToLower(CultureInfo.InvariantCulture), StringComparison.Ordinal)) builder.Suggest("true");
            if ("false".StartsWith(builder.Remaining.ToLower(CultureInfo.InvariantCulture), StringComparison.Ordinal)) builder.Suggest("false");
            return builder.BuildFuture();
        }

        public virtual ICollection<string> Examples => _examples;

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