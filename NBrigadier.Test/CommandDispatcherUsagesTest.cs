using System.Collections.Generic;
using System.Linq;
using NBrigadier.Builder;
using NBrigadier.Tree;
using NUnit.Framework;

namespace NBrigadier.Test
{
    public class CommandDispatcherUsagesTest
    {
        private CommandDispatcher<object> subject;
        private object source = new();
        private Command<object> command = context => 42;

        [SetUp]
        public virtual void SetUp()
        {
            subject = new CommandDispatcher<object>();
            subject.Register(LiteralArgumentBuilder<object>.Literal("a")
                .Then(LiteralArgumentBuilder<object>.Literal("1")
                    .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command))
                    .Then(LiteralArgumentBuilder<object>.Literal("ii").Executes(command))).Then(
                    LiteralArgumentBuilder<object>.Literal("2")
                        .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command))
                        .Then(LiteralArgumentBuilder<object>.Literal("ii").Executes(command))));
            subject.Register(LiteralArgumentBuilder<object>.Literal("b")
                .Then(LiteralArgumentBuilder<object>.Literal("1").Executes(command)));
            subject.Register(LiteralArgumentBuilder<object>.Literal("c").Executes(command));
            subject.Register(LiteralArgumentBuilder<object>.Literal("d").Requires(s => false).Executes(command));
            subject.Register(LiteralArgumentBuilder<object>.Literal("e").Executes(command).Then(
                LiteralArgumentBuilder<object>.Literal("1").Executes(command)
                    .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command))
                    .Then(LiteralArgumentBuilder<object>.Literal("ii").Executes(command))));
            subject.Register(LiteralArgumentBuilder<object>.Literal("f")
                .Then(LiteralArgumentBuilder<object>.Literal("1")
                    .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command))
                    .Then(LiteralArgumentBuilder<object>.Literal("ii").Executes(command).Requires(s => false))).Then(
                    LiteralArgumentBuilder<object>.Literal("2")
                        .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command).Requires(s => false))
                        .Then(LiteralArgumentBuilder<object>.Literal("ii").Executes(command))));
            subject.Register(LiteralArgumentBuilder<object>.Literal("g").Executes(command).Then(
                LiteralArgumentBuilder<object>.Literal("1")
                    .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command))));
            subject.Register(LiteralArgumentBuilder<object>.Literal("h").Executes(command)
                .Then(LiteralArgumentBuilder<object>.Literal("1")
                    .Then(LiteralArgumentBuilder<object>.Literal("i").Executes(command)))
                .Then(LiteralArgumentBuilder<object>.Literal("2").Then(LiteralArgumentBuilder<object>.Literal("i")
                    .Then(LiteralArgumentBuilder<object>.Literal("ii").Executes(command))))
                .Then(LiteralArgumentBuilder<object>.Literal("3").Executes(command)));
            subject.Register(LiteralArgumentBuilder<object>.Literal("i").Executes(command)
                .Then(LiteralArgumentBuilder<object>.Literal("1").Executes(command))
                .Then(LiteralArgumentBuilder<object>.Literal("2").Executes(command)));
            subject.Register(LiteralArgumentBuilder<object>.Literal("j").Redirect(subject.Root));
            subject.Register(LiteralArgumentBuilder<object>.Literal("k").Redirect(Get("h")));
        }

        private CommandNode<object> Get(string command)
        {
            return (subject.Parse(command, source).Context.Nodes).Last().Node;
        }

        private CommandNode<object> Get(StringReader command)
        {
            return (subject.Parse(command, source).Context.Nodes).Last().Node;
        }

        [Test]
        public virtual void TestAllUsageNoCommands()
        {
            subject = new CommandDispatcher<object>();
            string[] results = subject.GetAllUsage(subject.Root, source, true);
            Assert.AreEqual(results, (new string[0]));
        }

        [Test]
        public virtual void TestSmartUsageNoCommands()
        {
            subject = new CommandDispatcher<object>();
            IDictionary<CommandNode<object>, string> results = subject.GetSmartUsage(subject.Root, source);
            Assert.AreEqual(results.Count, 0);
        }

        [Test]
        public virtual void TestAllUsageRoot()
        {
            string[] results = subject.GetAllUsage(subject.Root, source, true);
            Assert.AreEqual(results,
                (new string[]
                {
                    "a 1 i", "a 1 ii", "a 2 i", "a 2 ii", "b 1", "c", "e", "e 1", "e 1 i", "e 1 ii", "f 1 i", "f 2 ii",
                    "g", "g 1 i", "h", "h 1 i", "h 2 i ii", "h 3", "i", "i 1", "i 2", "j ...", "k -> h"
                }));
        }

        [Test]
        public virtual void TestSmartUsageRoot()
        {
            IDictionary<CommandNode<object>, string> results = subject.GetSmartUsage(subject.Root, source);
            Assert.AreEqual(results,
                (
                    new Dictionary<CommandNode<object>, string>()
                    {
                        {Get("a"), "a (1|2)"},
                        {Get("b"), "b 1"},
                        {Get("c"), "c"},
                        {Get("e"), "e [1]"},
                        {Get("f"), "f (1|2)"},
                        {Get("g"), "g [1]"},
                        {Get("h"), "h [1|2|3]"},
                        {Get("i"), "i [1|2]"},
                        {Get("j"), "j ..."},
                        {Get("k"), "k -> h"},
                    }
                ));
        }

        [Test]
        public virtual void TestSmartUsageH()
        {
            IDictionary<CommandNode<object>, string> results = subject.GetSmartUsage(Get("h"), source);
            Assert.AreEqual(results, (new Dictionary<CommandNode<object>, string>
            {
                {Get("h 1"), "[1] i"},
                {Get("h 2"), "[2] i ii"},
                {Get("h 3"), "[3]"}
            }));
        }

        [Test]
        public virtual void TestSmartUsageOffsetH()
        {
            StringReader offsetH = new StringReader("/|/|/h");
            offsetH.Cursor = (5);

            IDictionary<CommandNode<object>, string> results = subject.GetSmartUsage(Get(offsetH), source);
            Assert.AreEqual(results, (new Dictionary<CommandNode<object>, string>()
            {
                {
                    Get("h 1"), "[1] i"
                },
                {
                    Get("h 2"), "[2] i ii"
                },
                {
                    Get("h 3"), "[3]"
                },
            }));
        }
    }
}