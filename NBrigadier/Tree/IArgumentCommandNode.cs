using System;
using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Suggestion;

namespace NBrigadier.Tree
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