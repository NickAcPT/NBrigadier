using System.Collections.Generic;
using NBrigadier.Tree;

namespace NBrigadier.Generics
{
    public interface IArgumentBuilder<S>
    {
        ICollection<CommandNode<S>> Arguments { get; }
        Command<S> Command { get; }
        System.Predicate<S> Requirement { get; }
        CommandNode<S> Redirect { get; }
        RedirectModifier<S> RedirectModifier { get; }
        bool Fork { get; }
        CommandNode<S> build();
    }
}