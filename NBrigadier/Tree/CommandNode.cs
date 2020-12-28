using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.CommandSuggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public abstract class CommandNode<TS> : IComparable<CommandNode<TS>>
	{
		private IDictionary<string, CommandNode<TS>> _children = new Dictionary<string, CommandNode<TS>>();
		private IDictionary<string, LiteralCommandNode<TS>> _literals = new Dictionary<string, LiteralCommandNode<TS>>();
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: private java.util.Map<String, ArgumentCommandNode<S, ?>> arguments = new java.util.LinkedHashMap<>();
		private IDictionary<string, IArgumentCommandNode<TS>> _arguments = new Dictionary<string, IArgumentCommandNode<TS>>();
		private System.Predicate<TS> _requirement;
		private CommandNode<TS> _redirect;
		private RedirectModifier<TS> _modifier;
		private bool _forks;
		private Command<TS> _command;

		protected internal CommandNode(Command<TS> command, System.Predicate<TS> requirement, CommandNode<TS> redirect, RedirectModifier<TS> modifier, bool forks)
		{
			this._command = command;
			this._requirement = requirement;
			this._redirect = redirect;
			this._modifier = modifier;
			this._forks = forks;
		}

		public virtual Command<TS> Command
		{
			get
			{
				return _command;
			}
		}

		public virtual ICollection<CommandNode<TS>> Children
		{
			get
			{
				return _children.Values;
			}
		}

		public virtual CommandNode<TS> GetChild(string name)
		{
			return _children.GetValueOrNull(name);
		}

		public virtual CommandNode<TS> Redirect
		{
			get
			{
				return _redirect;
			}
		}

		public virtual RedirectModifier<TS> RedirectModifier
		{
			get
			{
				return _modifier;
			}
		}

		public virtual bool CanUse(TS source)
		{
			return _requirement(source);
		}

		public virtual void AddChild(CommandNode<TS> node)
		{
			if (node is IRootCommandNode)
			{
				throw new System.NotSupportedException("Cannot add a RootCommandNode as a child to any other CommandNode");
			}

			 CommandNode<TS> child = _children.GetValueOrNull(node.Name);
			if (child != null)
			{
				// We've found something to merge onto
				if (node.Command != null)
				{
					child._command = node.Command;
				}
				foreach (CommandNode<TS> grandchild in node.Children)
				{
					child.AddChild(grandchild);
				}
			}
			else
			{
				_children[node.Name] = node;
				if (node is ILiteralCommandNode)
				{
					_literals[node.Name] = (LiteralCommandNode<TS>) node;
				}
				else if (node is IArgumentCommandNode<TS>)
				{
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: arguments.put(node.getName(), (ArgumentCommandNode<S, ?>) node);
					_arguments[node.Name] = (IArgumentCommandNode<TS>) node;
				}
			}

// TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
			_children = _children.SetOfKeyValuePairs().OrderBy(d => d.Value).ToDictionary(e => e.Key, e => e.Value);
		}

		public virtual void FindAmbiguities(AmbiguityConsumer<TS> consumer)
		{
			ISet<string> matches = new HashSet<string>();

			foreach (CommandNode<TS> child in _children.Values)
			{
				foreach (CommandNode<TS> sibling in _children.Values)
				{
					if (child == sibling)
					{
						continue;
					}

					foreach (string input in child.Examples)
					{
						if (sibling.IsValidInput(input))
						{
							matches.Add(input);
						}
					}

					if (matches.Count > 0)
					{
						consumer(this, child, sibling, matches);
						matches = new HashSet<string>();
					}
				}

				child.FindAmbiguities(consumer);
			}
		}

		protected internal abstract bool IsValidInput(string input);

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is CommandNode<TS>))
			{
				return false;
			}

			 CommandNode<TS> that = (CommandNode<TS>) o;

			if (!_children.Equals(that._children))
			{
				return false;
			}
			if (_command != null ?!_command.Equals(that._command) : that._command != null)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return 31 * _children.GetHashCode() + (_command != null ? _command.GetHashCode() : 0);
		}

		public virtual System.Predicate<TS> Requirement
		{
			get
			{
				return _requirement;
			}
		}

		public abstract string Name { get; }

		public abstract string UsageText { get; }

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public abstract void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		public abstract void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder);

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public abstract java.util.concurrent.CompletableFuture<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		public abstract System.Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder);

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public abstract com.mojang.brigadier.builder.ArgumentBuilder<S, ?> createBuilder();
		public abstract IArgumentBuilder<TS> CreateBuilder();

		protected internal abstract string SortedKey { get; }

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public java.util.Collection<? extends CommandNode<S>> getRelevantNodes(com.mojang.brigadier.StringReader input)
		public virtual ICollection<CommandNode<TS>> GetRelevantNodes(StringReader input)
		{
			if (_literals.Count > 0)
			{
				 int cursor = input.Cursor;
				while (input.CanRead() && input.Peek() != ' ')
				{
					input.Skip();
				}
				 string text = input.String.Substring(cursor, input.Cursor - cursor);
				input.Cursor = cursor;
				 LiteralCommandNode<TS> literal = _literals.GetValueOrNull(text);
				if (literal != null)
				{
					return CollectionsHelper.SingletonList(literal).Cast<CommandNode<TS>>().ToList();
				}
				else
				{
					return _arguments.Values.Cast<CommandNode<TS>>().ToList();
				}
			}
			else
			{
				return _arguments.Values.Cast<CommandNode<TS>>().ToList();
			}
		}

		public virtual int CompareTo(CommandNode<TS> o)
		{
			if (this is LiteralCommandNode<TS> == o is ILiteralCommandNode)
			{
				return SortedKey.CompareTo(o.SortedKey);
			}

			return (o is ILiteralCommandNode) ? 1 : -1;
		}

		public virtual bool Fork
		{
			get
			{
				return _forks;
			}
		}

		public abstract ICollection<string> Examples { get; }
	}

}