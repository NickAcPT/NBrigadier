using System.Collections.Generic;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public delegate ICollection<S> RedirectModifier<S>(CommandContext<S> context);
}