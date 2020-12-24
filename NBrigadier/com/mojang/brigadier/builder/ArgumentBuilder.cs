using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.builder
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier.tree;
	using com.mojang.brigadier.tree;


	public abstract class ArgumentBuilder<S, T> where T : ArgumentBuilder<S, T>
	{
		private readonly RootCommandNode<S> arguments = new RootCommandNode<S>();
		private Command<S> command;
		private System.Predicate<S> requirement = s => true;
		private CommandNode<S> target;
		private RedirectModifier<S> modifier = null;
		private bool forks;

		protected internal abstract T This {get;}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T then(final ArgumentBuilder<S, ?> argument)
		public virtual T Then<T1>(ArgumentBuilder<S, T1> argument) where T1 : ArgumentBuilder<S, T1>
        {
			if (target != null)
			{
				throw new System.InvalidOperationException("Cannot add children to a redirected node");
			}
			arguments.AddChild(argument.Build());
			return This;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T then(final com.mojang.brigadier.tree.CommandNode<S> argument)
		public virtual T Then(CommandNode<S> argument)
		{
			if (target != null)
			{
				throw new System.InvalidOperationException("Cannot add children to a redirected node");
			}
			arguments.AddChild(argument);
			return This;
		}

		public virtual ICollection<CommandNode<S>> Arguments
		{
			get
			{
				return arguments.Children;
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T executes(final com.mojang.brigadier.Command<S> command)
		public virtual T Executes(Command<S> command)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T requires(final java.util.function.Predicate<S> requirement)
		public virtual T Requires(System.Predicate<S> requirement)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T redirect(final com.mojang.brigadier.tree.CommandNode<S> target)
		public virtual T RedirectNode(CommandNode<S> target)
		{
			return Forward(target, null, false);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T redirect(final com.mojang.brigadier.tree.CommandNode<S> target, final com.mojang.brigadier.SingleRedirectModifier<S> modifier)
		public virtual T RedirectNode(CommandNode<S> target, SingleRedirectModifier<S> modifier)
		{
			return Forward(target, modifier == null ? null : o => new List<S> {modifier(o)}, false);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T fork(final com.mojang.brigadier.tree.CommandNode<S> target, final com.mojang.brigadier.RedirectModifier<S> modifier)
		public virtual T ForkNode(CommandNode<S> target, RedirectModifier<S> modifier)
		{
			return Forward(target, modifier, true);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public T forward(final com.mojang.brigadier.tree.CommandNode<S> target, final com.mojang.brigadier.RedirectModifier<S> modifier, final boolean fork)
		public virtual T Forward(CommandNode<S> target, RedirectModifier<S> modifier, bool fork)
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

		public abstract CommandNode<S> Build();
	}

}