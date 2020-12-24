// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using NBrigadier.Tree;

namespace NBrigadier.Context
{
    public class ParsedCommandNode<TS>
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_node != null ? _node.GetHashCode() : 0) * 397) ^ (_range != null ? _range.GetHashCode() : 0);
            }
        }

        protected bool Equals(ParsedCommandNode<TS> other)
        {
            return Equals(_node, other._node) && Equals(_range, other._range);
        }


        public static bool operator ==(ParsedCommandNode<TS> left, ParsedCommandNode<TS> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ParsedCommandNode<TS> left, ParsedCommandNode<TS> right)
        {
            return !Equals(left, right);
        }

        private readonly CommandNode<TS> _node;

        private readonly StringRange _range;

        public ParsedCommandNode(CommandNode<TS> node, StringRange range)
        {
            this._node = node;
            this._range = range;
        }

        public virtual CommandNode<TS> Node => _node;

        public virtual StringRange Range => _range;

        public override string ToString()
        {
            return _node + "@" + _range;
        }

        public override bool Equals(object o)
        {
            if (ReferenceEquals(null, o)) return false;
            if (ReferenceEquals(this, o)) return true;
            if (o.GetType() != this.GetType()) return false;
            return Equals((ParsedCommandNode<TS>) o);
        }
    }
}