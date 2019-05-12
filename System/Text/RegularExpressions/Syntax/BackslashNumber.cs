using System;
using System.Collections;

namespace System.Text.RegularExpressions.Syntax
{
	internal class BackslashNumber : Reference
	{
		private string literal;

		private bool ecma;

		public BackslashNumber(bool ignore, bool ecma) : base(ignore)
		{
			this.ecma = ecma;
		}

		public bool ResolveReference(string num_str, Hashtable groups)
		{
			if (this.ecma)
			{
				int num = 0;
				for (int i = 1; i < num_str.Length; i++)
				{
					if (groups[num_str.Substring(0, i)] != null)
					{
						num = i;
					}
				}
				if (num != 0)
				{
					base.CapturingGroup = (CapturingGroup)groups[num_str.Substring(0, num)];
					this.literal = num_str.Substring(num);
					return true;
				}
			}
			else if (num_str.Length == 1)
			{
				return false;
			}
			int num2 = 0;
			int num3 = Parser.ParseOctal(num_str, ref num2);
			if (num3 == -1)
			{
				return false;
			}
			if (num3 > 255 && this.ecma)
			{
				num3 /= 8;
				num2--;
			}
			num3 &= 255;
			this.literal = (char)num3 + num_str.Substring(num2);
			return true;
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			if (base.CapturingGroup != null)
			{
				base.Compile(cmp, reverse);
			}
			if (this.literal != null)
			{
				Literal.CompileLiteral(this.literal, cmp, base.IgnoreCase, reverse);
			}
		}
	}
}
