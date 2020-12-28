using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{

	public delegate void ResultConsumer<S>(com.mojang.brigadier.context.CommandContext<S> context, bool success, int result);

}