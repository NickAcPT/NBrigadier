using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier
{
	using com.mojang.brigadier.builder;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using com.mojang.brigadier.context;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;
	using Suggestions = com.mojang.brigadier.suggestion.Suggestions;
	using SuggestionsBuilder = com.mojang.brigadier.suggestion.SuggestionsBuilder;
	using com.mojang.brigadier.tree;
	using com.mojang.brigadier.tree;
	using com.mojang.brigadier.tree;



	/// <summary>
	/// The core command dispatcher, for registering, parsing, and executing commands.
	/// </summary>
	/// @param <S> a custom "source" type, such as a user or originator of a command </param>
	public class CommandDispatcher<S>
	{
		/// <summary>
		/// The string required to separate individual arguments in an input string
		/// </summary>
		/// <seealso cref= #ARGUMENT_SEPARATOR_CHAR </seealso>
		public const string ARGUMENT_SEPARATOR = " ";

		/// <summary>
		/// The char required to separate individual arguments in an input string
		/// </summary>
		/// <seealso cref= #ARGUMENT_SEPARATOR </seealso>
		public const char ARGUMENT_SEPARATOR_CHAR = ' ';

		private const string USAGE_OPTIONAL_OPEN = "[";
		private const string USAGE_OPTIONAL_CLOSE = "]";
		private const string USAGE_REQUIRED_OPEN = "(";
		private const string USAGE_REQUIRED_CLOSE = ")";
		private const string USAGE_OR = "|";

		private readonly RootCommandNode<S> root;

		private static readonly Func<CommandNode<S>, bool> hasCommand = (CommandNode<S> input) =>
		{
		return input != null && (input.Command != null || input.Children.Any(hasCommand));
		};
		private ResultConsumer<S> consumer = (c, s, r) =>
		{
		};

		/// <summary>
		/// Create a new <seealso cref="CommandDispatcher"/> with the specified root node.
		/// 
		/// <para>This is often useful to copy existing or pre-defined command trees.</para>
		/// </summary>
		/// <param name="root"> the existing <seealso cref="RootCommandNode"/> to use as the basis for this tree </param>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public CommandDispatcher(final com.mojang.brigadier.tree.RootCommandNode<S> root)
		public CommandDispatcher(RootCommandNode<S> root)
		{
			this.root = root;
		}

		/// <summary>
		/// Creates a new <seealso cref="CommandDispatcher"/> with an empty command tree.
		/// </summary>
		public CommandDispatcher() : this(new RootCommandNode<S>())
		{
		}

		/// <summary>
		/// Utility method for registering new commands.
		/// 
		/// <para>This is a shortcut for calling <seealso cref="RootCommandNode.addChild(CommandNode)"/> after building the provided {@code command}.</para>
		/// 
		/// <para>As <seealso cref="RootCommandNode"/> can only hold literals, this method will only allow literal arguments.</para>
		/// </summary>
		/// <param name="command"> a literal argument builder to add to this command tree </param>
		/// <returns> the node added to this tree </returns>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public com.mojang.brigadier.tree.LiteralCommandNode<S> register(final com.mojang.brigadier.builder.LiteralArgumentBuilder<S> command)
		public virtual LiteralCommandNode<S> Register(LiteralArgumentBuilder<S> command)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.tree.LiteralCommandNode<S> build = command.build();
			LiteralCommandNode<S> build = command.BuildLiteral();
			root.AddChild(build);
			return build;
		}

		/// <summary>
		/// Sets a callback to be informed of the result of every command.
		/// </summary>
		/// <param name="consumer"> the new result consumer to be called </param>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public void setConsumer(final ResultConsumer<S> consumer)
		public virtual ResultConsumer<S> Consumer
		{
			set
			{
				this.consumer = value;
			}
		}

		/// <summary>
		/// Parses and executes a given command.
		/// 
		/// <para>This is a shortcut to first <seealso cref="parse(StringReader, object)"/> and then <seealso cref="execute(ParseResults)"/>.</para>
		/// 
		/// <para>It is recommended to parse and execute as separate steps, as parsing is often the most expensive step, and easiest to cache.</para>
		/// 
		/// <para>If this command returns a value, then it successfully executed something. If it could not parse the command, or the execution was a failure,
		/// then an exception will be thrown. Most exceptions will be of type <seealso cref="CommandSyntaxException"/>, but it is possible that a <seealso cref="System.Exception"/>
		/// may bubble up from the result of a command. The meaning behind the returned result is arbitrary, and will depend
		/// entirely on what command was performed.</para>
		/// 
		/// <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then it will be 'forked'.
		/// A forked command will not bubble up any <seealso cref="CommandSyntaxException"/>s, and the 'result' returned will turn into
		/// 'amount of successful commands executes'.</para>
		/// 
		/// <para>After each and any command is ran, a registered callback given to <seealso cref="setConsumer(ResultConsumer)"/>
		/// will be notified of the result and success of the command. You can use that method to gather more meaningful
		/// results than this method will return, especially when a command forks.</para>
		/// </summary>
		/// <param name="input"> a command string to parse &amp; execute </param>
		/// <param name="source"> a custom "source" object, usually representing the originator of this command </param>
		/// <returns> a numeric result from a "command" that was performed </returns>
		/// <exception cref="CommandSyntaxException"> if the command failed to parse or execute </exception>
		/// <exception cref="RuntimeException"> if the command failed to execute and was not handled gracefully </exception>
		/// <seealso cref= #parse(String, Object) </seealso>
		/// <seealso cref= #parse(StringReader, Object) </seealso>
		/// <seealso cref= #execute(ParseResults) </seealso>
		/// <seealso cref= #execute(StringReader, Object) </seealso>
//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public int execute(final String input, final S source) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public virtual int Execute(string input, S source)
		{
			return Execute(new StringReader(input), source);
		}

		/// <summary>
		/// Parses and executes a given command.
		/// 
		/// <para>This is a shortcut to first <seealso cref="parse(StringReader, object)"/> and then <seealso cref="execute(ParseResults)"/>.</para>
		/// 
		/// <para>It is recommended to parse and execute as separate steps, as parsing is often the most expensive step, and easiest to cache.</para>
		/// 
		/// <para>If this command returns a value, then it successfully executed something. If it could not parse the command, or the execution was a failure,
		/// then an exception will be thrown. Most exceptions will be of type <seealso cref="CommandSyntaxException"/>, but it is possible that a <seealso cref="System.Exception"/>
		/// may bubble up from the result of a command. The meaning behind the returned result is arbitrary, and will depend
		/// entirely on what command was performed.</para>
		/// 
		/// <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then it will be 'forked'.
		/// A forked command will not bubble up any <seealso cref="CommandSyntaxException"/>s, and the 'result' returned will turn into
		/// 'amount of successful commands executes'.</para>
		/// 
		/// <para>After each and any command is ran, a registered callback given to <seealso cref="setConsumer(ResultConsumer)"/>
		/// will be notified of the result and success of the command. You can use that method to gather more meaningful
		/// results than this method will return, especially when a command forks.</para>
		/// </summary>
		/// <param name="input"> a command string to parse &amp; execute </param>
		/// <param name="source"> a custom "source" object, usually representing the originator of this command </param>
		/// <returns> a numeric result from a "command" that was performed </returns>
		/// <exception cref="CommandSyntaxException"> if the command failed to parse or execute </exception>
		/// <exception cref="RuntimeException"> if the command failed to execute and was not handled gracefully </exception>
		/// <seealso cref= #parse(String, Object) </seealso>
		/// <seealso cref= #parse(StringReader, Object) </seealso>
		/// <seealso cref= #execute(ParseResults) </seealso>
		/// <seealso cref= #execute(String, Object) </seealso>
//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public int execute(final StringReader input, final S source) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public virtual int Execute(StringReader input, S source)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParseResults<S> parse = parse(input, source);
			ParseResults<S> parse = Parse(input, source);
			return Execute(parse);
		}

		/// <summary>
		/// Executes a given pre-parsed command.
		/// 
		/// <para>If this command returns a value, then it successfully executed something. If the execution was a failure,
		/// then an exception will be thrown.
		/// Most exceptions will be of type <seealso cref="CommandSyntaxException"/>, but it is possible that a <seealso cref="System.Exception"/>
		/// may bubble up from the result of a command. The meaning behind the returned result is arbitrary, and will depend
		/// entirely on what command was performed.</para>
		/// 
		/// <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then it will be 'forked'.
		/// A forked command will not bubble up any <seealso cref="CommandSyntaxException"/>s, and the 'result' returned will turn into
		/// 'amount of successful commands executes'.</para>
		/// 
		/// <para>After each and any command is ran, a registered callback given to <seealso cref="setConsumer(ResultConsumer)"/>
		/// will be notified of the result and success of the command. You can use that method to gather more meaningful
		/// results than this method will return, especially when a command forks.</para>
		/// </summary>
		/// <param name="parse"> the result of a successful <seealso cref="parse(StringReader, object)"/> </param>
		/// <returns> a numeric result from a "command" that was performed. </returns>
		/// <exception cref="CommandSyntaxException"> if the command failed to parse or execute </exception>
		/// <exception cref="RuntimeException"> if the command failed to execute and was not handled gracefully </exception>
		/// <seealso cref= #parse(String, Object) </seealso>
		/// <seealso cref= #parse(StringReader, Object) </seealso>
		/// <seealso cref= #execute(String, Object) </seealso>
		/// <seealso cref= #execute(StringReader, Object) </seealso>
//WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: public int execute(final ParseResults<S> parse) throws com.mojang.brigadier.exceptions.CommandSyntaxException
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
		public virtual int Execute(ParseResults<S> parse)
		{
			if (parse.Reader.CanRead())
			{
				if (parse.Exceptions.Count == 1)
                {
                    throw parse.Exceptions.Values.First();
                }
				else if (parse.Context.Range.Empty)
				{
					throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DispatcherUnknownCommand().CreateWithContext(parse.Reader);
				}
				else
				{
					throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DispatcherUnknownArgument().CreateWithContext(parse.Reader);
				}
			}

			int result = 0;
			int successfulForks = 0;
			bool forked = false;
			bool foundCommand = false;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String command = parse.getReader().getString();
			string command = parse.Reader.String;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContext<S> original = parse.getContext().build(command);
			CommandContext<S> original = parse.Context.Build(command);
			IList<CommandContext<S>> contexts = new List<CommandContext<S>> {original};
			List<CommandContext<S>> next = null;

			while (contexts != null)
			{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int size = contexts.size();
				int size = contexts.Count;
				for (int i = 0; i < size; i++)
				{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContext<S> context = contexts.get(i);
					CommandContext<S> context = contexts[i];
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContext<S> child = context.getChild();
					CommandContext<S> child = context.Child;
					if (child != null)
					{
						forked |= context.Forked;
						if (child.HasNodes())
						{
							foundCommand = true;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final RedirectModifier<S> modifier = context.getRedirectModifier();
							RedirectModifier<S> modifier = context.RedirectModifier;
							if (modifier == null)
							{
								if (next == null)
								{
									next = new List<CommandContext<S>>(1);
								}
								next.Add(child.CopyFor(context.Source));
							}
							else
							{
								try
								{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Collection<S> results = modifier.apply(context);
									ICollection<S> results = modifier(context);
									if (results.Count > 0)
									{
										if (next == null)
										{
											next = new List<CommandContext<S>>(results.Count);
										}
										foreach (S source in results)
										{
											next.Add(child.CopyFor(source));
										}
									}
								}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final com.mojang.brigadier.exceptions.CommandSyntaxException ex)
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
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int value = context.getCommand().run(context);
							int value = context.Command(context);
							result += value;
							consumer(context, true, value);
							successfulForks++;
						}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final com.mojang.brigadier.exceptions.CommandSyntaxException ex)
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
				throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DispatcherUnknownCommand().CreateWithContext(parse.Reader);
			}

			return forked ? successfulForks : result;
		}

		/// <summary>
		/// Parses a given command.
		/// 
		/// <para>The result of this method can be cached, and it is advised to do so where appropriate. Parsing is often the
		/// most expensive step, and this allows you to essentially "precompile" a command if it will be ran often.</para>
		/// 
		/// <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then the resulting context will be marked as 'forked'.
		/// Forked contexts may contain child contexts, which may be modified by the <seealso cref="RedirectModifier"/> attached to the fork.</para>
		/// 
		/// <para>Parsing a command can never fail, you will always be provided with a new <seealso cref="ParseResults"/>.
		/// However, that does not mean that it will always parse into a valid command. You should inspect the returned results
		/// to check for validity. If its <seealso cref="ParseResults.getReader()"/> <seealso cref="StringReader.canRead()"/> then it did not finish
		/// parsing successfully. You can use that position as an indicator to the user where the command stopped being valid.
		/// You may inspect <seealso cref="ParseResults.getExceptions()"/> if you know the parse failed, as it will explain why it could
		/// not find any valid commands. It may contain multiple exceptions, one for each "potential node" that it could have visited,
		/// explaining why it did not go down that node.</para>
		/// 
		/// <para>When you eventually call <seealso cref="execute(ParseResults)"/> with the result of this method, the above error checking
		/// will occur. You only need to inspect it yourself if you wish to handle that yourself.</para>
		/// </summary>
		/// <param name="command"> a command string to parse </param>
		/// <param name="source"> a custom "source" object, usually representing the originator of this command </param>
		/// <returns> the result of parsing this command </returns>
		/// <seealso cref= #parse(StringReader, Object) </seealso>
		/// <seealso cref= #execute(ParseResults) </seealso>
		/// <seealso cref= #execute(String, Object) </seealso>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public ParseResults<S> parse(final String command, final S source)
		public virtual ParseResults<S> Parse(string command, S source)
		{
			return Parse(new StringReader(command), source);
		}

		/// <summary>
		/// Parses a given command.
		/// 
		/// <para>The result of this method can be cached, and it is advised to do so where appropriate. Parsing is often the
		/// most expensive step, and this allows you to essentially "precompile" a command if it will be ran often.</para>
		/// 
		/// <para>If the command passes through a node that is <seealso cref="CommandNode.isFork()"/> then the resulting context will be marked as 'forked'.
		/// Forked contexts may contain child contexts, which may be modified by the <seealso cref="RedirectModifier"/> attached to the fork.</para>
		/// 
		/// <para>Parsing a command can never fail, you will always be provided with a new <seealso cref="ParseResults"/>.
		/// However, that does not mean that it will always parse into a valid command. You should inspect the returned results
		/// to check for validity. If its <seealso cref="ParseResults.getReader()"/> <seealso cref="StringReader.canRead()"/> then it did not finish
		/// parsing successfully. You can use that position as an indicator to the user where the command stopped being valid.
		/// You may inspect <seealso cref="ParseResults.getExceptions()"/> if you know the parse failed, as it will explain why it could
		/// not find any valid commands. It may contain multiple exceptions, one for each "potential node" that it could have visited,
		/// explaining why it did not go down that node.</para>
		/// 
		/// <para>When you eventually call <seealso cref="execute(ParseResults)"/> with the result of this method, the above error checking
		/// will occur. You only need to inspect it yourself if you wish to handle that yourself.</para>
		/// </summary>
		/// <param name="command"> a command string to parse </param>
		/// <param name="source"> a custom "source" object, usually representing the originator of this command </param>
		/// <returns> the result of parsing this command </returns>
		/// <seealso cref= #parse(String, Object) </seealso>
		/// <seealso cref= #execute(ParseResults) </seealso>
		/// <seealso cref= #execute(String, Object) </seealso>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public ParseResults<S> parse(final StringReader command, final S source)
		public virtual ParseResults<S> Parse(StringReader command, S source)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContextBuilder<S> context = new com.mojang.brigadier.context.CommandContextBuilder<>(this, source, root, command.getCursor());
			CommandContextBuilder<S> context = new CommandContextBuilder<S>(this, source, root, command.Cursor);
			return ParseNodes(root, command, context);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private ParseResults<S> parseNodes(final com.mojang.brigadier.tree.CommandNode<S> node, final StringReader originalReader, final com.mojang.brigadier.context.CommandContextBuilder<S> contextSoFar)
		private ParseResults<S> ParseNodes(CommandNode<S> node, StringReader originalReader, CommandContextBuilder<S> contextSoFar)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final S source = contextSoFar.getSource();
			S source = contextSoFar.Source;
			IDictionary<CommandNode<S>, CommandSyntaxException> errors = null;
			IList<ParseResults<S>> potentials = null;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int cursor = originalReader.getCursor();
			int cursor = originalReader.Cursor;

			foreach (CommandNode<S> child in node.GetRelevantNodes(originalReader))
			{
				if (!child.CanUse(source))
				{
					continue;
				}
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContextBuilder<S> context = contextSoFar.copy();
				CommandContextBuilder<S> context = contextSoFar.Copy();
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringReader reader = new StringReader(originalReader);
				StringReader reader = new StringReader(originalReader);
				try
				{
					try
					{
						child.Parse(reader, context);
					}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final RuntimeException ex)
					catch (Exception ex)
					{
						throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DispatcherParseException().CreateWithContext(reader, ex.Message);
					}
					if (reader.CanRead())
					{
						if (reader.Peek() != ARGUMENT_SEPARATOR_CHAR)
						{
							throw CommandSyntaxException.BUILT_IN_EXCEPTIONS.DispatcherExpectedArgumentSeparator().CreateWithContext(reader);
						}
					}
				}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final com.mojang.brigadier.exceptions.CommandSyntaxException ex)
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

				context.WithCommand(child.Command);
				if (reader.CanRead(child.Redirect == null ? 2 : 1))
				{
					reader.Skip();
					if (child.Redirect != null)
					{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContextBuilder<S> childContext = new com.mojang.brigadier.context.CommandContextBuilder<>(this, source, child.getRedirect(), reader.getCursor());
						CommandContextBuilder<S> childContext = new CommandContextBuilder<S>(this, source, child.Redirect, reader.Cursor);
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParseResults<S> parse = parseNodes(child.getRedirect(), reader, childContext);
						ParseResults<S> parse = ParseNodes(child.Redirect, reader, childContext);
						context.WithChild(parse.Context);
						return new ParseResults<S>(context, parse.Reader, parse.Exceptions);
					}
					else
					{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final ParseResults<S> parse = parseNodes(child, reader, context);
						ParseResults<S> parse = ParseNodes(child, reader, context);
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
					potentials.Add(new ParseResults<S>(context, reader, new Dictionary<CommandNode<S>, CommandSyntaxException>()));
				}
			}

			if (potentials != null)
			{
				if (potentials.Count > 1)
				{
					potentials = potentials.OrderBy(c => c, Comparer<ParseResults<S>>.Create((a, b) => 
                    
					{
					if (!a.Reader.CanRead() && b.Reader.CanRead())
					{
						return -1;
					}
					if (a.Reader.CanRead() && !b.Reader.CanRead())
					{
						return 1;
					}
					if (!a.Exceptions.Any() && !(b.Exceptions.Any()))
					{
						return -1;
					}
					if (a.Exceptions.Any() && !b.Exceptions.Any())
					{
						return 1;
					}
					return 0;
					})).ToList();
				}
				return potentials[0];
			}

			return new ParseResults<S>(contextSoFar, originalReader, errors == null ? new Dictionary<CommandNode<S>, CommandSyntaxException>() : errors);
		}

		/// <summary>
		/// Gets all possible executable commands following the given node.
		/// 
		/// <para>You may use <seealso cref="getRoot()"/> as a target to get all usage data for the entire command tree.</para>
		/// 
		/// <para>The returned syntax will be in "simple" form: {@code <param>} and {@code literal}. "Optional" nodes will be
		/// listed as multiple entries: the parent node, and the child nodes.
		/// For example, a required literal "foo" followed by an optional param "int" will be two nodes:</para>
		/// <ul>
		///     <li>{@code foo}</li>
		///     <li>{@code foo <int>}</li>
		/// </ul>
		/// 
		/// <para>The path to the specified node will <b>not</b> be prepended to the output, as there can theoretically be many
		/// ways to reach a given node. It will only give you paths relative to the specified node, not absolute from root.</para>
		/// </summary>
		/// <param name="node"> target node to get child usage strings for </param>
		/// <param name="source"> a custom "source" object, usually representing the originator of this command </param>
		/// <param name="restricted"> if true, commands that the {@code source} cannot access will not be mentioned </param>
		/// <returns> array of full usage strings under the target node </returns>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public String[] getAllUsage(final com.mojang.brigadier.tree.CommandNode<S> node, final S source, final boolean restricted)
		public virtual string[] GetAllUsage(CommandNode<S> node, S source, bool restricted)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.ArrayList<String> result = new java.util.ArrayList<>();
			List<string> result = new List<string>();
			GetAllUsage(node, source, result, "", restricted);
			return result.ToArray();
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private void getAllUsage(final com.mojang.brigadier.tree.CommandNode<S> node, final S source, final java.util.ArrayList<String> result, final String prefix, final boolean restricted)
		private void GetAllUsage(CommandNode<S> node, S source, List<string> result, string prefix, bool restricted)
		{
			if (restricted && !node.CanUse(source))
			{
				return;
			}

			if (node.Command != null)
			{
				result.Add(prefix);
			}

			if (node.Redirect != null)
			{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String redirect = node.getRedirect() == root ? "..." : "-> " + node.getRedirect().getUsageText();
				string redirect = node.Redirect == root ? "..." : "-> " + node.Redirect.UsageText;
				result.Add(prefix.Length == 0 ? node.UsageText + ARGUMENT_SEPARATOR + redirect : prefix + ARGUMENT_SEPARATOR + redirect);
			}
			else if (node.Children.Count > 0)
			{
				foreach (CommandNode<S> child in node.Children)
				{
					GetAllUsage(child, source, result, prefix.Length == 0 ? child.UsageText : prefix + ARGUMENT_SEPARATOR + child.UsageText, restricted);
				}
			}
		}

		/// <summary>
		/// Gets the possible executable commands from a specified node.
		/// 
		/// <para>You may use <seealso cref="getRoot()"/> as a target to get usage data for the entire command tree.</para>
		/// 
		/// <para>The returned syntax will be in "smart" form: {@code <param>}, {@code literal}, {@code [optional]} and {@code (either|or)}.
		/// These forms may be mixed and matched to provide as much information about the child nodes as it can, without being too verbose.
		/// For example, a required literal "foo" followed by an optional param "int" can be compressed into one string:</para>
		/// <ul>
		///     <li>{@code foo [<int>]}</li>
		/// </ul>
		/// 
		/// <para>The path to the specified node will <b>not</b> be prepended to the output, as there can theoretically be many
		/// ways to reach a given node. It will only give you paths relative to the specified node, not absolute from root.</para>
		/// 
		/// <para>The returned usage will be restricted to only commands that the provided {@code source} can use.</para>
		/// </summary>
		/// <param name="node"> target node to get child usage strings for </param>
		/// <param name="source"> a custom "source" object, usually representing the originator of this command </param>
		/// <returns> array of full usage strings under the target node </returns>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public java.util.Map<com.mojang.brigadier.tree.CommandNode<S>, String> getSmartUsage(final com.mojang.brigadier.tree.CommandNode<S> node, final S source)
		public virtual IDictionary<CommandNode<S>, string> GetSmartUsage(CommandNode<S> node, S source)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Map<com.mojang.brigadier.tree.CommandNode<S>, String> result = new java.util.LinkedHashMap<>();
			IDictionary<CommandNode<S>, string> result = new Dictionary<CommandNode<S>, string>();

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean optional = node.getCommand() != null;
			bool optional = node.Command != null;
			foreach (CommandNode<S> child in node.Children)
			{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String usage = getSmartUsage(child, source, optional, false);
				string usage = GetSmartUsage(child, source, optional, false);
				if (!string.ReferenceEquals(usage, null))
				{
					result[child] = usage;
				}
			}
			return result;
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private String getSmartUsage(final com.mojang.brigadier.tree.CommandNode<S> node, final S source, final boolean optional, final boolean deep)
		private string GetSmartUsage(CommandNode<S> node, S source, bool optional, bool deep)
		{
			if (!node.CanUse(source))
			{
				return null;
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String self = optional ? USAGE_OPTIONAL_OPEN + node.getUsageText() + USAGE_OPTIONAL_CLOSE : node.getUsageText();
			string self = optional ? USAGE_OPTIONAL_OPEN + node.UsageText + USAGE_OPTIONAL_CLOSE : node.UsageText;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean childOptional = node.getCommand() != null;
			bool childOptional = node.Command != null;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String open = childOptional ? USAGE_OPTIONAL_OPEN : USAGE_REQUIRED_OPEN;
			string open = childOptional ? USAGE_OPTIONAL_OPEN : USAGE_REQUIRED_OPEN;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String close = childOptional ? USAGE_OPTIONAL_CLOSE : USAGE_REQUIRED_CLOSE;
			string close = childOptional ? USAGE_OPTIONAL_CLOSE : USAGE_REQUIRED_CLOSE;

			if (!deep)
			{
				if (node.Redirect != null)
				{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String redirect = node.getRedirect() == root ? "..." : "-> " + node.getRedirect().getUsageText();
					string redirect = node.Redirect == root ? "..." : "-> " + node.Redirect.UsageText;
					return self + ARGUMENT_SEPARATOR + redirect;
				}
				else
				{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Collection<com.mojang.brigadier.tree.CommandNode<S>> children = node.getChildren().stream().filter(c -> c.canUse(source)).collect(java.util.stream.Collectors.toList());
					ICollection<CommandNode<S>> children = node.Children.Where(c => c.CanUse(source)).ToList();
					if (children.Count == 1)
					{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String usage = getSmartUsage(children.iterator().next(), source, childOptional, childOptional);
						string usage = GetSmartUsage(children.First(), source, childOptional, childOptional);
						if (!string.ReferenceEquals(usage, null))
						{
							return self + ARGUMENT_SEPARATOR + usage;
						}
					}
					else if (children.Count > 1)
					{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.Set<String> childUsage = new java.util.LinkedHashSet<>();
						ISet<string> childUsage = new HashSet<string>();
						foreach (CommandNode<S> child in children)
						{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String usage = getSmartUsage(child, source, childOptional, true);
							string usage = GetSmartUsage(child, source, childOptional, true);
							if (!string.ReferenceEquals(usage, null))
							{
								childUsage.Add(usage);
							}
						}
						if (childUsage.Count == 1)
						{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String usage = childUsage.iterator().next();
                            string usage = childUsage.First();
							return self + ARGUMENT_SEPARATOR + (childOptional ? USAGE_OPTIONAL_OPEN + usage + USAGE_OPTIONAL_CLOSE : usage);
						}
						else if (childUsage.Count > 1)
						{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final StringBuilder builder = new StringBuilder(open);
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

		/// <summary>
		/// Gets suggestions for a parsed input string on what comes next.
		/// 
		/// <para>As it is ultimately up to custom argument types to provide suggestions, it may be an asynchronous operation,
		/// for example getting in-game data or player names etc. As such, this method returns a future and no guarantees
		/// are made to when or how the future completes.</para>
		/// 
		/// <para>The suggestions provided will be in the context of the end of the parsed input string, but may suggest
		/// new or replacement strings for earlier in the input string. For example, if the end of the string was
		/// {@code foobar} but an argument preferred it to be {@code minecraft:foobar}, it will suggest a replacement for that
		/// whole segment of the input.</para>
		/// </summary>
		/// <param name="parse"> the result of a <seealso cref="parse(StringReader, object)"/> </param>
		/// <returns> a future that will eventually resolve into a <seealso cref="Suggestions"/> object </returns>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> getCompletionSuggestions(final ParseResults<S> parse)
		public virtual Func<Suggestions> GetCompletionSuggestions(ParseResults<S> parse)
		{
			return GetCompletionSuggestions(parse, parse.Reader.TotalLength);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> getCompletionSuggestions(final ParseResults<S> parse, int cursor)
		public virtual Func<Suggestions> GetCompletionSuggestions(ParseResults<S> parse, int cursor)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.CommandContextBuilder<S> context = parse.getContext();
			CommandContextBuilder<S> context = parse.Context;

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.context.SuggestionContext<S> nodeBeforeCursor = context.findSuggestionContext(cursor);
			SuggestionContext<S> nodeBeforeCursor = context.FindSuggestionContext(cursor);
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final com.mojang.brigadier.tree.CommandNode<S> parent = nodeBeforeCursor.parent;
			CommandNode<S> parent = nodeBeforeCursor.parent;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int start = Math.min(nodeBeforeCursor.startPos, cursor);
			int start = Math.Min(nodeBeforeCursor.startPos, cursor);

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String fullInput = parse.getReader().getString();
			string fullInput = parse.Reader.String;
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final String truncatedInput = fullInput.substring(0, cursor);
			string truncatedInput = fullInput.Substring(0, cursor);
//TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") final java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions>[] futures = new java.util.concurrent.System.Action[parent.getChildren().size()];
//WARNING: The original Java variable was marked 'final':
            Func<Suggestions>[] futures = new Func<Suggestions>[parent.Children.Count];
			int i = 0;
			foreach (CommandNode<S> node in parent.Children)
			{
                Func<Suggestions> future = Suggestions.Empty();
				try
				{
					future = node.ListSuggestions(context.Build(truncatedInput), new SuggestionsBuilder(truncatedInput, start));
				}
//WARNING: 'final' catch parameters are not available in C#:
//ORIGINAL LINE: catch (final com.mojang.brigadier.exceptions.CommandSyntaxException ignored)
				catch (com.mojang.brigadier.exceptions.CommandSyntaxException)
				{
				}
				futures[i++] = future;
			}

//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.concurrent.System.Action<com.mojang.brigadier.suggestion.Suggestions> result = new java.util.concurrent.System.Action<>();

            IList<Suggestions> suggestions = new List<Suggestions>();
			
            
            Parallel.Invoke(futures.Select(c => new Action(() =>
            {
                suggestions.Add(c());
            })).ToArray());
            

			return () => Suggestions.Merge(fullInput, suggestions);
		}

		/// <summary>
		/// Gets the root of this command tree.
		/// 
		/// <para>This is often useful as a target of a <seealso cref="com.mojang.brigadier.builder.ArgumentBuilder.redirect(CommandNode)"/>,
		/// <seealso cref="getAllUsage(CommandNode, object, bool)"/> or <seealso cref="getSmartUsage(CommandNode, object)"/>.
		/// You may also use it to clone the command tree via <seealso cref="CommandDispatcher(RootCommandNode)"/>.</para>
		/// </summary>
		/// <returns> root of the command tree </returns>
		public virtual RootCommandNode<S> Root
		{
			get
			{
				return root;
			}
		}

		/// <summary>
		/// Finds a valid path to a given node on the command tree.
		/// 
		/// <para>There may theoretically be multiple paths to a node on the tree, especially with the use of forking or redirecting.
		/// As such, this method makes no guarantees about which path it finds. It will not look at forks or redirects,
		/// and find the first instance of the target node on the tree.</para>
		/// 
		/// <para>The only guarantee made is that for the same command tree and the same version of this library, the result of
		/// this method will <b>always</b> be a valid input for <seealso cref="findNode(System.Collections.ICollection)"/>, which should return the same node
		/// as provided to this method.</para>
		/// </summary>
		/// <param name="target"> the target node you are finding a path for </param>
		/// <returns> a path to the resulting node, or an empty list if it was not found </returns>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public java.util.Collection<String> getPath(final com.mojang.brigadier.tree.CommandNode<S> target)
		public virtual ICollection<string> GetPath(CommandNode<S> target)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<java.util.List<com.mojang.brigadier.tree.CommandNode<S>>> nodes = new java.util.ArrayList<>();
			IList<IList<CommandNode<S>>> nodes = new List<IList<CommandNode<S>>>();
			AddPaths(root, nodes, new List<CommandNode<S>>());

			foreach (IList<CommandNode<S>> list in nodes)
			{
				if (Equals(list.ElementAtOrDefault(list.Count - 1), target))
				{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<String> result = new java.util.ArrayList<>(list.size());
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

			return new List<string>();
		}

		/// <summary>
		/// Finds a node by its path
		/// 
		/// <para>Paths may be generated with <seealso cref="getPath(CommandNode)"/>, and are guaranteed (for the same tree, and the
		/// same version of this library) to always produce the same valid node by this method.</para>
		/// 
		/// <para>If a node could not be found at the specified path, then {@code null} will be returned.</para>
		/// </summary>
		/// <param name="path"> a generated path to a node </param>
		/// <returns> the node at the given path, or null if not found </returns>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public com.mojang.brigadier.tree.CommandNode<S> findNode(final java.util.Collection<String> path)
		public virtual CommandNode<S> FindNode(ICollection<string> path)
		{
			CommandNode<S> node = root;
			foreach (String name in path)
			{
				node = node.GetChild(name);
				if (node == null)
				{
					return null;
				}
			}
			return node;
		}

		/// <summary>
		/// Scans the command tree for potential ambiguous commands.
		/// 
		/// <para>This is a shortcut for <seealso cref="CommandNode.findAmbiguities(AmbiguityConsumer)"/> on <seealso cref="getRoot()"/>.</para>
		/// 
		/// <para>Ambiguities are detected by testing every <seealso cref="CommandNode.getExamples()"/> on one node verses every sibling
		/// node. This is not fool proof, and relies a lot on the providers of the used argument types to give good examples.</para>
		/// </summary>
		/// <param name="consumer"> a callback to be notified of potential ambiguities </param>
//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: public void findAmbiguities(final AmbiguityConsumer<S> consumer)
		public virtual void FindAmbiguities(AmbiguityConsumer<S> consumer)
		{
			root.FindAmbiguities(consumer);
		}

//WARNING: 'final' parameters are ignored unless the option to convert to C# 7.2 'in' parameters is selected:
//ORIGINAL LINE: private void addPaths(final com.mojang.brigadier.tree.CommandNode<S> node, final java.util.List<java.util.List<com.mojang.brigadier.tree.CommandNode<S>>> result, final java.util.List<com.mojang.brigadier.tree.CommandNode<S>> parents)
		private void AddPaths(CommandNode<S> node, IList<IList<CommandNode<S>>> result, IList<CommandNode<S>> parents)
		{
//WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.util.List<com.mojang.brigadier.tree.CommandNode<S>> current = new java.util.ArrayList<>(parents);
			IList<CommandNode<S>> current = new List<CommandNode<S>>(parents);
			current.Add(node);
			result.Add(current);

			foreach (CommandNode<S> child in node.Children)
			{
				AddPaths(child, result, current);
			}
		}
	}

}