using System;
using System.Globalization;
using System.Text;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public class StringReader : IMmutableStringReader
    {
        private static readonly char _syntaxEscape = '\\';
        private static readonly char _syntaxDoubleQuote = '"';
        private static readonly char _syntaxSingleQuote = '\'';

        private readonly string _string;
        private int _cursor;

        public StringReader(StringReader other)
        {
            _string = other._string;
            _cursor = other._cursor;
        }

        public StringReader(string @string)
        {
            _string = @string;
        }

        public virtual string String => _string;

        public virtual int Cursor
        {
            set => _cursor = value;
            get => _cursor;
        }

        public virtual int RemainingLength => _string.Length - _cursor;

        public virtual int TotalLength => _string.Length;


        public virtual string PreviouslyReadString => _string.Substring(0, _cursor);

        public virtual string Remaining => _string.Substring(_cursor);

        public virtual bool CanRead(int length)
        {
            return _cursor + length <= _string.Length;
        }

        public virtual bool CanRead()
        {
            return CanRead(1);
        }

        public virtual char Peek()
        {
            return _string[_cursor];
        }

        public virtual char Peek(int offset)
        {
            return _string[_cursor + offset];
        }

        public virtual char Read()
        {
            return _string[_cursor++];
        }

        public virtual void Skip()
        {
            _cursor++;
        }

        public static bool IsAllowedNumber(char c)
        {
            return c >= '0' && c <= '9' || c == '.' || c == '-';
        }

        public static bool IsQuotedStringStart(char c)
        {
            return c == _syntaxDoubleQuote || c == _syntaxSingleQuote;
        }

        public virtual void SkipWhitespace()
        {
            while (CanRead() && char.IsWhiteSpace(Peek())) Skip();
        }

        public virtual int ReadInt()
        {
            var start = _cursor;
            while (CanRead() && IsAllowedNumber(Peek())) Skip();
            var number = _string.Substring(start, _cursor - start);
            if (number.Length == 0)
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedInt().CreateWithContext(this);
            try
            {
                return int.Parse(number, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                _cursor = start;
                throw CommandSyntaxException.builtInExceptions.ReaderInvalidInt().CreateWithContext(this, number);
            }
        }

        public virtual long ReadLong()
        {
            var start = _cursor;
            while (CanRead() && IsAllowedNumber(Peek())) Skip();
            var number = _string.Substring(start, _cursor - start);
            if (number.Length == 0)
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedLong().CreateWithContext(this);
            try
            {
                return long.Parse(number, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                _cursor = start;
                throw CommandSyntaxException.builtInExceptions.ReaderInvalidLong().CreateWithContext(this, number);
            }
        }

        public virtual double ReadDouble()
        {
            var start = _cursor;
            while (CanRead() && IsAllowedNumber(Peek())) Skip();
            var number = _string.Substring(start, _cursor - start);
            if (number.Length == 0)
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedDouble().CreateWithContext(this);
            try
            {
                return double.Parse(number, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                _cursor = start;
                throw CommandSyntaxException.builtInExceptions.ReaderInvalidDouble().CreateWithContext(this, number);
            }
        }

        public virtual float ReadFloat()
        {
            var start = _cursor;
            while (CanRead() && IsAllowedNumber(Peek())) Skip();
            var number = _string.Substring(start, _cursor - start);
            if (number.Length == 0)
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedFloat().CreateWithContext(this);
            try
            {
                return float.Parse(number, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                _cursor = start;
                throw CommandSyntaxException.builtInExceptions.ReaderInvalidFloat().CreateWithContext(this, number);
            }
        }

        public static bool IsAllowedInUnquotedString(char c)
        {
            return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-' ||
                   c == '.' || c == '+';
        }

        public virtual string ReadUnquotedString()
        {
            var start = _cursor;
            while (CanRead() && IsAllowedInUnquotedString(Peek())) Skip();
            return _string.Substring(start, _cursor - start);
        }

        public virtual string ReadQuotedString()
        {
            if (!CanRead()) return "";
            var next = Peek();
            if (!IsQuotedStringStart(next))
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedStartOfQuote().CreateWithContext(this);
            Skip();
            return ReadStringUntil(next);
        }

        public virtual string ReadStringUntil(char terminator)
        {
            var result = new StringBuilder();
            var escaped = false;
            while (CanRead())
            {
                var c = Read();
                if (escaped)
                {
                    if (c == terminator || c == _syntaxEscape)
                    {
                        result.Append(c);
                        escaped = false;
                    }
                    else
                    {
                        Cursor = Cursor - 1;
                        throw CommandSyntaxException.builtInExceptions.ReaderInvalidEscape()
                            .CreateWithContext(this, c.ToString());
                    }
                }
                else if (c == _syntaxEscape)
                {
                    escaped = true;
                }
                else if (c == terminator)
                {
                    return result.ToString();
                }
                else
                {
                    result.Append(c);
                }
            }

            throw CommandSyntaxException.builtInExceptions.ReaderExpectedEndOfQuote().CreateWithContext(this);
        }

        public virtual string ReadString()
        {
            if (!CanRead()) return "";
            var next = Peek();
            if (IsQuotedStringStart(next))
            {
                Skip();
                return ReadStringUntil(next);
            }

            return ReadUnquotedString();
        }

        public virtual bool ReadBoolean()
        {
            var start = _cursor;
            var value = ReadString();
            if (value.Length == 0)
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedBool().CreateWithContext(this);

            if (value.Equals("true", StringComparison.Ordinal)) return true;

            if (value.Equals("false", StringComparison.Ordinal)) return false;

            _cursor = start;
            throw CommandSyntaxException.builtInExceptions.ReaderInvalidBool().CreateWithContext(this, value);
        }

        public virtual void Expect(char c)
        {
            if (!CanRead() || Peek() != c)
                throw CommandSyntaxException.builtInExceptions.ReaderExpectedSymbol()
                    .CreateWithContext(this, c.ToString());
            Skip();
        }
    }
}