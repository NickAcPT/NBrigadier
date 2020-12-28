using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{

	public delegate void ResultConsumer<TS>(CommandContext<TS> context, bool success, int result);

}