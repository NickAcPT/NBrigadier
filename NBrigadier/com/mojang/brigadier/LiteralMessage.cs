// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	public class LiteralMessage : Message
	{
		private readonly string @string;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public LiteralMessage(final String string)
		public LiteralMessage(string @string)
		{
			this.@string = @string;
		}

		public virtual string String
		{
			get
			{
				return @string;
			}
		}

		public override string ToString()
		{
			return @string;
		}
	}

}