// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using com.mojang.brigadier.context;

	public delegate void ResultConsumer<S>(CommandContext<S> context, bool success, int result);

}