// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public delegate S SingleRedirectModifier<S>(CommandContext<S> context);

}