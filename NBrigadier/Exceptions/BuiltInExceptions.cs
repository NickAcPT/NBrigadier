

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using LiteralMessage = LiteralMessage;

	public class BuiltInExceptions : IBuiltInExceptionProvider
	{
		private static Dynamic2CommandExceptionType _doubleTooSmall = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Double must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType _doubleTooBig = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Double must not be more than " + max + ", found " + found));

		private static Dynamic2CommandExceptionType _floatTooSmall = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Float must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType _floatTooBig = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Float must not be more than " + max + ", found " + found));

		private static Dynamic2CommandExceptionType _integerTooSmall = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Integer must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType _integerTooBig = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Integer must not be more than " + max + ", found " + found));

		private static Dynamic2CommandExceptionType _longTooSmall = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Long must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType _longTooBig = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Long must not be more than " + max + ", found " + found));

		private static DynamicCommandExceptionType _literalIncorrect = new DynamicCommandExceptionType(expected => new LiteralMessage("Expected literal " + expected));

		private static SimpleCommandExceptionType _expectedStartOfQuote = new SimpleCommandExceptionType(new LiteralMessage("Expected quote to start a string"));
		private static SimpleCommandExceptionType _expectedEndOfQuote = new SimpleCommandExceptionType(new LiteralMessage("Unclosed quoted string"));
		private static DynamicCommandExceptionType _invalidEscape = new DynamicCommandExceptionType(character => new LiteralMessage("Invalid escape sequence '" + character + "' in quoted string"));
		private static DynamicCommandExceptionType _invalidBool = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid bool, expected true or false but found '" + value + "'"));
		private static DynamicCommandExceptionType _invalidInt = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid integer '" + value + "'"));
		private static SimpleCommandExceptionType _expectedInt = new SimpleCommandExceptionType(new LiteralMessage("Expected integer"));
		private static DynamicCommandExceptionType _invalidLong = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid long '" + value + "'"));
		private static SimpleCommandExceptionType _expectedLong = new SimpleCommandExceptionType((new LiteralMessage("Expected long")));
		private static DynamicCommandExceptionType _invalidDouble = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid double '" + value + "'"));
		private static SimpleCommandExceptionType _expectedDouble = new SimpleCommandExceptionType(new LiteralMessage("Expected double"));
		private static DynamicCommandExceptionType _invalidFloat = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid float '" + value + "'"));
		private static SimpleCommandExceptionType _expectedFloat = new SimpleCommandExceptionType(new LiteralMessage("Expected float"));
		private static SimpleCommandExceptionType _expectedBool = new SimpleCommandExceptionType(new LiteralMessage("Expected bool"));
		private static DynamicCommandExceptionType _expectedSymbol = new DynamicCommandExceptionType(symbol => new LiteralMessage("Expected '" + symbol + "'"));

		private static SimpleCommandExceptionType _dispatcherUnknownCommand = new SimpleCommandExceptionType(new LiteralMessage("Unknown command"));
		private static SimpleCommandExceptionType _dispatcherUnknownArgument = new SimpleCommandExceptionType(new LiteralMessage("Incorrect argument for command"));
		private static SimpleCommandExceptionType _dispatcherExpectedArgumentSeparator = new SimpleCommandExceptionType(new LiteralMessage("Expected whitespace to end one argument, but found trailing data"));
		private static DynamicCommandExceptionType _dispatcherParseException = new DynamicCommandExceptionType(message => new LiteralMessage("Could not parse command: " + message));

		public virtual Dynamic2CommandExceptionType DoubleTooLow()
		{
			return _doubleTooSmall;
		}

		public virtual Dynamic2CommandExceptionType DoubleTooHigh()
		{
			return _doubleTooBig;
		}

		public virtual Dynamic2CommandExceptionType FloatTooLow()
		{
			return _floatTooSmall;
		}

		public virtual Dynamic2CommandExceptionType FloatTooHigh()
		{
			return _floatTooBig;
		}

		public virtual Dynamic2CommandExceptionType IntegerTooLow()
		{
			return _integerTooSmall;
		}

		public virtual Dynamic2CommandExceptionType IntegerTooHigh()
		{
			return _integerTooBig;
		}

		public virtual Dynamic2CommandExceptionType LongTooLow()
		{
			return _longTooSmall;
		}

		public virtual Dynamic2CommandExceptionType LongTooHigh()
		{
			return _longTooBig;
		}

		public virtual DynamicCommandExceptionType LiteralIncorrect()
		{
			return _literalIncorrect;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedStartOfQuote()
		{
			return _expectedStartOfQuote;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedEndOfQuote()
		{
			return _expectedEndOfQuote;
		}

		public virtual DynamicCommandExceptionType ReaderInvalidEscape()
		{
			return _invalidEscape;
		}

		public virtual DynamicCommandExceptionType ReaderInvalidBool()
		{
			return _invalidBool;
		}

		public virtual DynamicCommandExceptionType ReaderInvalidInt()
		{
			return _invalidInt;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedInt()
		{
			return _expectedInt;
		}

		public virtual DynamicCommandExceptionType ReaderInvalidLong()
		{
			return _invalidLong;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedLong()
		{
			return _expectedLong;
		}

		public virtual DynamicCommandExceptionType ReaderInvalidDouble()
		{
			return _invalidDouble;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedDouble()
		{
			return _expectedDouble;
		}

		public virtual DynamicCommandExceptionType ReaderInvalidFloat()
		{
			return _invalidFloat;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedFloat()
		{
			return _expectedFloat;
		}

		public virtual SimpleCommandExceptionType ReaderExpectedBool()
		{
			return _expectedBool;
		}

		public virtual DynamicCommandExceptionType ReaderExpectedSymbol()
		{
			return _expectedSymbol;
		}

		public virtual SimpleCommandExceptionType DispatcherUnknownCommand()
		{
			return _dispatcherUnknownCommand;
		}

		public virtual SimpleCommandExceptionType DispatcherUnknownArgument()
		{
			return _dispatcherUnknownArgument;
		}

		public virtual SimpleCommandExceptionType DispatcherExpectedArgumentSeparator()
		{
			return _dispatcherExpectedArgumentSeparator;
		}

		public virtual DynamicCommandExceptionType DispatcherParseException()
		{
			return _dispatcherParseException;
		}
	}

}