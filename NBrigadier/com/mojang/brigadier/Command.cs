﻿using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public delegate int Command<S>(com.mojang.brigadier.context.CommandContext<S> context);

}