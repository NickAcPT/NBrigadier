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
		private IDictionary<string, IParsedArgument> _arguments = new Dictionary<string, IParsedArgument>();
		private CommandNode<TS> _rootNode;
		private IList<ParsedCommandNode<TS>> _nodes = new List<ParsedCommandNode<TS>>();
		private CommandDispatcher<TS> _dispatcher;
		private TS _source;
		private Command<TS> _command;
		private CommandContextBuilder<TS> _child;
		private StringRange _range;
		private RedirectModifier<TS> _modifier = null;
		private bool _forks;

		public CommandContextBuilder(CommandDispatcher<TS> dispatcher, TS source, CommandNode<TS> rootNode, int start)
		{
			this._rootNode = rootNode;
			this._dispatcher = dispatcher;
			this._source = source;
			this._range = StringRange.At(start);
		}

		public virtual CommandContextBuilder<TS> WithSource(TS source)
		{
			this._source = source;
			return this;
		}

		public virtual TS Source
		{
			get
			{
				return _source;
			}
		}

		public virtual CommandNode<TS> RootNode
		{
			get
			{
				return _rootNode;
			}
		}

		public virtual CommandContextBuilder<TS> WithArgument<T1>(string name, IParsedArgument argument)
		{
			this._arguments[name] = argument;
			return this;
		}

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public java.util.Map<String, ParsedArgument<S, ?>> getArguments()
		public virtual IDictionary<string, IParsedArgument> Arguments
		{
			get
			{
				return _arguments;
			}
		}

		public virtual CommandContextBuilder<TS> WithCommand(Command<TS> command)
		{
			this._command = command;
			return this;
		}

		public virtual CommandContextBuilder<TS> WithNode(CommandNode<TS> node, StringRange range)
		{
			_nodes.Add(new ParsedCommandNode<TS>(node, range));
			this._range = StringRange.Encompassing(this._range, range);
			this._modifier = node.RedirectModifier;
			this._forks = node.Fork;
			return this;
		}

		public virtual CommandContextBuilder<TS> Copy()
		{
			 CommandContextBuilder<TS> copy = new CommandContextBuilder<TS>(_dispatcher, _source, _rootNode, _range.Start);
			copy._command = _command;
			copy._arguments.PutAll(_arguments);
			((List<ParsedCommandNode<TS>>)copy._nodes).AddRange(_nodes);
			copy._child = _child;
			copy._range = _range;
			copy._forks = _forks;
			return copy;
		}

		public virtual CommandContextBuilder<TS> WithChild(CommandContextBuilder<TS> child)
		{
			this._child = child;
			return this;
		}

		public virtual CommandContextBuilder<TS> Child
		{
			get
			{
				return _child;
			}
		}

		public virtual CommandContextBuilder<TS> LastChild
		{
			get
			{
				CommandContextBuilder<TS> result = this;
				while (result.Child != null)
				{
					result = result.Child;
				}
				return result;
			}
		}

		public virtual Command<TS> Command
		{
			get
			{
				return _command;
			}
		}

		public virtual IList<ParsedCommandNode<TS>> Nodes
		{
			get
			{
				return _nodes;
			}
		}

		public virtual CommandContext<TS> Build(string input)
		{
			return new CommandContext<TS>(_source, input, _arguments, _command, _rootNode, _nodes, _range, _child == null ? null : _child.Build(input), _modifier, _forks);
		}

		public virtual CommandDispatcher<TS> Dispatcher
		{
			get
			{
				return _dispatcher;
			}
		}

		public virtual StringRange Range
		{
			get
			{
				return _range;
			}
		}

		public virtual SuggestionContext<TS> FindSuggestionContext(int cursor)
		{
			if (_range.Start <= cursor)
			{
				if (_range.End < cursor)
				{
					if (_child != null)
					{
						return _child.FindSuggestionContext(cursor);
					}
					else if (_nodes.Count > 0)
					{
						 ParsedCommandNode<TS> last = _nodes[_nodes.Count - 1];
						return new SuggestionContext<TS>(last.Node, last.Range.End + 1);
					}
					else
					{
						return new SuggestionContext<TS>(_rootNode, _range.Start);
					}
				}
				else
				{
					CommandNode<TS> prev = _rootNode;
					foreach (ParsedCommandNode<TS> node in _nodes)
					{
						 StringRange nodeRange = node.Range;
						if (nodeRange.Start <= cursor && cursor <= nodeRange.End)
						{
							return new SuggestionContext<TS>(prev, nodeRange.Start);
						}
						prev = node.Node;
					}
					if (prev == null)
					{
						throw new System.InvalidOperationException("Can't find node before cursor");
					}
					return new SuggestionContext<TS>(prev, _range.Start);
				}
			}
			throw new System.InvalidOperationException("Can't find node before cursor");
		}
	}

}