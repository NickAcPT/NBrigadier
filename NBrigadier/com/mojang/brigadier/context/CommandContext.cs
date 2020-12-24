using System;
using System.Collections.Generic;
using System.Linq;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.context
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier.tree;


	public class CommandContext<S>
	{

		private static readonly IDictionary<Type, Type> PRIMITIVE_TO_WRAPPER = new Dictionary<Type, Type>();

		static CommandContext()
		{
		}

		private readonly S source;
		private readonly string input;
		private readonly Command<S> command;
//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private final java.util.Map<String, ParsedArgument<S, ?>> arguments;
		private readonly IDictionary<string, ParsedArgument<S, object>> arguments;
		private readonly CommandNode<S> rootNode;
		private readonly IList<ParsedCommandNode<S>> nodes;
		private readonly StringRange range;
		private readonly CommandContext<S> child;
		private readonly RedirectModifier<S> modifier;
		private readonly bool forks;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContext(final S source, final String input, final java.util.Map<String, ParsedArgument<S, ?>> arguments, final com.mojang.brigadier.Command<S> command, final com.mojang.brigadier.tree.CommandNode<S> rootNode, final java.util.List<ParsedCommandNode<S>> nodes, final StringRange range, final CommandContext<S> child, final com.mojang.brigadier.RedirectModifier<S> modifier, boolean forks)
//TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandContext<S> copyFor(final S source)
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

//TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <V> V getArgument(final String name, final Class<V> clazz)
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
public virtual V GetArgument<V>(string name)
{
    return GetArgument<V>(name, typeof(V));
}

public virtual V GetArgument<V>(string name, Type clazz)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParsedArgument<S, ?> argument = arguments.get(name);
//WARNING: Java wildcard generics have no direct equivalent in C#:
			ParsedArgument<S, object> argument = arguments[name];

			if (argument == null)
			{
				throw new System.ArgumentException("No such argument '" + name + "' exists on this command");
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Object result = argument.getResult();
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CommandContext that = (CommandContext) o;
			CommandContext<S> that = (CommandContext<S>) o;

			if (!arguments.Equals(that.arguments))
			{
				return false;
			}
			if (!rootNode.Equals(that.rootNode))
			{
				return false;
			}
//WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
//ORIGINAL LINE: if (nodes.size() != that.nodes.size() || !nodes.equals(that.nodes))
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