using System.Text;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
	using CommandSyntaxException = CommandSyntaxException;

	public class StringReader : IMmutableStringReader
	{
		private static char _syntaxEscape = '\\';
		private static char _syntaxDoubleQuote = '"';
		private static char _syntaxSingleQuote = '\'';

		private string _string;
		private int _cursor;

		public StringReader(StringReader other)
		{
			this._string = other._string;
			this._cursor = other._cursor;
		}

		public StringReader(string @string)
		{
			this._string = @string;
		}

		public virtual string String
		{
			get
			{
				return _string;
			}
		}

		public virtual int Cursor
		{
			set
			{
				this._cursor = value;
			}
			get
			{
				return _cursor;
			}
		}

		public virtual int RemainingLength
		{
			get
			{
				return _string.Length - _cursor;
			}
		}

		public virtual int TotalLength
		{
			get
			{
				return _string.Length;
			}
		}


		public virtual string PreviouslyReadString
		{
			get
			{
				return _string.Substring(0, _cursor);
			}
		}

		public virtual string Remaining
		{
			get
			{
				return _string.Substring(_cursor);
			}
		}

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
			while (CanRead() && char.IsWhiteSpace(Peek()))
			{
				Skip();
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public int readInt() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int ReadInt()
		{
			 int start = _cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			 string number = _string.Substring(start, _cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedInt().CreateWithContext(this);
			}
			try
			{
				return int.Parse(number);
			}
			catch (System.FormatException)
			{
				_cursor = start;
				throw CommandSyntaxException.builtInExceptions.ReaderInvalidInt().CreateWithContext(this, number);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public long readLong() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual long ReadLong()
		{
			 int start = _cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			 string number = _string.Substring(start, _cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedLong().CreateWithContext(this);
			}
			try
			{
				return long.Parse(number);
			}
			catch (System.FormatException)
			{
				_cursor = start;
				throw CommandSyntaxException.builtInExceptions.ReaderInvalidLong().CreateWithContext(this, number);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public double readDouble() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual double ReadDouble()
		{
			 int start = _cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			 string number = _string.Substring(start, _cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedDouble().CreateWithContext(this);
			}
			try
			{
				return double.Parse(number);
			}
			catch (System.FormatException)
			{
				_cursor = start;
				throw CommandSyntaxException.builtInExceptions.ReaderInvalidDouble().CreateWithContext(this, number);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public float readFloat() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual float ReadFloat()
		{
			 int start = _cursor;
			while (CanRead() && IsAllowedNumber(Peek()))
			{
				Skip();
			}
			 string number = _string.Substring(start, _cursor - start);
			if (number.Length == 0)
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedFloat().CreateWithContext(this);
			}
			try
			{
				return float.Parse(number);
			}
			catch (System.FormatException)
			{
				_cursor = start;
				throw CommandSyntaxException.builtInExceptions.ReaderInvalidFloat().CreateWithContext(this, number);
			}
		}

		public static bool IsAllowedInUnquotedString(char c)
		{
			return c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z' || c == '_' || c == '-' || c == '.' || c == '+';
		}

		public virtual string ReadUnquotedString()
		{
			 int start = _cursor;
			while (CanRead() && IsAllowedInUnquotedString(Peek()))
			{
				Skip();
			}
			return _string.Substring(start, _cursor - start);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public String readQuotedString() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string ReadQuotedString()
		{
			if (!CanRead())
			{
				return "";
			}
			 char next = Peek();
			if (!IsQuotedStringStart(next))
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedStartOfQuote().CreateWithContext(this);
			}
			Skip();
			return ReadStringUntil(next);
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public String readStringUntil(char terminator) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string ReadStringUntil(char terminator)
		{
			 StringBuilder result = new StringBuilder();
			bool escaped = false;
			while (CanRead())
			{
				 char c = Read();
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
						throw CommandSyntaxException.builtInExceptions.ReaderInvalidEscape().CreateWithContext(this, c.ToString());
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

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public String readString() throws com.mojang.brigadier.exceptions.CommandSyntaxException
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

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public boolean readBoolean() throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual bool ReadBoolean()
		{
			 int start = _cursor;
			 string value = ReadString();
			if (value.Length == 0)
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedBool().CreateWithContext(this);
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
				_cursor = start;
				throw CommandSyntaxException.builtInExceptions.ReaderInvalidBool().CreateWithContext(this, value);
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public void expect(char c) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual void Expect(char c)
		{
			if (!CanRead() || Peek() != c)
			{
				throw CommandSyntaxException.builtInExceptions.ReaderExpectedSymbol().CreateWithContext(this, c.ToString());
			}
			Skip();
		}
	}

}