// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Tree;

namespace NBrigadier.Context
{
    public class SuggestionContext<TS>
    {
        public readonly CommandNode<TS> parent;
        public readonly int startPos;

        public SuggestionContext(CommandNode<TS> parent, int startPos)
        {
            this.parent = parent;
            this.startPos = startPos;
        }
    }
}