using System;
using System.Collections.Generic;
using NBrigadier.CommandSuggestion;
using NBrigadier.Tree;

namespace NBrigadier.Generics
{
    public interface IArgumentCommandNode<TS>
    {
        SuggestionProvider<TS> CustomSuggestions { get; }
        Command<TS> Command { get; }
        ICollection<CommandNode<TS>> Children { get; }
        CommandNode<TS> Redirect { get; }
        RedirectModifier<TS> RedirectModifier { get; }
        Predicate<TS> Requirement { get; }
        bool Fork { get; }
        CommandNode<TS> GetChild(string name);
        bool CanUse(TS source);
        void AddChild(CommandNode<TS> node);
        void FindAmbiguities(AmbiguityConsumer<TS> consumer);
        ICollection<CommandNode<TS>> GetRelevantNodes(StringReader input);
    }
}