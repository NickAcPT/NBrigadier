// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using ImmutableStringReader = ImmutableStringReader;
	using Message = Message;

	public class DynamicNCommandExceptionType : CommandExceptionType
	{
		private readonly Function function;

		public DynamicNCommandExceptionType(Function function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException Create(object a, params object[] args)
		{
			return new CommandSyntaxException(this, function.Apply(args));
		}

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