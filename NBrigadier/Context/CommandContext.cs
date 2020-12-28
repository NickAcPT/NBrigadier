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

		private static IDictionary<Type, Type> _primitiveToWrapper = new Dictionary<Type, Type>();

		static CommandContext()
		{
			_primitiveToWrapper[typeof(bool)] = typeof(Boolean);
			_primitiveToWrapper[typeof(sbyte)] = typeof(Byte);
			_primitiveToWrapper[typeof(short)] = typeof(short);
			_primitiveToWrapper[typeof(char)] = typeof(char);
			_primitiveToWrapper[typeof(int)] = typeof(int);
			_primitiveToWrapper[typeof(long)] = typeof(long);
			_primitiveToWrapper[typeof(float)] = typeof(float);
			_primitiveToWrapper[typeof(double)] = typeof(double);
		}

		private TS _source;
		private string _input;
		private Command<TS> _command;
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: private java.util.Map<String, ParsedArgument<S, ?>> arguments;
		private IDictionary<string, IParsedArgument> _arguments;
		private CommandNode<TS> _rootNode;
		private IList<ParsedCommandNode<TS>> _nodes;
		private StringRange _range;
		private CommandContext<TS> _child;
		private RedirectModifier<TS> _modifier;
		private bool _forks;

// TODO TASK: Wildcard generics in constructor parameters are not converted. Move the generic type parameter and constraint to the class header:
// ORIGINAL LINE: public CommandContext(S source, String input, java.util.Map<String, ParsedArgument<S, ?>> arguments, com.mojang.brigadier.Command<S> command, com.mojang.brigadier.tree.CommandNode<S> rootNode, java.util.List<ParsedCommandNode<S>> nodes, StringRange range, CommandContext<S> child, com.mojang.brigadier.RedirectModifier<S> modifier, boolean forks)
		public CommandContext(TS source, string input, IDictionary<string, IParsedArgument> arguments, Command<TS> command, CommandNode<TS> rootNode, IList<ParsedCommandNode<TS>> nodes, StringRange range, CommandContext<TS> child, RedirectModifier<TS> modifier, bool forks)
		{
			this._source = source;
			this._input = input;
			this._arguments = arguments;
			this._command = command;
			this._rootNode = rootNode;
			this._nodes = nodes;
			this._range = range;
			this._child = child;
			this._modifier = modifier;
			this._forks = forks;
		}

		public virtual CommandContext<TS> CopyFor(TS source)
		{
			if (this._source.Equals(source))
			{
				return this;
			}
			return new CommandContext<TS>(source, _input, _arguments, _command, _rootNode, _nodes, _range, _child, _modifier, _forks);
		}

		public virtual CommandContext<TS> Child
		{
			get
			{
				return _child;
			}
		}

		public virtual CommandContext<TS> LastChild
		{
			get
			{
				CommandContext<TS> result = this;
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

		public virtual TS Source
		{
			get
			{
				return _source;
			}
		}

// TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
// ORIGINAL LINE: @SuppressWarnings("unchecked") public <V> V getArgument(String name, Class<V> clazz)
		public virtual TV GetArgument<TV>(string name, Type clazz)
		{
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: ParsedArgument<S, ?> argument = arguments.get(name);
			 IParsedArgument argument = _arguments.GetValueOrNull(name);

			if (argument == null)
			{
				throw new System.ArgumentException("No such argument '" + name + "' exists on this command");
			}

			 object result = argument.ResultObject;
// ORIGINAL LINE: if (PRIMITIVE_TO_WRAPPER.getOrDefault(clazz, clazz).isAssignableFrom(result.getClass()))
			if (MapHelper.GetOrDefault(_primitiveToWrapper, clazz, clazz).IsAssignableFrom(result.GetType()))
			{
				return (TV) result;
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

			 CommandContext<TS> that = (CommandContext<TS>) o;

			if (!_arguments.Equals(that._arguments))
			{
				return false;
			}
			if (!_rootNode.Equals(that._rootNode))
			{
				return false;
			}
// WARNING: LINQ 'SequenceEqual' is not always identical to Java AbstractList 'equals':
// ORIGINAL LINE: if (nodes.size() != that.nodes.size() || !nodes.equals(that.nodes))
			if (_nodes.Count != that._nodes.Count || !_nodes.SequenceEqual(that._nodes))
			{
				return false;
			}
			if (_command != null ?!_command.Equals(that._command) : that._command != null)
			{
				return false;
			}
			if (!_source.Equals(that._source))
			{
				return false;
			}
			if (_child != null ?!_child.Equals(that._child) : that._child != null)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			int result = _source.GetHashCode();
			result = 31 * result + _arguments.GetHashCode();
			result = 31 * result + (_command != null ? _command.GetHashCode() : 0);
			result = 31 * result + _rootNode.GetHashCode();
			result = 31 * result + _nodes.GetHashCode();
			result = 31 * result + (_child != null ? _child.GetHashCode() : 0);
			return result;
		}

		public virtual RedirectModifier<TS> RedirectModifier
		{
			get
			{
				return _modifier;
			}
		}

		public virtual StringRange Range
		{
			get
			{
				return _range;
			}
		}

		public virtual string Input
		{
			get
			{
				return _input;
			}
		}

		public virtual CommandNode<TS> RootNode
		{
			get
			{
				return _rootNode;
			}
		}

		public virtual IList<ParsedCommandNode<TS>> Nodes
		{
			get
			{
				return _nodes;
			}
		}

		public virtual bool HasNodes()
		{
			return _nodes.Count > 0;
		}

		public virtual bool Forked
		{
			get
			{
				return _forks;
			}
		}
	}

}