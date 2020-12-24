using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using com.mojang.brigadier.tree;

	public delegate void AmbiguityConsumer<S>(CommandNode<S> parent, CommandNode<S> child, CommandNode<S> sibling, ICollection<string> inputs);

}