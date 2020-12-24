// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public delegate int Command<S>(CommandContext<S> context);

	public static class Command_Fields
	{
		public const int SINGLE_SUCCESS = 1;
	}

}