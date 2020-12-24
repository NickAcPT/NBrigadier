﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class Dynamic3CommandExceptionType : CommandExceptionType
	{
		private readonly Function function;

		public Dynamic3CommandExceptionType(Function function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException Create(object a, object b, object c)
		{
			return new CommandSyntaxException(this, function.Apply(a, b, c));
		}

		public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object a, object b, object c)
		{
			return new CommandSyntaxException(this, function.Apply(a, b, c), reader.String, reader.Cursor);
		}

		public interface Function
		{
			Message Apply(object a, object b, object c);
		}
	}

}