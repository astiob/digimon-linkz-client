using System;
using System.Text.RegularExpressions.Syntax;

namespace System.Text.RegularExpressions
{
	internal class ReplacementEvaluator
	{
		private Regex regex;

		private int n_pieces;

		private int[] pieces;

		private string replacement;

		public ReplacementEvaluator(Regex regex, string replacement)
		{
			this.regex = regex;
			this.replacement = replacement;
			this.pieces = null;
			this.n_pieces = 0;
			this.Compile();
		}

		public static string Evaluate(string replacement, Match match)
		{
			ReplacementEvaluator replacementEvaluator = new ReplacementEvaluator(match.Regex, replacement);
			return replacementEvaluator.Evaluate(match);
		}

		public string Evaluate(Match match)
		{
			if (this.n_pieces == 0)
			{
				return this.replacement;
			}
			StringBuilder stringBuilder = new StringBuilder();
			this.EvaluateAppend(match, stringBuilder);
			return stringBuilder.ToString();
		}

		public void EvaluateAppend(Match match, StringBuilder sb)
		{
			if (this.n_pieces == 0)
			{
				sb.Append(this.replacement);
				return;
			}
			int i = 0;
			while (i < this.n_pieces)
			{
				int num = this.pieces[i++];
				if (num >= 0)
				{
					int count = this.pieces[i++];
					sb.Append(this.replacement, num, count);
				}
				else if (num < -3)
				{
					Group group = match.Groups[-(num + 4)];
					sb.Append(group.Text, group.Index, group.Length);
				}
				else if (num == -1)
				{
					sb.Append(match.Text);
				}
				else if (num == -2)
				{
					sb.Append(match.Text, 0, match.Index);
				}
				else
				{
					int num2 = match.Index + match.Length;
					sb.Append(match.Text, num2, match.Text.Length - num2);
				}
			}
		}

		public bool NeedsGroupsOrCaptures
		{
			get
			{
				return this.n_pieces != 0;
			}
		}

		private void Ensure(int size)
		{
			if (this.pieces == null)
			{
				int num = 4;
				if (num < size)
				{
					num = size;
				}
				this.pieces = new int[num];
			}
			else if (size >= this.pieces.Length)
			{
				int num = this.pieces.Length + (this.pieces.Length >> 1);
				if (num < size)
				{
					num = size;
				}
				int[] destinationArray = new int[num];
				Array.Copy(this.pieces, destinationArray, this.n_pieces);
				this.pieces = destinationArray;
			}
		}

		private void AddFromReplacement(int start, int end)
		{
			if (start == end)
			{
				return;
			}
			this.Ensure(this.n_pieces + 2);
			this.pieces[this.n_pieces++] = start;
			this.pieces[this.n_pieces++] = end - start;
		}

		private void AddInt(int i)
		{
			this.Ensure(this.n_pieces + 1);
			this.pieces[this.n_pieces++] = i;
		}

		private void Compile()
		{
			int num = 0;
			int i = 0;
			while (i < this.replacement.Length)
			{
				char c = this.replacement[i++];
				if (c == '$')
				{
					if (i == this.replacement.Length)
					{
						break;
					}
					if (this.replacement[i] == '$')
					{
						this.AddFromReplacement(num, i);
						i = (num = i + 1);
					}
					else
					{
						int end = i - 1;
						int num2 = this.CompileTerm(ref i);
						if (num2 < 0)
						{
							this.AddFromReplacement(num, end);
							this.AddInt(num2);
							num = i;
						}
					}
				}
			}
			if (num != 0)
			{
				this.AddFromReplacement(num, i);
			}
		}

		private int CompileTerm(ref int ptr)
		{
			char c = this.replacement[ptr];
			if (char.IsDigit(c))
			{
				int num = Parser.ParseDecimal(this.replacement, ref ptr);
				if (num < 0 || num > this.regex.GroupCount)
				{
					return 0;
				}
				return -num - 4;
			}
			else
			{
				ptr++;
				char c2 = c;
				switch (c2)
				{
				case '&':
					return -4;
				case '\'':
					return -3;
				default:
				{
					if (c2 == '_')
					{
						return -1;
					}
					if (c2 == '`')
					{
						return -2;
					}
					if (c2 != '{')
					{
						return 0;
					}
					int num2 = -1;
					string text;
					try
					{
						if (char.IsDigit(this.replacement[ptr]))
						{
							num2 = Parser.ParseDecimal(this.replacement, ref ptr);
							text = string.Empty;
						}
						else
						{
							text = Parser.ParseName(this.replacement, ref ptr);
						}
					}
					catch (IndexOutOfRangeException)
					{
						ptr = this.replacement.Length;
						return 0;
					}
					if (ptr == this.replacement.Length || this.replacement[ptr] != '}' || text == null)
					{
						return 0;
					}
					ptr++;
					if (text != string.Empty)
					{
						num2 = this.regex.GroupNumberFromName(text);
					}
					if (num2 < 0 || num2 > this.regex.GroupCount)
					{
						return 0;
					}
					return -num2 - 4;
				}
				case '+':
					return -this.regex.GroupCount - 4;
				}
			}
		}
	}
}
