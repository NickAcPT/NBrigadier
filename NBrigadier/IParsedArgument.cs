using com.mojang.brigadier.context;

namespace NBrigadier
{
    public interface IParsedArgument
    {
        StringRange Range { get; }
    }
}