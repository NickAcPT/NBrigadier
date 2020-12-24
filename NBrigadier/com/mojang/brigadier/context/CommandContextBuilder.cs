using System.Collections.Generic;

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
//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private final java.util.Map<String, ParsedArgument<S, ?>> arguments = new java.util.LinkedHashMap<>();
        private readonly IDictionary<string, ParsedArgument<S, object>> arguments =
            new Dictionary<string, ParsedArgument<S, object>>();
        private readonly CommandNode<S> rootNode;
		private readonly IList<ParsedCommandNode<S>> nodes = new List<ParsedCommandNode<S>>();
		private readonly CommandDispatcher<S> dispatcher;
		private S source;
		private Command<S> command;
		private CommandContextBuilder<S> child;
		private StringRange range;
		private RedirectModifier<S> modifier = null;
		private bool forks;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContextBuilder(final com.mojang.brigadier.CommandDispatcher<S> dispatcher, final S source, final com.mojang.brigadier.tree.CommandNode<S> rootNode, final int start)
		public CommandContextBuilder(CommandDispatcher<S> dispatcher, S source, CommandNode<S> rootNode, int start)
		{
			this.rootNode = rootNode;
			this.dispatcher = dispatcher;
			this.source = source;
			this.range = StringRange.At(start);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContextBuilder<S> withSource(final S source)
		public virtual CommandContextBuilder<S> WithSource(S source)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContextBuilder<S> withArgument(final String name, final ParsedArgument<S, ?> argument)
		public virtual CommandContextBuilder<S> WithArgument(string name, ParsedArgument<S, object> argument)
		{
			this.arguments[name] = argument;
			return this;
		}

//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public java.util.Map<String, ParsedArgument<S, ?>> getArguments()
		public virtual IDictionary<string, ParsedArgument<S, object>> Arguments
		{
			get
			{
                return arguments;
			}
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContextBuilder<S> withCommand(final com.mojang.brigadier.Command<S> command)
		public virtual CommandContextBuilder<S> WithCommand(Command<S> command)
		{
			this.command = command;
			return this;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContextBuilder<S> withNode(final com.mojang.brigadier.tree.CommandNode<S> node, final StringRange range)
		public virtual CommandContextBuilder<S> WithNode(CommandNode<S> node, StringRange range)
		{
			nodes.Add(new ParsedCommandNode<S>(node, range));
			this.range = StringRange.Encompassing(this.range, range);
			this.modifier = node.RedirectModifier;
			this.forks = node.Fork;
			return this;
		}

		public virtual CommandContextBuilder<S> Copy()
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CommandContextBuilder<S> copy = new CommandContextBuilder<>(dispatcher, source, rootNode, range.getStart());
			CommandContextBuilder<S> copy = new CommandContextBuilder<S>(dispatcher, source, rootNode, range.Start);
			copy.command = command;
//TODO TASK: There is no .NET Dictionary equivalent to the Java 'putAll' method:
            foreach (var (key, value) in arguments)
            {
                arguments[key] = value;
            }
			((List<ParsedCommandNode<S>>)copy.nodes).AddRange(nodes);
			copy.child = child;
			copy.range = range;
			copy.forks = forks;
			return copy;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContextBuilder<S> withChild(final CommandContextBuilder<S> child)
		public virtual CommandContextBuilder<S> WithChild(CommandContextBuilder<S> child)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContext<S> build(final String input)
		public virtual CommandContext<S> Build(string input)
		{
			return new CommandContext<S>(source, input, arguments, command, rootNode, nodes, range, child == null ? null : child.Build(input), modifier, forks);
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public SuggestionContext<S> findSuggestionContext(final int cursor)
		public virtual SuggestionContext<S> FindSuggestionContext(int cursor)
		{
			if (range.Start <= cursor)
			{
				if (range.End < cursor)
				{
					if (child != null)
					{
						return child.FindSuggestionContext(cursor);
					}
					else if (nodes.Count > 0)
					{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParsedCommandNode<S> last = nodes.get(nodes.size() - 1);
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringRange nodeRange = node.getRange();
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