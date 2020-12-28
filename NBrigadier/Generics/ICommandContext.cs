using NBrigadier.Context;

namespace NBrigadier.Generics
{
    public interface ICommandContext
    {
        StringRange Range { get; }
        string Input { get; }
        bool Forked { get; }
        bool HasNodes();
    }
}