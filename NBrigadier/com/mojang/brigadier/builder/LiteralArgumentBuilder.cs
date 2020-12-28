using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.builder
{
	using com.mojang.brigadier.tree;

	public class LiteralArgumentBuilder<S> : ArgumentBuilder<S, LiteralArgumentBuilder<S>>
	{
// NOTE: Fields cannot have the same name as methods of the current type:
		private string literal_Conflict;

		protected internal LiteralArgumentBuilder(string literal)
		{
			this.literal_Conflict = literal;
		}

		public static LiteralArgumentBuilder<S> literal(string name)
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
				return literal_Conflict;
			}
		}

		public override com.mojang.brigadier.tree.LiteralCommandNode<S> build()
		{
			 com.mojang.brigadier.tree.LiteralCommandNode<S> result = new com.mojang.brigadier.tree.LiteralCommandNode<S>(Literal, Command, Requirement, Redirect, RedirectModifier, Fork);

			foreach (CommandNode<S> argument in Arguments)
			{
				result.addChild(argument);
			}

			return result;
		}
	}

}