using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;
using NBrigadier.Generics;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.context
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier.tree;


	public class CommandContextBuilder<S>
	{
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: private java.util.Map<String, ParsedArgument<S, ?>> arguments = new java.util.LinkedHashMap<>();
		private IDictionary<string, IParsedArgument> arguments = new Dictionary<string, IParsedArgument>();
		private CommandNode<S> rootNode;
		private IList<ParsedCommandNode<S>> nodes = new List<ParsedCommandNode<S>>();
		private CommandDispatcher<S> dispatcher;
		private S source;
		private Command<S> command;
		private CommandContextBuilder<S> child;
		private StringRange range;
		private RedirectModifier<S> modifier = null;
		private bool forks;

		public CommandContextBuilder(CommandDispatcher<S> dispatcher, S source, CommandNode<S> rootNode, int start)
		{
			this.rootNode = rootNode;
			this.dispatcher = dispatcher;
			this.source = source;
			this.range = StringRange.at(start);
		}

		public virtual CommandContextBuilder<S> withSource(S source)
		{
			this.source = source;
			return this;
		}

		public virtual S Source
		{
			get
			{
				return source;
			}
		}

		public virtual CommandNode<S> RootNode
		{
			get
			{
				return rootNode;
			}
		}

		public virtual CommandContextBuilder<S> withArgument<T1>(string name, IParsedArgument argument)
		{
			this.arguments[name] = argument;
			return this;
		}

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public java.util.Map<String, ParsedArgument<S, ?>> getArguments()
		public virtual IDictionary<string, IParsedArgument> Arguments
		{
			get
			{
				return arguments;
			}
		}

		public virtual CommandContextBuilder<S> withCommand(Command<S> command)
		{
			this.command = command;
			return this;
		}

		public virtual CommandContextBuilder<S> withNode(CommandNode<S> node, StringRange range)
		{
			nodes.Add(new ParsedCommandNode<S>(node, range));
			this.range = StringRange.encompassing(this.range, range);
			this.modifier = node.RedirectModifier;
			this.forks = node.Fork;
			return this;
		}

		public virtual CommandContextBuilder<S> copy()
		{
			 CommandContextBuilder<S> copy = new CommandContextBuilder<S>(dispatcher, source, rootNode, range.Start);
			copy.command = command;
			copy.arguments.PutAll(arguments);
			((List<ParsedCommandNode<S>>)copy.nodes).AddRange(nodes);
			copy.child = child;
			copy.range = range;
			copy.forks = forks;
			return copy;
		}

		public virtual CommandContextBuilder<S> withChild(CommandContextBuilder<S> child)
		{
			this.child = child;
			return this;
		}

		public virtual CommandContextBuilder<S> Child
		{
			get
			{
				return child;
			}
		}

		public virtual CommandContextBuilder<S> LastChild
		{
			get
			{
				CommandContextBuilder<S> result = this;
				while (result.Child != null)
				{
					result = result.Child;
				}
				return result;
			}
		}

		public virtual Command<S> Command
		{
			get
			{
				return command;
			}
		}

		public virtual IList<ParsedCommandNode<S>> Nodes
		{
			get
			{
				return nodes;
			}
		}

		public virtual CommandContext<S> build(string input)
		{
			return new CommandContext<S>(source, input, arguments, command, rootNode, nodes, range, child == null ? null : child.build(input), modifier, forks);
		}

		public virtual CommandDispatcher<S> Dispatcher
		{
			get
			{
				return dispatcher;
			}
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public virtual SuggestionContext<S> findSuggestionContext(int cursor)
		{
			if (range.Start <= cursor)
			{
				if (range.End < cursor)
				{
					if (child != null)
					{
						return child.findSuggestionContext(cursor);
					}
					else if (nodes.Count > 0)
					{
						 ParsedCommandNode<S> last = nodes[nodes.Count - 1];
						return new SuggestionContext<S>(last.Node, last.Range.End + 1);
					}
					else
					{
						return new SuggestionContext<S>(rootNode, range.Start);
					}
				}
				else
				{
					CommandNode<S> prev = rootNode;
					foreach (ParsedCommandNode<S> node in nodes)
					{
						 StringRange nodeRange = node.Range;
						if (nodeRange.Start <= cursor && cursor <= nodeRange.End)
						{
							return new SuggestionContext<S>(prev, nodeRange.Start);
						}
						prev = node.Node;
					}
					if (prev == null)
					{
						throw new System.InvalidOperationException("Can't find node before cursor");
					}
					return new SuggestionContext<S>(prev, range.Start);
				}
			}
			throw new System.InvalidOperationException("Can't find node before cursor");
		}
	}

}