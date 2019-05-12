using System;

namespace System.IO
{
	internal class SearchPattern
	{
		private SearchPattern.Op ops;

		private bool ignore;

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

		public SearchPattern(string pattern) : this(pattern, false)
		{
		}

		public SearchPattern(string pattern, bool ignore)
		{
			this.ignore = ignore;
			this.Compile(pattern);
		}

		public bool IsMatch(string text)
		{
			return this.Match(this.ops, text, 0);
		}

		private void Compile(string pattern)
		{
			if (pattern == null || pattern.IndexOfAny(SearchPattern.InvalidChars) >= 0)
			{
				throw new ArgumentException("Invalid search pattern.");
			}
			if (pattern == "*")
			{
				this.ops = new SearchPattern.Op(SearchPattern.OpCode.True);
				return;
			}
			this.ops = null;
			int i = 0;
			SearchPattern.Op op = null;
			while (i < pattern.Length)
			{
				char c = pattern[i];
				SearchPattern.Op op2;
				if (c != '*')
				{
					if (c != '?')
					{
						op2 = new SearchPattern.Op(SearchPattern.OpCode.ExactString);
						int num = pattern.IndexOfAny(SearchPattern.WildcardChars, i);
						if (num < 0)
						{
							num = pattern.Length;
						}
						op2.Argument = pattern.Substring(i, num - i);
						if (this.ignore)
						{
							op2.Argument = op2.Argument.ToLowerInvariant();
						}
						i = num;
					}
					else
					{
						op2 = new SearchPattern.Op(SearchPattern.OpCode.AnyChar);
						i++;
					}
				}
				else
				{
					op2 = new SearchPattern.Op(SearchPattern.OpCode.AnyString);
					i++;
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
				this.ops = new SearchPattern.Op(SearchPattern.OpCode.End);
			}
			else
			{
				op.Next = new SearchPattern.Op(SearchPattern.OpCode.End);
			}
		}

		private bool Match(SearchPattern.Op op, string text, int ptr)
		{
			while (op != null)
			{
				switch (op.Code)
				{
				case SearchPattern.OpCode.ExactString:
				{
					int length = op.Argument.Length;
					if (ptr + length > text.Length)
					{
						return false;
					}
					string text2 = text.Substring(ptr, length);
					if (this.ignore)
					{
						text2 = text2.ToLowerInvariant();
					}
					if (text2 != op.Argument)
					{
						return false;
					}
					ptr += length;
					break;
				}
				case SearchPattern.OpCode.AnyChar:
					if (++ptr > text.Length)
					{
						return false;
					}
					break;
				case SearchPattern.OpCode.AnyString:
					while (ptr <= text.Length)
					{
						if (this.Match(op.Next, text, ptr))
						{
							return true;
						}
						ptr++;
					}
					return false;
				case SearchPattern.OpCode.End:
					return ptr == text.Length;
				case SearchPattern.OpCode.True:
					return true;
				}
				op = op.Next;
			}
			return true;
		}

		private class Op
		{
			public SearchPattern.OpCode Code;

			public string Argument;

			public SearchPattern.Op Next;

			public Op(SearchPattern.OpCode code)
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
