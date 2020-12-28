using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBrigadier.Builder;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    // / <summary>
    // / The core command dispatcher, for registering, parsing, and executing commands.
    // / </summary>
    // / @param <S> a custom "source" type, such as a user or originator of a command </param>
    public class CommandDispatcher<TS>
    {
        // / <summary>
        // / The string required to separate individual arguments in an input string
        // / </summary>
        // / <seealso cref= #ARGUMENT_SEPARATOR_CHAR </seealso>
        public static string argumentSeparator = " ";

        // / <summary>
        // / The char required to separate individual arguments in an input string
        // / </summary>
        // / <seealso cref= #ARGUMENT_SEPARATOR </seealso>
        public static char argumentSeparatorChar = ' ';

        private static readonly string _usageOptionalOpen = "[";
        private static readonly string _usageOptionalClose = "]";
        private static readonly string _usageRequiredOpen = "(";
        private static readonly string _usageRequiredClose = ")";
        private static readonly string _usageOr = "|";

        private static Predicate<CommandNode<TS>> _hasCommand = input =>
        {
            return input != null && (input.Command != null || input.Children.Any(_hasCommand));
        };

        private readonly RootCommandNode<TS> _root;

        private ResultConsumer<TS> _consumer = (c, s, r) => { };

        // / <summary>
        // / Create a new <seealso cref="CommandDispatcher"/> with the specified root node.
        // / 
        // / <para>This is often useful to copy existing or pre-defined command trees.</para>
        // / </summary>
        // / <param name="root"> the existing <seealso cref="RootCommandNode"/> to use as the basis for this tree </param>
        public CommandDispatcher(RootCommandNode<TS> root)
        {
            _root = root;
        }

        // / <summary>
        // / Creates a new <seealso cref="CommandDispatcher"/> with an empty command tree.
        // / </summary>
        public CommandDispatcher() : this(new RootCommandNode<TS>())
        {
        }

        // / <summary>
        // / Sets a callback to be informed of the result of every command.
        // / </summary>
        // / <param name="consumer"> the new result consumer to be called </param>
        public virtual ResultConsumer<TS> Consumer
        {
            set => _consumer = value;
        }

        // / <summary>
        // / Gets the root of this command tree.
        // / 
        // / <para>This is often useful as a target of a <seealso cref="com.mojang.brigadier.builder.ArgumentBuilder.redirect(CommandNode)"/>,
        // / <seealso cref="getAllUsage(CommandNode, object, bool)"/> or <seealso cref="getSmartUsage(CommandNode, object)"/>.
        // / You may also use it to clone the command tree via <seealso cref="CommandDispatcher(RootCommandNode)"/>.</para>
        // / </summary>
        // / <returns> root of the command tree </returns>
        public virtual RootCommandNode<TS> Root => _root;

        // / <summary>
        // / Utility method for registering new commands.
        // / 
        // / <para>This is a shortcut for calling <seealso cref="RootCommandNode.addChild(CommandNode)"/> after building the provided {@code command}.</para>
        // / 
        // / <para>As <seealso cref="RootCommandNode"/> can only hold literals, this method will only allow literal arguments.</para>
        // / </summary>
        // / <param name="command"> a literal argument builder to add to this command tree </param>
        // / <returns> the node added to this tree </returns>
        public virtual LiteralCommandNode<TS> Register(LiteralArgumentBuilder<TS> command)
        {
            var build = command.Build();
            _root.AddChild(build);
            // ReSharper disable RedundantCast
            return (LiteralCommandNode<TS>) build;
            // ReSharper restore RedundantCast
        }

        // / <summary>
        // / Parses and executes a given command.
        // / 
        // / <para>This is a shortcut to first <seealso cref="parse(StringReader, object)"/> and then <seealso cref="execute(ParseResults)"/>.</para>
        // / 
        // / <para>It is recommended to parse and execute as separate steps, as parsing is often the most expensive step, and easiest to cache.</para>
        // / 
        // / <para>If this command returns a value, then it successfully executed something. If it could not parse the command, or the execution was a failure,
        // / then an exception will be thrown. Most exceptions will be of type <seealso cref="CommandSyntaxException"/>, but it is possible that a <seealso cref="System.Exception"/>
        // / may bubble up from the result of a command. The meaning behind the returned result is arbitrary, and will depend
        // / entirely on what command was performed.</para>
        // / 
        // / <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then it will be 'forked'.
        // / A forked command will not bubble up any <seealso cref="CommandSyntaxException"/>s, and the 'result' returned will turn into
        // / 'amount of successful commands executes'.</para>
        // / 
        // / <para>After each and any command is ran, a registered callback given to <seealso cref="setConsumer(ResultConsumer)"/>
        // / will be notified of the result and success of the command. You can use that method to gather more meaningful
        // / results than this method will return, especially when a command forks.</para>
        // / </summary>
        // / <param name="input"> a command string to parse &amp; execute </param>
        // / <param name="source"> a custom "source" object, usually representing the originator of this command </param>
        // / <returns> a numeric result from a "command" that was performed </returns>
        // / <exception cref="CommandSyntaxException"> if the command failed to parse or execute </exception>
        // / <exception cref="RuntimeException"> if the command failed to execute and was not handled gracefully </exception>
        // / <seealso cref= #parse(String, Object) </seealso>
        // / <seealso cref= #parse(StringReader, Object) </seealso>
        // / <seealso cref= #execute(ParseResults) </seealso>
        // / <seealso cref= #execute(StringReader, Object) </seealso>
        public virtual int Execute(string input, TS source)
        {
            return Execute(new StringReader(input), source);
        }

        // / <summary>
        // / Parses and executes a given command.
        // / 
        // / <para>This is a shortcut to first <seealso cref="parse(StringReader, object)"/> and then <seealso cref="execute(ParseResults)"/>.</para>
        // / 
        // / <para>It is recommended to parse and execute as separate steps, as parsing is often the most expensive step, and easiest to cache.</para>
        // / 
        // / <para>If this command returns a value, then it successfully executed something. If it could not parse the command, or the execution was a failure,
        // / then an exception will be thrown. Most exceptions will be of type <seealso cref="CommandSyntaxException"/>, but it is possible that a <seealso cref="System.Exception"/>
        // / may bubble up from the result of a command. The meaning behind the returned result is arbitrary, and will depend
        // / entirely on what command was performed.</para>
        // / 
        // / <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then it will be 'forked'.
        // / A forked command will not bubble up any <seealso cref="CommandSyntaxException"/>s, and the 'result' returned will turn into
        // / 'amount of successful commands executes'.</para>
        // / 
        // / <para>After each and any command is ran, a registered callback given to <seealso cref="setConsumer(ResultConsumer)"/>
        // / will be notified of the result and success of the command. You can use that method to gather more meaningful
        // / results than this method will return, especially when a command forks.</para>
        // / </summary>
        // / <param name="input"> a command string to parse &amp; execute </param>
        // / <param name="source"> a custom "source" object, usually representing the originator of this command </param>
        // / <returns> a numeric result from a "command" that was performed </returns>
        // / <exception cref="CommandSyntaxException"> if the command failed to parse or execute </exception>
        // / <exception cref="RuntimeException"> if the command failed to execute and was not handled gracefully </exception>
        // / <seealso cref= #parse(String, Object) </seealso>
        // / <seealso cref= #parse(StringReader, Object) </seealso>
        // / <seealso cref= #execute(ParseResults) </seealso>
        // / <seealso cref= #execute(String, Object) </seealso>
        public virtual int Execute(StringReader input, TS source)
        {
            var parse = Parse(input, source);
            return Execute(parse);
        }

        // / <summary>
        // / Executes a given pre-parsed command.
        // / 
        // / <para>If this command returns a value, then it successfully executed something. If the execution was a failure,
        // / then an exception will be thrown.
        // / Most exceptions will be of type <seealso cref="CommandSyntaxException"/>, but it is possible that a <seealso cref="System.Exception"/>
        // / may bubble up from the result of a command. The meaning behind the returned result is arbitrary, and will depend
        // / entirely on what command was performed.</para>
        // / 
        // / <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then it will be 'forked'.
        // / A forked command will not bubble up any <seealso cref="CommandSyntaxException"/>s, and the 'result' returned will turn into
        // / 'amount of successful commands executes'.</para>
        // / 
        // / <para>After each and any command is ran, a registered callback given to <seealso cref="setConsumer(ResultConsumer)"/>
        // / will be notified of the result and success of the command. You can use that method to gather more meaningful
        // / results than this method will return, especially when a command forks.</para>
        // / </summary>
        // / <param name="parse"> the result of a successful <seealso cref="parse(StringReader, object)"/> </param>
        // / <returns> a numeric result from a "command" that was performed. </returns>
        // / <exception cref="CommandSyntaxException"> if the command failed to parse or execute </exception>
        // / <exception cref="RuntimeException"> if the command failed to execute and was not handled gracefully </exception>
        // / <seealso cref= #parse(String, Object) </seealso>
        // / <seealso cref= #parse(StringReader, Object) </seealso>
        // / <seealso cref= #execute(String, Object) </seealso>
        // / <seealso cref= #execute(StringReader, Object) </seealso>
        public virtual int Execute(ParseResults<TS> parse)
        {
            if (parse.Reader.CanRead())
            {
                if (parse.Exceptions.Count == 1)
                    throw parse.Exceptions.Values.First();
                if (parse.Context.Range.Empty)
                    throw CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()
                        .CreateWithContext(parse.Reader);
                throw CommandSyntaxException.builtInExceptions.DispatcherUnknownArgument()
                    .CreateWithContext(parse.Reader);
            }

            var result = 0;
            var successfulForks = 0;
            var forked = false;
            var foundCommand = false;
            var command = parse.Reader.String;
            var original = parse.Context.Build(command);
            IList<CommandContext<TS>> contexts = CollectionsHelper.SingletonList(original);
            List<CommandContext<TS>> next = null;

            while (contexts != null)
            {
                var size = contexts.Count;
                for (var i = 0; i < size; i++)
                {
                    var context = contexts[i];
                    var child = context.Child;
                    if (child != null)
                    {
                        forked |= context.Forked;
                        if (child.HasNodes())
                        {
                            foundCommand = true;
                            var modifier = context.RedirectModifier;
                            if (modifier == null)
                            {
                                if (next == null) next = new List<CommandContext<TS>>(1);
                                next.Add(child.CopyFor(context.Source));
                            }
                            else
                            {
                                try
                                {
                                    var results = modifier(context);
                                    if (results.Count > 0)
                                    {
                                        if (next == null) next = new List<CommandContext<TS>>(results.Count);
                                        foreach (var source in results) next.Add(child.CopyFor(source));
                                    }
                                }
                                catch (CommandSyntaxException ex)
                                {
                                    _consumer(context, false, 0);
                                    if (!forked) throw;
                                }
                            }
                        }
                    }
                    else if (context.Command != null)
                    {
                        foundCommand = true;
                        try
                        {
                            var value = context.Command(context);
                            result += value;
                            _consumer(context, true, value);
                            successfulForks++;
                        }
                        catch (CommandSyntaxException ex)
                        {
                            _consumer(context, false, 0);
                            if (!forked) throw;
                        }
                    }
                }

                contexts = next;
                next = null;
            }

            if (!foundCommand)
            {
                _consumer(original, false, 0);
                throw CommandSyntaxException.builtInExceptions.DispatcherUnknownCommand()
                    .CreateWithContext(parse.Reader);
            }

            return forked ? successfulForks : result;
        }

        // / <summary>
        // / Parses a given command.
        // / 
        // / <para>The result of this method can be cached, and it is advised to do so where appropriate. Parsing is often the
        // / most expensive step, and this allows you to essentially "precompile" a command if it will be ran often.</para>
        // / 
        // / <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then the resulting context will be marked as 'forked'.
        // / Forked contexts may contain child contexts, which may be modified by the <seealso cref="RedirectModifier"/> attached to the fork.</para>
        // / 
        // / <para>Parsing a command can never fail, you will always be provided with a new <seealso cref="ParseResults"/>.
        // / However, that does not mean that it will always parse into a valid command. You should inspect the returned results
        // / to check for validity. If its <seealso cref="ParseResults.getReader()"/> <seealso cref="StringReader.canRead()"/> then it did not finish
        // / parsing successfully. You can use that position as an indicator to the user where the command stopped being valid.
        // / You may inspect <seealso cref="ParseResults.getExceptions()"/> if you know the parse failed, as it will explain why it could
        // / not find any valid commands. It may contain multiple exceptions, one for each "potential node" that it could have visited,
        // / explaining why it did not go down that node.</para>
        // / 
        // / <para>When you eventually call <seealso cref="execute(ParseResults)"/> with the result of this method, the above error checking
        // / will occur. You only need to inspect it yourself if you wish to handle that yourself.</para>
        // / </summary>
        // / <param name="command"> a command string to parse </param>
        // / <param name="source"> a custom "source" object, usually representing the originator of this command </param>
        // / <returns> the result of parsing this command </returns>
        // / <seealso cref= #parse(StringReader, Object) </seealso>
        // / <seealso cref= #execute(ParseResults) </seealso>
        // / <seealso cref= #execute(String, Object) </seealso>
        public virtual ParseResults<TS> Parse(string command, TS source)
        {
            return Parse(new StringReader(command), source);
        }

        // / <summary>
        // / Parses a given command.
        // / 
        // / <para>The result of this method can be cached, and it is advised to do so where appropriate. Parsing is often the
        // / most expensive step, and this allows you to essentially "precompile" a command if it will be ran often.</para>
        // / 
        // / <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then the resulting context will be marked as 'forked'.
        // / Forked contexts may contain child contexts, which may be modified by the <seealso cref="RedirectModifier"/> attached to the fork.</para>
        // / 
        // / <para>Parsing a command can never fail, you will always be provided with a new <seealso cref="ParseResults"/>.
        // / However, that does not mean that it will always parse into a valid command. You should inspect the returned results
        // / to check for validity. If its <seealso cref="ParseResults.getReader()"/> <seealso cref="StringReader.canRead()"/> then it did not finish
        // / parsing successfully. You can use that position as an indicator to the user where the command stopped being valid.
        // / You may inspect <seealso cref="ParseResults.getExceptions()"/> if you know the parse failed, as it will explain why it could
        // / not find any valid commands. It may contain multiple exceptions, one for each "potential node" that it could have visited,
        // / explaining why it did not go down that node.</para>
        // / 
        // / <para>When you eventually call <seealso cref="execute(ParseResults)"/> with the result of this method, the above error checking
        // / will occur. You only need to inspect it yourself if you wish to handle that yourself.</para>
        // / </summary>
        // / <param name="command"> a command string to parse </param>
        // / <param name="source"> a custom "source" object, usually representing the originator of this command </param>
        // / <returns> the result of parsing this command </returns>
        // / <seealso cref= #parse(String, Object) </seealso>
        // / <seealso cref= #execute(ParseResults) </seealso>
        // / <seealso cref= #execute(String, Object) </seealso>
        public virtual ParseResults<TS> Parse(StringReader command, TS source)
        {
            var context = new CommandContextBuilder<TS>(this, source, _root, command.Cursor);
            return ParseNodes(_root, command, context);
        }

        private ParseResults<TS> ParseNodes(CommandNode<TS> node, StringReader originalReader,
            CommandContextBuilder<TS> contextSoFar)
        {
            var source = contextSoFar.Source;
            IDictionary<CommandNode<TS>, CommandSyntaxException> errors = null;
            IList<ParseResults<TS>> potentials = null;
            var cursor = originalReader.Cursor;

            foreach (var child in node.GetRelevantNodes(originalReader))
            {
                if (!child.CanUse(source)) continue;
                var context = contextSoFar.Copy();
                var reader = new StringReader(originalReader);
                try
                {
                    try
                    {
                        child.Parse(reader, context);
                    }
                    catch (Exception ex)
                    {
                        throw CommandSyntaxException.builtInExceptions.DispatcherParseException()
                            .CreateWithContext(reader, ex.Message);
                    }

                    if (reader.CanRead())
                        if (reader.Peek() != argumentSeparatorChar)
                            throw CommandSyntaxException.builtInExceptions.DispatcherExpectedArgumentSeparator()
                                .CreateWithContext(reader);
                }
                catch (CommandSyntaxException ex)
                {
                    if (errors == null) errors = new Dictionary<CommandNode<TS>, CommandSyntaxException>();
                    errors[child] = ex;
                    reader.Cursor = cursor;
                    continue;
                }

                context.WithCommand(child.Command);
                if (reader.CanRead(child.Redirect == null ? 2 : 1))
                {
                    reader.Skip();
                    if (child.Redirect != null)
                    {
                        var childContext = new CommandContextBuilder<TS>(this, source, child.Redirect, reader.Cursor);
                        var parse = ParseNodes(child.Redirect, reader, childContext);
                        context.WithChild(parse.Context);
                        return new ParseResults<TS>(context, parse.Reader, parse.Exceptions);
                    }
                    else
                    {
                        var parse = ParseNodes(child, reader, context);
                        if (potentials == null) potentials = new List<ParseResults<TS>>(1);
                        potentials.Add(parse);
                    }
                }
                else
                {
                    if (potentials == null) potentials = new List<ParseResults<TS>>(1);
                    potentials.Add(new ParseResults<TS>(context, reader,
                        CollectionsHelper.EmptyMap<CommandNode<TS>, CommandSyntaxException>()));
                }
            }

            if (potentials != null)
            {
                if (potentials.Count > 1)
                    potentials = potentials.Sort((a, b) =>
                    {
                        if (!a.Reader.CanRead() && b.Reader.CanRead()) return -1;
                        if (a.Reader.CanRead() && !b.Reader.CanRead()) return 1;
                        if (a.Exceptions.IsEmpty() && !b.Exceptions.IsEmpty()) return -1;
                        if (!a.Exceptions.IsEmpty() && b.Exceptions.IsEmpty()) return 1;
                        return 0;
                    }).ToList();
                return potentials[0];
            }

            return new ParseResults<TS>(contextSoFar, originalReader,
                errors == null ? CollectionsHelper.EmptyMap<CommandNode<TS>, CommandSyntaxException>() : errors);
        }

        // / <summary>
        // / Gets all possible executable commands following the given node.
        // / 
        // / <para>You may use <seealso cref="getRoot()"/> as a target to get all usage data for the entire command tree.</para>
        // / 
        // / <para>The returned syntax will be in "simple" form: {@code <param>} and {@code literal}. "Optional" nodes will be
        // / listed as multiple entries: the parent node, and the child nodes.
        // / For example, a required literal "foo" followed by an optional param "int" will be two nodes:</para>
        // / <ul>
        // /     <li>{@code foo}</li>
        // /     <li>{@code foo <int>}</li>
        // / </ul>
        // / 
        // / <para>The path to the specified node will <b>not</b> be prepended to the output, as there can theoretically be many
        // / ways to reach a given node. It will only give you paths relative to the specified node, not absolute from root.</para>
        // / </summary>
        // / <param name="node"> target node to get child usage strings for </param>
        // / <param name="source"> a custom "source" object, usually representing the originator of this command </param>
        // / <param name="restricted"> if true, commands that the {@code source} cannot access will not be mentioned </param>
        // / <returns> array of full usage strings under the target node </returns>
        public virtual string[] GetAllUsage(CommandNode<TS> node, TS source, bool restricted)
        {
            var result = new List<string>();
            GetAllUsage(node, source, result, "", restricted);
            return result.ToArray();
        }

        private void GetAllUsage(CommandNode<TS> node, TS source, List<string> result, string prefix, bool restricted)
        {
            if (restricted && !node.CanUse(source)) return;

            if (node.Command != null) result.Add(prefix);

            if (node.Redirect != null)
            {
                var redirect = node.Redirect == _root ? "..." : "-> " + node.Redirect.UsageText;
                result.Add(prefix.Length == 0
                    ? node.UsageText + argumentSeparator + redirect
                    : prefix + argumentSeparator + redirect);
            }
            else if (node.Children.Count > 0)
            {
                foreach (var child in node.Children)
                    GetAllUsage(child, source, result,
                        prefix.Length == 0 ? child.UsageText : prefix + argumentSeparator + child.UsageText,
                        restricted);
            }
        }

        // / <summary>
        // / Gets the possible executable commands from a specified node.
        // / 
        // / <para>You may use <seealso cref="getRoot()"/> as a target to get usage data for the entire command tree.</para>
        // / 
        // / <para>The returned syntax will be in "smart" form: {@code <param>}, {@code literal}, {@code [optional]} and {@code (either|or)}.
        // / These forms may be mixed and matched to provide as much information about the child nodes as it can, without being too verbose.
        // / For example, a required literal "foo" followed by an optional param "int" can be compressed into one string:</para>
        // / <ul>
        // /     <li>{@code foo [<int>]}</li>
        // / </ul>
        // / 
        // / <para>The path to the specified node will <b>not</b> be prepended to the output, as there can theoretically be many
        // / ways to reach a given node. It will only give you paths relative to the specified node, not absolute from root.</para>
        // / 
        // / <para>The returned usage will be restricted to only commands that the provided {@code source} can use.</para>
        // / </summary>
        // / <param name="node"> target node to get child usage strings for </param>
        // / <param name="source"> a custom "source" object, usually representing the originator of this command </param>
        // / <returns> array of full usage strings under the target node </returns>
        public virtual IDictionary<CommandNode<TS>, string> GetSmartUsage(CommandNode<TS> node, TS source)
        {
            IDictionary<CommandNode<TS>, string> result = new Dictionary<CommandNode<TS>, string>();

            var optional = node.Command != null;
            foreach (var child in node.Children)
            {
                var usage = GetSmartUsage(child, source, optional, false);
                if (!ReferenceEquals(usage, null)) result[child] = usage;
            }

            return result;
        }

        private string GetSmartUsage(CommandNode<TS> node, TS source, bool optional, bool deep)
        {
            if (!node.CanUse(source)) return null;

            var self = optional ? _usageOptionalOpen + node.UsageText + _usageOptionalClose : node.UsageText;
            var childOptional = node.Command != null;
            var open = childOptional ? _usageOptionalOpen : _usageRequiredOpen;
            var close = childOptional ? _usageOptionalClose : _usageRequiredClose;

            if (!deep)
            {
                if (node.Redirect != null)
                {
                    var redirect = node.Redirect == _root ? "..." : "-> " + node.Redirect.UsageText;
                    return self + argumentSeparator + redirect;
                }

                ICollection<CommandNode<TS>> children = node.Children.Where(c => c.CanUse(source)).ToList();
                if (children.Count == 1)
                {
                    var usage = GetSmartUsage(children.First(), source, childOptional, childOptional);
                    if (!ReferenceEquals(usage, null)) return self + argumentSeparator + usage;
                }
                else if (children.Count > 1)
                {
                    ISet<string> childUsage = new HashSet<string>();
                    foreach (var child in children)
                    {
                        var usage = GetSmartUsage(child, source, childOptional, true);
                        if (!ReferenceEquals(usage, null)) childUsage.Add(usage);
                    }

                    if (childUsage.Count == 1)
                    {
                        var usage = childUsage.First();
                        return self + argumentSeparator +
                               (childOptional ? _usageOptionalOpen + usage + _usageOptionalClose : usage);
                    }

                    if (childUsage.Count > 1)
                    {
                        var builder = new StringBuilder(open);
                        var count = 0;
                        foreach (var child in children)
                        {
                            if (count > 0) builder.Append(_usageOr);
                            builder.Append(child.UsageText);
                            count++;
                        }

                        if (count > 0)
                        {
                            builder.Append(close);
                            return self + argumentSeparator + builder;
                        }
                    }
                }
            }

            return self;
        }

        // / <summary>
        // / Gets suggestions for a parsed input string on what comes next.
        // / 
        // / <para>As it is ultimately up to custom argument types to provide suggestions, it may be an asynchronous operation,
        // / for example getting in-game data or player names etc. As such, this method returns a future and no guarantees
        // / are made to when or how the future completes.</para>
        // / 
        // / <para>The suggestions provided will be in the context of the end of the parsed input string, but may suggest
        // / new or replacement strings for earlier in the input string. For example, if the end of the string was
        // / {@code foobar} but an argument preferred it to be {@code minecraft:foobar}, it will suggest a replacement for that
        // / whole segment of the input.</para>
        // / </summary>
        // / <param name="parse"> the result of a <seealso cref="parse(StringReader, object)"/> </param>
        // / <returns> a future that will eventually resolve into a <seealso cref="Suggestions"/> object </returns>
        public virtual Func<Suggestions> GetCompletionSuggestions(ParseResults<TS> parse)
        {
            return GetCompletionSuggestions(parse, parse.Reader.TotalLength);
        }

        public virtual Func<Suggestions> GetCompletionSuggestions(ParseResults<TS> parse, int cursor)
        {
            var context = parse.Context;

            var nodeBeforeCursor = context.FindSuggestionContext(cursor);
            var parent = nodeBeforeCursor.parent;
            var start = Math.Min(nodeBeforeCursor.startPos, cursor);

            var fullInput = parse.Reader.String;
            var truncatedInput = fullInput.Substring(0, cursor);
            var futures = new Func<Suggestions>[parent.Children.Count];
            var i = 0;
            foreach (var node in parent.Children)
            {
                var future = Suggestions.Empty();
                try
                {
                    future = node.ListSuggestions(context.Build(truncatedInput),
                        new SuggestionsBuilder(truncatedInput, start));
                }
                catch (CommandSyntaxException)
                {
                }

                futures[i++] = future;
            }


            IList<Suggestions> suggestions = new List<Suggestions>();


            foreach (var action in futures.Select(c => new Action(() => { suggestions.Add(c()); }))) action();

            return () => Suggestions.Merge(fullInput, suggestions);
        }

        // / <summary>
        // / Finds a valid path to a given node on the command tree.
        // / 
        // / <para>There may theoretically be multiple paths to a node on the tree, especially with the use of forking or redirecting.
        // / As such, this method makes no guarantees about which path it finds. It will not look at forks or redirects,
        // / and find the first instance of the target node on the tree.</para>
        // / 
        // / <para>The only guarantee made is that for the same command tree and the same version of this library, the result of
        // / this method will <b>always</b> be a valid input for <seealso cref="findNode(System.Collections.ICollection)"/>, which should return the same node
        // / as provided to this method.</para>
        // / </summary>
        // / <param name="target"> the target node you are finding a path for </param>
        // / <returns> a path to the resulting node, or an empty list if it was not found </returns>
        public virtual ICollection<string> GetPath(CommandNode<TS> target)
        {
            IList<IList<CommandNode<TS>>> nodes = new List<IList<CommandNode<TS>>>();
            AddPaths(_root, nodes, new List<CommandNode<TS>>());

            foreach (var list in nodes)
                if (list[list.Count - 1] == target)
                {
                    IList<string> result = new List<string>(list.Count);
                    foreach (var node in list)
                        if (node != _root)
                            result.Add(node.Name);
                    return result;
                }

            return CollectionsHelper.EmptyList<string>();
        }

        // / <summary>
        // / Finds a node by its path
        // / 
        // / <para>Paths may be generated with <seealso cref="getPath(CommandNode)"/>, and are guaranteed (for the same tree, and the
        // / same version of this library) to always produce the same valid node by this method.</para>
        // / 
        // / <para>If a node could not be found at the specified path, then {@code null} will be returned.</para>
        // / </summary>
        // / <param name="path"> a generated path to a node </param>
        // / <returns> the node at the given path, or null if not found </returns>
        public virtual CommandNode<TS> FindNode(ICollection<string> path)
        {
            CommandNode<TS> node = _root;
            foreach (var name in path)
            {
                node = node.GetChild(name);
                if (node == null) return null;
            }

            return node;
        }

        // / <summary>
        // / Scans the command tree for potential ambiguous commands.
        // / 
        // / <para>This is a shortcut for <seealso cref="CommandNode.findAmbiguities(AmbiguityConsumer)"/> on <seealso cref="getRoot()"/>.</para>
        // / 
        // / <para>Ambiguities are detected by testing every <seealso cref="CommandNode.getExamples()"/> on one node verses every sibling
        // / node. This is not fool proof, and relies a lot on the providers of the used argument types to give good examples.</para>
        // / </summary>
        // / <param name="consumer"> a callback to be notified of potential ambiguities </param>
        public virtual void FindAmbiguities(AmbiguityConsumer<TS> consumer)
        {
            _root.FindAmbiguities(consumer);
        }

        private void AddPaths(CommandNode<TS> node, IList<IList<CommandNode<TS>>> result,
            IList<CommandNode<TS>> parents)
        {
            IList<CommandNode<TS>> current = new List<CommandNode<TS>>(parents);
            current.Add(node);
            result.Add(current);

            foreach (var child in node.Children) AddPaths(child, result, current);
        }
    }
}