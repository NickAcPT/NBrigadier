using System;
using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
	using Message = Message;

	public class CommandSyntaxException : Exception
	{
		public const int CONTEXT_AMOUNT = 10;
		public static bool ENABLE_COMMAND_STACK_TRACES = true;
		public static BuiltInExceptionProvider BUILT_IN_EXCEPTIONS = new BuiltInExceptions();

		private readonly CommandExceptionType type;
		private readonly Message message;
		private readonly string input;
		private readonly int cursor;

		public CommandSyntaxException(CommandExceptionType type, Message message) : base(message.String)//, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
		{
			this.type = type;
			this.message = message;
			this.input = null;
			this.cursor = -1;
		}

		public CommandSyntaxException(CommandExceptionType type, Message message, string input, int cursor) : base(message.String)//, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
		{
			this.type = type;
			this.message = message;
			this.input = input;
			this.cursor = cursor;
		}

		public override string Message
		{
			get
			{
				string message = this.message.String;
						string context = Context;
				if (!string.ReferenceEquals(context, null))
				{
					message += " at position " + cursor + ": " + context;
				}
				return message;
			}
		}

		public virtual Message RawMessage
		{
			get
			{
				return message;
			}
		}

		public virtual string Context
		{
			get
			{
				if (string.ReferenceEquals(input, null))
				{
					return null;
				}
						int cursor = Math.Min(input.Length, this.cursor);
				StringBuilder builder = new StringBuilder();
		    
				if (cursor > CONTEXT_AMOUNT)
				{
					builder.Append("...");
				}
    
				builder.Append(StringHelper.SubstringSpecial(input, Math.Max(0, cursor - CONTEXT_AMOUNT), cursor));
				builder.Append("<--[HERE]");
    
				return builder.ToString();
			}
		}

		public virtual CommandExceptionType Type
		{
			get
			{
				return type;
			}
		}

		public virtual string Input
		{
			get
			{
				return input;
			}
		}

		public virtual int Cursor
		{
			get
			{
				return cursor;
			}
		}
	}

}