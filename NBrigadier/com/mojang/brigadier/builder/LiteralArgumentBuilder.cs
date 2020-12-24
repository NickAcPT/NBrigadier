// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.builder
{
	using com.mojang.brigadier.tree;
	using com.mojang.brigadier.tree;

	public class LiteralArgumentBuilder<S> : ArgumentBuilder<S, LiteralArgumentBuilder<S>>
	{
		private readonly string literal;

		protected internal LiteralArgumentBuilder(string literal)
		{
			this.literal = literal;
		}

		public static LiteralArgumentBuilder<S> LiteralBuilder<S>(string name)
		{
			return new LiteralArgumentBuilder<S>(name);
		}

		protected internal override LiteralArgumentBuilder<S> This
		{
			get
			{
				return this;
			}
		}

		public virtual string Literal
		{
			get
			{
				return literal;
			}
		}

        public override CommandNode<S> Build()
        {
            return BuildLiteral();
        }

        public LiteralCommandNode<S> BuildLiteral()
        {
			LiteralCommandNode<S> result = new LiteralCommandNode<S>(Literal, Command, Requirement, Redirect, RedirectModifier, Fork);

			foreach (CommandNode<S> argument in Arguments)
			{
				result.AddChild(argument);
			}

			return result;
		}
	}

}