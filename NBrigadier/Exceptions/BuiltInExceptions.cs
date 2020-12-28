// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class BuiltInExceptions : IBuiltInExceptionProvider
    {
        private static readonly Dynamic2CommandExceptionType _doubleTooSmall = new((found, min) =>
            new LiteralMessage("Double must not be less than " + min + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _doubleTooBig = new((found, max) =>
            new LiteralMessage("Double must not be more than " + max + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _floatTooSmall = new((found, min) =>
            new LiteralMessage("Float must not be less than " + min + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _floatTooBig = new((found, max) =>
            new LiteralMessage("Float must not be more than " + max + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _integerTooSmall = new((found, min) =>
            new LiteralMessage("Integer must not be less than " + min + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _integerTooBig = new((found, max) =>
            new LiteralMessage("Integer must not be more than " + max + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _longTooSmall = new((found, min) =>
            new LiteralMessage("Long must not be less than " + min + ", found " + found));

        private static readonly Dynamic2CommandExceptionType _longTooBig = new((found, max) =>
            new LiteralMessage("Long must not be more than " + max + ", found " + found));

        private static readonly DynamicCommandExceptionType _literalIncorrect =
            new(expected => new LiteralMessage("Expected literal " + expected));

        private static readonly SimpleCommandExceptionType _expectedStartOfQuote =
            new(new LiteralMessage("Expected quote to start a string"));

        private static readonly SimpleCommandExceptionType _expectedEndOfQuote =
            new(new LiteralMessage("Unclosed quoted string"));

        private static readonly DynamicCommandExceptionType _invalidEscape = new(character =>
            new LiteralMessage("Invalid escape sequence '" + character + "' in quoted string"));

        private static readonly DynamicCommandExceptionType _invalidBool = new(value =>
            new LiteralMessage("Invalid bool, expected true or false but found '" + value + "'"));

        private static readonly DynamicCommandExceptionType _invalidInt =
            new(value => new LiteralMessage("Invalid integer '" + value + "'"));

        private static readonly SimpleCommandExceptionType _expectedInt = new(new LiteralMessage("Expected integer"));

        private static readonly DynamicCommandExceptionType _invalidLong =
            new(value => new LiteralMessage("Invalid long '" + value + "'"));

        private static readonly SimpleCommandExceptionType _expectedLong = new(new LiteralMessage("Expected long"));

        private static readonly DynamicCommandExceptionType _invalidDouble =
            new(value => new LiteralMessage("Invalid double '" + value + "'"));

        private static readonly SimpleCommandExceptionType _expectedDouble = new(new LiteralMessage("Expected double"));

        private static readonly DynamicCommandExceptionType _invalidFloat =
            new(value => new LiteralMessage("Invalid float '" + value + "'"));

        private static readonly SimpleCommandExceptionType _expectedFloat = new(new LiteralMessage("Expected float"));
        private static readonly SimpleCommandExceptionType _expectedBool = new(new LiteralMessage("Expected bool"));

        private static readonly DynamicCommandExceptionType _expectedSymbol =
            new(symbol => new LiteralMessage("Expected '" + symbol + "'"));

        private static readonly SimpleCommandExceptionType _dispatcherUnknownCommand =
            new(new LiteralMessage("Unknown command"));

        private static readonly SimpleCommandExceptionType _dispatcherUnknownArgument =
            new(new LiteralMessage("Incorrect argument for command"));

        private static readonly SimpleCommandExceptionType _dispatcherExpectedArgumentSeparator =
            new(new LiteralMessage("Expected whitespace to end one argument, but found trailing data"));

        private static readonly DynamicCommandExceptionType _dispatcherParseException =
            new(message => new LiteralMessage("Could not parse command: " + message));

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