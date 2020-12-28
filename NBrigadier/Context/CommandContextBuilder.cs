using System;
using System.Collections.Generic;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class CommandContextBuilder<TS>
    {
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: private java.util.Map<String, ParsedArgument<S, ?>> arguments = new java.util.LinkedHashMap<>();
        private readonly IDictionary<string, IParsedArgument> _arguments = new Dictionary<string, IParsedArgument>();
        private readonly CommandDispatcher<TS> _dispatcher;
        private readonly IList<ParsedCommandNode<TS>> _nodes = new List<ParsedCommandNode<TS>>();
        private readonly CommandNode<TS> _rootNode;
        private CommandContextBuilder<TS> _child;
        private Command<TS> _command;
        private bool _forks;
        private RedirectModifier<TS> _modifier;
        private StringRange _range;
        private TS _source;

        public CommandContextBuilder(CommandDispatcher<TS> dispatcher, TS source, CommandNode<TS> rootNode, int start)
        {
            _rootNode = rootNode;
            _dispatcher = dispatcher;
            _source = source;
            _range = StringRange.At(start);
        }

        public virtual TS Source => _source;

        public virtual CommandNode<TS> RootNode => _rootNode;

        // WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public java.util.Map<String, ParsedArgument<S, ?>> getArguments()
        public virtual IDictionary<string, IParsedArgument> Arguments => _arguments;

        public virtual CommandContextBuilder<TS> Child => _child;

        public virtual CommandContextBuilder<TS> LastChild
        {
            get
            {
                var result = this;
                while (result.Child != null) result = result.Child;
                return result;
            }
        }

        public virtual Command<TS> Command => _command;

        public virtual IList<ParsedCommandNode<TS>> Nodes => _nodes;

        public virtual CommandDispatcher<TS> Dispatcher => _dispatcher;

        public virtual StringRange Range => _range;

        public virtual CommandContextBuilder<TS> WithSource(TS source)
        {
            _source = source;
            return this;
        }

        public virtual CommandContextBuilder<TS> WithArgument<T1>(string name, IParsedArgument argument)
        {
            _arguments[name] = argument;
            return this;
        }

        public virtual CommandContextBuilder<TS> WithCommand(Command<TS> command)
        {
            _command = command;
            return this;
        }

        public virtual CommandContextBuilder<TS> WithNode(CommandNode<TS> node, StringRange range)
        {
            _nodes.Add(new ParsedCommandNode<TS>(node, range));
            _range = StringRange.Encompassing(_range, range);
            _modifier = node.RedirectModifier;
            _forks = node.Fork;
            return this;
        }

        public virtual CommandContextBuilder<TS> Copy()
        {
            var copy = new CommandContextBuilder<TS>(_dispatcher, _source, _rootNode, _range.Start);
            copy._command = _command;
            copy._arguments.PutAll(_arguments);
            ((List<ParsedCommandNode<TS>>) copy._nodes).AddRange(_nodes);
            copy._child = _child;
            copy._range = _range;
            copy._forks = _forks;
            return copy;
        }

        public virtual CommandContextBuilder<TS> WithChild(CommandContextBuilder<TS> child)
        {
            _child = child;
            return this;
        }

        public virtual CommandContext<TS> Build(string input)
        {
            return new(_source, input, _arguments, _command, _rootNode, _nodes, _range,
                _child == null ? null : _child.Build(input), _modifier, _forks);
        }

        public virtual SuggestionContext<TS> FindSuggestionContext(int cursor)
        {
            if (_range.Start <= cursor)
            {
                if (_range.End < cursor)
                {
                    if (_child != null) return _child.FindSuggestionContext(cursor);

                    if (_nodes.Count > 0)
                    {
                        var last = _nodes[_nodes.Count - 1];
                        return new SuggestionContext<TS>(last.Node, last.Range.End + 1);
                    }

                    return new SuggestionContext<TS>(_rootNode, _range.Start);
                }

                var prev = _rootNode;
                foreach (var node in _nodes)
                {
                    var nodeRange = node.Range;
                    if (nodeRange.Start <= cursor && cursor <= nodeRange.End)
                        return new SuggestionContext<TS>(prev, nodeRange.Start);
                    prev = node.Node;
                }

                if (prev == null) throw new InvalidOperationException("Can't find node before cursor");
                return new SuggestionContext<TS>(prev, _range.Start);
            }

            throw new InvalidOperationException("Can't find node before cursor");
        }
    }
}