using System;
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
        private readonly RootCommandNode<TS> _arguments = new();
        private Command<TS> _command;
        private bool _forks;
        private RedirectModifier<TS> _modifier;
        private Predicate<TS> _requirement = s => true;
        private CommandNode<TS> _target;

        protected internal abstract T This { get; }

        public virtual ICollection<CommandNode<TS>> Arguments => _arguments.Children;

        public virtual Command<TS> Command => _command;

        public virtual Predicate<TS> Requirement => _requirement;

        public virtual CommandNode<TS> RedirectTarget => _target;

        public virtual RedirectModifier<TS> RedirectModifier => _modifier;

        public virtual bool HasFork => _forks;

        public abstract CommandNode<TS> Build();

        public virtual T Then(IArgumentBuilder<TS> argument)
        {
            if (_target != null) throw new InvalidOperationException("Cannot add children to a redirected node");
            _arguments.AddChild(argument.Build());
            return This;
        }

        public virtual T Then(CommandNode<TS> argument)
        {
            if (_target != null) throw new InvalidOperationException("Cannot add children to a redirected node");
            _arguments.AddChild(argument);
            return This;
        }

        public virtual T Executes(Command<TS> command)
        {
            _command = command;
            return This;
        }

        public virtual T Requires(Predicate<TS> requirement)
        {
            _requirement = requirement;
            return This;
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
            if (Arguments.Count > 0) throw new InvalidOperationException("Cannot forward a node with children");
            _target = target;
            _modifier = modifier;
            _forks = fork;
            return This;
        }
    }
}