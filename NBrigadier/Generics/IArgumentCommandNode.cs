using System.Collections.Generic;
using NBrigadier.Suggestion;
using NBrigadier.Tree;

namespace NBrigadier.Generics
{
    public interface IArgumentCommandNode<S>
    {
        SuggestionProvider<S> CustomSuggestions { get; }
        Command<S> Command { get; }
        ICollection<CommandNode<S>> Children { get; }
        CommandNode<S> Redirect { get; }
        RedirectModifier<S> RedirectModifier { get; }
        System.Predicate<S> Requirement { get; }
        bool Fork { get; }
        CommandNode<S> getChild(string name);
        bool canUse(S source);
        void addChild(CommandNode<S> node);
        void findAmbiguities(AmbiguityConsumer<S> consumer);
        ICollection<CommandNode<S>> getRelevantNodes(StringReader input);
    }
}