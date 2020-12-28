using System.Collections.Generic;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public delegate void AmbiguityConsumer<TS>(CommandNode<TS> parent, CommandNode<TS> child, CommandNode<TS> sibling,
        ICollection<string> inputs);
}