﻿using System;
using System.Collections.Generic;
using System.Text;
using NBrigadier.CommandSuggestion;
using NBrigadier.Context;
using NBrigadier.Helpers;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
    public class StringArgumentType : IArgumentType<string>
    {
        private readonly StringType _type;

        private StringArgumentType(StringType type)
        {
            _type = type;
        }

        public virtual StringType Type => _type;

        public virtual string Parse(StringReader reader)
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

        public virtual ICollection<string> Examples => _type.Examples;

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

            public static readonly StringType SingleWord = new("SINGLE_WORD", InnerEnum.SingleWord, "word",
                "words_with_underscores");

            public static readonly StringType QuotablePhrase = new("QUOTABLE_PHRASE", InnerEnum.QuotablePhrase,
                "\"quoted phrase\"", "word", "\"\"");

            public static readonly StringType GreedyPhrase = new("GREEDY_PHRASE", InnerEnum.GreedyPhrase, "word",
                "words with spaces", "\"and symbols\"");

            private static readonly List<StringType> VALUE_LIST = new();
            private static int _nextOrdinal;
            private readonly string _nameValue;
            private readonly int _ordinalValue;

            public readonly InnerEnum innerEnumValue;

            internal ICollection<string> examples;

            static StringType()
            {
                VALUE_LIST.Add(SingleWord);
                VALUE_LIST.Add(QuotablePhrase);
                VALUE_LIST.Add(GreedyPhrase);
            }

            internal StringType(string name, InnerEnum innerEnum, params string[] examples)
            {
                this.examples = CollectionsHelper.AsList(examples);

                _nameValue = name;
                _ordinalValue = _nextOrdinal++;
                innerEnumValue = innerEnum;
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