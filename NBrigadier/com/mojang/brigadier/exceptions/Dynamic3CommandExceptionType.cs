// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.exceptions
{
	using ImmutableStringReader = com.mojang.brigadier.ImmutableStringReader;
	using Message = com.mojang.brigadier.Message;

	public class Dynamic3CommandExceptionType : CommandExceptionType
	{
		private readonly Function function;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public Dynamic3CommandExceptionType(final Function function)
		public Dynamic3CommandExceptionType(Function function)
		{
			this.function = function;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException create(final Object a, final Object b, final Object c)
		public virtual CommandSyntaxException Create(object a, object b, object c)
		{
			return new CommandSyntaxException(this, function.Apply(a, b, c));
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandSyntaxException createWithContext(final com.mojang.brigadier.ImmutableStringReader reader, final Object a, final Object b, final Object c)
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