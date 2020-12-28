

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class DynamicNCommandExceptionType : ICommandExceptionType
	{
		private IFunction _function;

		public DynamicNCommandExceptionType(IFunction function)
		{
			this._function = function;
		}

		public virtual CommandSyntaxException Create(object a, params object[] args)
		{
			return new CommandSyntaxException(this, _function.Apply(args));
		}

		public virtual CommandSyntaxException CreateWithContext(IMmutableStringReader reader, params object[] args)
		{
			return new CommandSyntaxException(this, _function.Apply(args), reader.String, reader.Cursor);
		}

		public interface IFunction
		{
			IMessage Apply(object[] args);
		}
	}

}