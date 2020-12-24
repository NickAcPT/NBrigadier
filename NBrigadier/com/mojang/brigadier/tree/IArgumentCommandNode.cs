using com.mojang.brigadier.arguments;
using com.mojang.brigadier.context;
using com.mojang.brigadier.suggestion;
using System;
using System.Collections.Generic;

namespace com.mojang.brigadier.tree
{
    public interface IArgumentCommandNode<S>
    {
        SuggestionProvider<S> CustomSuggestions { get; }
        ICollection<string> Examples { get; }
        string Name { get; }
        string UsageText { get; }

        bool Equals(object o);
        int GetHashCode();
        bool IsValidInput(string input);
        Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder);
        void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder);
        string ToString();
    }
}