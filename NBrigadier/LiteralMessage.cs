using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	public class LiteralMessage : Message
	{
		private string @string;

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