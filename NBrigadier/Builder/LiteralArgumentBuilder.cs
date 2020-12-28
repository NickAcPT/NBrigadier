using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public class LiteralArgumentBuilder<TS> : ArgumentBuilder<TS, LiteralArgumentBuilder<TS>>
	{
// NOTE: Fields cannot have the same name as methods of the current type:
		private string _literalConflict;

		protected internal LiteralArgumentBuilder(string literal)
		{
			this._literalConflict = literal;
		}

		public static LiteralArgumentBuilder<TS> Literal(string name)
		{
			return new LiteralArgumentBuilder<TS>(name);
		}

		protected internal override LiteralArgumentBuilder<TS> This
		{
			get
			{
				return this;
			}
		}

		public virtual string LiteralValue
		{
			get
			{
				return _literalConflict;
			}
		}

		public override LiteralCommandNode<TS> Build()
		{
			 LiteralCommandNode<TS> result = new LiteralCommandNode<TS>(LiteralValue, Command, Requirement, RedirectTarget, RedirectModifier, HasFork);

			foreach (CommandNode<TS> argument in Arguments)
			{
				result.AddChild(argument);
			}

			return result;
		}
	}

}