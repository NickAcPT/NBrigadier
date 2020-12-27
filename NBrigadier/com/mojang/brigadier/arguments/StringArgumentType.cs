using NBrigadier;
using NBrigadier.Helpers;
using System.Linq;
using System.Collections.Generic;
using System.Text;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace com.mojang.brigadier.arguments
{
	using StringReader = com.mojang.brigadier.StringReader;
	using CommandSyntaxException = com.mojang.brigadier.exceptions.CommandSyntaxException;

	public class StringArgumentType : ArgumentType<string>
	{
		private StringType type;

		private StringArgumentType(StringType type)
		{
			this.type = type;
		}

		public static StringArgumentType word()
		{
			return new StringArgumentType(StringType.SINGLE_WORD);
		}

		public static StringArgumentType @string()
		{
			return new StringArgumentType(StringType.QUOTABLE_PHRASE);
		}

		public static StringArgumentType greedyString()
		{
			return new StringArgumentType(StringType.GREEDY_PHRASE);
		}

		public static string getString<T1>(com.mojang.brigadier.context.CommandContext<T1> context, string name)
		{
			return context.getArgument(name, typeof(string));
		}

		public virtual StringType Type
		{
			get
			{
				return type;
			}
		}

// WARNING: Method 'throws' clauses are not available in C#:
// ORIGINAL LINE: @Override public String parse(com.mojang.brigadier.StringReader reader) throws com.mojang.brigadier.exceptions.CommandSyntaxException
		public virtual string parse(StringReader reader)
		{
			if (type == StringType.GREEDY_PHRASE)
			{
				 string text = reader.Remaining;
				reader.Cursor = reader.TotalLength;
				return text;
			}
			else if (type == StringType.SINGLE_WORD)
			{
				return reader.readUnquotedString();
			}
			else
			{
				return reader.readString();
			}
		}

		public override string ToString()
		{
			return "string()";
		}

		public virtual ICollection<string> Examples
		{
			get
			{
				return type.Examples;
			}
		}

		public static string escapeIfRequired(string input)
		{
			foreach (char c in input.ToCharArray())
			{
				if (!StringReader.isAllowedInUnquotedString(c))
				{
					return escape(input);
				}
			}
			return input;
		}

		private static string escape(string input)
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
			public static readonly StringType SINGLE_WORD = new StringType("SINGLE_WORD", InnerEnum.SINGLE_WORD, "word", "words_with_underscores");
			public static readonly StringType QUOTABLE_PHRASE = new StringType("QUOTABLE_PHRASE", InnerEnum.QUOTABLE_PHRASE, "\"quoted phrase\"", "word", "\"\"");
			public static readonly StringType GREEDY_PHRASE = new StringType("GREEDY_PHRASE", InnerEnum.GREEDY_PHRASE, "word", "words with spaces", "\"and symbols\"");

			private static readonly List<StringType> valueList = new List<StringType>();

			static StringType()
			{
				valueList.Add(SINGLE_WORD);
				valueList.Add(QUOTABLE_PHRASE);
				valueList.Add(GREEDY_PHRASE);
			}

			public enum InnerEnum
			{
				SINGLE_WORD,
				QUOTABLE_PHRASE,
				GREEDY_PHRASE
			}

			public readonly InnerEnum innerEnumValue;
			private readonly string nameValue;
			private readonly int ordinalValue;
			private static int nextOrdinal = 0;

			internal ICollection<string> examples;

			internal StringType(string name, InnerEnum innerEnum, params string[] examples)
			{
				this.examples = CollectionsHelper.AsList(examples);

				nameValue = name;
				ordinalValue = nextOrdinal++;
				innerEnumValue = innerEnum;
			}

			public ICollection<string> Examples
			{
				get
				{
					return examples;
				}
			}

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
				foreach (StringType enumInstance in StringType.valueList)
				{
					if (enumInstance.nameValue == name)
					{
						return enumInstance;
					}
				}
				throw new System.ArgumentException(name);
			}
		}
	}

}