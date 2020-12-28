using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.CommandSuggestion;
using NBrigadier.Tree;
using NUnit.Framework;

namespace NBrigadier.Test
{
    public class CommandSuggestionsTest
    {
        private object source = new();
        private CommandDispatcher<object> _subject = new();

        [SetUp]
        public void SetupTest()
        {
            _subject = new CommandDispatcher<object>();
        }

        private void TestSuggestions(string contents, int cursor, StringRange range, params string[] suggestions)
        {
            Suggestions result = _subject.GetCompletionSuggestions(_subject.Parse(contents, source), cursor).Invoke();
            Assert.AreEqual(result.Range, (range));

            IList<CommandSuggestion.Suggestion> expected = new List<CommandSuggestion.Suggestion>();
            foreach (string suggestion in suggestions)
            {
                expected.Add(new CommandSuggestion.Suggestion(range, suggestion));
            }

            Assert.AreEqual(result.List, (expected));
        }

        private static StringReader InputWithOffset(string input, int offset)
        {
            StringReader result = new StringReader(input);
            result.Cursor = offset;
            return result;
        }

        [Test]
        public void GetCompletionSuggestions_rootCommands()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("bar"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("baz"));

            Suggestions result = _subject.GetCompletionSuggestions(_subject.Parse("", source))();

            Assert.AreEqual(result.Range, (StringRange.At(0)));
            Assert.AreEqual(result.List, new List<CommandSuggestion.Suggestion>
            {
                new(StringRange.At(0), "bar"),
                new(StringRange.At(0), "baz"),
                new(StringRange.At(0), "foo"),
            });
        }

        [Test]
        public virtual void GetCompletionSuggestions_rootCommands_withInputOffset()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("bar"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("baz"));

            Suggestions result = _subject.GetCompletionSuggestions(_subject.Parse(InputWithOffset("OOO", 3), source))();

            Assert.AreEqual(result.Range, (StringRange.At(3)));
            Assert.AreEqual(result.List, new List<Suggestion>
            {
                new(StringRange.At(3), "bar"),
                new(StringRange.At(3), "baz"),
                new(StringRange.At(3), "foo")
            });
        }

        [Test]
        public virtual void GetCompletionSuggestions_rootCommands_partial()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("bar"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("baz"));

            Suggestions result = _subject.GetCompletionSuggestions(_subject.Parse("b", source)).Invoke();

            Assert.AreEqual(result.Range, (StringRange.Between(0, 1)));
            Suggestion[] values = {new(StringRange.Between(0, 1), "bar"), new(StringRange.Between(0, 1), "baz")};
            Assert.AreEqual(result.List,
                values.ToList());
        }

        [Test]
        public virtual void GetCompletionSuggestions_rootCommands_partial_withInputOffset()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("foo"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("bar"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("baz"));

            Suggestions result = _subject.GetCompletionSuggestions(_subject.Parse(InputWithOffset("Zb", 1), source))
                .Invoke();

            Assert.AreEqual(result.Range, (StringRange.Between(1, 2)));
            Suggestion[] values = {new(StringRange.Between(1, 2), "bar"), new(StringRange.Between(1, 2), "baz")};
            Assert.AreEqual(result.List,
                values.ToList());
        }

        private List<T> NewList<T>(params T[] values)
        {
            return values.ToList();
        }


        [Test]
        public virtual void GetCompletionSuggestions_subCommands()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("parent")
                .Then(LiteralArgumentBuilder<object>.Literal("foo").Build())
                .Then(LiteralArgumentBuilder<object>.Literal("bar").Build())
                .Then(LiteralArgumentBuilder<object>.Literal("baz").Build()));

            Suggestions result = _subject.GetCompletionSuggestions(_subject.Parse("parent ", source)).Invoke();

            Assert.AreEqual(result.Range, (StringRange.At(7)));
            Suggestion[] values =
                {new(StringRange.At(7), "bar"), new(StringRange.At(7), "baz"), new(StringRange.At(7), "foo")};
            Assert.AreEqual(result.List,
                values.ToList());
        }

        [Test]
        public virtual void GetCompletionSuggestions_movingCursor_subCommands()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("parent_one")
                .Then(LiteralArgumentBuilder<object>.Literal("faz").Build())
                .Then(LiteralArgumentBuilder<object>.Literal("fbz").Build())
                .Then(LiteralArgumentBuilder<object>.Literal("gaz").Build()));

            _subject.Register(LiteralArgumentBuilder<object>.Literal("parent_two"));

            TestSuggestions("parent_one faz ", 0, StringRange.At(0), "parent_one", "parent_two");
            TestSuggestions("parent_one faz ", 1, StringRange.Between(0, 1), "parent_one", "parent_two");
            TestSuggestions("parent_one faz ", 7, StringRange.Between(0, 7), "parent_one", "parent_two");
            TestSuggestions("parent_one faz ", 8, StringRange.Between(0, 8), "parent_one");
            TestSuggestions("parent_one faz ", 10, StringRange.At(0));
            TestSuggestions("parent_one faz ", 11, StringRange.At(11), "faz", "fbz", "gaz");
            TestSuggestions("parent_one faz ", 12, StringRange.Between(11, 12), "faz", "fbz");
            TestSuggestions("parent_one faz ", 13, StringRange.Between(11, 13), "faz");
            TestSuggestions("parent_one faz ", 14, StringRange.At(0));
            TestSuggestions("parent_one faz ", 15, StringRange.At(0));
        }

        [Test]
        public virtual void GetCompletionSuggestions_subCommands_partial()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("parent")
                .Then(LiteralArgumentBuilder<object>.Literal("foo").Build())
                .Then(LiteralArgumentBuilder<object>.Literal("bar").Build())
                .Then(LiteralArgumentBuilder<object>.Literal("baz").Build()));

            ParseResults<object> parse = _subject.Parse("parent b", source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.Range, (StringRange.Between(7, 8)));
            Suggestion[] values = {new(StringRange.Between(7, 8), "bar"), new(StringRange.Between(7, 8), "baz")};
            Assert.AreEqual(result.List,
                values.ToList());
        }


        [Test]
        public virtual void GetCompletionSuggestions_subCommands_partial_withInputOffset()
        {
            _subject.Register(LiteralArgumentBuilder<object>.Literal("parent")
                .Then(LiteralArgumentBuilder<object>.Literal("foo")).Then(LiteralArgumentBuilder<object>.Literal("bar"))
                .Then(LiteralArgumentBuilder<object>.Literal("baz")));

            ParseResults<object> parse = _subject.Parse(InputWithOffset("junk parent b", 5), source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.Range, (StringRange.Between(12, 13)));
            Assert.AreEqual(result.List,
                (NewList(new Suggestion(StringRange.Between(12, 13), "bar"),
                    new Suggestion(StringRange.Between(12, 13), "baz"))));
        }

        [Test]
        public virtual void GetCompletionSuggestions_redirect()
        {
            LiteralCommandNode<object> actual =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual")
                    .Then(LiteralArgumentBuilder<object>.Literal("sub")));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect").Redirect(actual));

            ParseResults<object> parse = _subject.Parse("redirect ", source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.Range, (StringRange.At(9)));
            Assert.AreEqual(result.List, (NewList(new Suggestion(StringRange.At(9), "sub"))));
        }

        [Test]
        public virtual void GetCompletionSuggestions_redirectPartial()
        {
            LiteralCommandNode<object> actual =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual")
                    .Then(LiteralArgumentBuilder<object>.Literal("sub")));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect").Redirect(actual));

            ParseResults<object> parse = _subject.Parse("redirect s", source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.Range, (StringRange.Between(9, 10)));
            Assert.AreEqual(result.List, (NewList(new Suggestion(StringRange.Between(9, 10), "sub"))));
        }

        public virtual void GetCompletionSuggestions_movingCursor_redirect()
        {
            LiteralCommandNode<object> actualOne =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual_one")
                    .Then(LiteralArgumentBuilder<object>.Literal("faz"))
                    .Then(LiteralArgumentBuilder<object>.Literal("fbz"))
                    .Then(LiteralArgumentBuilder<object>.Literal("gaz")));

            LiteralCommandNode<object> actualTwo =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual_two"));

            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect_one").Redirect(actualOne));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect_two").Redirect(actualOne));

            TestSuggestions("redirect_one faz ", 0, StringRange.At(0), "actual_one", "actual_two", "redirect_one",
                "redirect_two");
            TestSuggestions("redirect_one faz ", 9, StringRange.Between(0, 9), "redirect_one", "redirect_two");
            TestSuggestions("redirect_one faz ", 10, StringRange.Between(0, 10), "redirect_one");
            TestSuggestions("redirect_one faz ", 12, StringRange.At(0));
            TestSuggestions("redirect_one faz ", 13, StringRange.At(13), "faz", "fbz", "gaz");
            TestSuggestions("redirect_one faz ", 14, StringRange.Between(13, 14), "faz", "fbz");
            TestSuggestions("redirect_one faz ", 15, StringRange.Between(13, 15), "faz");
            TestSuggestions("redirect_one faz ", 16, StringRange.At(0));
            TestSuggestions("redirect_one faz ", 17, StringRange.At(0));
        }

        [Test]
        public virtual void GetCompletionSuggestions_redirectPartial_withInputOffset()
        {
            LiteralCommandNode<object> actual =
                _subject.Register(LiteralArgumentBuilder<object>.Literal("actual")
                    .Then(LiteralArgumentBuilder<object>.Literal("sub")));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect").Redirect(actual));

            ParseResults<object> parse = _subject.Parse(InputWithOffset("/redirect s", 1), source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.Range, (StringRange.Between(10, 11)));
            Assert.AreEqual(result.List, (NewList(new Suggestion(StringRange.Between(10, 11), "sub"))));
        }

        [Test]
        public virtual void GetCompletionSuggestions_redirect_lots()
        {
            LiteralCommandNode<object> loop = _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("redirect")
                .Then(LiteralArgumentBuilder<object>.Literal("loop").Then(RequiredArgumentBuilder<object, int>
                    .Argument("loop", IntegerArgumentType.Integer()).Redirect(loop))));

            Suggestions result = _subject
                .GetCompletionSuggestions(_subject.Parse("redirect loop 1 loop 02 loop 003 ", source)).Invoke();

            Assert.AreEqual(result.Range, (StringRange.At(33)));
            Assert.AreEqual(result.List, (NewList(new Suggestion(StringRange.At(33), "loop"))));
        }

        [Test]
        public virtual void GetCompletionSuggestions_execute_simulation()
        {
            LiteralCommandNode<object> execute = _subject.Register(LiteralArgumentBuilder<object>.Literal("execute"));
            _subject.Register(LiteralArgumentBuilder<object>.Literal("execute")
                .Then(LiteralArgumentBuilder<object>.Literal("as").Then(RequiredArgumentBuilder<object, string>
                    .Argument("name", StringArgumentType.Word()).Redirect(execute)))
                .Then(LiteralArgumentBuilder<object>.Literal("store").Then(RequiredArgumentBuilder<object, string>
                    .Argument("name", StringArgumentType.Word()).Redirect(execute)))
                .Then(LiteralArgumentBuilder<object>.Literal("run").Executes(c => 0)));

            ParseResults<object> parse = _subject.Parse("execute as Dinnerbone as", source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.IsEmpty, (true));
        }

        [Test]
        public virtual void GetCompletionSuggestions_execute_simulation_partial()
        {
            var execute = _subject.Register(LiteralArgumentBuilder<object>.Literal("execute"));

            _subject.Register(LiteralArgumentBuilder<object>.Literal("execute")
                .Then(LiteralArgumentBuilder<object>.Literal("as")
                    .Then(LiteralArgumentBuilder<object>.Literal("bar").Redirect(execute))
                    .Then(LiteralArgumentBuilder<object>.Literal("baz").Redirect(execute)))
                .Then(LiteralArgumentBuilder<object>.Literal("store").Then(RequiredArgumentBuilder<object, string>
                    .Argument("name", StringArgumentType.Word()).Redirect(execute)))
                .Then(LiteralArgumentBuilder<object>.Literal("run").Executes(c => 0)));


            ParseResults<object> parse = _subject.Parse("execute as bar as ", source);
            Suggestions result = _subject.GetCompletionSuggestions(parse).Invoke();

            Assert.AreEqual(result.Range, (StringRange.At(18)));
            Assert.AreEqual(result.List,
                (NewList(new Suggestion(StringRange.At(18), "bar"),
                    new Suggestion(StringRange.At(18), "baz"))));
        }
    }
}