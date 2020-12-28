using com.mojang.brigadier.context;

namespace NBrigadier.Generics
{
    public interface ICommandContext
    {
        StringRange Range { get; }
        string Input { get; }
        bool Forked { get; }
        bool hasNodes();
    }
}