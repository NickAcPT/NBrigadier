using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class ParsedCommandNode<S>
	{

		private CommandNode<S> node;

		private StringRange range;

		public ParsedCommandNode(CommandNode<S> node, StringRange range)
		{
			this.node = node;
			this.range = range;
		}

		public virtual CommandNode<S> Node
		{
			get
			{
				return node;
			}
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public override string ToString()
		{
			return node + "@" + range;
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (o == null || this.GetType() != o.GetType())
			{
				return false;
			}
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: ParsedCommandNode<?> that = (ParsedCommandNode<?>) o;
			ParsedCommandNode<object> that = (ParsedCommandNode<object>) o;
			return ObjectsHelper.Equals(node, that.node) && ObjectsHelper.Equals(range, that.range);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.hash(node, range);
		}
	}

}