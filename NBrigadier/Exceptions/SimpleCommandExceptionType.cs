using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class SimpleCommandExceptionType : CommandExceptionType
	{
		private Message message;

		public SimpleCommandExceptionType(Message message)
		{
			this.message = message;
		}

		public virtual CommandSyntaxException create()
		{
			return new CommandSyntaxException(this, message);
		}

		public virtual CommandSyntaxException createWithContext(ImmutableStringReader reader)
		{
			return new CommandSyntaxException(this, message, reader.String, reader.Cursor);
		}

		public override string ToString()
		{
			return message.String;
		}
	}

}