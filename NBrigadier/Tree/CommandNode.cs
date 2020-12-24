using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
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
		private IDictionary<string, IArgumentCommandNode<S>> arguments = new Dictionary<string, IArgumentCommandNode<S>>();
		private readonly System.Predicate<S> requirement;
		private readonly CommandNode<S> redirect;
		private readonly RedirectModifier<S> modifier;
		private readonly bool forks;
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

		public virtual bool CanUse(S source)
		{
			return requirement(source);
		}

		public virtual void AddChild(CommandNode<S> node)
		{
			if (node is RootCommandNode<S>)
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
					arguments[node.Name] = (IArgumentCommandNode<S>) node;
				}
			}

			children = children.SetOfKeyValuePairs().OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
		}

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

public abstract bool IsValidInput(string input);

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

		public abstract string Name {get;}

		public abstract string UsageText {get;}

		public abstract void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder);

		public abstract Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder);

		public abstract ArgumentBuilder<S, T> CreateBuilder<T>() where T : ArgumentBuilder<S, T>;

		protected internal abstract string SortedKey {get;}

		public virtual ICollection<CommandNode<S>> GetRelevantNodes(StringReader input)
		{
			if (literals.Count > 0)
			{
				int cursor = input.Cursor;
				while (input.CanRead() && input.Peek() != ' ')
				{
					input.Skip();
				}
				string text = input.String.Substring(cursor, input.Cursor - cursor);
				input.Cursor = cursor;
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