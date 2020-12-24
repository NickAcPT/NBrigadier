// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Tree;

namespace NBrigadier.Builder
{
    public class LiteralArgumentBuilder<TS> : ArgumentBuilder<TS, LiteralArgumentBuilder<TS>>
    {
        protected internal LiteralArgumentBuilder(string literal)
        {
            this.Literal = literal;
        }

        protected internal override LiteralArgumentBuilder<TS> This => this;

        public virtual string Literal { get; }

        public static LiteralArgumentBuilder<TS> LiteralBuilder(string name)
        {
            return new(name);
        }

        public override CommandNode<TS> Build()
        {
            return BuildLiteral();
        }

        public LiteralCommandNode<TS> BuildLiteral()
        {
            var result = new LiteralCommandNode<TS>(Literal, Command, Requirement, Redirect, RedirectModifier, Fork);

            foreach (var argument in Arguments) result.AddChild(argument);

            return result;
        }
    }
}