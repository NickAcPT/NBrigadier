// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    public interface ImmutableStringReader
    {
        string String { get; }

        int RemainingLength { get; }

        int TotalLength { get; }

        int Cursor { get; }

        string ReadValue { get; }

        string Remaining { get; }

        bool CanRead(int length);

        bool CanRead();

        char Peek();

        char Peek(int offset);
    }
}