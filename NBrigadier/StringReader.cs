using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public class StringReader : ImmutableStringReader
	{
		private const char SYNTAX_ESCAPE = '\\';
		private const char SYNTAX_DOUBLE_QUOTE = '"';
		private const char SYNTAX_SINGLE_QUOTE = '\'';

		private readonly string @string;
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


		public virtual string ReadValue
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

		public bool CanRead(int length)
		{
			return cursor + length <= @string.Length;
		}

		public bool CanRead()
		{
			return CanRead(1);
		}

		public char Peek()
		{
			return @string[cursor];
		}

		public char Peek(int offset)
		{
			return @string[cursor + offset];
		}

		public virtual char Read()
		{
			return @string[cursor++];
		}

		public virtual void Skip()
		{
			cursor++;
		}

		public static bool IsAllowedNumber(char c)
		{
			return c >= '0' && c <= '9' || c == '.' || c == '-';
		}

		public static bool IsQuotedStringStart(char c)
		{
			return c == SYNTAX_DOUBLE_QUOTE || c == SYNTAX_SINGLE_QUOTE;
		}

		public virtual void SkipWhitespace()
		{
			while (CanRead() && char.IsWhiteSpace(Peek()))
			{
				Skip();
			}
		}

		public virtual int ReadInt()
		{
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedInt().CreateWithContext(this);
			}
			try
			{
				return int.Parse(number);
			}
			catch 
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidInt().CreateWithContext(this, number);
			}
		}

		public virtual long ReadLong()
		{
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedLong().CreateWithContext(this);
			}
			try
			{
				return long.Parse(number);
			}
			catch
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidLong().CreateWithContext(this, number);
			}
		}

		public virtual double ReadDouble()
		{
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedDouble().CreateWithContext(this);
			}
			try
			{
				return double.Parse(number);
			}
			catch
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidDouble().CreateWithContext(this, number);
			}
		}

		public virtual float ReadFloat()
		{
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedFloat().CreateWithContext(this);
			}
			try
			{
				return float.Parse(number);
			}
			catch
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidFloat().CreateWithContext(this, number);
			}
		}

		public static bool IsAllowedInUnquotedString(char c)
		{
			return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-' || c == '.' || c == '+';
		}

		public virtual string ReadUnquotedString()
		{
			int start = cursor;
			while (CanRead() && IsAllowedInUnquotedString(Peek()))
			{
				Skip();
			}
			return @string.Substring(start, cursor - start);
		}

		public virtual string ReadQuotedString()
		{
			if (!CanRead())
			{
				return "";
			}
			char next = Peek();
			if (!IsQuotedStringStart(next))
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedStartOfQuote().CreateWithContext(this);
			}
			Skip();
			return ReadStringUntil(next);
		}

		public virtual string ReadStringUntil(char terminator)
		{
			StringBuilder result = new StringBuilder();
			bool escaped = false;
			while (CanRead())
			{
				char c = Read();
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
						throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidEscape().CreateWithContext(this, c.ToString());
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

			throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedEndOfQuote().CreateWithContext(this);
		}

		public virtual string ReadString()
		{
			if (!CanRead())
			{
				return "";
			}
			char next = Peek();
			if (IsQuotedStringStart(next))
			{
				Skip();
				return ReadStringUntil(next);
			}
			return ReadUnquotedString();
		}

		public virtual bool ReadBoolean()
		{
			int start = cursor;
			string value = ReadString();
			if (value.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedBool().CreateWithContext(this);
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
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidBool().CreateWithContext(this, value);
			}
		}

		public virtual void Expect(char c)
		{
			if (!CanRead() || Peek() != c)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedSymbol().CreateWithContext(this, c.ToString());
			}
			Skip();
		}
	}

}