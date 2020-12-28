using System.Collections.Generic;
using System.Linq;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Tree;
using NUnit.Framework;

namespace NBrigadier.Test
{
    public class CommandDispatcherTest
    {
        private CommandDispatcher<object> subject;
        private Command<object> command;
        private object source;

        [SetUp]
        public virtual void SetUp()
        {
            subject = new CommandDispatcher<object>();
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command));

            Assert.AreEqual(subject.Execute("foo", source), (42));
        }

        [Test]
        public virtual void TestCreateAndExecuteOffsetCommand()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command));

            Assert.AreEqual(subject.Execute(InputWithOffset("/foo", 1), source), (42));
        }

        [Test]
        public virtual void TestCreateAndMergeCommands()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("base")
                .Then(LiteralArgumentBuilder<object>.Literal("foo").Executes(command)));
            subject.Register(LiteralArgumentBuilder<object>.Literal("base")
                .Then(LiteralArgumentBuilder<object>.Literal("bar").Executes(command)));

            Assert.AreEqual(subject.Execute("base foo", source), (42));
            Assert.AreEqual(subject.Execute("base bar", source), (42));
        }

        [Test]
        public virtual void TestExecuteUnknownCommand()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("bar"));
            subject.Register(LiteralArgumentBuilder<object>.Literal("baz"));

            try
            {
                subject.Execute("foo", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Requires(s => false));

            try
            {
                subject.Execute("foo", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal(""));

            try
            {
                subject.Execute("", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command));

            try
            {
                subject.Execute("foo bar", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command)
                .Then(LiteralArgumentBuilder<object>.Literal("bar")));

            try
            {
                subject.Execute("foo baz", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Executes(command)
                .Then(LiteralArgumentBuilder<object>.Literal("bar"))
                .Then(LiteralArgumentBuilder<object>.Literal("baz")));

            try
            {
                subject.Execute("foo unknown", source);
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

            subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(LiteralArgumentBuilder<object>.Literal("a"))
                .Then(LiteralArgumentBuilder<object>.Literal("=").Executes(subCommand))
                .Then(LiteralArgumentBuilder<object>.Literal("c")).Executes(command));

            Assert.AreEqual(subject.Execute("foo =", source), (100));
        }

        [Test]
        public virtual void TestParseIncompleteLiteral()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(LiteralArgumentBuilder<object>.Literal("bar").Executes(command)));

            ParseResults<object> parse = subject.Parse("foo ", source);
            Assert.AreEqual(parse.Reader.Remaining, (" "));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
        }

        [Test]
        public virtual void TestParseIncompleteArgument()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(RequiredArgumentBuilder<object, int>
                .Argument("bar", IntegerArgumentType.Integer()).Executes(command)));

            ParseResults<object> parse = subject.Parse("foo ", source);
            Assert.AreEqual(parse.Reader.Remaining, (" "));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
        }

        [Test]
        public virtual void TestExecuteAmbiguiousParentSubcommand()
        {
            Command<object> subCommand = _ => 100;

            subject.Register(LiteralArgumentBuilder<object>.Literal("test")
                .Then(RequiredArgumentBuilder<object, int>.Argument("incorrect", IntegerArgumentType.Integer())
                    .Executes(command))
                .Then(RequiredArgumentBuilder<object, int>
                    .Argument("right", IntegerArgumentType.Integer()).Then(RequiredArgumentBuilder<object, int>
                        .Argument("sub", IntegerArgumentType.Integer()).Executes(subCommand))));

            Assert.AreEqual(subject.Execute("test 1 2", source), (100));
        }

        [Test]
        public virtual void TestExecuteAmbiguiousParentSubcommandViaRedirect()
        {
            Command<object> subCommand = _ => 100;

            LiteralCommandNode<object> real = subject.Register(LiteralArgumentBuilder<object>.Literal("test")
                .Then(RequiredArgumentBuilder<object, int>.Argument("incorrect", IntegerArgumentType.Integer())
                    .Executes(command)).Then(RequiredArgumentBuilder<object, int>
                    .Argument("right", IntegerArgumentType.Integer()).Then(RequiredArgumentBuilder<object, int>
                        .Argument("sub", IntegerArgumentType.Integer()).Executes(subCommand))));

            subject.Register(LiteralArgumentBuilder<object>.Literal("redirect").Redirect(real));

            Assert.AreEqual(subject.Execute("redirect 1 2", source), (100));
        }

        [Test]
        public virtual void TestExecuteRedirectedMultipleTimes()
        {
            LiteralCommandNode<object> concreteNode =
                subject.Register(LiteralArgumentBuilder<object>.Literal("actual").Executes(command));
            LiteralCommandNode<object> redirectNode =
                subject.Register(LiteralArgumentBuilder<object>.Literal("redirected").Redirect(subject.Root));

            string input = "redirected redirected actual";

            ParseResults<object> parse = subject.Parse(input, source);
            Assert.AreEqual(parse.Context.Range.Get(input), ("redirected"));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
            Assert.AreEqual(parse.Context.RootNode, (subject.Root));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Range, (parse.Context.Range));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Node, (redirectNode));

            CommandContextBuilder<object> child1 = parse.Context.Child;
            Assert.NotNull(child1);
            Assert.AreEqual(child1.Range.Get(input), ("redirected"));
            Assert.AreEqual(child1.Nodes.Count, (1));
            Assert.AreEqual(child1.RootNode, (subject.Root));
            Assert.AreEqual(child1.Nodes.ElementAt(0).Range, (child1.Range));
            Assert.AreEqual(child1.Nodes.ElementAt(0).Node, (redirectNode));

            CommandContextBuilder<object> child2 = child1.Child;
            Assert.NotNull(child2);
            Assert.AreEqual(child2.Range.Get(input), ("actual"));
            Assert.AreEqual(child2.Nodes.Count, (1));
            Assert.AreEqual(child2.RootNode, (subject.Root));
            Assert.AreEqual(child2.Nodes.ElementAt(0).Range, (child2.Range));
            Assert.AreEqual(child2.Nodes.ElementAt(0).Node, (concreteNode));

            Assert.AreEqual(subject.Execute(parse), (42));
        }

        [Test]
        public virtual void TestExecuteRedirected()
        {
            object source1 = new object();
            object source2 = new object();

            RedirectModifier<object> modifier = context => { return NewList(source1, source2); };

            LiteralCommandNode<object> concreteNode =
                subject.Register(LiteralArgumentBuilder<object>.Literal("actual").Executes(command));
            LiteralCommandNode<object> redirectNode =
                subject.Register(LiteralArgumentBuilder<object>.Literal("redirected").Fork(subject.Root, modifier));

            string input = "redirected actual";
            ParseResults<object> parse = subject.Parse(input, source);
            Assert.AreEqual(parse.Context.Range.Get(input), ("redirected"));
            Assert.AreEqual(parse.Context.Nodes.Count, (1));
            Assert.AreEqual(parse.Context.RootNode, (subject.Root));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Range, (parse.Context.Range));
            Assert.AreEqual(parse.Context.Nodes.ElementAt(0).Node, (redirectNode));
            Assert.AreEqual(parse.Context.Source, (source));

            CommandContextBuilder<object> parent = parse.Context.Child;
            Assert.That(parent, Is.Not.Null);
            Assert.AreEqual(parent.Range.Get(input), ("actual"));
            Assert.AreEqual(parent.Nodes.Count, (1));
            Assert.AreEqual(parse.Context.RootNode, (subject.Root));
            Assert.AreEqual(parent.Nodes.ElementAt(0).Range, (parent.Range));
            Assert.AreEqual(parent.Nodes.ElementAt(0).Node, (concreteNode));
            Assert.AreEqual(parent.Source, (source));

            Assert.AreEqual(subject.Execute(parse), (2));
        }

        [Test]
        public virtual void TestExecuteOrphanedSubcommand()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(RequiredArgumentBuilder<object, int>.Argument("bar", IntegerArgumentType.Integer()))
                .Executes(command));

            try
            {
                subject.Execute("foo 5", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("w").Executes(wrongCommand));
            subject.Register(LiteralArgumentBuilder<object>.Literal("world").Executes(command));

            Assert.AreEqual(subject.Execute("world", source), (42));
        }

        [Test]
        public virtual void ParseNoSpaceSeparator()
        {
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(RequiredArgumentBuilder<object, int>
                .Argument("bar", IntegerArgumentType.Integer()).Executes(command)));

            try
            {
                subject.Execute("foo$", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo")
                .Then(RequiredArgumentBuilder<object, int>.Argument("bar", IntegerArgumentType.Integer()))
                .Executes(command));

            try
            {
                subject.Execute("foo bar", source);
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
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(bar));

            Assert.AreEqual(subject.GetPath(bar), (NewList("foo", "bar")));
        }

        [Test]
        public virtual void TestFindNodeExists()
        {
            LiteralCommandNode<object> bar = LiteralArgumentBuilder<object>.Literal("bar").Build();
            subject.Register(LiteralArgumentBuilder<object>.Literal("foo").Then(bar));

            Assert.AreEqual(subject.FindNode(NewList("foo", "bar")), (bar));
        }

        [Test]
        public virtual void TestFindNodeDoesntExist()
        {
            Assert.That(subject.FindNode(NewList("foo", "bar")), Is.Null);
        }

        private List<T> NewList<T>(params T[] values)
        {
            return values.ToList();
        }
    }
}