using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBrigadier.Context;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class StringArgumentType : ArgumentType<string>
    {
        private readonly StringType type;

        private StringArgumentType(StringType type)
        {
            this.type = type;
        }

        public virtual StringType Type => type;

        public virtual ICollection<string> Examples => type.Examples;

        public string Parse(StringReader reader)
        {
            if (type == StringType.GreedyPhrase)
            {
                var text = reader.Remaining;
                reader.Cursor = reader.TotalLength;
                return text;
            }

            if (type == StringType.SingleWord)
                return reader.ReadUnquotedString();
            return reader.ReadString();
        }

        public static StringArgumentType Word()
        {
            return new(StringType.SingleWord);
        }

        public static StringArgumentType String()
        {
            return new(StringType.QuotablePhrase);
        }

        public static StringArgumentType GreedyString()
        {
            return new(StringType.GreedyPhrase);
        }

        public static string GetString<T1>(CommandContext<T1> context, string name)
        {
            return context.GetArgument<string>(name, typeof(string));
        }

        public override string ToString()
        {
            return "string()";
        }

        public static string EscapeIfRequired(string input)
        {
            foreach (var c in input)
                if (!StringReader.IsAllowedInUnquotedString(c))
                    return Escape(input);
            return input;
        }

        private static string Escape(string input)
        {
            var result = new StringBuilder("\"");

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '\\' || c == '"') result.Append('\\');
                result.Append(c);
            }

            result.Append("\"");
            return result.ToString();
        }

        public sealed class StringType
        {
            public enum InnerEnum
            {
                SingleWord,
                QuotablePhrase,
                GreedyPhrase
            }

            public static readonly StringType SingleWord = new("SingleWord", InnerEnum.SingleWord, "word",
                "words_with_underscores");

            public static readonly StringType QuotablePhrase = new("QuotablePhrase", InnerEnum.QuotablePhrase,
                "\"quoted phrase\"", "word", "\"\"");

            public static readonly StringType GreedyPhrase = new("GreedyPhrase", InnerEnum.GreedyPhrase, "word",
                "words with spaces", "\"and symbols\"");

            private static readonly List<StringType> valueList = new();
            private static int nextOrdinal;

            internal readonly ICollection<string> examples;

            public readonly InnerEnum innerEnumValue;
            private readonly string nameValue;
            private readonly int ordinalValue;

            static StringType()
            {
                valueList.Add(SingleWord);
                valueList.Add(QuotablePhrase);
                valueList.Add(GreedyPhrase);
            }

            internal StringType(string name, InnerEnum innerEnum, params string[] examples)
            {
                this.examples = examples.ToList();

                nameValue = name;
                ordinalValue = nextOrdinal++;
                innerEnumValue = innerEnum;
            }

            public ICollection<string> Examples => examples;

            public static StringType[] values()
            {
                return valueList.ToArray();
            }

            public int ordinal()
            {
                return ordinalValue;
            }

            public override string ToString()
            {
                return nameValue;
            }

            public static StringType valueOf(string name)
            {
                foreach (var enumInstance in valueList)
                    if (enumInstance.nameValue == name)
                        return enumInstance;
                throw new ArgumentException(name);
            }
        }
    }
}