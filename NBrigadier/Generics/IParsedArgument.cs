using NBrigadier.Context;

namespace NBrigadier.Generics
{
    public interface IParsedArgument
    {
        StringRange Range { get; }

        object ResultObject { get; }
    }
}