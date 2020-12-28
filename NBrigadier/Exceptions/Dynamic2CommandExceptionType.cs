

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class Dynamic2CommandExceptionType : ICommandExceptionType
	{
		private Function _function;

		public Dynamic2CommandExceptionType(Function function)
		{
			this._function = function;
		}

		public virtual CommandSyntaxException Create(object a, object b)
		{
			return new CommandSyntaxException(this, _function(a, b));
		}

		public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, object a, object b)
		{
			return new CommandSyntaxException(this, _function(a, b), reader.String, reader.Cursor);
		}

		public delegate IMessage Function(object a, object b);
		
	}

}