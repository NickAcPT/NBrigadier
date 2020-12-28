using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class CommandContext<TS> : ICommandContext
    {
        private static readonly IDictionary<Type, Type> _primitiveToWrapper = new Dictionary<Type, Type>();

        private readonly IDictionary<string, IParsedArgument> _arguments;
        private readonly CommandContext<TS> _child;
        private readonly Command<TS> _command;
        private readonly bool _forks;
        private readonly string _input;
        private readonly RedirectModifier<TS> _modifier;
        private readonly IList<ParsedCommandNode<TS>> _nodes;
        private readonly StringRange _range;
        private readonly CommandNode<TS> _rootNode;

        private TS _source;

        static CommandContext()
        {
            _primitiveToWrapper[typeof(bool)] = typeof(bool);
            _primitiveToWrapper[typeof(sbyte)] = typeof(byte);
            _primitiveToWrapper[typeof(short)] = typeof(short);
            _primitiveToWrapper[typeof(char)] = typeof(char);
            _primitiveToWrapper[typeof(int)] = typeof(int);
            _primitiveToWrapper[typeof(long)] = typeof(long);
            _primitiveToWrapper[typeof(float)] = typeof(float);
            _primitiveToWrapper[typeof(double)] = typeof(double);
        }

        public CommandContext(TS source, string input, IDictionary<string, IParsedArgument> arguments,
            Command<TS> command, CommandNode<TS> rootNode, IList<ParsedCommandNode<TS>> nodes, StringRange range,
            CommandContext<TS> child, RedirectModifier<TS> modifier, bool forks)
        {
            _source = source;
            _input = input;
            _arguments = arguments;
            _command = command;
            _rootNode = rootNode;
            _nodes = nodes;
            _range = range;
            _child = child;
            _modifier = modifier;
            _forks = forks;
        }

        public virtual CommandContext<TS> Child => _child;

        public virtual CommandContext<TS> LastChild
        {
            get
            {
                var result = this;
                while (result.Child != null) result = result.Child;
                return result;
            }
        }

        public virtual Command<TS> Command => _command;

        public virtual TS Source => _source;

        public virtual RedirectModifier<TS> RedirectModifier => _modifier;

        public virtual CommandNode<TS> RootNode => _rootNode;

        public virtual IList<ParsedCommandNode<TS>> Nodes => _nodes;

        public virtual StringRange Range => _range;

        public virtual string Input => _input;

        public virtual bool HasNodes()
        {
            return _nodes.Count > 0;
        }

        public virtual bool Forked => _forks;

        public virtual CommandContext<TS> CopyFor(TS source)
        {
            if (_source.Equals(source)) return this;
            return new CommandContext<TS>(source, _input, _arguments, _command, _rootNode, _nodes, _range, _child,
                _modifier, _forks);
        }

        public virtual TV GetArgument<TV>(string name, Type clazz)
        {
            var argument = _arguments.GetValueOrNull(name);

            if (argument == null) throw new ArgumentException("No such argument '" + name + "' exists on this command");

            var result = argument.ResultObject;
            if (_primitiveToWrapper.GetOrDefault(clazz, clazz).IsAssignableFrom(result.GetType()))
                return (TV) result;
            throw new ArgumentException("Argument '" + name + "' is defined as " + result.GetType().Name + ", not " +
                                        clazz);
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;
            if (!(o is ICommandContext)) return false;

            var that = (CommandContext<TS>) o;

            if (!_arguments.Equals(that._arguments)) return false;
            if (!_rootNode.Equals(that._rootNode)) return false;
            if (_nodes.Count != that._nodes.Count || !_nodes.SequenceEqual(that._nodes)) return false;
            if (_command != null ? !_command.Equals(that._command) : that._command != null) return false;
            if (!_source.Equals(that._source)) return false;
            if (_child != null ? !_child.Equals(that._child) : that._child != null) return false;

            return true;
        }

        public override int GetHashCode()
        {
            var result = _source.GetHashCode();
            result = 31 * result + _arguments.GetHashCode();
            result = 31 * result + (_command != null ? _command.GetHashCode() : 0);
            result = 31 * result + _rootNode.GetHashCode();
            result = 31 * result + _nodes.GetHashCode();
            result = 31 * result + (_child != null ? _child.GetHashCode() : 0);
            return result;
        }
    }
}