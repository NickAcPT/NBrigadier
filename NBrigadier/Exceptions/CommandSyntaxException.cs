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
        private readonly int _cursor;
        private readonly string _input;
        private readonly IMessage _message;

        public CommandSyntaxException(ICommandExceptionType type, IMessage message) :
            base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
        {
            Type = type;
            _message = message;
            _input = null;
            _cursor = -1;
        }

        public CommandSyntaxException(ICommandExceptionType type, IMessage message, string input, int cursor) :
            base(message.String) //, null, ENABLE_COMMAND_STACK_TRACES, ENABLE_COMMAND_STACK_TRACES)
        {
            Type = type;
            _message = message;
            _input = input;
            _cursor = cursor;
        }

        public override string Message
        {
            get
            {
                var message = _message.String;
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
                if (ReferenceEquals(_input, null) || _cursor < 0) return null;
                var builder = new StringBuilder();
                var cursor = Math.Min(_input.Length, _cursor);

                if (cursor > contextAmount) builder.Append("...");

                builder.Append(_input.SubstringSpecial(Math.Max(0, cursor - contextAmount), cursor));
                builder.Append("<--[HERE]");

                return builder.ToString();
            }
        }

        public virtual ICommandExceptionType Type { get; }

        public virtual string Input => _input;

        public virtual int Cursor => _cursor;
    }
}