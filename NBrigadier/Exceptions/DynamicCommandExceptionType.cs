// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using ImmutableStringReader = ImmutableStringReader;
	using Message = Message;

	public class DynamicCommandExceptionType : CommandExceptionType
	{
		private readonly System.Func<object, Message> function;

		public DynamicCommandExceptionType(System.Func<object, Message> function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException Create(object arg)
		{
			return new CommandSyntaxException(this, function(arg));
		}

		public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object arg)
		{
			return new CommandSyntaxException(this, function(arg), reader.String, reader.Cursor);
		}
	}

}