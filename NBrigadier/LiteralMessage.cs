

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
	public class LiteralMessage : IMessage
	{
		private string _string;

		public LiteralMessage(string @string)
		{
			this._string = @string;
		}

		public virtual string String
		{
			get
			{
				return _string;
			}
		}

		public override string ToString()
		{
			return _string;
		}
	}

}