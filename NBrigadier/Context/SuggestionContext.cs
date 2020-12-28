using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
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