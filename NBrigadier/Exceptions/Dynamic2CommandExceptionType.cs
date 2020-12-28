

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using ImmutableStringReader = ImmutableStringReader;
	using Message = Message;

	public class Dynamic2CommandExceptionType : CommandExceptionType
	{
		private Function function;

		public Dynamic2CommandExceptionType(Function function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException create(object a, object b)
		{
			return new CommandSyntaxException(this, function(a, b));
		}

		public virtual CommandSyntaxException createWithContext(ImmutableStringReader reader, object a, object b)
		{
			return new CommandSyntaxException(this, function(a, b), reader.String, reader.Cursor);
		}

		public delegate Message Function(object a, object b);
		
	}

}