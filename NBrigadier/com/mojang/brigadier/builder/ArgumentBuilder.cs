using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.builder
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier.tree;


	public abstract class ArgumentBuilder<S, T> : IArgumentBuilder<S> where T : ArgumentBuilder<S, T>
	{
		private com.mojang.brigadier.tree.RootCommandNode<S> arguments = new com.mojang.brigadier.tree.RootCommandNode<S>();
		private Command<S> command;
		private System.Predicate<S> requirement = s => true;
		private CommandNode<S> target;
		private RedirectModifier<S> modifier = null;
		private bool forks;

		protected internal abstract T This { get; }

		public virtual T then<T1>(IArgumentBuilder<S> argument)
		{
			if (target != null)
			{
				throw new System.InvalidOperationException("Cannot add children to a redirected node");
			}
			arguments.addChild(argument.build());
			return This;
		}

		public virtual T then(CommandNode<S> argument)
		{
			if (target != null)
			{
				throw new System.InvalidOperationException("Cannot add children to a redirected node");
			}
			arguments.addChild(argument);
			return This;
		}

		public virtual ICollection<CommandNode<S>> Arguments
		{
			get
			{
				return arguments.Children;
			}
		}

		public virtual T executes(Command<S> command)
		{
			this.command = command;
			return This;
		}

		public virtual Command<S> Command
		{
			get
			{
				return command;
			}
		}

		public virtual T requires(System.Predicate<S> requirement)
		{
			this.requirement = requirement;
			return This;
		}

		public virtual System.Predicate<S> Requirement
		{
			get
			{
				return requirement;
			}
		}

		public virtual T redirect(CommandNode<S> target)
		{
			return forward(target, null, false);
		}

		public virtual T redirect(CommandNode<S> target, SingleRedirectModifier<S> modifier)
		{
			return forward(target, modifier == null ? null : o => CollectionsHelper.SingletonList(modifier(o)), false);
		}

		public virtual T fork(CommandNode<S> target, RedirectModifier<S> modifier)
		{
			return forward(target, modifier, true);
		}

		public virtual T forward(CommandNode<S> target, RedirectModifier<S> modifier, bool fork)
		{
			if (arguments.Children.Count > 0)
			{
				throw new System.InvalidOperationException("Cannot forward a node with children");
			}
			this.target = target;
			this.modifier = modifier;
			this.forks = fork;
			return This;
		}

		public virtual CommandNode<S> Redirect
		{
			get
			{
				return target;
			}
		}

		public virtual RedirectModifier<S> RedirectModifier
		{
			get
			{
				return modifier;
			}
		}

		public virtual bool Fork
		{
			get
			{
				return forks;
			}
		}

		public abstract CommandNode<S> build();
	}

}