

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using ImmutableStringReader = ImmutableStringReader;
	using Message = Message;

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