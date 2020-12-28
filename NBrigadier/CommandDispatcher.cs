using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBrigadier.Builder;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Suggestion;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    using CommandSyntaxException = CommandSyntaxException;
	using Suggestions = Suggestions;
	using SuggestionsBuilder = SuggestionsBuilder;


    // / <summary>
	// / The core command dispatcher, for registering, parsing, and executing commands.
	// / </summary>
	// / @param <S> a custom "source" type, such as a user or originator of a command </param>
	public class CommandDispatcher<S>
	{
		// / <summary>
		// / The string required to separate individual arguments in an input string
		// / </summary>
		// / <seealso cref= #ARGUMENT_SEPARATOR_CHAR </seealso>
		public static string ARGUMENT_SEPARATOR = " ";

		// / <summary>
		// / The char required to separate individual arguments in an input string
		// / </summary>
		// / <seealso cref= #ARGUMENT_SEPARATOR </seealso>
		public static char ARGUMENT_SEPARATOR_CHAR = ' ';

		private static string USAGE_OPTIONAL_OPEN = "[";
		private static string USAGE_OPTIONAL_CLOSE = "]";
		private static string USAGE_REQUIRED_OPEN = "(";
		private static string USAGE_REQUIRED_CLOSE = ")";
		private static string USAGE_OR = "|";

		private RootCommandNode<S> root;

		private static System.Predicate<CommandNode<S>> hasCommand = (CommandNode<S> input) =>
		{
		return input != null && (input.Command != null || input.Children.Any(hasCommand));
		};
		private ResultConsumer<S> consumer = (c, s, r) =>
		{
		};

		// / <summary>
		// / Create a new <seealso cref="CommandDispatcher"/> with the specified root node.
		// / 
		// / <para>This is often useful to copy existing or pre-defined command trees.</para>
		// / </summary>
		// / <param name="root"> the existing <seealso cref="RootCommandNode"/> to use as the basis for this tree </param>
		public CommandDispatcher(RootCommandNode<S> root)
		{
			this.root = root;
		}

		// / <summary>
		// / Creates a new <seealso cref="CommandDispatcher"/> with an empty command tree.
		// / </summary>
		public CommandDispatcher() : this(new RootCommandNode<S>())
		{
		}

		// / <summary>
		// / Utility method for registering new commands.
		// / 
		// / <para>This is a shortcut for calling <seealso cref="RootCommandNode.addChild(CommandNode)"/> after building the provided {@code command}.</para>
		// / 
		// / <para>As <seealso cref="RootCommandNode"/> can only hold literals, this method will only allow literal arguments.</para>
		// / </summary>
		// / <param name="command"> a literal argument builder to add to this command tree </param>
		// / <returns> the node added to this tree </returns>
		public virtual LiteralCommandNode<S> register(LiteralArgumentBuilder<S> command)
		{
			 LiteralCommandNode<S> build = command.build();
			root.addChild(build);
			return build;
		}

		// / <summary>
		// / Sets a callback to be informed of the result of every command.
		// / </summary>
		// / <param name="consumer"> the new result consumer to be called </param>
		public virtual ResultConsumer<S> Consumer
		{
			set
			{
				this.consumer = value;
			}
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
// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public int execute(String input, S source) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int execute(string input, S source)
		{
			return execute(new StringReader(input), source);
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
// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public int execute(StringReader input, S source) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int execute(StringReader input, S source)
		{
			 ParseResults<S> parse = this.parse(input, source);
			return execute(parse);
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
// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: public int execute(ParseResults<S> parse) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual int execute(ParseResults<S> parse)
		{
			if (parse.Reader.canRead())
			{
				if (parse.Exceptions.Count == 1)
				{
					throw parse.Exceptions.Values.First();
				}
				else if (parse.Context.Range.Empty)
				{
					throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.dispatcherUnknownCommand().createWithContext(parse.Reader);
				}
				else
				{
					throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.dispatcherUnknownArgument().createWithContext(parse.Reader);
				}
			}

			int result = 0;
			int successfulForks = 0;
			bool forked = false;
			bool foundCommand = false;
			 string command = parse.Reader.String;
			 CommandContext<S> original = parse.Context.build(command);
			IList<CommandContext<S>> contexts = CollectionsHelper.SingletonList(original);
			List<CommandContext<S>> next = null;

			while (contexts != null)
			{
				 int size = contexts.Count;
				for (int i = 0; i < size; i++)
				{
					 CommandContext<S> context = contexts[i];
					 CommandContext<S> child = context.Child;
					if (child != null)
					{
						forked |= context.Forked;
						if (child.hasNodes())
						{
							foundCommand = true;
							 RedirectModifier<S> modifier = context.RedirectModifier;
							if (modifier == null)
							{
								if (next == null)
								{
									next = new List<CommandContext<S>>(1);
								}
								next.Add(child.copyFor(context.Source));
							}
							else
							{
								try
								{
									 ICollection<S> results = modifier(context);
									if (results.Count > 0)
									{
										if (next == null)
										{
											next = new List<CommandContext<S>>(results.Count);
										}
										foreach (S source in results)
										{
											next.Add(child.copyFor(source));
										}
									}
								}
								catch (CommandSyntaxException ex)
								{
									consumer(context, false, 0);
									if (!forked)
									{
										throw ex;
									}
								}
							}
						}
					}
					else if (context.Command != null)
					{
						foundCommand = true;
						try
						{
							 int value = context.Command(context);
							result += value;
							consumer(context, true, value);
							successfulForks++;
						}
						catch (CommandSyntaxException ex)
						{
							consumer(context, false, 0);
							if (!forked)
							{
								throw ex;
							}
						}
					}
				}

				contexts = next;
				next = null;
			}

			if (!foundCommand)
			{
				consumer(original, false, 0);
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.dispatcherUnknownCommand().createWithContext(parse.Reader);
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
		public virtual ParseResults<S> parse(string command, S source)
		{
			return parse(new StringReader(command), source);
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
		public virtual ParseResults<S> parse(StringReader command, S source)
		{
			 CommandContextBuilder<S> context = new CommandContextBuilder<S>(this, source, root, command.Cursor);
			return parseNodes(root, command, context);
		}

		private ParseResults<S> parseNodes(CommandNode<S> node, StringReader originalReader, CommandContextBuilder<S> contextSoFar)
		{
			 S source = contextSoFar.Source;
			IDictionary<CommandNode<S>, CommandSyntaxException> errors = null;
			IList<ParseResults<S>> potentials = null;
			 int cursor = originalReader.Cursor;

			foreach (CommandNode<S> child in node.getRelevantNodes(originalReader))
			{
				if (!child.canUse(source))
				{
					continue;
				}
				 CommandContextBuilder<S> context = contextSoFar.copy();
				 StringReader reader = new StringReader(originalReader);
				try
				{
					try
					{
						child.parse(reader, context);
					}
					catch (Exception ex)
					{
						throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.dispatcherParseException().createWithContext(reader, ex.Message);
					}
					if (reader.canRead())
					{
						if (reader.peek() != ARGUMENT_SEPARATOR_CHAR)
						{
							throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.dispatcherExpectedArgumentSeparator().createWithContext(reader);
						}
					}
				}
				catch (CommandSyntaxException ex)
				{
					if (errors == null)
					{
						errors = new Dictionary<CommandNode<S>, CommandSyntaxException>();
					}
					errors[child] = ex;
					reader.Cursor = cursor;
					continue;
				}

				context.withCommand(child.Command);
				if (reader.canRead(child.Redirect == null ? 2 : 1))
				{
					reader.skip();
					if (child.Redirect != null)
					{
						 CommandContextBuilder<S> childContext = new CommandContextBuilder<S>(this, source, child.Redirect, reader.Cursor);
						 ParseResults<S> parse = parseNodes(child.Redirect, reader, childContext);
						context.withChild(parse.Context);
						return new ParseResults<S>(context, parse.Reader, parse.Exceptions);
					}
					else
					{
						 ParseResults<S> parse = parseNodes(child, reader, context);
						if (potentials == null)
						{
							potentials = new List<ParseResults<S>>(1);
						}
						potentials.Add(parse);
					}
				}
				else
				{
					if (potentials == null)
					{
						potentials = new List<ParseResults<S>>(1);
					}
					potentials.Add(new ParseResults<S>(context, reader, CollectionsHelper.EmptyMap<CommandNode<S>, CommandSyntaxException>()));
				}
			}

			if (potentials != null)
			{
				if (potentials.Count > 1)
				{
					potentials.Sort((a, b) =>
					{
					if (!a.Reader.canRead() && b.Reader.canRead())
					{
						return -1;
					}
					if (a.Reader.canRead() && !b.Reader.canRead())
					{
						return 1;
					}
					if (a.Exceptions.IsEmpty() && !b.Exceptions.IsEmpty())
					{
						return -1;
					}
					if (!a.Exceptions.IsEmpty() && b.Exceptions.IsEmpty())
					{
						return 1;
					}
					return 0;
					});
				}
				return potentials[0];
			}

			return new ParseResults<S>(contextSoFar, originalReader, errors == null ? CollectionsHelper.EmptyMap<CommandNode<S>, CommandSyntaxException>() : errors);
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
		public virtual string[] getAllUsage(CommandNode<S> node, S source, bool restricted)
		{
			 List<string> result = new List<string>();
			getAllUsage(node, source, result, "", restricted);
			return result.ToArray();
		}

		private void getAllUsage(CommandNode<S> node, S source, List<string> result, string prefix, bool restricted)
		{
			if (restricted && !node.canUse(source))
			{
				return;
			}

			if (node.Command != null)
			{
				result.Add(prefix);
			}

			if (node.Redirect != null)
			{
				 string redirect = node.Redirect == root ? "..." : "-> " + node.Redirect.UsageText;
				result.Add(prefix.Length == 0 ? node.UsageText + ARGUMENT_SEPARATOR + redirect : prefix + ARGUMENT_SEPARATOR + redirect);
			}
			else if (node.Children.Count > 0)
			{
				foreach (CommandNode<S> child in node.Children)
				{
					getAllUsage(child, source, result, prefix.Length == 0 ? child.UsageText : prefix + ARGUMENT_SEPARATOR + child.UsageText, restricted);
				}
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
		public virtual IDictionary<CommandNode<S>, string> getSmartUsage(CommandNode<S> node, S source)
		{
			 IDictionary<CommandNode<S>, string> result = new Dictionary<CommandNode<S>, string>();

			 bool optional = node.Command != null;
			foreach (CommandNode<S> child in node.Children)
			{
				 string usage = getSmartUsage(child, source, optional, false);
				if (!string.ReferenceEquals(usage, null))
				{
					result[child] = usage;
				}
			}
			return result;
		}

		private string getSmartUsage(CommandNode<S> node, S source, bool optional, bool deep)
		{
			if (!node.canUse(source))
			{
				return null;
			}

			 string self = optional ? USAGE_OPTIONAL_OPEN + node.UsageText + USAGE_OPTIONAL_CLOSE : node.UsageText;
			 bool childOptional = node.Command != null;
			 string open = childOptional ? USAGE_OPTIONAL_OPEN : USAGE_REQUIRED_OPEN;
			 string close = childOptional ? USAGE_OPTIONAL_CLOSE : USAGE_REQUIRED_CLOSE;

			if (!deep)
			{
				if (node.Redirect != null)
				{
					 string redirect = node.Redirect == root ? "..." : "-> " + node.Redirect.UsageText;
					return self + ARGUMENT_SEPARATOR + redirect;
				}
				else
				{
					 ICollection<CommandNode<S>> children = node.Children.Where(c => c.canUse(source)).ToList();
					if (children.Count == 1)
					{
						 string usage = getSmartUsage(children.First(), source, childOptional, childOptional);
						if (!string.ReferenceEquals(usage, null))
						{
							return self + ARGUMENT_SEPARATOR + usage;
						}
					}
					else if (children.Count > 1)
					{
						 ISet<string> childUsage = new HashSet<string>();
						foreach (CommandNode<S> child in children)
						{
							 string usage = getSmartUsage(child, source, childOptional, true);
							if (!string.ReferenceEquals(usage, null))
							{
								childUsage.Add(usage);
							}
						}
						if (childUsage.Count == 1)
						{
							 string usage = childUsage.First();
							return self + ARGUMENT_SEPARATOR + (childOptional ? USAGE_OPTIONAL_OPEN + usage + USAGE_OPTIONAL_CLOSE : usage);
						}
						else if (childUsage.Count > 1)
						{
							 StringBuilder builder = new StringBuilder(open);
							int count = 0;
							foreach (CommandNode<S> child in children)
							{
								if (count > 0)
								{
									builder.Append(USAGE_OR);
								}
								builder.Append(child.UsageText);
								count++;
							}
							if (count > 0)
							{
								builder.Append(close);
								return self + ARGUMENT_SEPARATOR + builder.ToString();
							}
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
		public virtual System.Func<Suggestions> getCompletionSuggestions(ParseResults<S> parse)
		{
			return getCompletionSuggestions(parse, parse.Reader.TotalLength);
		}

		public virtual System.Func<Suggestions> getCompletionSuggestions(ParseResults<S> parse, int cursor)
		{
			 CommandContextBuilder<S> context = parse.Context;

			 SuggestionContext<S> nodeBeforeCursor = context.findSuggestionContext(cursor);
			 CommandNode<S> parent = nodeBeforeCursor.parent;
			 int start = Math.Min(nodeBeforeCursor.startPos, cursor);

			 string fullInput = parse.Reader.String;
			 string truncatedInput = fullInput.Substring(0, cursor);
// TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
// ORIGINAL LINE: @SuppressWarnings("unchecked") java.util.concurrent.CompletableFuture<com.mojang.brigadier.suggestion.Suggestions>[] futures = new java.util.concurrent.CompletableFuture[parent.getChildren().size()];
            var futures = new Func<Suggestions>[parent.Children.Count];
            var i = 0;
            foreach (var node in parent.Children)
            {
                var future = Suggestions.empty();
                try
                {
                    future = node.listSuggestions(context.build(truncatedInput),
                        new SuggestionsBuilder(truncatedInput, start));
                }
                catch (CommandSyntaxException)
                {
                }

                futures[i++] = future;
            }


            IList<Suggestions> suggestions = new List<Suggestions>();


            Parallel.Invoke(futures.Select(c => new Action(() => { suggestions.Add(c()); })).ToArray());


            return () => Suggestions.merge(fullInput, suggestions);
		}

		// / <summary>
		// / Gets the root of this command tree.
		// / 
		// / <para>This is often useful as a target of a <seealso cref="com.mojang.brigadier.builder.ArgumentBuilder.redirect(CommandNode)"/>,
		// / <seealso cref="getAllUsage(CommandNode, object, bool)"/> or <seealso cref="getSmartUsage(CommandNode, object)"/>.
		// / You may also use it to clone the command tree via <seealso cref="CommandDispatcher(RootCommandNode)"/>.</para>
		// / </summary>
		// / <returns> root of the command tree </returns>
		public virtual RootCommandNode<S> Root
		{
			get
			{
				return root;
			}
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
		public virtual ICollection<string> getPath(CommandNode<S> target)
		{
			 IList<IList<CommandNode<S>>> nodes = new List<IList<CommandNode<S>>>();
			addPaths(root, nodes, new List<CommandNode<S>>());

			foreach (IList<CommandNode<S>> list in nodes)
			{
				if (list[list.Count - 1] == target)
				{
					 IList<string> result = new List<string>(list.Count);
					foreach (CommandNode<S> node in list)
					{
						if (node != root)
						{
							result.Add(node.Name);
						}
					}
					return result;
				}
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
		public virtual CommandNode<S> findNode(ICollection<string> path)
		{
			CommandNode<S> node = root;
			foreach (string name in path)
			{
				node = node.getChild(name);
				if (node == null)
				{
					return null;
				}
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
		public virtual void findAmbiguities(AmbiguityConsumer<S> consumer)
		{
			root.findAmbiguities(consumer);
		}

		private void addPaths(CommandNode<S> node, IList<IList<CommandNode<S>>> result, IList<CommandNode<S>> parents)
		{
			 IList<CommandNode<S>> current = new List<CommandNode<S>>(parents);
			current.Add(node);
			result.Add(current);

			foreach (CommandNode<S> child in node.Children)
			{
				addPaths(child, result, current);
			}
		}
	}

}