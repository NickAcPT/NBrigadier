using System.Collections.Generic;
using NBrigadier.Context;
using NBrigadier.Exceptions;
using NBrigadier.Helpers;
using NBrigadier.Tree;

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace NBrigadier
{
    using CommandSyntaxException = CommandSyntaxException;


    public class ParseResults<TS>
	{
		private CommandContextBuilder<TS> _context;
		private IDictionary<CommandNode<TS>, CommandSyntaxException> _exceptions;
		private IMmutableStringReader _reader;

		public ParseResults(CommandContextBuilder<TS> context, IMmutableStringReader reader, IDictionary<CommandNode<TS>, CommandSyntaxException> exceptions)
		{
			this._context = context;
			this._reader = reader;
			this._exceptions = exceptions;
		}

		public ParseResults(CommandContextBuilder<TS> context) : this(context, new StringReader(""), CollectionsHelper.EmptyMap<CommandNode<TS>, CommandSyntaxException>())
		{
		}

		public virtual CommandContextBuilder<TS> Context
		{
			get
			{
				return _context;
			}
		}

		public virtual IMmutableStringReader Reader
		{
			get
			{
				return _reader;
			}
		}

		public virtual IDictionary<CommandNode<TS>, CommandSyntaxException> Exceptions
		{
			get
			{
				return _exceptions;
			}
		}
	}

}