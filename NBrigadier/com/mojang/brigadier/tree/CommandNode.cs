using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NBrigadier;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.tree
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.builder;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


	public abstract class CommandNode<S> : IComparable<CommandNode<S>>
	{
		private IDictionary<string, CommandNode<S>> children = new Dictionary<string, CommandNode<S>>();
		private IDictionary<string, LiteralCommandNode<S>> literals = new Dictionary<string, LiteralCommandNode<S>>();
//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: private java.util.Map<String, ArgumentCommandNode<S, ?>> arguments = new java.util.LinkedHashMap<>();
		private IDictionary<string, IArgumentCommandNode<S>> arguments = new Dictionary<string, IArgumentCommandNode<S>>();
		private readonly System.Predicate<S> requirement;
		private readonly CommandNode<S> redirect;
		private readonly RedirectModifier<S> modifier;
		private readonly bool forks;
		private Command<S> command;

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: protected CommandNode(final com.mojang.brigadier.Command<S> command, final java.util.function.Predicate<S> requirement, final CommandNode<S> redirect, final com.mojang.brigadier.RedirectModifier<S> modifier, final boolean forks)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandNode<S> getChild(final String name)
		public virtual CommandNode<S> GetChild(string name)
		{
			return children[name];
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public boolean canUse(final S source)
		public virtual bool CanUse(S source)
		{
			return requirement(source);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public void addChild(final CommandNode<S> node)
		public virtual void AddChild(CommandNode<S> node)
		{
			if (node is RootCommandNode<S>)
			{
				throw new System.NotSupportedException("Cannot add a RootCommandNode as a child to any other CommandNode");
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CommandNode<S> child = children.get(node.getName());
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
					child.AddChild(grandchild);
				}
			}
			else
			{
				children[node.Name] = node;
				if (node is LiteralCommandNode<S>)
				{
					literals[node.Name] = (LiteralCommandNode<S>) node;
				}
				else if (node is IArgumentCommandNode<S>)
				{
//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: arguments.put(node.getName(), (ArgumentCommandNode<S, ?>) node);
					arguments[node.Name] = (IArgumentCommandNode<S>) node;
				}
			}

//TODO TASK: Method reference constructor syntax is not converted by Java to C# Converter:
			children = children.SetOfKeyValuePairs().OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public void findAmbiguities(final com.mojang.brigadier.AmbiguityConsumer<S> consumer)
		public virtual void FindAmbiguities(AmbiguityConsumer<S> consumer)
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

					foreach (String input in child.Examples)
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: protected abstract boolean isValidInput(final String input);
public abstract bool IsValidInput(string input);

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public boolean equals(final Object o)
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

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final CommandNode<S> that = (CommandNode<S>) o;
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

		public abstract string Name {get;}

		public abstract string UsageText {get;}

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract void parse(com.mojang.brigadier.StringReader reader, com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		public abstract void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder);

//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public abstract java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(com.mojang.brigadier.context.CommandContext<S> context, com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException;
		public abstract Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder);

//WARNING: Java wildcard generics have no direct equivalent in C#:
//ORIGINAL LINE: public abstract com.mojang.brigadier.builder.ArgumentBuilder<S, object> createBuilder();
		public abstract ArgumentBuilder<S, T> CreateBuilder<T>() where T : ArgumentBuilder<S, T>;

		protected internal abstract string SortedKey {get;}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public java.util.Collection<? extends CommandNode<S>> getRelevantNodes(final com.mojang.brigadier.StringReader input)
//WARNING: Java wildcard generics have no direct equivalent in C#:
		public virtual ICollection<CommandNode<S>> GetRelevantNodes(StringReader input)
		{
			if (literals.Count > 0)
			{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int cursor = input.getCursor();
				int cursor = input.Cursor;
				while (input.CanRead() && input.Peek() != ' ')
				{
					input.Skip();
				}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String text = input.getString().substring(cursor, input.getCursor() - cursor);
				string text = input.String.Substring(cursor, input.Cursor - cursor);
				input.Cursor = cursor;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final LiteralCommandNode<S> literal = literals.get(text);
				LiteralCommandNode<S> literal = literals[text];
				if (literal != null)
				{
					return new List<CommandNode<S>>{(literal)};
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

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: @Override public int compareTo(final CommandNode<S> o)
		public int CompareTo(CommandNode<S> o)
		{
			if (this is LiteralCommandNode<S> == o is LiteralCommandNode<S>)
			{
				return String.Compare(SortedKey, o.SortedKey, StringComparison.Ordinal);
			}

			return (o is LiteralCommandNode<S>) ? 1 : -1;
		}

		public virtual bool Fork
		{
			get
			{
				return forks;
			}
		}

		public abstract ICollection<string> Examples {get;}
	}

}