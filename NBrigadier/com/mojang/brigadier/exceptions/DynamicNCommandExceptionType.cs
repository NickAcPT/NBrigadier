// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class DynamicNCommandExceptionType : CommandExceptionType
	{
		private readonly Function function;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public DynamicNCommandExceptionType(final Function function)
		public DynamicNCommandExceptionType(Function function)
		{
			this.function = function;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException create(final Object a, final Object... args)
		public virtual CommandSyntaxException Create(object a, params object[] args)
		{
			return new CommandSyntaxException(this, function.Apply(args));
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException createWithContext(final com.mojang.brigadier.ImmutableStringReader reader, final Object... args)
		public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, params object[] args)
		{
			return new CommandSyntaxException(this, function.Apply(args), reader.String, reader.Cursor);
		}

		public interface Function
		{
			Message Apply(object[] args);
		}
	}

}