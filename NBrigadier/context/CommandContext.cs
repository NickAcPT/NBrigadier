using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class CommandContext<S>
    {
        private static readonly IDictionary<Type, Type> PRIMITIVE_TO_WRAPPER = new Dictionary<Type, Type>();
        private readonly IDictionary<string, ParsedArgument<S, object>> arguments;
        private readonly CommandContext<S> child;
        private readonly Command<S> command;
        private readonly bool forks;
        private readonly string input;
        private readonly RedirectModifier<S> modifier;
        private readonly IList<ParsedCommandNode<S>> nodes;
        private readonly StringRange range;
        private readonly CommandNode<S> rootNode;

        private readonly S source;

        static CommandContext()
        {
        }

        public CommandContext(S source, string input, IDictionary<string, ParsedArgument<S, object>> arguments,
            Command<S> command, CommandNode<S> rootNode, IList<ParsedCommandNode<S>> nodes, StringRange range,
            CommandContext<S> child, RedirectModifier<S> modifier, bool forks)
        {
            this.source = source;
            this.input = input;
            this.arguments = arguments;
            this.command = command;
            this.rootNode = rootNode;
            this.nodes = nodes;
            this.range = range;
            this.child = child;
            this.modifier = modifier;
            this.forks = forks;
        }

        public virtual CommandContext<S> Child => child;

        public virtual CommandContext<S> LastChild
        {
            get
            {
                var result = this;
                while (result.Child != null) result = result.Child;
                return result;
            }
        }

        public virtual Command<S> Command => command;

        public virtual S Source => source;

        public virtual RedirectModifier<S> RedirectModifier => modifier;

        public virtual StringRange Range => range;

        public virtual string Input => input;

        public virtual CommandNode<S> RootNode => rootNode;

        public virtual IList<ParsedCommandNode<S>> Nodes => nodes;

        public virtual bool Forked => forks;

        public virtual CommandContext<S> CopyFor(S source)
        {
            if (Equals(this.source, source)) return this;
            return new CommandContext<S>(source, input, arguments, command, rootNode, nodes, range, child, modifier,
                forks);
        }

        public virtual V GetArgument<V>(string name)
        {
            return GetArgument<V>(name, typeof(V));
        }

        public virtual V GetArgument<V>(string name, Type clazz)
        {
            var argument = arguments[name];

            if (argument == null) throw new ArgumentException("No such argument '" + name + "' exists on this command");

            var result = argument.Result;
            if (clazz.IsInstanceOfType(argument.Result))
                return (V) result;
            throw new ArgumentException("Argument '" + name + "' is defined as " + result.GetType().Name + ", not " +
                                        clazz);
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is CommandContext<S>)) return false;

            var that = (CommandContext<S>) o;

            if (!arguments.Equals(that.arguments)) return false;
            if (!rootNode.Equals(that.rootNode)) return false;
            if (nodes.Count != that.nodes.Count || !nodes.SequenceEqual(that.nodes)) return false;
            if (!command?.Equals(that.command) ?? that.command != null) return false;
            if (!source.Equals(that.source)) return false;
            if (!child?.Equals(that.child) ?? that.child != null) return false;

            return true;
        }

        public override int GetHashCode()
        {
            var result = source.GetHashCode();
            result = 31 * result + arguments.GetHashCode();
            result = 31 * result + (command != null ? command.GetHashCode() : 0);
            result = 31 * result + rootNode.GetHashCode();
            result = 31 * result + nodes.GetHashCode();
            result = 31 * result + (child != null ? child.GetHashCode() : 0);
            return result;
        }

        public virtual bool HasNodes()
        {
            return nodes.Count > 0;
        }
    }
}