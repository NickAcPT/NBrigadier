using System;
using System.Text;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class CommandSyntaxException : Exception
	{
		public static int contextAmount = 10;
		public static bool enableCommandStackTraces = true;
		public static IBuiltInExceptionProvider builtInExceptions = new BuiltInExceptions();

		private ICommandExceptionType _type;
		private IMessage _message;
		private string _input;
		private int _cursor;

		public CommandSyntaxException(ICommandExceptionType type, IMessage message) : base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
		{
			this._type = type;
			this._message = message;
			this._input = null;
			this._cursor = -1;
		}

		public CommandSyntaxException(ICommandExceptionType type, IMessage message, string input, int cursor) : base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
		{
			this._type = type;
			this._message = message;
			this._input = input;
			this._cursor = cursor;
		}

		public override string Message
		{
			get
			{
				string message = this._message.String;
				 string context = Context;
				if (!string.ReferenceEquals(context, null))
				{
					message += " at position " + _cursor + ": " + context;
				}
				return message;
			}
		}

		public virtual IMessage RawMessage
		{
			get
			{
				return _message;
			}
		}

		public virtual string Context
		{
			get
			{
				if (string.ReferenceEquals(_input, null) || this._cursor < 0)
				{
					return null;
				}
				 StringBuilder builder = new StringBuilder();
				 int cursor = Math.Min(_input.Length, this._cursor);
    
				if (cursor > contextAmount)
				{
					builder.Append("...");
				}
    
				builder.Append(StringHelper.SubstringSpecial(_input, Math.Max(0, cursor - contextAmount), cursor));
				builder.Append("<--[HERE]");
    
				return builder.ToString();
			}
		}

		public virtual ICommandExceptionType Type
		{
			get
			{
				return _type;
			}
		}

		public virtual string Input
		{
			get
			{
				return _input;
			}
		}

		public virtual int Cursor
		{
			get
			{
				return _cursor;
			}
		}
	}

}