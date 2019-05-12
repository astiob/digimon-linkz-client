using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class CapturingGroup : Group, IComparable
	{
		private int gid;

		private string name;

		public CapturingGroup()
		{
			this.gid = 0;
			this.name = null;
		}

		public int Index
		{
			get
			{
				return this.gid;
			}
			set
			{
				this.gid = value;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public bool IsNamed
		{
			get
			{
				return this.name != null;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			cmp.EmitOpen(this.gid);
			base.Compile(cmp, reverse);
			cmp.EmitClose(this.gid);
		}

		public override bool IsComplex()
		{
			return true;
		}

		public int CompareTo(object other)
		{
			return this.gid - ((CapturingGroup)other).gid;
		}
	}
}
