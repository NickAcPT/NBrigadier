// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Context;
using NBrigadier.Exceptions;

namespace NBrigadier
{
    public delegate S SingleRedirectModifier<S>(CommandContext<S> context);

}