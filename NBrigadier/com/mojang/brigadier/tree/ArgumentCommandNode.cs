using System.Collections.Generic;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.tree
{
	using com.mojang.brigadier;
	using com.mojang.brigadier;
	using StringReader = com.mojang.brigadier.StringReader;
	using com.mojang.brigadier.arguments;
	using com.mojang.brigadier.builder;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using com.mojang.brigadier.suggestion;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;


    public class ArgumentCommandNode<S, T> : CommandNode<S>, IArgumentCommandNode<S>
    {
        private const string USAGE_ARGUMENT_OPEN = "<";
        private const string USAGE_ARGUMENT_CLOSE = ">";

        private readonly string name;
        private readonly ArgumentType<T> type;
        private readonly SuggestionProvider<S> customSuggestions;

        //WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
        //ORIGINAL LINE: public ArgumentCommandNode(final String name, final com.mojang.brigadier.arguments.ArgumentType<T> type, final com.mojang.brigadier.Command<S> command, final java.util.function.Predicate<S> requirement, final CommandNode<S> redirect, final com.mojang.brigadier.RedirectModifier<S> modifier, final boolean forks, final com.mojang.brigadier.suggestion.SuggestionProvider<S> customSuggestions)
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

        //WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: @Override public void parse(final com.mojang.brigadier.StringReader reader, final com.mojang.brigadier.context.CommandContextBuilder<S> contextBuilder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
        //WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
        public override void Parse(StringReader reader, CommandContextBuilder<S> contextBuilder)
        {
            //WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int start = reader.getCursor();
            int start = reader.Cursor;
            //WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final T result = type.parse(reader);
            T result = type.Parse(reader);
            //WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final com.mojang.brigadier.context.ParsedArgument<S, T> parsed = new com.mojang.brigadier.context.ParsedArgument<>(start, reader.getCursor(), result);
            ParsedArgument<S, object> parsed = new ParsedArgument<S, object>(start, reader.Cursor, result);

            contextBuilder.WithArgument(name, parsed);
            contextBuilder.WithNode(this, parsed.Range);
        }

        //WARNING: Method 'throws' clauses are not available in C#:
        //ORIGINAL LINE: @Override public java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> listSuggestions(final com.mojang.brigadier.context.CommandContext<S> context, final com.mojang.brigadier.suggestion.SuggestionsBuilder builder) throws com.mojang.brigadier.exceptions.CommandSyntaxException
        //WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
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
            //WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final com.mojang.brigadier.builder.RequiredArgumentBuilder<S, T> builder = com.mojang.brigadier.builder.RequiredArgumentBuilder.argument(name, type);
            RequiredArgumentBuilder<S, T> builder = RequiredArgumentBuilder<S, object>.Argument<S, T>(name, type as ArgumentType<T>);
            builder.Requires(Requirement);
            builder.Forward(Redirect, RedirectModifier, Fork);
            builder.Suggests(customSuggestions);
            if (Command != null)
            {
                builder.Executes(Command);
            }
            return builder;
        }

        //WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
        //ORIGINAL LINE: @Override public boolean isValidInput(final String input)
        public override bool IsValidInput(string input)
        {
            try
            {
                //WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final com.mojang.brigadier.StringReader reader = new com.mojang.brigadier.StringReader(input);
                StringReader reader = new StringReader(input);
                type.Parse(reader);
                return !reader.CanRead() || reader.Peek() == ' ';
            }
            //WARNING: 'final' catch parameters are not available in C#:
            //ORIGINAL LINE: catch (final com.mojang.brigadier.exceptions.CommandSyntaxException ignored)
            catch (com.mojang.brigadier.exceptions.CommandSyntaxException)
            {
                return false;
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
            if (!(o is ArgumentCommandNode<S, T>))
            {
                return false;
            }

            //WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final ArgumentCommandNode that = (ArgumentCommandNode) o;
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