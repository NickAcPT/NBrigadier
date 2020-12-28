using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class SuggestionContext<TS>
	{
		public CommandNode<TS> parent;
		public int startPos;

		public SuggestionContext(CommandNode<TS> parent, int startPos)
		{
			this.parent = parent;
			this.startPos = startPos;
		}
	}

}