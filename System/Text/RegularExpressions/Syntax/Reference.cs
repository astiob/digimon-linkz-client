using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Reference : Expression
	{
		private CapturingGroup group;

		private bool ignore;

		public Reference(bool ignore)
		{
			this.ignore = ignore;
		}

		public CapturingGroup CapturingGroup
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
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

		public override void Compile(ICompiler cmp, bool reverse)
		{
			cmp.EmitReference(this.group.Index, this.ignore, reverse);
		}

		public override void GetWidth(out int min, out int max)
		{
			min = 0;
			max = int.MaxValue;
		}

		public override bool IsComplex()
		{
			return true;
		}
	}
}
