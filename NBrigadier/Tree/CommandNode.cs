using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Generics;
using NBrigadier.Helpers;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    using StringReader = StringReader;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


	public abstract class CommandNode<S> : IComparable<CommandNode<S>>
	{
		private IDictionary<string, CommandNode<S>> children = new Dictionary<string, CommandNode<S>>();
		private IDictionary<string, LiteralCommandNode<S>> literals = new Dictionary<string, LiteralCommandNode<S>>();
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: private java.util.Map<String, ArgumentCommandNode<S, ?>> arguments = new java.util.LinkedHashMap<>();
		private IDictionary<string, IArgumentCommandNode<S>> arguments = new Dictionary<string, IArgumentCommandNode<S>>();
		private System.Predicate<S> requirement;
		private CommandNode<S> redirect;
		private RedirectModifier<S> modifier;
		private bool forks;
		private Command<S> command;

		protected internal CommandNode(Command<S> command, System.Predicate<S> requirement, CommandNode<S> redirect, RedirectModifier<S> modifier, bool forks)
		{
			this.command = command;
			this.requirement = requirement;
			this.redirect = redirect;
			this.modifier = modifier;
			this.forks = forks;
		}

		public virtual Command<S> Command
		{
			get
			{
				return command;
			}
		}

		public virtual ICollection<CommandNode<S>> Children
		{
			get
			{
				return children.Values;
			}
		}

		public virtual CommandNode<S> getChild(string name)
		{
			return children.GetValueOrNull(name);
		}

		public virtual CommandNode<S> Redirect
		{
			get
			{
				return redirect;
			}
		}

		public virtual RedirectModifier<S> RedirectModifier
		{
			get
			{
				return modifier;
			}
		}

		public virtual bool canUse(S source)
		{
			return requirement(source);
		}

		public virtual void addChild(CommandNode<S> node)
		{
			if (node is IRootCommandNode)
			{
				throw new System.NotSupportedException("Cannot add a RootCommandNode as a child to any other CommandNode");
			}

			 CommandNode<S> child = children.GetValueOrNull(node.Name);
			if (child != null)
			{
				// We've found something to merge onto
				if (node.Command != null)
				{
					child.command = node.Command;
				}
				foreach (CommandNode<S> grandchild in node.Children)
				{
					child.addChild(grandchild);
				}
			}
			else
			{
				children[node.Name] = node;
				if (node is ILiteralCommandNode)
				{
					literals[node.Name] = (LiteralCommandNode<S>) node;
				}
				else if (node is IArgumentCommandNode<S>)
				{
// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: arguments.put(node.getName(), (ArgumentCommandNode<S, ?>) node);
					arguments[node.Name] = (IArgumentCommandNode<S>) node;
				}
			}

// TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
			children = children.SetOfKeyValuePairs().OrderBy(d => d.Value).ToDictionary(e => e.Key, e => e.Value);
		}

		public virtual void findAmbiguities(AmbiguityConsumer<S> consumer)
		{
			ISet<string> matches = new HashSet<string>();

			foreach (CommandNode<S> child in children.Values)
			{
				foreach (CommandNode<S> sibling in children.Values)
				{
					if (child == sibling)
					{
						continue;
					}

					foreach (string input in child.Examples)
					{
						if (sibling.isValidInput(input))
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

				child.findAmbiguities(consumer);
			}
		}

		protected internal abstract bool isValidInput(string input);

		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			if (!(o is CommandNode<S>))
			{
				return false;
			}

			 CommandNode<S> that = (CommandNode<S>) o;

			if (!children.Equals(that.children))
			{
				return false;
			}
			if (command != null ?!command.Equals(that.command) : that.command != null)
			{
				return false;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return 31 * children.GetHashCode() + (command != null ? command.GetHashCode() : 0);
		}

		public virtual System.Predicate<S> Requirement
		{
			get
			{
				return requirement;
			}
		}

		public abstract string Name { get; }

		public abstract string UsageText { get; }

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public abstract void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		public abstract void parse(StringReader reader, CommandContextBuilder<S> contextBuilder);

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public abstract java.util.concurrent.CompletableFuture<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		public abstract System.Func<Suggestions> listSuggestions(CommandContext<S> context, SuggestionsBuilder builder);

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public abstract com.mojang.brigadier.builder.ArgumentBuilder<S, ?> createBuilder();
		public abstract IArgumentBuilder<S> createBuilder();

		protected internal abstract string SortedKey { get; }

// WARNING: Java wildcard generics have no direct equivalent in C#:
// ORIGINAL LINE: public java.util.Collection<? extends CommandNode<S>> getRelevantNodes(com.mojang.brigadier.StringReader input)
		public virtual ICollection<CommandNode<S>> getRelevantNodes(StringReader input)
		{
			if (literals.Count > 0)
			{
				 int cursor = input.Cursor;
				while (input.canRead() && input.peek() != ' ')
				{
					input.skip();
				}
				 string text = input.String.Substring(cursor, input.Cursor - cursor);
				input.Cursor = cursor;
				 LiteralCommandNode<S> literal = literals.GetValueOrNull(text);
				if (literal != null)
				{
					return CollectionsHelper.SingletonList(literal).Cast<CommandNode<S>>().ToList();
				}
				else
				{
					return arguments.Values.Cast<CommandNode<S>>().ToList();
				}
			}
			else
			{
				return arguments.Values.Cast<CommandNode<S>>().ToList();
			}
		}

		public virtual int CompareTo(CommandNode<S> o)
		{
			if (this is LiteralCommandNode<S> == o is ILiteralCommandNode)
			{
				return SortedKey.CompareTo(o.SortedKey);
			}

			return (o is ILiteralCommandNode) ? 1 : -1;
		}

		public virtual bool Fork
		{
			get
			{
				return forks;
			}
		}

		public abstract ICollection<string> Examples { get; }
	}

}