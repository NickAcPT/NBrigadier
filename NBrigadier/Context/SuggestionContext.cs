// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Tree;

namespace NBrigadier.Context
{
    public class SuggestionContext<TS>
    {
        public readonly CommandNode<TS> Parent;
        public readonly int StartPos;

        public SuggestionContext(CommandNode<TS> parent, int startPos)
        {
            Parent = parent;
            StartPos = startPos;
        }
    }
}