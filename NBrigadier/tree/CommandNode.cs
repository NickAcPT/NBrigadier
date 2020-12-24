using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public abstract class CommandNode<S> : IComparable<CommandNode<S>>
    {
        private readonly Predicate<S> requirement;

        private readonly IDictionary<string, IArgumentCommandNode<S>> arguments =
            new Dictionary<string, IArgumentCommandNode<S>>();

        private IDictionary<string, CommandNode<S>> children = new Dictionary<string, CommandNode<S>>();
        private Command<S> command;

        private readonly IDictionary<string, LiteralCommandNode<S>> literals =
            new Dictionary<string, LiteralCommandNode<S>>();

        protected internal CommandNode(Command<S> command, Predicate<S> requirement, CommandNode<S> redirect,
            RedirectModifier<S> modifier, bool forks)
        {
            this.command = command;
            this.requirement = requirement;
            Redirect = redirect;
            RedirectModifier = modifier;
            Fork = forks;
        }

        public virtual Command<S> Command => command;

        public virtual ICollection<CommandNode<S>> Children => children.Values;

        public virtual CommandNode<S> Redirect { get; }

        public virtual RedirectModifier<S> RedirectModifier { get; }

        public virtual Predicate<S> Requirement => requirement;

        public abstract string Name { get; }

        public abstract string UsageText { get; }

        protected internal abstract string SortedKey { get; }

        public virtual bool Fork { get; }

        public abstract ICollection<string> Examples { get; }

        public int CompareTo(CommandNode<S> o)
        {
            if (this is LiteralCommandNode<S> == o is LiteralCommandNode<S>)
                return string.Compare(SortedKey, o.SortedKey, StringComparison.Ordinal);

            return o is LiteralCommandNode<S> ? 1 : -1;
        }

        public virtual CommandNode<S> GetChild(string name)
        {
            return children[name];
        }

        public virtual bool CanUse(S source)
        {
            return requirement(source);
        }

        public virtual void AddChild(CommandNode<S> node)
        {
            if (node is RootCommandNode<S>)
                throw new NotSupportedException("Cannot add a RootCommandNode as a child to any other CommandNode");

            var child = children.GetValueOrNull(node.Name);
            if (child != null)
            {
                // We've found something to merge onto
                if (node.Command != null) child.command = node.Command;
                foreach (var grandchild in node.Children) child.AddChild(grandchild);
            }
            else
            {
                children[node.Name] = node;
                if (node is LiteralCommandNode<S>)
                    literals[node.Name] = (LiteralCommandNode<S>) node;
                else if (node is IArgumentCommandNode<S>) arguments[node.Name] = (IArgumentCommandNode<S>) node;
            }

            children = children.SetOfKeyValuePairs().OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
        }

        public virtual void FindAmbiguities(AmbiguityConsumer<S> consumer)
        {
            ISet<string> matches = new HashSet<string>();

            foreach (var child in children.Values)
            {
                foreach (var sibling in children.Values)
                {
                    if (child == sibling) continue;

                    foreach (var input in child.Examples)
                        if (sibling.IsValidInput(input))
                            matches.Add(input);

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
            if (this == o) return true;
            if (!(o is CommandNode<S>)) return false;

            var that = (CommandNode<S>) o;

            if (!children.Equals(that.children)) return false;
            if (command != null ? !command.Equals(that.command) : that.command != null) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return 31 * children.GetHashCode() + (command != null ? command.GetHashCode() : 0);
        }

        public abstract void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder);

        public abstract Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder);

        public abstract ArgumentBuilder<S, T> CreateBuilder<T>() where T : ArgumentBuilder<S, T>;

        public virtual ICollection<CommandNode<S>> GetRelevantNodes(StringReader input)
        {
            if (literals.Count > 0)
            {
                var cursor = input.Cursor;
                while (input.CanRead() && input.Peek() != ' ') input.Skip();
                var text = input.String.Substring(cursor, input.Cursor - cursor);
                input.Cursor = cursor;
                var literal = literals[text];
                if (literal != null)
                    return new List<CommandNode<S>> {literal};
                return arguments.Values.Cast<CommandNode<S>>().ToList();
            }

            return arguments.Values.Cast<CommandNode<S>>().ToList();
        }
    }
}