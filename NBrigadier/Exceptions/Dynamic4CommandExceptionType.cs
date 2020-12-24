// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using ImmutableStringReader = ImmutableStringReader;
	using Message = Message;

	public class Dynamic4CommandExceptionType : CommandExceptionType
	{
		private readonly Function function;

		public Dynamic4CommandExceptionType(Function function)
		{
			this.function = function;
		}

		public virtual CommandSyntaxException Create(object a, object b, object c, object d)
		{
			return new CommandSyntaxException(this, function.Apply(a, b, c, d));
		}

		public virtual CommandSyntaxException CreateWithContext(ImmutableStringReader reader, object a, object b, object c, object d)
		{
			return new CommandSyntaxException(this, function.Apply(a, b, c, d), reader.String, reader.Cursor);
		}

		public interface Function
		{
			Message Apply(object a, object b, object c, object d);
		}
	}

}