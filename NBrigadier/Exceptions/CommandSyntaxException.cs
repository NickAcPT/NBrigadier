using System;
using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class CommandSyntaxException : Exception
    {
        public const int ContextAmount = 10;
        public static bool EnableCommandStackTraces = true;
        public static IBuiltInExceptionProvider BuiltInExceptions = new BuiltInExceptions();
        private readonly int _cursor;
        private readonly string _input;
        private readonly IMessage _message;

        public CommandSyntaxException(ICommandExceptionType type, IMessage message) :
            base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
        {
            this.Type = type;
            this._message = message;
            _input = null;
            _cursor = -1;
        }

        public CommandSyntaxException(ICommandExceptionType type, IMessage message, string input, int cursor) :
            base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
        {
            this.Type = type;
            this._message = message;
            this._input = input;
            this._cursor = cursor;
        }

        public override string Message
        {
            get
            {
                var message = this._message.String;
                var context = Context;
                if (!ReferenceEquals(context, null)) message += " at position " + _cursor + ": " + context;
                return message;
            }
        }

        public virtual IMessage RawMessage => _message;

        public virtual string Context
        {
            get
            {
                if (ReferenceEquals(_input, null)) return null;
                var cursor = Math.Min(_input.Length, this._cursor);
                var builder = new StringBuilder();

                if (cursor > ContextAmount) builder.Append("...");

                builder.Append(_input.SubstringSpecial(Math.Max(0, cursor - ContextAmount), cursor));
                builder.Append("<--[HERE]");

                return builder.ToString();
            }
        }

        public virtual ICommandExceptionType Type { get; }

        public virtual string Input => _input;

        public virtual int Cursor => _cursor;
    }
}