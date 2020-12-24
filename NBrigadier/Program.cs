using System;
using NBrigadier;
using NBrigadier.Arguments;
using NBrigadier.Builder;

Console.WriteLine("Hello world");
var dispatcher = new CommandDispatcher<string>();
dispatcher.Register(
    LiteralArgumentBuilder<string>.LiteralBuilder("test")
        .Then(
            RequiredArgumentBuilder<string, int>.Argument("myInt", IntegerArgumentType.Integer())
                .Executes(
                    command =>
                    {
                        Console.WriteLine($"Hello world. Received {command.GetArgument<int>("myInt")} from the user!");

                        return 0;
                    })
        )
);
dispatcher.Execute("test 1", "NickAc");