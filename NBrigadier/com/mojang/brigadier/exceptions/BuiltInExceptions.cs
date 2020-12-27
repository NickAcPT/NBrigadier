using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using LiteralMessage = com.mojang.brigadier.LiteralMessage;

	public class BuiltInExceptions : BuiltInExceptionProvider
	{
		private static Dynamic2CommandExceptionType DOUBLE_TOO_SMALL = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Double must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType DOUBLE_TOO_BIG = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Double must not be more than " + max + ", found " + found));

		private static Dynamic2CommandExceptionType FLOAT_TOO_SMALL = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Float must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType FLOAT_TOO_BIG = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Float must not be more than " + max + ", found " + found));

		private static Dynamic2CommandExceptionType INTEGER_TOO_SMALL = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Integer must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType INTEGER_TOO_BIG = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Integer must not be more than " + max + ", found " + found));

		private static Dynamic2CommandExceptionType LONG_TOO_SMALL = new Dynamic2CommandExceptionType((found, min) => new LiteralMessage("Long must not be less than " + min + ", found " + found));
		private static Dynamic2CommandExceptionType LONG_TOO_BIG = new Dynamic2CommandExceptionType((found, max) => new LiteralMessage("Long must not be more than " + max + ", found " + found));

		private static DynamicCommandExceptionType LITERAL_INCORRECT = new DynamicCommandExceptionType(expected => new LiteralMessage("Expected literal " + expected));

		private static SimpleCommandExceptionType READER_EXPECTED_START_OF_QUOTE = new SimpleCommandExceptionType(new LiteralMessage("Expected quote to start a string"));
		private static SimpleCommandExceptionType READER_EXPECTED_END_OF_QUOTE = new SimpleCommandExceptionType(new LiteralMessage("Unclosed quoted string"));
		private static DynamicCommandExceptionType READER_INVALID_ESCAPE = new DynamicCommandExceptionType(character => new LiteralMessage("Invalid escape sequence '" + character + "' in quoted string"));
		private static DynamicCommandExceptionType READER_INVALID_BOOL = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid bool, expected true or false but found '" + value + "'"));
		private static DynamicCommandExceptionType READER_INVALID_INT = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid integer '" + value + "'"));
		private static SimpleCommandExceptionType READER_EXPECTED_INT = new SimpleCommandExceptionType(new LiteralMessage("Expected integer"));
		private static DynamicCommandExceptionType READER_INVALID_LONG = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid long '" + value + "'"));
		private static SimpleCommandExceptionType READER_EXPECTED_LONG = new SimpleCommandExceptionType((new LiteralMessage("Expected long")));
		private static DynamicCommandExceptionType READER_INVALID_DOUBLE = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid double '" + value + "'"));
		private static SimpleCommandExceptionType READER_EXPECTED_DOUBLE = new SimpleCommandExceptionType(new LiteralMessage("Expected double"));
		private static DynamicCommandExceptionType READER_INVALID_FLOAT = new DynamicCommandExceptionType(value => new LiteralMessage("Invalid float '" + value + "'"));
		private static SimpleCommandExceptionType READER_EXPECTED_FLOAT = new SimpleCommandExceptionType(new LiteralMessage("Expected float"));
		private static SimpleCommandExceptionType READER_EXPECTED_BOOL = new SimpleCommandExceptionType(new LiteralMessage("Expected bool"));
		private static DynamicCommandExceptionType READER_EXPECTED_SYMBOL = new DynamicCommandExceptionType(symbol => new LiteralMessage("Expected '" + symbol + "'"));

		private static SimpleCommandExceptionType DISPATCHER_UNKNOWN_COMMAND = new SimpleCommandExceptionType(new LiteralMessage("Unknown command"));
		private static SimpleCommandExceptionType DISPATCHER_UNKNOWN_ARGUMENT = new SimpleCommandExceptionType(new LiteralMessage("Incorrect argument for command"));
		private static SimpleCommandExceptionType DISPATCHER_EXPECTED_ARGUMENT_SEPARATOR = new SimpleCommandExceptionType(new LiteralMessage("Expected whitespace to end one argument, but found trailing data"));
		private static DynamicCommandExceptionType DISPATCHER_PARSE_EXCEPTION = new DynamicCommandExceptionType(message => new LiteralMessage("Could not parse command: " + message));

		public virtual Dynamic2CommandExceptionType doubleTooLow()
		{
			return DOUBLE_TOO_SMALL;
		}

		public virtual Dynamic2CommandExceptionType doubleTooHigh()
		{
			return DOUBLE_TOO_BIG;
		}

		public virtual Dynamic2CommandExceptionType floatTooLow()
		{
			return FLOAT_TOO_SMALL;
		}

		public virtual Dynamic2CommandExceptionType floatTooHigh()
		{
			return FLOAT_TOO_BIG;
		}

		public virtual Dynamic2CommandExceptionType integerTooLow()
		{
			return INTEGER_TOO_SMALL;
		}

		public virtual Dynamic2CommandExceptionType integerTooHigh()
		{
			return INTEGER_TOO_BIG;
		}

		public virtual Dynamic2CommandExceptionType longTooLow()
		{
			return LONG_TOO_SMALL;
		}

		public virtual Dynamic2CommandExceptionType longTooHigh()
		{
			return LONG_TOO_BIG;
		}

		public virtual DynamicCommandExceptionType literalIncorrect()
		{
			return LITERAL_INCORRECT;
		}

		public virtual SimpleCommandExceptionType readerExpectedStartOfQuote()
		{
			return READER_EXPECTED_START_OF_QUOTE;
		}

		public virtual SimpleCommandExceptionType readerExpectedEndOfQuote()
		{
			return READER_EXPECTED_END_OF_QUOTE;
		}

		public virtual DynamicCommandExceptionType readerInvalidEscape()
		{
			return READER_INVALID_ESCAPE;
		}

		public virtual DynamicCommandExceptionType readerInvalidBool()
		{
			return READER_INVALID_BOOL;
		}

		public virtual DynamicCommandExceptionType readerInvalidInt()
		{
			return READER_INVALID_INT;
		}

		public virtual SimpleCommandExceptionType readerExpectedInt()
		{
			return READER_EXPECTED_INT;
		}

		public virtual DynamicCommandExceptionType readerInvalidLong()
		{
			return READER_INVALID_LONG;
		}

		public virtual SimpleCommandExceptionType readerExpectedLong()
		{
			return READER_EXPECTED_LONG;
		}

		public virtual DynamicCommandExceptionType readerInvalidDouble()
		{
			return READER_INVALID_DOUBLE;
		}

		public virtual SimpleCommandExceptionType readerExpectedDouble()
		{
			return READER_EXPECTED_DOUBLE;
		}

		public virtual DynamicCommandExceptionType readerInvalidFloat()
		{
			return READER_INVALID_FLOAT;
		}

		public virtual SimpleCommandExceptionType readerExpectedFloat()
		{
			return READER_EXPECTED_FLOAT;
		}

		public virtual SimpleCommandExceptionType readerExpectedBool()
		{
			return READER_EXPECTED_BOOL;
		}

		public virtual DynamicCommandExceptionType readerExpectedSymbol()
		{
			return READER_EXPECTED_SYMBOL;
		}

		public virtual SimpleCommandExceptionType dispatcherUnknownCommand()
		{
			return DISPATCHER_UNKNOWN_COMMAND;
		}

		public virtual SimpleCommandExceptionType dispatcherUnknownArgument()
		{
			return DISPATCHER_UNKNOWN_ARGUMENT;
		}

		public virtual SimpleCommandExceptionType dispatcherExpectedArgumentSeparator()
		{
			return DISPATCHER_EXPECTED_ARGUMENT_SEPARATOR;
		}

		public virtual DynamicCommandExceptionType dispatcherParseException()
		{
			return DISPATCHER_PARSE_EXCEPTION;
		}
	}

}