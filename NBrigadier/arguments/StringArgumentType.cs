using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBrigadier.Context;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class StringArgumentType : IArgumentType<string>
    {
        private readonly StringType _type;

        private StringArgumentType(StringType type)
        {
            this._type = type;
        }

        public virtual StringType Type => _type;

        public virtual ICollection<string> Examples => _type.Examples;

        public string Parse(StringReader reader)
        {
            if (_type == StringType.GreedyPhrase)
            {
                var text = reader.Remaining;
                reader.Cursor = reader.TotalLength;
                return text;
            }

            if (_type == StringType.SingleWord)
                return reader.ReadUnquotedString();
            return reader.ReadString();
        }

        public Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public IList<string> GetExamples()
        {
            return new List<string>();
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

            private static readonly List<StringType> VALUE_LIST = new();
            private static int _nextOrdinal;

            internal readonly ICollection<string> examples;

            public readonly InnerEnum InnerEnumValue;
            private readonly string _nameValue;
            private readonly int _ordinalValue;

            static StringType()
            {
                VALUE_LIST.Add(SingleWord);
                VALUE_LIST.Add(QuotablePhrase);
                VALUE_LIST.Add(GreedyPhrase);
            }

            internal StringType(string name, InnerEnum innerEnum, params string[] examples)
            {
                this.examples = examples.ToList();

                _nameValue = name;
                _ordinalValue = _nextOrdinal++;
                InnerEnumValue = innerEnum;
            }

            public ICollection<string> Examples => examples;

            public static StringType[] Values()
            {
                return VALUE_LIST.ToArray();
            }

            public int Ordinal()
            {
                return _ordinalValue;
            }

            public override string ToString()
            {
                return _nameValue;
            }

            public static StringType ValueOf(string name)
            {
                foreach (var enumInstance in VALUE_LIST)
                    if (enumInstance._nameValue == name)
                        return enumInstance;
                throw new ArgumentException(name);
            }
        }
    }
}