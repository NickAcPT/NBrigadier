using System;
using System.Collections.Generic;
using System.Text;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Suggestion;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;

    public class StringArgumentType : IArgumentType<string>
	{
		private StringType _type;

		private StringArgumentType(StringType type)
		{
			this._type = type;
		}

		public static StringArgumentType Word()
		{
			return new StringArgumentType(StringType.SingleWord);
		}

		public static StringArgumentType String()
		{
			return new StringArgumentType(StringType.QuotablePhrase);
		}

		public static StringArgumentType GreedyString()
		{
			return new StringArgumentType(StringType.GreedyPhrase);
		}

		public static string GetString<T1>(CommandContext<T1> context, string name)
		{
			return context.GetArgument<string>(name, typeof(string));
		}

		public virtual StringType Type
		{
			get
			{
				return _type;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public String parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string Parse(StringReader reader)
		{
			if (_type == StringType.GreedyPhrase)
			{
				 string text = reader.Remaining;
				reader.Cursor = reader.TotalLength;
				return text;
			}
			else if (_type == StringType.SingleWord)
			{
				return reader.ReadUnquotedString();
			}
			else
			{
				return reader.ReadString();
			}
		}

        public Func<Suggestions> ListSuggestions<TS>(CommandContext<TS> context, SuggestionsBuilder builder)
        {
            return Suggestions.Empty();
        }

        public override string ToString()
		{
			return "string()";
		}

		public virtual ICollection<string> Examples
		{
			get
			{
				return _type.Examples;
			}
		}

		public static string EscapeIfRequired(string input)
		{
			foreach (char c in input.ToCharArray())
			{
				if (!StringReader.IsAllowedInUnquotedString(c))
				{
					return Escape(input);
				}
			}
			return input;
		}

		private static string Escape(string input)
		{
			 StringBuilder result = new StringBuilder("\"");

			for (int i = 0; i < input.Length; i++)
			{
				 char c = input[i];
				if (c == '\\' || c == '"')
				{
					result.Append('\\');
				}
				result.Append(c);
			}

			result.Append("\"");
			return result.ToString();
		}

		public sealed class StringType
		{
			public static readonly StringType SingleWord = new StringType("SINGLE_WORD", InnerEnum.SingleWord, "word", "words_with_underscores");
			public static readonly StringType QuotablePhrase = new StringType("QUOTABLE_PHRASE", InnerEnum.QuotablePhrase, "\"quoted phrase\"", "word", "\"\"");
			public static readonly StringType GreedyPhrase = new StringType("GREEDY_PHRASE", InnerEnum.GreedyPhrase, "word", "words with spaces", "\"and symbols\"");

			private static readonly List<StringType> VALUE_LIST = new List<StringType>();

			static StringType()
			{
				VALUE_LIST.Add(SingleWord);
				VALUE_LIST.Add(QuotablePhrase);
				VALUE_LIST.Add(GreedyPhrase);
			}

			public enum InnerEnum
			{
				SingleWord,
				QuotablePhrase,
				GreedyPhrase
			}

			public readonly InnerEnum innerEnumValue;
			private readonly string _nameValue;
			private readonly int _ordinalValue;
			private static int _nextOrdinal = 0;

			internal ICollection<string> examples;

			internal StringType(string name, InnerEnum innerEnum, params string[] examples)
			{
				this.examples = CollectionsHelper.AsList(examples);

				_nameValue = name;
				_ordinalValue = _nextOrdinal++;
				innerEnumValue = innerEnum;
			}

			public ICollection<string> Examples
			{
				get
				{
					return examples;
				}
			}

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
				foreach (StringType enumInstance in StringType.VALUE_LIST)
				{
					if (enumInstance._nameValue == name)
					{
						return enumInstance;
					}
				}
				throw new System.ArgumentException(name);
			}
		}
	}

}