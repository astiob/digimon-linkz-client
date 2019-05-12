using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Literal : Expression
	{
		private string str;

		private bool ignore;

		public Literal(string str, bool ignore)
		{
			this.str = str;
			this.ignore = ignore;
		}

		public string String
		{
			get
			{
				return this.str;
			}
			set
			{
				this.str = value;
			}
		}

		public bool IgnoreCase
		{
			get
			{
				return this.ignore;
			}
			set
			{
				this.ignore = value;
			}
		}

		public static void CompileLiteral(string str, ICompiler cmp, bool ignore, bool reverse)
		{
			if (str.Length == 0)
			{
				return;
			}
			if (str.Length == 1)
			{
				cmp.EmitCharacter(str[0], false, ignore, reverse);
			}
			else
			{
				cmp.EmitString(str, ignore, reverse);
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			Literal.CompileLiteral(this.str, cmp, this.ignore, reverse);
		}

		public override void GetWidth(out int min, out int max)
		{
			min = (max = this.str.Length);
		}

		public override AnchorInfo GetAnchorInfo(bool reverse)
		{
			return new AnchorInfo(this, 0, this.str.Length, this.str, this.ignore);
		}

		public override bool IsComplex()
		{
			return false;
		}
	}
}
