// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class SimpleCommandExceptionType : CommandExceptionType
	{
		private readonly Message message;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SimpleCommandExceptionType(final com.mojang.brigadier.Message message)
		public SimpleCommandExceptionType(Message message)
		{
			this.message = message;
		}

		public virtual CommandSyntaxException Create()
		{
			return new CommandSyntaxException(this, message);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException createWithContext(final com.mojang.brigadier.ImmutableStringReader reader)
		public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader)
		{
			return new CommandSyntaxException(this, message, reader.String, reader.Cursor);
		}

		public override string ToString()
		{
			return message.String;
		}
	}

}