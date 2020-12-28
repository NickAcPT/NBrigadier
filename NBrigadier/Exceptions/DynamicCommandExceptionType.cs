using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class DynamicCommandExceptionType : CommandExceptionType
	{
		private System.Func<object, Message> function;

		public DynamicCommandExceptionType(System.Func<object, Message> function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException create(object arg)
		{
			return new CommandSyntaxException(this, function(arg));
		}

		public virtual CommandSyntaxException createWithContext(ImmutableStringReader reader, object arg)
		{
			return new CommandSyntaxException(this, function(arg), reader.String, reader.Cursor);
		}
	}

}