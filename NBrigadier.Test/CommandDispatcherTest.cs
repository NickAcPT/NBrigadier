using System.Collections.Generic;
using System.Linq;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Tree;
using NUnit.Framework;

namespace NBrigadier.Test
{
    public class CommandDispatcherTest
    {
        private CommandDispatcher<object> _subject;
        private Command<object> command;
        private object source;

        [SetUp]
        public virtual void SetUp()
        {
            _subject = new CommandDispatcher<object>();
            command = context => 42;
            source = new object();
        }

        private static StringReader InputWithOffset(string input, int offset)
        {
            StringReader result = new StringReader(input);
            result.Cursor = offset;
            return result;
        }

        [Test]
        public virtual void TestCreateAndExecuteCommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command));

            Assert.AreEqual(_subject.Execute("foo", source), (42));
        }

        [Test]
        public virtual void TestCreateAndExecuteOffsetCommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command));

            Assert.AreEqual(_subject.Execute(InputWithOffset("/foo", 1), source), (42));
        }

        [Test]
        public virtual void TestCreateAndMergeCommands()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("base")
                .Then(LiteralArgumentBuilder<object>.Literal("foo").Executes(command)));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("base")
                .Then(LiteralArgumentBuilder<object>.Literal("bar").Executes(command)));

            Assert.AreEqual(_subject.Execute("base foo", source), (42));
            Assert.AreEqual(_subject.Execute("base bar", source), (42));
        }

        [Test]
        public virtual void TestExecuteUnknownCommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("bar"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("baz"));

            try
            {
                _subject.Execute("foo", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void TestExecuteImpermissibleCommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Requires(s => false));

            try
            {
                _subject.Execute("foo", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void TestExecuteEmptyCommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal(""));

            try
            {
                _subject.Execute("", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void TestExecuteUnknownSubcommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command));

            try
            {
                _subject.Execute("foo bar", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownArgument()));
                Assert.AreEqual(ex.Cursor, (4));
            }
        }

        [Test]
        public virtual void TestExecuteIncorrectLiteral()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command)
                .Then(LiteralArgumentBuilder<object>.Literal("bar")));

            try
            {
                _subject.Execute("foo baz", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownArgument()));
                Assert.AreEqual(ex.Cursor, (4));
            }
        }

        [Test]
        public virtual void TestExecuteAmbiguousIncorrectArgument()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command)
                .Then(LiteralArgumentBuilder<object>.Literal("bar"))
                .Then(LiteralArgumentBuilder<object>.Literal("baz")));

            try
            {
                _subject.Execute("foo unknown", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownArgument()));
                Assert.AreEqual(ex.Cursor, (4));
            }
        }

        [Test]
        public virtual void TestExecuteSubcommand()
        {
            Command<object> subCommand = _ => 100;

            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(LiteralArgumentBuilder<object>.Literal("a"))
                .Then(LiteralArgumentBuilder<object>.Literal("=").Executes(subCommand))
                .Then(LiteralArgumentBuilder<object>.Literal("c")).Executes(command));

            Assert.AreEqual(_subject.Execute("foo =", source), (100));
        }

        [Test]
        public virtual void TestParseIncompleteLiteral()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(LiteralArgumentBuilder<object>.Literal("bar").Executes(command)));

            ParseResults<object> parse = _subject.Parse("foo ", source);
            Assert.AreEqual(parse.Reader.Remaining, (" "));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
        }

        [Test]
        public virtual void TestParseIncompleteArgument()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(RequiredArgumentBuilder<object, int>
                .Argument("bar", IntegerArgumentType.Integer()).Executes(command)));

            ParseResults<object> parse = _subject.Parse("foo ", source);
            Assert.AreEqual(parse.Reader.Remaining, (" "));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
        }

        [Test]
        public virtual void TestExecuteAmbiguiousParentSubcommand()
        {
            Command<object> subCommand = _ => 100;

            _subject.Register(LiteralArgumentBuilder<object>.Literal("test")
                .Then(RequiredArgumentBuilder<object, int>.Argument("incorrect", IntegerArgumentType.Integer())
                    .Executes(command))
                .Then(RequiredArgumentBuilder<object, int>
                    .Argument("right", IntegerArgumentType.Integer()).Then(RequiredArgumentBuilder<object, int>
                        .Argument("sub", IntegerArgumentType.Integer()).Executes(subCommand))));

            Assert.AreEqual(_subject.Execute("test 1 2", source), (100));
        }

        [Test]
        public virtual void TestExecuteAmbiguiousParentSubcommandViaRedirect()
        {
            Command<object> subCommand = _ => 100;

            LiteralCommandNode<object> real = _subject.Register(LiteralArgumentBuilder<object>.Literal("test")
                .Then(RequiredArgumentBuilder<object, int>.Argument("incorrect", IntegerArgumentType.Integer())
                    .Executes(command)).Then(RequiredArgumentBuilder<object, int>
                    .Argument("right", IntegerArgumentType.Integer()).Then(RequiredArgumentBuilder<object, int>
                        .Argument("sub", IntegerArgumentType.Integer()).Executes(subCommand))));

            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect").Redirect(real));

            Assert.AreEqual(_subject.Execute("redirect 1 2", source), (100));
        }

        [Test]
        public virtual void TestExecuteRedirectedMultipleTimes()
        {
            LiteralCommandNode<object> concreteNode =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual").Executes(command));
            LiteralCommandNode<object> redirectNode =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("redirected").Redirect(_subject.Root));

            string input = "redirected redirected actual";

            ParseResults<object> parse = _subject.Parse(input, source);
            Assert.AreEqual(parse.Context.Range.Get(input), ("redirected"));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
            Assert.AreEqual(parse.Context.RootNode, (_subject.Root));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Range, (parse.Context.Range));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Node, (redirectNode));

            CommandContextBuilder<object> child1 = parse.Context.Child;
            Assert.NotNull(child1);
            Assert.AreEqual(child1.Range.Get(input), ("redirected"));
            Assert.AreEqual(child1.Nodes.Count, (1));
            Assert.AreEqual(child1.RootNode, (_subject.Root));
            Assert.AreEqual(child1.Nodes.ElementAt(0).Range, (child1.Range));
            Assert.AreEqual(child1.Nodes.ElementAt(0).Node, (redirectNode));

            CommandContextBuilder<object> child2 = child1.Child;
            Assert.NotNull(child2);
            Assert.AreEqual(child2.Range.Get(input), ("actual"));
            Assert.AreEqual(child2.Nodes.Count, (1));
            Assert.AreEqual(child2.RootNode, (_subject.Root));
            Assert.AreEqual(child2.Nodes.ElementAt(0).Range, (child2.Range));
            Assert.AreEqual(child2.Nodes.ElementAt(0).Node, (concreteNode));

            Assert.AreEqual(_subject.Execute(parse), (42));
        }

        [Test]
        public virtual void TestExecuteRedirected()
        {
            object source1 = new object();
            object source2 = new object();

            RedirectModifier<object> modifier = context => { return NewList(source1, source2); };

            LiteralCommandNode<object> concreteNode =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual").Executes(command));
            LiteralCommandNode<object> redirectNode =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("redirected").Fork(_subject.Root, modifier));

            string input = "redirected actual";
            ParseResults<object> parse = _subject.Parse(input, source);
            Assert.AreEqual(parse.Context.Range.Get(input), ("redirected"));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
            Assert.AreEqual(parse.Context.RootNode, (_subject.Root));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Range, (parse.Context.Range));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Node, (redirectNode));
            Assert.AreEqual(parse.Context.Source, (source));

            CommandContextBuilder<object> parent = parse.Context.Child;
            Assert.That(parent, Is.Not.Null);
            Assert.AreEqual(parent.Range.Get(input), ("actual"));
            Assert.AreEqual(parent.Nodes.Count, (1));
            Assert.AreEqual(parse.Context.RootNode, (_subject.Root));
            Assert.AreEqual(parent.Nodes.ElementAt(0).Range, (parent.Range));
            Assert.AreEqual(parent.Nodes.ElementAt(0).Node, (concreteNode));
            Assert.AreEqual(parent.Source, (source));

            Assert.AreEqual(_subject.Execute(parse), (2));
        }

        [Test]
        public virtual void TestExecuteOrphanedSubcommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(RequiredArgumentBuilder<object, int>.Argument("bar", IntegerArgumentType.Integer()))
                .Executes(command));

            try
            {
                _subject.Execute("foo 5", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()));
                Assert.AreEqual(ex.Cursor, (5));
            }
        }

        [Test]
        public virtual void TestExecuteInvalidOther()
        {
            Command<object> wrongCommand = _ => 0;
            _subject.Register(LiteralArgumentBuilder<object>.Literal("w").Executes(wrongCommand));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("world").Executes(command));

            Assert.AreEqual(_subject.Execute("world", source), (42));
        }

        [Test]
        public virtual void ParseNoSpaceSeparator()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(RequiredArgumentBuilder<object, int>
                .Argument("bar", IntegerArgumentType.Integer()).Executes(command)));

            try
            {
                _subject.Execute("foo$", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void TestExecuteInvalidSubcommand()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(RequiredArgumentBuilder<object, int>.Argument("bar", IntegerArgumentType.Integer()))
                .Executes(command));

            try
            {
                _subject.Execute("foo bar", source);
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Cursor, (4));
            }
        }

        [Test]
        public virtual void TestGetPath()
        {
            LiteralCommandNode<object> bar = LiteralArgumentBuilder<object>.Literal("bar").Build();
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(bar));

            Assert.AreEqual(_subject.GetPath(bar), (NewList("foo", "bar")));
        }

        [Test]
        public virtual void TestFindNodeExists()
        {
            LiteralCommandNode<object> bar = LiteralArgumentBuilder<object>.Literal("bar").Build();
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(bar));

            Assert.AreEqual(_subject.FindNode(NewList("foo", "bar")), (bar));
        }

        [Test]
        public virtual void TestFindNodeDoesntExist()
        {
            Assert.That(_subject.FindNode(NewList("foo", "bar")), Is.Null);
        }



        [Test]
        public virtual void ExecuteCommandSimulationTest()
        {
            var newSource = new object();
            
            _subject.Register(LiteralArgumentBuilder<object>.Literal("hey")
                .Requires(c => newSource == c)
                .Executes(_ => 69));

            var execute = _subject.Register(LiteralArgumentBuilder<object>.Literal("execute"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("execute")
                .Then(LiteralArgumentBuilder<object>.Literal("as").Then(RequiredArgumentBuilder<object, string>
                    .Argument("name", StringArgumentType.Word()).Redirect(execute, context =>
                    {
                        if (context.GetArgument<string>("name") == "Dinnerbone2")
                            return newSource;
                        return context.Source;
                        
                    })))
                .Then(LiteralArgumentBuilder<object>.Literal("run").Redirect(_subject.Root)));

            Assert.Throws(typeof(CommandSyntaxException),
                () => _subject.Execute("execute as Dinnerbone run hey", source));
            Assert.AreEqual(69, _subject.Execute("execute as Dinnerbone2 run hey", source));
        }

        private List<T> NewList<T>(params T[] values)
        {
            return values.ToList();
        }
    }
}