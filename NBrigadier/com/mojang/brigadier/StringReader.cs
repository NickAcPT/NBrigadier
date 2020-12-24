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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public StringReader(final StringReader other)
		public StringReader(StringReader other)
		{
			this.@string = other.@string;
			this.cursor = other.cursor;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public StringReader(final String string)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public void setCursor(final int cursor)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean canRead(final int length)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public char peek(final int offset)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static boolean isAllowedNumber(final char c)
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public int readInt() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int ReadInt()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = cursor;
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String number = string.substring(start, cursor - start);
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedInt().CreateWithContext(this);
			}
			try
			{
				return int.Parse(number);
			}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final NumberFormatException ex)
			catch 
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidInt().CreateWithContext(this, number);
			}
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public long readLong() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual long ReadLong()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = cursor;
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String number = string.substring(start, cursor - start);
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedLong().CreateWithContext(this);
			}
			try
			{
				return long.Parse(number);
			}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final NumberFormatException ex)
			catch
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidLong().CreateWithContext(this, number);
			}
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public double readDouble() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual double ReadDouble()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = cursor;
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String number = string.substring(start, cursor - start);
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedDouble().CreateWithContext(this);
			}
			try
			{
				return double.Parse(number);
			}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final NumberFormatException ex)
			catch
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidDouble().CreateWithContext(this, number);
			}
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public float readFloat() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual float ReadFloat()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = cursor;
			int start = cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String number = string.substring(start, cursor - start);
			string number = @string.Substring(start, cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedFloat().CreateWithContext(this);
			}
			try
			{
				return float.Parse(number);
			}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final NumberFormatException ex)
			catch
			{
				cursor = start;
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderInvalidFloat().CreateWithContext(this, number);
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public static boolean isAllowedInUnquotedString(final char c)
		public static bool IsAllowedInUnquotedString(char c)
		{
			return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-' || c == '.' || c == '+';
		}

		public virtual string ReadUnquotedString()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = cursor;
			int start = cursor;
			while (CanRead() && IsAllowedInUnquotedString(Peek()))
			{
				Skip();
			}
			return @string.Substring(start, cursor - start);
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public String readQuotedString() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string ReadQuotedString()
		{
			if (!CanRead())
			{
				return "";
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final char next = peek();
			char next = Peek();
			if (!IsQuotedStringStart(next))
			{
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.ReaderExpectedStartOfQuote().CreateWithContext(this);
			}
			Skip();
			return ReadStringUntil(next);
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public String readStringUntil(char terminator) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string ReadStringUntil(char terminator)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringBuilder result = new StringBuilder();
			StringBuilder result = new StringBuilder();
			bool escaped = false;
			while (CanRead())
			{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final char c = read();
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public String readString() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string ReadString()
		{
			if (!CanRead())
			{
				return "";
			}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final char next = peek();
			char next = Peek();
			if (IsQuotedStringStart(next))
			{
				Skip();
				return ReadStringUntil(next);
			}
			return ReadUnquotedString();
		}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public boolean readBoolean() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual bool ReadBoolean()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = cursor;
			int start = cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String value = readString();
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

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public void expect(final char c) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
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