using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class DynamicNCommandExceptionType : CommandExceptionType
	{
		private Function function;

		public DynamicNCommandExceptionType(Function function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException create(object a, params object[] args)
		{
			return new CommandSyntaxException(this, function.apply(args));
		}

		public virtual CommandSyntaxException createWithContext(ImmutableStringReader reader, params object[] args)
		{
			return new CommandSyntaxException(this, function.apply(args), reader.String, reader.Cursor);
		}

		public interface Function
		{
			Message apply(object[] args);
		}
	}

}