// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NBrigadier.Tree;

namespace NBrigadier.Builder
{
    public class LiteralArgumentBuilder<S> : ArgumentBuilder<S, LiteralArgumentBuilder<S>>
    {
        protected internal LiteralArgumentBuilder(string literal)
        {
            this.Literal = literal;
        }

        protected internal override LiteralArgumentBuilder<S> This => this;

        public virtual string Literal { get; }

        public static LiteralArgumentBuilder<S> LiteralBuilder<S>(string name)
        {
            return new(name);
        }

        public override CommandNode<S> Build()
        {
            return BuildLiteral();
        }

        public LiteralCommandNode<S> BuildLiteral()
        {
            var result = new LiteralCommandNode<S>(Literal, Command, Requirement, Redirect, RedirectModifier, Fork);

            foreach (var argument in Arguments) result.AddChild(argument);

            return result;
        }
    }
}