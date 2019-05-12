using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal abstract class Expression
	{
		public abstract void Compile(ICompiler cmp, bool reverse);

		public abstract void GetWidth(out int min, out int max);

		public int GetFixedWidth()
		{
			int num;
			int num2;
			this.GetWidth(out num, out num2);
			if (num == num2)
			{
				return num;
			}
			return -1;
		}

		public virtual AnchorInfo GetAnchorInfo(bool reverse)
		{
			return new AnchorInfo(this, this.GetFixedWidth());
		}

		public abstract bool IsComplex();
	}
}
