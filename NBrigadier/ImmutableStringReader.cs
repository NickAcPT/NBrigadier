

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

		string Read { get; }

		string Remaining { get; }

		bool canRead(int length);

		bool canRead();

		char peek();

		char peek(int offset);
	}

}