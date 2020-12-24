using System.Collections.Generic;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public delegate void AmbiguityConsumer<S>(CommandNode<S> parent, CommandNode<S> child, CommandNode<S> sibling,
        ICollection<string> inputs);
}