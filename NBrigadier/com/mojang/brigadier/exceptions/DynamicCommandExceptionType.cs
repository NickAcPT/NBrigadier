// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class DynamicCommandExceptionType : CommandExceptionType
	{
		private readonly System.Func<object, Message> function;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public DynamicCommandExceptionType(final java.util.function.Function<Object, com.mojang.brigadier.Message> function)
		public DynamicCommandExceptionType(System.Func<object, Message> function)
		{
			this.function = function;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException create(final Object arg)
		public virtual CommandSyntaxException Create(object arg)
		{
			return new CommandSyntaxException(this, function(arg));
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException createWithContext(final com.mojang.brigadier.ImmutableStringReader reader, final Object arg)
		public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object arg)
		{
			return new CommandSyntaxException(this, function(arg), reader.String, reader.Cursor);
		}
	}

}