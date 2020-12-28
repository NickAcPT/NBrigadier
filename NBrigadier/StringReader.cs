using System.Text;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
	using CommandSyntaxException = CommandSyntaxException;

	public class StringReader : ImmutableStringReader
	{
		private static char SYNTAX_ESCAPE = '\\';
		private static char SYNTAX_DOUBLE_QUOTE = '"';
		private static char SYNTAX_SINGLE_QUOTE = '\'';

		private string @string;
		private int cursor;

		public StringReader(StringReader other)
		{
			this.@string = other.@string;
			this.cursor = other.cursor;
		}

		public StringReader(string @string)
		{
			this.@string = @string;
		}

		public virtual string String
		{
			get
			{
				return @string;
			}
		}

		public virtual int Cursor
		{
			set
			{
				this.cursor = value;
			}
			get
			{
				return cursor;
			}
		}

		public virtual int RemainingLength
		{
			get
			{
				return @string.Length - cursor;
			}
		}

		public virtual int TotalLength
		{
			get
			{
				return @string.Length;
			}
		}


		public virtual string Read
		{
			get
			{
				return @string.Substring(0, cursor);
			}
		}

		public virtual string Remaining
		{
			get
			{
				return @string.Substring(cursor);
			}
		}

		public virtual bool canRead(int length)
		{
			return cursor + length <= @string.Length;
		}

		public virtual bool canRead()
		{
			return canRead(1);
		}

		public virtual char peek()
		{
			return @string[cursor];
		}

		public virtual char peek(int offset)
		{
			return @string[cursor + offset];
		}

		public virtual char read()
		{
			return @string[cursor++];
		}

		public virtual void skip()
		{
			cursor++;
		}

		public static bool isAllowedNumber(char c)
		{
			return c >= '0' && c <= '9' || c == '.' || c == '-';
		}

		public static bool isQuotedStringStart(char c)
		{
			return c == SYNTAX_DOUBLE_QUOTE || c == SYNTAX_SINGLE_QUOTE;
		}

		public virtual void skipWhitespace()
		{
			while (canRead() && char.IsWhiteSpace(peek()))
			{
				skip();
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public int readInt() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int readInt()
		{
			 int start = cursor;
			while (canRead() && isAllowedNumber(peek()))
			{
				skip();
			}
			 string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedInt().createWithContext(this);
			}
			try
			{
				return int.Parse(number);
			}
			catch (System.FormatException)
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerInvalidInt().createWithContext(this, number);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public long readLong() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual long readLong()
		{
			 int start = cursor;
			while (canRead() && isAllowedNumber(peek()))
			{
				skip();
			}
			 string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedLong().createWithContext(this);
			}
			try
			{
				return long.Parse(number);
			}
			catch (System.FormatException)
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerInvalidLong().createWithContext(this, number);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public double readDouble() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual double readDouble()
		{
			 int start = cursor;
			while (canRead() && isAllowedNumber(peek()))
			{
				skip();
			}
			 string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedDouble().createWithContext(this);
			}
			try
			{
				return double.Parse(number);
			}
			catch (System.FormatException)
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerInvalidDouble().createWithContext(this, number);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public float readFloat() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual float readFloat()
		{
			 int start = cursor;
			while (canRead() && isAllowedNumber(peek()))
			{
				skip();
			}
			 string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedFloat().createWithContext(this);
			}
			try
			{
				return float.Parse(number);
			}
			catch (System.FormatException)
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerInvalidFloat().createWithContext(this, number);
			}
		}

		public static bool isAllowedInUnquotedString(char c)
		{
			return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-' || c == '.' || c == '+';
		}

		public virtual string readUnquotedString()
		{
			 int start = cursor;
			while (canRead() && isAllowedInUnquotedString(peek()))
			{
				skip();
			}
			return @string.Substring(start, cursor - start);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public String readQuotedString() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string readQuotedString()
		{
			if (!canRead())
			{
				return "";
			}
			 char next = peek();
			if (!isQuotedStringStart(next))
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedStartOfQuote().createWithContext(this);
			}
			skip();
			return readStringUntil(next);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public String readStringUntil(char terminator) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string readStringUntil(char terminator)
		{
			 StringBuilder result = new StringBuilder();
			bool escaped = false;
			while (canRead())
			{
				 char c = read();
				if (escaped)
				{
					if (c == terminator || c == SYNTAX_ESCAPE)
					{
						result.Append(c);
						escaped = false;
					}
					else
					{
						Cursor = Cursor - 1;
						throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerInvalidEscape().createWithContext(this, c.ToString());
					}
				}
				else if (c == SYNTAX_ESCAPE)
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

			throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedEndOfQuote().createWithContext(this);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public String readString() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string readString()
		{
			if (!canRead())
			{
				return "";
			}
			 char next = peek();
			if (isQuotedStringStart(next))
			{
				skip();
				return readStringUntil(next);
			}
			return readUnquotedString();
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public boolean readBoolean() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual bool readBoolean()
		{
			 int start = cursor;
			 string value = readString();
			if (value.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedBool().createWithContext(this);
			}

			if (value.Equals("true"))
			{
				return true;
			}
			else if (value.Equals("false"))
			{
				return false;
			}
			else
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerInvalidBool().createWithContext(this, value);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public void expect(char c) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual void expect(char c)
		{
			if (!canRead() || peek() != c)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.readerExpectedSymbol().createWithContext(this, c.ToString());
			}
			skip();
		}
	}

}