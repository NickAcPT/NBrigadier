using System;
using System.Collections.Generic;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Builder
{
    public abstract class ArgumentBuilder<S, T> where T : ArgumentBuilder<S, T>
    {
        private readonly RootCommandNode<S> arguments = new();
        private Command<S> command;
        private bool forks;
        private RedirectModifier<S> modifier;
        private Predicate<S> requirement = s => true;
        private CommandNode<S> target;

        protected internal abstract T This { get; }

        public virtual ICollection<CommandNode<S>> Arguments => arguments.Children;

        public virtual Command<S> Command => command;

        public virtual Predicate<S> Requirement => requirement;

        public virtual CommandNode<S> Redirect => target;

        public virtual RedirectModifier<S> RedirectModifier => modifier;

        public virtual bool Fork => forks;

        public virtual T Then<T1>(ArgumentBuilder<S, T1> argument) where T1 : ArgumentBuilder<S, T1>
        {
            if (target != null) throw new InvalidOperationException("Cannot add children to a redirected node");
            arguments.AddChild(argument.Build());
            return This;
        }

        public virtual T Then(CommandNode<S> argument)
        {
            if (target != null) throw new InvalidOperationException("Cannot add children to a redirected node");
            arguments.AddChild(argument);
            return This;
        }

        public virtual T Executes(Command<S> command)
        {
            this.command = command;
            return This;
        }

        public virtual T Requires(Predicate<S> requirement)
        {
            this.requirement = requirement;
            return This;
        }

        public virtual T RedirectNode(CommandNode<S> target)
        {
            return Forward(target, null, false);
        }

        public virtual T RedirectNode(CommandNode<S> target, SingleRedirectModifier<S> modifier)
        {
            return Forward(target, modifier == null ? null : o => new List<S> {modifier(o)}, false);
        }

        public virtual T ForkNode(CommandNode<S> target, RedirectModifier<S> modifier)
        {
            return Forward(target, modifier, true);
        }

        public virtual T Forward(CommandNode<S> target, RedirectModifier<S> modifier, bool fork)
        {
            if (arguments.Children.Count > 0)
                throw new InvalidOperationException("Cannot forward a node with children");
            this.target = target;
            this.modifier = modifier;
            forks = fork;
            return This;
        }

        public abstract CommandNode<S> Build();
    }
}