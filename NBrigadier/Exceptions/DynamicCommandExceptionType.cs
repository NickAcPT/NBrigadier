

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class DynamicCommandExceptionType : ICommandExceptionType
	{
		private System.Func<object, IMessage> _function;

		public DynamicCommandExceptionType(System.Func<object, IMessage> function)
		{
			this._function = function;
		}

		public virtual CommandSyntaxException Create(object arg)
		{
			return new CommandSyntaxException(this, _function(arg));
		}

		public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, object arg)
		{
			return new CommandSyntaxException(this, _function(arg), reader.String, reader.Cursor);
		}
	}

}