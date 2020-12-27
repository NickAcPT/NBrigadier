using com.mojang.brigadier.context;

namespace NBrigadier
{
    public interface ICommandContext
    {
        StringRange Range { get; }
        string Input { get; }
        bool Forked { get; }
        bool hasNodes();
    }
}