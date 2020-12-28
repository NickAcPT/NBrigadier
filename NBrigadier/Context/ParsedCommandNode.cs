using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class ParsedCommandNode<TS>
	{

		private CommandNode<TS> _node;

		private StringRange _range;

		public ParsedCommandNode(CommandNode<TS> node, StringRange range)
		{
			this._node = node;
			this._range = range;
		}

		public virtual CommandNode<TS> Node
		{
			get
			{
				return _node;
			}
		}

		public virtual StringRange Range
		{
			get
			{
				return _range;
			}
		}

		public override string ToString()
		{
			return _node + "@" + _range;
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
			return ObjectsHelper.Equals(_node, that._node) && ObjectsHelper.Equals(_range, that._range);
		}

		public override int GetHashCode()
		{
			return NBrigadier.Helpers.ObjectsHelper.Hash(_node, _range);
		}
	}

}