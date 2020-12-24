using System.Collections.Generic;
using NBrigadier.Arguments;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Tree
{
    using StringReader = StringReader;
    using CommandSyntaxException = CommandSyntaxException;
    using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


    public class ArgumentCommandNode<S, T> : CommandNode<S>, IArgumentCommandNode<S>
    {
        private const string USAGE_ARGUMENT_OPEN = "<";
        private const string USAGE_ARGUMENT_CLOSE = ">";

        private readonly string name;
        private readonly ArgumentType<T> type;
        private readonly SuggestionProvider<S> customSuggestions;

                        public ArgumentCommandNode(string name, ArgumentType<T> type, Command<S> command, System.Predicate<S> requirement, CommandNode<S> redirect, RedirectModifier<S> modifier, bool forks, SuggestionProvider<S> customSuggestions) : base(command, requirement, redirect, modifier, forks)
        {
            this.name = name;
            this.type = type;
            this.customSuggestions = customSuggestions;
        }

        public virtual ArgumentType<T> Type {
            get {
                return type;
            }
        }

        public override string Name {
            get {
                return name;
            }
        }

        public override string UsageText {
            get {
                return USAGE_ARGUMENT_OPEN + name + USAGE_ARGUMENT_CLOSE;
            }
        }

        public virtual SuggestionProvider<S> CustomSuggestions {
            get {
                return customSuggestions;
            }
        }

                                public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
        {
                                    int start = reader.Cursor;
                                    T result = type.Parse(reader);
                                    ParsedArgument<S, object> parsed = new ParsedArgument<S, object>(start, reader.Cursor, result);

            contextBuilder.WithArgument(name, parsed);
            contextBuilder.WithNode(this, parsed.Range);
        }

                                public override System.Func<Suggestions> ListSuggestions(CommandContext<S> context, SuggestionsBuilder builder)
        {
            if (customSuggestions == null)
            {
                return type.ListSuggestions(context, builder);
            }
            else
            {
                return customSuggestions(context, builder);
            }
        }

        public override ArgumentBuilder<S, T> CreateBuilder<T>()
        {
            return CreateRequiredArgumentBuilder<T>() as ArgumentBuilder<S, T>;
        }

        public RequiredArgumentBuilder<S, T> CreateRequiredArgumentBuilder<T>() where T : ArgumentBuilder<S, T>
        {
            RequiredArgumentBuilder<S, T> builder = RequiredArgumentBuilder<S, T>.Argument(name, type as ArgumentType<T>);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            builder.Suggests(customSuggestions);
            if (Command != null)
            {
                builder.Executes(Command);
            }
            return builder;
        }

                        public override bool IsValidInput(string input)
        {
            try
            {
                                                StringReader reader = new StringReader(input);
                type.Parse(reader);
                return !reader.CanRead() || reader.Peek() == ' ';
            }
                                    catch (CommandSyntaxException)
            {
                return false;
            }
        }

                        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is ArgumentCommandNode<S, T>))
            {
                return false;
            }

                                    ArgumentCommandNode<S, T> that = (ArgumentCommandNode<S, T>)o;

            if (!name.Equals(that.name))
            {
                return false;
            }
            if (!type.Equals(that.type))
            {
                return false;
            }
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            int result = name.GetHashCode();
            result = 31 * result + type.GetHashCode();
            return result;
        }

        protected internal override string SortedKey {
            get {
                return name;
            }
        }

        public override ICollection<string> Examples {
            get {
                return type.GetExamples();
            }
        }

        public override string ToString()
        {
            return "<argument " + name + ":" + type + ">";
        }
    }

}