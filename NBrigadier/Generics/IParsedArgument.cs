using com.mojang.brigadier.context;

namespace NBrigadier.Generics
{
    public interface IParsedArgument
    {
        StringRange Range { get; }
        
        object ResultObject { get; }
    }
}