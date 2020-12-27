using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.context
{
	using com.mojang.brigadier.tree;

	public class SuggestionContext<S>
	{
		public CommandNode<S> parent;
		public int startPos;

		public SuggestionContext(CommandNode<S> parent, int startPos)
		{
			this.parent = parent;
			this.startPos = startPos;
		}
	}

}