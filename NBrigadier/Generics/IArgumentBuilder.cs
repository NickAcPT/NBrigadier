using System;
using System.Collections.Generic;
using NBrigadier.Tree;

namespace NBrigadier.Generics
{
    public interface IArgumentBuilder<TS>
    {
        ICollection<CommandNode<TS>> Arguments { get; }
        Command<TS> Command { get; }
        Predicate<TS> Requirement { get; }
        CommandNode<TS> RedirectTarget { get; }
        RedirectModifier<TS> RedirectModifier { get; }
        bool HasFork { get; }
        CommandNode<TS> Build();
    }
}