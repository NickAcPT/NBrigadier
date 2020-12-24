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

		static CommandContext()
		{
		}

		private readonly S source;
		private readonly string input;
		private readonly Command<S> command;
		private readonly IDictionary<string, ParsedArgument<S, object>> arguments;
		private readonly CommandNode<S> rootNode;
		private readonly IList<ParsedCommandNode<S>> nodes;
		private readonly StringRange range;
		private readonly CommandContext<S> child;
		private readonly RedirectModifier<S> modifier;
		private readonly bool forks;

		public CommandContext(S source, string input, IDictionary<string, ParsedArgument<S, object>> arguments, Command<S> command, CommandNode<S> rootNode, IList<ParsedCommandNode<S>> nodes, StringRange range, CommandContext<S> child, RedirectModifier<S> modifier, bool forks)
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

		public virtual CommandContext<S> CopyFor(S source)
		{
			if (Equals(this.source, source))
			{
				return this;
			}
			return new CommandContext<S>(source, input, arguments, command, rootNode, nodes, range, child, modifier, forks);
		}

		public virtual CommandContext<S> Child
		{
			get
			{
				return child;
			}
		}

		public virtual CommandContext<S> LastChild
		{
			get
			{
				CommandContext<S> result = this;
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

		public virtual S Source
		{
			get
			{
				return source;
			}
		}

public virtual V GetArgument<V>(string name)
{
    return GetArgument<V>(name, typeof(V));
}

public virtual V GetArgument<V>(string name, Type clazz)
		{
			ParsedArgument<S, object> argument = arguments[name];

			if (argument == null)
			{
				throw new System.ArgumentException("No such argument '" + name + "' exists on this command");
			}

			object result = argument.Result;
			if (clazz.IsInstanceOfType(argument.Result))
			{
				return (V) result;
			}
			else
			{
				throw new System.ArgumentException("Argument '" + name + "' is defined as " + result.GetType().Name + ", not " + clazz);
			}
		}

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is CommandContext<S>))
			{
				return false;
			}

			CommandContext<S> that = (CommandContext<S>) o;

			if (!arguments.Equals(that.arguments))
			{
				return false;
			}
			if (!rootNode.Equals(that.rootNode))
			{
				return false;
			}
			if (nodes.Count != that.nodes.Count || !nodes.SequenceEqual(that.nodes))
			{
				return false;
			}
			if (command != null ?!command.Equals(that.command) : that.command != null)
			{
				return false;
			}
			if (!source.Equals(that.source))
			{
				return false;
			}
			if (child != null ?!child.Equals(that.child) : that.child != null)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			int result = source.GetHashCode();
			result = 31 * result + arguments.GetHashCode();
			result = 31 * result + (command != null ? command.GetHashCode() : 0);
			result = 31 * result + rootNode.GetHashCode();
			result = 31 * result + nodes.GetHashCode();
			result = 31 * result + (child != null ? child.GetHashCode() : 0);
			return result;
		}

		public virtual RedirectModifier<S> RedirectModifier
		{
			get
			{
				return modifier;
			}
		}

		public virtual StringRange Range
		{
			get
			{
				return range;
			}
		}

		public virtual string Input
		{
			get
			{
				return input;
			}
		}

		public virtual CommandNode<S> RootNode
		{
			get
			{
				return rootNode;
			}
		}

		public virtual IList<ParsedCommandNode<S>> Nodes
		{
			get
			{
				return nodes;
			}
		}

		public virtual bool HasNodes()
		{
			return nodes.Count > 0;
		}

		public virtual bool Forked
		{
			get
			{
				return forks;
			}
		}
	}

}