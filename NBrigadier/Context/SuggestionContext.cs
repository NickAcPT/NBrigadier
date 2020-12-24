// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Tree;

namespace NBrigadier.Context
{
    public class SuggestionContext<S>
    {
        public readonly CommandNode<S> parent;
        public readonly int startPos;

        public SuggestionContext(CommandNode<S> parent, int startPos)
        {
            this.parent = parent;
            this.startPos = startPos;
        }
    }
}