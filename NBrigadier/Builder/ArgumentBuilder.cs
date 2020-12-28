using System.Collections.Generic;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public abstract class ArgumentBuilder<TS, T> : IArgumentBuilder<TS> where T : ArgumentBuilder<TS, T>
	{
		private RootCommandNode<TS> _arguments = new RootCommandNode<TS>();
		private Command<TS> _command;
		private System.Predicate<TS> _requirement = s => true;
		private CommandNode<TS> _target;
		private RedirectModifier<TS> _modifier = null;
		private bool _forks;

		protected internal abstract T This { get; }

		public virtual T Then(IArgumentBuilder<TS> argument)
		{
			if (_target != null)
			{
				throw new System.InvalidOperationException("Cannot add children to a redirected node");
			}
			_arguments.AddChild(argument.Build());
			return This;
		}

        public virtual T Then(CommandNode<TS> argument)
        {
			if (_target != null)
			{
				throw new System.InvalidOperationException("Cannot add children to a redirected node");
			}
			_arguments.AddChild(argument);
			return This;
		}

		public virtual ICollection<CommandNode<TS>> Arguments
		{
			get
			{
				return _arguments.Children;
			}
		}

		public virtual T Executes(Command<TS> command)
		{
			this._command = command;
			return This;
		}

		public virtual Command<TS> Command
		{
			get
			{
				return _command;
			}
		}

		public virtual T Requires(System.Predicate<TS> requirement)
		{
			this._requirement = requirement;
			return This;
		}

		public virtual System.Predicate<TS> Requirement
		{
			get
			{
				return _requirement;
			}
		}

		public virtual T Redirect(CommandNode<TS> target)
		{
			return Forward(target, null, false);
		}

		public virtual T Redirect(CommandNode<TS> target, SingleRedirectModifier<TS> modifier)
		{
			return Forward(target, modifier == null ? null : o => CollectionsHelper.SingletonList(modifier(o)), false);
		}

		public virtual T Fork(CommandNode<TS> target, RedirectModifier<TS> modifier)
		{
			return Forward(target, modifier, true);
		}

		public virtual T Forward(CommandNode<TS> target, RedirectModifier<TS> modifier, bool fork)
		{
			if (Arguments.Count > 0)
			{
				throw new System.InvalidOperationException("Cannot forward a node with children");
			}
			this._target = target;
			this._modifier = modifier;
			this._forks = fork;
			return This;
		}

		public virtual CommandNode<TS> RedirectTarget
		{
			get
			{
				return _target;
			}
		}

		public virtual RedirectModifier<TS> RedirectModifier
		{
			get
			{
				return _modifier;
			}
		}

		public virtual bool HasFork
		{
			get
			{
				return _forks;
			}
		}

		public abstract CommandNode<TS> Build();
	}

}