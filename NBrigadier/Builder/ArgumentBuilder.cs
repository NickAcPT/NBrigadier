using System;
using System.Collections.Generic;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public abstract class ArgumentBuilder<TS, T> where T : ArgumentBuilder<TS, T>
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

        public virtual CommandNode<TS> Redirect => _target;

        public virtual RedirectModifier<TS> RedirectModifier => _modifier;

        public virtual bool Fork => _forks;

        public virtual T Then<T1>(ArgumentBuilder<TS, T1> argument) where T1 : ArgumentBuilder<TS, T1>
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
            this._command = command;
            return This;
        }

        public virtual T Requires(Predicate<TS> requirement)
        {
            this._requirement = requirement;
            return This;
        }

        public virtual T RedirectNode(CommandNode<TS> target)
        {
            return Forward(target, null, false);
        }

        public virtual T RedirectNode(CommandNode<TS> target, SingleRedirectModifier<TS> modifier)
        {
            return Forward(target, modifier == null ? null : o => new List<TS> {modifier(o)}, false);
        }

        public virtual T ForkNode(CommandNode<TS> target, RedirectModifier<TS> modifier)
        {
            return Forward(target, modifier, true);
        }

        public virtual T Forward(CommandNode<TS> target, RedirectModifier<TS> modifier, bool fork)
        {
            if (_arguments.Children.Count > 0)
                throw new InvalidOperationException("Cannot forward a node with children");
            this._target = target;
            this._modifier = modifier;
            _forks = fork;
            return This;
        }

        public abstract CommandNode<TS> Build();
    }
}