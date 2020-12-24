using System;
using System.Collections.Generic;
using System.Linq;
using NBrigadier.Context;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    public abstract class CommandNode<TS> : IComparable<CommandNode<TS>>
    {
        private readonly IDictionary<string, IArgumentCommandNode<TS>> _arguments =
            new Dictionary<string, IArgumentCommandNode<TS>>();

        private readonly IDictionary<string, LiteralCommandNode<TS>> _literals =
            new Dictionary<string, LiteralCommandNode<TS>>();

        private readonly Predicate<TS> _requirement;

        private IDictionary<string, CommandNode<TS>> _children = new Dictionary<string, CommandNode<TS>>();
        private Command<TS> _command;

        protected internal CommandNode(Command<TS> command, Predicate<TS> requirement, CommandNode<TS> redirect,
            RedirectModifier<TS> modifier, bool forks)
        {
            _command = command;
            _requirement = requirement;
            Redirect = redirect;
            RedirectModifier = modifier;
            Fork = forks;
        }

        public virtual Command<TS> Command => _command;

        public virtual ICollection<CommandNode<TS>> Children => _children.Values;

        public virtual CommandNode<TS> Redirect { get; }

        public virtual RedirectModifier<TS> RedirectModifier { get; }

        public virtual Predicate<TS> Requirement => _requirement;

        public abstract string Name { get; }

        public abstract string UsageText { get; }

        protected internal abstract string SortedKey { get; }

        public virtual bool Fork { get; }

        public abstract ICollection<string> Examples { get; }

        public int CompareTo(CommandNode<TS> o)
        {
            if (this is LiteralCommandNode<TS> == o is LiteralCommandNode<TS>)
                return string.Compare(SortedKey, o.SortedKey, StringComparison.Ordinal);

            return o is LiteralCommandNode<TS> ? 1 : -1;
        }

        public virtual CommandNode<TS> GetChild(string name)
        {
            return _children.GetValueOrNull(name);
        }

        public virtual bool CanUse(TS source)
        {
            return _requirement(source);
        }

        public virtual void AddChild(CommandNode<TS> node)
        {
            if (node is RootCommandNode<TS>)
                throw new NotSupportedException("Cannot add a RootCommandNode as a child to any other CommandNode");

            var child = _children.GetValueOrNull(node.Name);
            if (child != null)
            {
                // We've found something to merge onto
                if (node.Command != null) child._command = node.Command;
                foreach (var grandchild in node.Children) child.AddChild(grandchild);
            }
            else
            {
                _children[node.Name] = node;
                if (node is LiteralCommandNode<TS> commandNode)
                    _literals[node.Name] = commandNode;
                else if (node is IArgumentCommandNode<TS> argumentCommandNode)
                    _arguments[node.Name] = argumentCommandNode;
            }

            _children = _children.SetOfKeyValuePairs().OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
        }

        public virtual void FindAmbiguities(AmbiguityConsumer<TS> consumer)
        {
            ISet<string> matches = new HashSet<string>();

            foreach (var child in _children.Values)
            {
                foreach (var sibling in _children.Values)
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
            if (!(o is CommandNode<TS>)) return false;

            var that = (CommandNode<TS>) o;

            if (!_children.Equals(that._children)) return false;
            if (!_command?.Equals(that._command) ?? that._command != null) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return 31 * _children.GetHashCode() + (_command != null ? _command.GetHashCode() : 0);
        }

        public abstract void Parse(StringReader reader, CommandContextBuilder<TS> contextBuilder);

        public abstract Func<Suggestions> ListSuggestions(CommandContext<TS> context, SuggestionsBuilder builder);

        public virtual ICollection<CommandNode<TS>> GetRelevantNodes(StringReader input)
        {
            if (_literals.Count > 0)
            {
                var cursor = input.Cursor;
                while (input.CanRead() && input.Peek() != ' ') input.Skip();
                var text = input.String.Substring(cursor, input.Cursor - cursor);
                input.Cursor = cursor;
                var literal = _literals.GetValueOrNull(text);
                if (literal != null)
                    return new List<CommandNode<TS>> {literal};
                return _arguments.Values.Cast<CommandNode<TS>>().ToList();
            }

            return _arguments.Values.Cast<CommandNode<TS>>().ToList();
        }
    }
}