using System;
using System.Collections.Generic;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Context
{
    public class CommandContextBuilder<S>
    {
        private readonly IDictionary<string, ParsedArgument<S, object>> arguments =
            new Dictionary<string, ParsedArgument<S, object>>();

        private readonly CommandDispatcher<S> dispatcher;
        private readonly IList<ParsedCommandNode<S>> nodes = new List<ParsedCommandNode<S>>();
        private readonly CommandNode<S> rootNode;
        private CommandContextBuilder<S> child;
        private Command<S> command;
        private bool forks;
        private RedirectModifier<S> modifier;
        private StringRange range;
        private S source;

        public CommandContextBuilder(CommandDispatcher<S> dispatcher, S source, CommandNode<S> rootNode, int start)
        {
            this.rootNode = rootNode;
            this.dispatcher = dispatcher;
            this.source = source;
            range = StringRange.At(start);
        }

        public virtual S Source => source;

        public virtual CommandNode<S> RootNode => rootNode;

        public virtual IDictionary<string, ParsedArgument<S, object>> Arguments => arguments;

        public virtual CommandContextBuilder<S> Child => child;

        public virtual CommandContextBuilder<S> LastChild
        {
            get
            {
                var result = this;
                while (result.Child != null) result = result.Child;
                return result;
            }
        }

        public virtual Command<S> Command => command;

        public virtual IList<ParsedCommandNode<S>> Nodes => nodes;

        public virtual CommandDispatcher<S> Dispatcher => dispatcher;

        public virtual StringRange Range => range;

        public virtual CommandContextBuilder<S> WithSource(S source)
        {
            this.source = source;
            return this;
        }

        public virtual CommandContextBuilder<S> WithArgument(string name, ParsedArgument<S, object> argument)
        {
            arguments[name] = argument;
            return this;
        }

        public virtual CommandContextBuilder<S> WithCommand(Command<S> command)
        {
            this.command = command;
            return this;
        }

        public virtual CommandContextBuilder<S> WithNode(CommandNode<S> node, StringRange range)
        {
            nodes.Add(new ParsedCommandNode<S>(node, range));
            this.range = StringRange.Encompassing(this.range, range);
            modifier = node.RedirectModifier;
            forks = node.Fork;
            return this;
        }

        public virtual CommandContextBuilder<S> Copy()
        {
            var copy = new CommandContextBuilder<S>(dispatcher, source, rootNode, range.Start);
            copy.command = command;
            foreach (var (key, value) in arguments) arguments[key] = value;
            ((List<ParsedCommandNode<S>>) copy.nodes).AddRange(nodes);
            copy.child = child;
            copy.range = range;
            copy.forks = forks;
            return copy;
        }

        public virtual CommandContextBuilder<S> WithChild(CommandContextBuilder<S> child)
        {
            this.child = child;
            return this;
        }

        public virtual CommandContext<S> Build(string input)
        {
            return new(source, input, arguments, command, rootNode, nodes, range,
                child == null ? null : child.Build(input), modifier, forks);
        }

        public virtual SuggestionContext<S> FindSuggestionContext(int cursor)
        {
            if (range.Start <= cursor)
            {
                if (range.End < cursor)
                {
                    if (child != null) return child.FindSuggestionContext(cursor);

                    if (nodes.Count > 0)
                    {
                        var last = nodes[nodes.Count - 1];
                        return new SuggestionContext<S>(last.Node, last.Range.End + 1);
                    }

                    return new SuggestionContext<S>(rootNode, range.Start);
                }

                var prev = rootNode;
                foreach (var node in nodes)
                {
                    var nodeRange = node.Range;
                    if (nodeRange.Start <= cursor && cursor <= nodeRange.End)
                        return new SuggestionContext<S>(prev, nodeRange.Start);
                    prev = node.Node;
                }

                if (prev == null) throw new InvalidOperationException("Can't find node before cursor");
                return new SuggestionContext<S>(prev, range.Start);
            }

            throw new InvalidOperationException("Can't find node before cursor");
        }
    }
}