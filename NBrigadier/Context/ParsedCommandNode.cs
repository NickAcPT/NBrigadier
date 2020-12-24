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

        public virtual CommandNode<S> Node => node;

        public virtual StringRange Range => range;

        public override string ToString()
        {
            return node + "@" + range;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (o == null || GetType() != o.GetType()) return false;
            var that = (ParsedCommandNode<object>) o;
            return Equals(node, that.node) && Equals(range, that.range);
        }
    }
}