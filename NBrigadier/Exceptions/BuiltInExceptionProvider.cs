﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	public interface BuiltInExceptionProvider
	{
		Dynamic2CommandExceptionType DoubleTooLow();

		Dynamic2CommandExceptionType DoubleTooHigh();

		Dynamic2CommandExceptionType FloatTooLow();

		Dynamic2CommandExceptionType FloatTooHigh();

		Dynamic2CommandExceptionType IntegerTooLow();

		Dynamic2CommandExceptionType IntegerTooHigh();

		Dynamic2CommandExceptionType LongTooLow();

		Dynamic2CommandExceptionType LongTooHigh();

		DynamicCommandExceptionType LiteralIncorrect();

		SimpleCommandExceptionType ReaderExpectedStartOfQuote();

		SimpleCommandExceptionType ReaderExpectedEndOfQuote();

		DynamicCommandExceptionType ReaderInvalidEscape();

		DynamicCommandExceptionType ReaderInvalidBool();

		DynamicCommandExceptionType ReaderInvalidInt();

		SimpleCommandExceptionType ReaderExpectedInt();

		DynamicCommandExceptionType ReaderInvalidLong();

		SimpleCommandExceptionType ReaderExpectedLong();

		DynamicCommandExceptionType ReaderInvalidDouble();

		SimpleCommandExceptionType ReaderExpectedDouble();

		DynamicCommandExceptionType ReaderInvalidFloat();

		SimpleCommandExceptionType ReaderExpectedFloat();

		SimpleCommandExceptionType ReaderExpectedBool();

		DynamicCommandExceptionType ReaderExpectedSymbol();

		SimpleCommandExceptionType DispatcherUnknownCommand();

		SimpleCommandExceptionType DispatcherUnknownArgument();

		SimpleCommandExceptionType DispatcherExpectedArgumentSeparator();

		DynamicCommandExceptionType DispatcherParseException();
	}

}