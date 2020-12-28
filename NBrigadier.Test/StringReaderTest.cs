using NBrigadier.Exceptions;
using NUnit.Framework;

namespace NBrigadier.Test
{
    public class StringReaderTest
    {
        [Test]
        public virtual void CanRead()
        {
            StringReader reader = new StringReader("abc");
            Assert.AreEqual(reader.CanRead(), (true));
            reader.Skip(); // 'a'
            Assert.AreEqual(reader.CanRead(), (true));
            reader.Skip(); // 'b'
            Assert.AreEqual(reader.CanRead(), (true));
            reader.Skip(); // 'c'
            Assert.AreEqual(reader.CanRead(), (false));
        }

        [Test]
        public virtual void GetRemainingLength()
        {
            StringReader reader = new StringReader("abc");
            Assert.AreEqual(reader.RemainingLength, (3));
            reader.Cursor = 1;
            Assert.AreEqual(reader.RemainingLength, (2));
            reader.Cursor = 2;
            Assert.AreEqual(reader.RemainingLength, (1));
            reader.Cursor = 3;
            Assert.AreEqual(reader.RemainingLength, (0));
        }

        [Test]
        public virtual void CanReadLength()
        {
            StringReader reader = new StringReader("abc");
            Assert.AreEqual(reader.CanRead(1), (true));
            Assert.AreEqual(reader.CanRead(2), (true));
            Assert.AreEqual(reader.CanRead(3), (true));
            Assert.AreEqual(reader.CanRead(4), (false));
            Assert.AreEqual(reader.CanRead(5), (false));
        }

        [Test]
        public virtual void Peek()
        {
            StringReader reader = new StringReader("abc");
            Assert.AreEqual(reader.Peek(), ('a'));
            Assert.AreEqual(reader.Cursor, (0));
            reader.Cursor = 2;
            Assert.AreEqual(reader.Peek(), ('c'));
            Assert.AreEqual(reader.Cursor, (2));
        }

        [Test]
        public virtual void PeekLength()
        {
            StringReader reader = new StringReader("abc");
            Assert.AreEqual(reader.Peek(0), ('a'));
            Assert.AreEqual(reader.Peek(2), ('c'));
            Assert.AreEqual(reader.Cursor, (0));
            reader.Cursor = 1;
            Assert.AreEqual(reader.Peek(1), ('c'));
            Assert.AreEqual(reader.Cursor, (1));
        }

        [Test]
        public virtual void Read()
        {
            StringReader reader = new StringReader("abc");
            Assert.AreEqual(reader.Read(), ('a'));
            Assert.AreEqual(reader.Read(), ('b'));
            Assert.AreEqual(reader.Read(), ('c'));
            Assert.AreEqual(reader.Cursor, (3));
        }

        [Test]
        public virtual void Skip()
        {
            StringReader reader = new StringReader("abc");
            reader.Skip();
            Assert.AreEqual(reader.Cursor, (1));
        }

        [Test]
        public virtual void getRemaining()
        {
            StringReader reader = new StringReader("Hello!");
            Assert.AreEqual(reader.Remaining, ("Hello!"));
            reader.Cursor = 3;
            Assert.AreEqual(reader.Remaining, ("lo!"));
            reader.Cursor = 6;
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void getRead()
        {
            StringReader reader = new StringReader("Hello!");
            Assert.AreEqual(reader.PreviouslyReadString, (""));
            reader.Cursor = 3;
            Assert.AreEqual(reader.PreviouslyReadString, ("Hel"));
            reader.Cursor = 6;
            Assert.AreEqual(reader.PreviouslyReadString, ("Hello!"));
        }

        [Test]
        public virtual void SkipWhitespaceNone()
        {
            StringReader reader = new StringReader("Hello!");
            reader.SkipWhitespace();
            Assert.AreEqual(reader.Cursor, (0));
        }

        [Test]
        public virtual void SkipWhitespaceMixed()
        {
            StringReader reader = new StringReader(" \t \t\nHello!");
            reader.SkipWhitespace();
            Assert.AreEqual(reader.Cursor, (5));
        }

        [Test]
        public virtual void SkipWhitespaceEmpty()
        {
            StringReader reader = new StringReader("");
            reader.SkipWhitespace();
            Assert.AreEqual(reader.Cursor, (0));
        }

        [Test]
        public virtual void ReadUnquotedString()
        {
            StringReader reader = new StringReader("hello world");
            Assert.AreEqual(reader.ReadUnquotedString(), ("hello"));
            Assert.AreEqual(reader.PreviouslyReadString, ("hello"));
            Assert.AreEqual(reader.Remaining, (" world"));
        }

        [Test]
        public virtual void ReadUnquotedStringEmpty()
        {
            StringReader reader = new StringReader("");
            Assert.AreEqual(reader.ReadUnquotedString(), (""));
            Assert.AreEqual(reader.PreviouslyReadString, (""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadUnquotedStringEmptyWithRemaining()
        {
            StringReader reader = new StringReader(" hello world");
            Assert.AreEqual(reader.ReadUnquotedString(), (""));
            Assert.AreEqual(reader.PreviouslyReadString, (""));
            Assert.AreEqual(reader.Remaining, (" hello world"));
        }

        [Test]
        public virtual void ReadQuotedString()
        {
            StringReader reader = new StringReader("\"hello world\"");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello world"));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"hello world\""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadSingleQuotedString()
        {
            StringReader reader = new StringReader("'hello world'");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello world"));
            Assert.AreEqual(reader.PreviouslyReadString, ("'hello world'"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadMixedQuotedStringDoubleInsideSingle()
        {
            StringReader reader = new StringReader("'hello \"world\"'");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello \"world\""));
            Assert.AreEqual(reader.PreviouslyReadString, ("'hello \"world\"'"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadMixedQuotedStringSingleInsideDouble()
        {
            StringReader reader = new StringReader("\"hello 'world'\"");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello 'world'"));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"hello 'world'\""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadQuotedStringEmpty()
        {
            StringReader reader = new StringReader("");
            Assert.AreEqual(reader.ReadQuotedString(), (""));
            Assert.AreEqual(reader.PreviouslyReadString, (""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadQuotedStringEmptyQuoted()
        {
            StringReader reader = new StringReader("\"\"");
            Assert.AreEqual(reader.ReadQuotedString(), (""));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"\""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadQuotedStringEmptyQuotedWithRemaining()
        {
            StringReader reader = new StringReader("\"\" hello world");
            Assert.AreEqual(reader.ReadQuotedString(), (""));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"\""));
            Assert.AreEqual(reader.Remaining, (" hello world"));
        }

        [Test]
        public virtual void ReadQuotedStringWithEscapedQuote()
        {
            StringReader reader = new StringReader("\"hello \\\"world\\\"\"");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello \"world\""));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"hello \\\"world\\\"\""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadQuotedStringWithEscapedEscapes()
        {
            StringReader reader = new StringReader("\"\\\\o/\"");
            Assert.AreEqual(reader.ReadQuotedString(), ("\\o/"));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"\\\\o/\""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadQuotedStringWithRemaining()
        {
            StringReader reader = new StringReader("\"hello world\" foo bar");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello world"));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"hello world\""));
            Assert.AreEqual(reader.Remaining, (" foo bar"));
        }

        [Test]
        public virtual void ReadQuotedStringWithImmediateRemaining()
        {
            StringReader reader = new StringReader("\"hello world\"foo bar");
            Assert.AreEqual(reader.ReadQuotedString(), ("hello world"));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"hello world\""));
            Assert.AreEqual(reader.Remaining, ("foo bar"));
        }

        [Test]
        public virtual void ReadQuotedStringNoOpen()
        {
            try
            {
                (new StringReader("hello world\"")).ReadQuotedString();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedStartOfQuote()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadQuotedStringNoClose()
        {
            try
            {
                (new StringReader("\"hello world")).ReadQuotedString();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedEndOfQuote()));
                Assert.AreEqual(ex.Cursor, (12));
            }
        }

        [Test]
        public virtual void ReadQuotedStringInvalidEscape()
        {
            try
            {
                (new StringReader("\"hello\\nworld\"")).ReadQuotedString();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidEscape()));
                Assert.AreEqual(ex.Cursor, (7));
            }
        }

        [Test]
        public virtual void ReadQuotedStringInvalidQuoteEscape()
        {
            try
            {
                (new StringReader("'hello\\\"\'world")).ReadQuotedString();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidEscape()));
                Assert.AreEqual(ex.Cursor, (7));
            }
        }

        [Test]
        public virtual void ReadStringNoQuotes()
        {
            StringReader reader = new StringReader("hello world");
            Assert.AreEqual(reader.ReadString(), ("hello"));
            Assert.AreEqual(reader.PreviouslyReadString, ("hello"));
            Assert.AreEqual(reader.Remaining, (" world"));
        }

        [Test]
        public virtual void ReadStringSingleQuotes()
        {
            StringReader reader = new StringReader("'hello world'");
            Assert.AreEqual(reader.ReadString(), ("hello world"));
            Assert.AreEqual(reader.PreviouslyReadString, ("'hello world'"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadStringDoubleQuotes()
        {
            StringReader reader = new StringReader("\"hello world\"");
            Assert.AreEqual(reader.ReadString(), ("hello world"));
            Assert.AreEqual(reader.PreviouslyReadString, ("\"hello world\""));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadInt()
        {
            StringReader reader = new StringReader("1234567890");
            Assert.AreEqual(reader.ReadInt(), (1234567890));
            Assert.AreEqual(reader.PreviouslyReadString, ("1234567890"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadIntNegative()
        {
            StringReader reader = new StringReader("-1234567890");
            Assert.AreEqual(reader.ReadInt(), (-1234567890));
            Assert.AreEqual(reader.PreviouslyReadString, ("-1234567890"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadIntInvalid()
        {
            try
            {
                (new StringReader("12.34")).ReadInt();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidInt()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadIntNone()
        {
            try
            {
                (new StringReader("")).ReadInt();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedInt()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadIntWithRemaining()
        {
            StringReader reader = new StringReader("1234567890 foo bar");
            Assert.AreEqual(reader.ReadInt(), (1234567890));
            Assert.AreEqual(reader.PreviouslyReadString, ("1234567890"));
            Assert.AreEqual(reader.Remaining, (" foo bar"));
        }

        [Test]
        public virtual void ReadIntWithRemainingImmediate()
        {
            StringReader reader = new StringReader("1234567890foo bar");
            Assert.AreEqual(reader.ReadInt(), (1234567890));
            Assert.AreEqual(reader.PreviouslyReadString, ("1234567890"));
            Assert.AreEqual(reader.Remaining, ("foo bar"));
        }

        [Test]
        public virtual void ReadLong()
        {
            StringReader reader = new StringReader("1234567890");
            Assert.AreEqual(reader.ReadLong(), (1234567890L));
            Assert.AreEqual(reader.PreviouslyReadString, ("1234567890"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadLongNegative()
        {
            StringReader reader = new StringReader("-1234567890");
            Assert.AreEqual(reader.ReadLong(), (-1234567890L));
            Assert.AreEqual(reader.PreviouslyReadString, ("-1234567890"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadLongInvalid()
        {
            try
            {
                (new StringReader("12.34")).ReadLong();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidLong()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadLongNone()
        {
            try
            {
                (new StringReader("")).ReadLong();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedLong()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadLongWithRemaining()
        {
            StringReader reader = new StringReader("1234567890 foo bar");
            Assert.AreEqual(reader.ReadLong(), (1234567890L));
            Assert.AreEqual(reader.PreviouslyReadString, ("1234567890"));
            Assert.AreEqual(reader.Remaining, (" foo bar"));
        }

        [Test]
        public virtual void ReadLongWithRemainingImmediate()
        {
            StringReader reader = new StringReader("1234567890foo bar");
            Assert.AreEqual(reader.ReadLong(), (1234567890L));
            Assert.AreEqual(reader.PreviouslyReadString, ("1234567890"));
            Assert.AreEqual(reader.Remaining, ("foo bar"));
        }

        [Test]
        public virtual void ReadDouble()
        {
            StringReader reader = new StringReader("123");
            Assert.AreEqual(reader.ReadDouble(), (123.0));
            Assert.AreEqual(reader.PreviouslyReadString, ("123"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadDoubleWithDecimal()
        {
            StringReader reader = new StringReader("12.34");
            Assert.AreEqual(reader.ReadDouble(), (12.34));
            Assert.AreEqual(reader.PreviouslyReadString, ("12.34"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadDoubleNegative()
        {
            StringReader reader = new StringReader("-123");
            Assert.AreEqual(reader.ReadDouble(), (-123.0));
            Assert.AreEqual(reader.PreviouslyReadString, ("-123"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadDoubleInvalid()
        {
            try
            {
                (new StringReader("12.34.56")).ReadDouble();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidDouble()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadDoubleNone()
        {
            try
            {
                (new StringReader("")).ReadDouble();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedDouble()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadDoubleWithRemaining()
        {
            StringReader reader = new StringReader("12.34 foo bar");
            Assert.AreEqual(reader.ReadDouble(), (12.34));
            Assert.AreEqual(reader.PreviouslyReadString, ("12.34"));
            Assert.AreEqual(reader.Remaining, (" foo bar"));
        }

        [Test]
        public virtual void ReadDoubleWithRemainingImmediate()
        {
            StringReader reader = new StringReader("12.34foo bar");
            Assert.AreEqual(reader.ReadDouble(), (12.34));
            Assert.AreEqual(reader.PreviouslyReadString, ("12.34"));
            Assert.AreEqual(reader.Remaining, ("foo bar"));
        }

        [Test]
        public virtual void ReadFloat()
        {
            StringReader reader = new StringReader("123");
            Assert.AreEqual(reader.ReadFloat(), (123.0f));
            Assert.AreEqual(reader.PreviouslyReadString, ("123"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadFloatWithDecimal()
        {
            StringReader reader = new StringReader("12.34");
            Assert.AreEqual(reader.ReadFloat(), (12.34f));
            Assert.AreEqual(reader.PreviouslyReadString, ("12.34"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadFloatNegative()
        {
            StringReader reader = new StringReader("-123");
            Assert.AreEqual(reader.ReadFloat(), (-123.0f));
            Assert.AreEqual(reader.PreviouslyReadString, ("-123"));
            Assert.AreEqual(reader.Remaining, (""));
        }

        [Test]
        public virtual void ReadFloatInvalid()
        {
            try
            {
                (new StringReader("12.34.56")).ReadFloat();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidFloat()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadFloatNone()
        {
            try
            {
                (new StringReader("")).ReadFloat();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedFloat()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadFloatWithRemaining()
        {
            StringReader reader = new StringReader("12.34 foo bar");
            Assert.AreEqual(reader.ReadFloat(), (12.34f));
            Assert.AreEqual(reader.PreviouslyReadString, ("12.34"));
            Assert.AreEqual(reader.Remaining, (" foo bar"));
        }

        [Test]
        public virtual void ReadFloatWithRemainingImmediate()
        {
            StringReader reader = new StringReader("12.34foo bar");
            Assert.AreEqual(reader.ReadFloat(), (12.34f));
            Assert.AreEqual(reader.PreviouslyReadString, ("12.34"));
            Assert.AreEqual(reader.Remaining, ("foo bar"));
        }

        [Test]
        public virtual void ExpectCorrect()
        {
            StringReader reader = new StringReader("abc");
            reader.Expect('a');
            Assert.AreEqual(reader.Cursor, (1));
        }

        [Test]
        public virtual void ExpectIncorrect()
        {
            StringReader reader = new StringReader("bcd");
            try
            {
                reader.Expect('a');
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedSymbol()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ExpectNone()
        {
            StringReader reader = new StringReader("");
            try
            {
                reader.Expect('a');
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedSymbol()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadBooleanCorrect()
        {
            StringReader reader = new StringReader("true");
            Assert.AreEqual(reader.ReadBoolean(), (true));
            Assert.AreEqual(reader.PreviouslyReadString, ("true"));
        }

        [Test]
        public virtual void ReadBooleanIncorrect()
        {
            StringReader reader = new StringReader("tuesday");
            try
            {
                reader.ReadBoolean();
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderInvalidBool()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }

        [Test]
        public virtual void ReadBooleanNone()
        {
            StringReader reader = new StringReader("");
            try
            {
                reader.ReadBoolean();
                Assert.Fail();
            }
            catch (CommandSyntaxException ex)
            {
                Assert.AreEqual(ex.Type, (CommandSyntaxException.builtInExceptions.ReaderExpectedBool()));
                Assert.AreEqual(ex.Cursor, (0));
            }
        }
    }
}