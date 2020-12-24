// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Tree;

namespace NBrigadier.Context
{
    public class ParsedCommandNode<S>
	{

		private readonly CommandNode<S> node;

		private readonly StringRange range;

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
			ParsedCommandNode<object> that = (ParsedCommandNode<object>) o;
			return object.Equals(node, that.node) && object.Equals(range, that.range);
		}

	}

}