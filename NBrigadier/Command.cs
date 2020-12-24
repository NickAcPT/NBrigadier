// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Context;
using NBrigadier.Exceptions;

namespace NBrigadier
{
    public delegate int Command<S>(CommandContext<S> context);

	public static class Command_Fields
	{
		public const int SINGLE_SUCCESS = 1;
	}

}