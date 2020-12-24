using System;
using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Exceptions
{
    public class CommandSyntaxException : Exception
    {
        public const int CONTEXT_AMOUNT = 10;
        public static bool ENABLE_COMMAND_STACK_TRACES = true;
        public static BuiltInExceptionProvider BUILT_IN_EXCEPTIONS = new BuiltInExceptions();
        private readonly int cursor;
        private readonly string input;
        private readonly Message message;

        public CommandSyntaxException(CommandExceptionType type, Message message) :
            base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
        {
            this.Type = type;
            this.message = message;
            input = null;
            cursor = -1;
        }

        public CommandSyntaxException(CommandExceptionType type, Message message, string input, int cursor) :
            base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
        {
            this.Type = type;
            this.message = message;
            this.input = input;
            this.cursor = cursor;
        }

        public override string Message
        {
            get
            {
                var message = this.message.String;
                var context = Context;
                if (!ReferenceEquals(context, null)) message += " at position " + cursor + ": " + context;
                return message;
            }
        }

        public virtual Message RawMessage => message;

        public virtual string Context
        {
            get
            {
                if (ReferenceEquals(input, null)) return null;
                var cursor = Math.Min(input.Length, this.cursor);
                var builder = new StringBuilder();

                if (cursor > CONTEXT_AMOUNT) builder.Append("...");

                builder.Append(input.SubstringSpecial(Math.Max(0, cursor - CONTEXT_AMOUNT), cursor));
                builder.Append("<--[HERE]");

                return builder.ToString();
            }
        }

        public virtual CommandExceptionType Type { get; }

        public virtual string Input => input;

        public virtual int Cursor => cursor;
    }
}