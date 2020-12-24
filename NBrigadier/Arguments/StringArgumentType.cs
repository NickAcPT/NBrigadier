using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBrigadier.Context;
using NBrigadier.Exceptions;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier.Arguments
{
	using StringReader = StringReader;


    public class StringArgumentType : ArgumentType<string>
	{
		private readonly StringType type;

		private StringArgumentType(StringType type)
		{
			this.type = type;
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
				return type;
			}
		}

		public string Parse(StringReader reader)
		{
			if (type == StringType.GreedyPhrase)
			{
				string text = reader.Remaining;
				reader.Cursor = reader.TotalLength;
				return text;
			}
			else if (type == StringType.SingleWord)
			{
				return reader.ReadUnquotedString();
			}
			else
			{
				return reader.ReadString();
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
			public static readonly StringType SingleWord = new StringType("SingleWord", InnerEnum.SingleWord, "word", "words_with_underscores");
			public static readonly StringType QuotablePhrase = new StringType("QuotablePhrase", InnerEnum.QuotablePhrase, "\"quoted phrase\"", "word", "\"\"");
			public static readonly StringType GreedyPhrase = new StringType("GreedyPhrase", InnerEnum.GreedyPhrase, "word", "words with spaces", "\"and symbols\"");

			private static readonly List<StringType> valueList = new List<StringType>();

			static StringType()
			{
				valueList.Add(SingleWord);
				valueList.Add(QuotablePhrase);
				valueList.Add(GreedyPhrase);
			}

			public enum InnerEnum
			{
				SingleWord,
				QuotablePhrase,
				GreedyPhrase,
                
			}

			public readonly InnerEnum innerEnumValue;
			private readonly string nameValue;
			private readonly int ordinalValue;
			private static int nextOrdinal = 0;

			internal readonly ICollection<string> examples;

			internal StringType(string name, InnerEnum innerEnum, params string[] examples)
			{
				this.examples = (examples).ToList();

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