using System;

namespace System.IO
{
	internal class SearchPattern2
	{
		private SearchPattern2.Op ops;

		private bool ignore;

		private bool hasWildcard;

		private string pattern;

		internal static readonly char[] WildcardChars = new char[]
		{
			'*',
			'?'
		};

		internal static readonly char[] InvalidChars = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		};

		public SearchPattern2(string pattern) : this(pattern, false)
		{
		}

		public SearchPattern2(string pattern, bool ignore)
		{
			this.ignore = ignore;
			this.pattern = pattern;
			this.Compile(pattern);
		}

		public bool IsMatch(string text, bool ignorecase)
		{
			if (this.hasWildcard)
			{
				return this.Match(this.ops, text, 0);
			}
			bool flag = string.Compare(this.pattern, text, ignorecase) == 0;
			if (flag)
			{
				return true;
			}
			int num = text.LastIndexOf('/');
			if (num == -1)
			{
				return false;
			}
			num++;
			return num != text.Length && string.Compare(this.pattern, text.Substring(num), ignorecase) == 0;
		}

		public bool IsMatch(string text)
		{
			return this.IsMatch(text, this.ignore);
		}

		public bool HasWildcard
		{
			get
			{
				return this.hasWildcard;
			}
		}

		private void Compile(string pattern)
		{
			if (pattern == null || pattern.IndexOfAny(SearchPattern2.InvalidChars) >= 0)
			{
				throw new ArgumentException("Invalid search pattern: '" + pattern + "'");
			}
			if (pattern == "*")
			{
				this.ops = new SearchPattern2.Op(SearchPattern2.OpCode.True);
				this.hasWildcard = true;
				return;
			}
			this.ops = null;
			int i = 0;
			SearchPattern2.Op op = null;
			while (i < pattern.Length)
			{
				char c = pattern[i];
				SearchPattern2.Op op2;
				if (c != '*')
				{
					if (c != '?')
					{
						op2 = new SearchPattern2.Op(SearchPattern2.OpCode.ExactString);
						int num = pattern.IndexOfAny(SearchPattern2.WildcardChars, i);
						if (num < 0)
						{
							num = pattern.Length;
						}
						op2.Argument = pattern.Substring(i, num - i);
						if (this.ignore)
						{
							op2.Argument = op2.Argument.ToLower();
						}
						i = num;
					}
					else
					{
						op2 = new SearchPattern2.Op(SearchPattern2.OpCode.AnyChar);
						i++;
						this.hasWildcard = true;
					}
				}
				else
				{
					op2 = new SearchPattern2.Op(SearchPattern2.OpCode.AnyString);
					i++;
					this.hasWildcard = true;
				}
				if (op == null)
				{
					this.ops = op2;
				}
				else
				{
					op.Next = op2;
				}
				op = op2;
			}
			if (op == null)
			{
				this.ops = new SearchPattern2.Op(SearchPattern2.OpCode.End);
			}
			else
			{
				op.Next = new SearchPattern2.Op(SearchPattern2.OpCode.End);
			}
		}

		private bool Match(SearchPattern2.Op op, string text, int ptr)
		{
			while (op != null)
			{
				switch (op.Code)
				{
				case SearchPattern2.OpCode.ExactString:
				{
					int length = op.Argument.Length;
					if (ptr + length > text.Length)
					{
						return false;
					}
					string text2 = text.Substring(ptr, length);
					if (this.ignore)
					{
						text2 = text2.ToLower();
					}
					if (text2 != op.Argument)
					{
						return false;
					}
					ptr += length;
					break;
				}
				case SearchPattern2.OpCode.AnyChar:
					if (++ptr > text.Length)
					{
						return false;
					}
					break;
				case SearchPattern2.OpCode.AnyString:
					while (ptr <= text.Length)
					{
						if (this.Match(op.Next, text, ptr))
						{
							return true;
						}
						ptr++;
					}
					return false;
				case SearchPattern2.OpCode.End:
					return ptr == text.Length;
				case SearchPattern2.OpCode.True:
					return true;
				}
				op = op.Next;
			}
			return true;
		}

		private class Op
		{
			public SearchPattern2.OpCode Code;

			public string Argument;

			public SearchPattern2.Op Next;

			public Op(SearchPattern2.OpCode code)
			{
				this.Code = code;
				this.Argument = null;
				this.Next = null;
			}
		}

		private enum OpCode
		{
			ExactString,
			AnyChar,
			AnyString,
			End,
			True
		}
	}
}
