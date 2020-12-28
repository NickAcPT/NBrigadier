using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public class LiteralArgumentBuilder<TS> : ArgumentBuilder<TS, LiteralArgumentBuilder<TS>>
    {

        protected internal LiteralArgumentBuilder(string literal)
        {
            LiteralValue = literal;
        }

        protected internal override LiteralArgumentBuilder<TS> This => this;

        public virtual string LiteralValue { get; }

        public static LiteralArgumentBuilder<TS> Literal(string name)
        {
            return new(name);
        }

#if NETSTANDARD
        public override CommandNode<TS> Build()
#else
        public override LiteralCommandNode<TS> Build()
#endif
        {
            var result = new LiteralCommandNode<TS>(LiteralValue, Command, Requirement, RedirectTarget,
                RedirectModifier, HasFork);

            foreach (var argument in Arguments) result.AddChild(argument);

            return result;
        }
    }
}