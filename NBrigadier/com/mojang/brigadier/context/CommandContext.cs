using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
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


	public class CommandContext<S> : ICommandContext
	{

		private static IDictionary<Type, Type> PRIMITIVE_TO_WRAPPER = new Dictionary<Type, Type>();

		static CommandContext()
		{
			PRIMITIVE_TO_WRAPPER[typeof(bool)] = typeof(Boolean);
			PRIMITIVE_TO_WRAPPER[typeof(sbyte)] = typeof(Byte);
			PRIMITIVE_TO_WRAPPER[typeof(short)] = typeof(short);
			PRIMITIVE_TO_WRAPPER[typeof(char)] = typeof(char);
			PRIMITIVE_TO_WRAPPER[typeof(int)] = typeof(int);
			PRIMITIVE_TO_WRAPPER[typeof(long)] = typeof(long);
			PRIMITIVE_TO_WRAPPER[typeof(float)] = typeof(float);
			PRIMITIVE_TO_WRAPPER[typeof(double)] = typeof(double);
		}

		private S source;
		private string input;
		private Command<S> command;
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: private java.util.Map<String, ParsedArgument<S, ?>> arguments;
		private IDictionary<string, ParsedArgument<S, object>> arguments;
		private CommandNode<S> rootNode;
		private IList<ParsedCommandNode<S>> nodes;
		private StringRange range;
		private CommandContext<S> child;
		private RedirectModifier<S> modifier;
		private bool forks;

// TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
// ORIGINAL LINE: public CommandContext(S source, String input, java.util.Map<String, ParsedArgument<S, ?>> arguments, com.mojang.brigadier.Command<S> command, com.mojang.brigadier.tree.CommandNode<S> rootNode, java.util.List<ParsedCommandNode<S>> nodes, StringRange range, CommandContext<S> child, com.mojang.brigadier.RedirectModifier<S> modifier, boolean forks)
		public CommandContext(S source, string input, IDictionary<T1> arguments, Command<S> command, CommandNode<S> rootNode, IList<ParsedCommandNode<S>> nodes, StringRange range, CommandContext<S> child, RedirectModifier<S> modifier, bool forks)
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

		public virtual CommandContext<S> copyFor(S source)
		{
			if (this.source == source)
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

// TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
// ORIGINAL LINE: @SuppressWarnings("unchecked") public <V> V getArgument(String name, Class<V> clazz)
		public virtual V getArgument<V>(string name, Type clazz)
		{
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: ParsedArgument<S, ?> argument = arguments.get(name);
			 ParsedArgument<S, object> argument = arguments.GetValueOrNull(name);

			if (argument == null)
			{
				throw new System.ArgumentException("No such argument '" + name + "' exists on this command");
			}

			 object result = argument.Result;
// ORIGINAL LINE: if (PRIMITIVE_TO_WRAPPER.getOrDefault(clazz, clazz).isAssignableFrom(result.getClass()))
			if (MapHelper.GetOrDefault(PRIMITIVE_TO_WRAPPER, clazz, clazz).IsAssignableFrom(result.GetType()))
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
			if (!(o is ICommandContext))
			{
				return false;
			}

			 ICommandContext that = (ICommandContext) o;

			if (!arguments.Equals(that.arguments))
			{
				return false;
			}
			if (!rootNode.Equals(that.rootNode))
			{
				return false;
			}
// WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
// ORIGINAL LINE: if (nodes.size() != that.nodes.size() || !nodes.equals(that.nodes))
			if (nodes.Count != that.nodes.size() || !nodes.SequenceEqual(that.nodes))
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

		public virtual bool hasNodes()
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