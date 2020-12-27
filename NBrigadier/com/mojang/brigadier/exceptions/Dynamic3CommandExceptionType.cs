﻿using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class Dynamic3CommandExceptionType : CommandExceptionType
	{
		private Function function;

		public Dynamic3CommandExceptionType(Function function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException create(object a, object b, object c)
		{
			return new CommandSyntaxException(this, function.apply(a, b, c));
		}

		public virtual CommandSyntaxException createWithContext(ImmutableStringReader reader, object a, object b, object c)
		{
			return new CommandSyntaxException(this, function.apply(a, b, c), reader.String, reader.Cursor);
		}

		public interface Function
		{
			Message apply(object a, object b, object c);
		}
	}

}