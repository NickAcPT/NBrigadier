// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Context;

namespace NBrigadier
{
    public delegate int Command<TS>(CommandContext<TS> context);

    public static class CommandFields
    {
        public const int SingleSuccess = 1;
    }
}