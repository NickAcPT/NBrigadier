// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public class LiteralMessage : IMessage
    {
        private readonly string _string;

        public LiteralMessage(string @string)
        {
            _string = @string;
        }

        public virtual string String => _string;

        public override string ToString()
        {
            return _string;
        }
    }
}