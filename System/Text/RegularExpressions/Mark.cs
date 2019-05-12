using System;

namespace System.Text.RegularExpressions
{
	internal struct Mark
	{
		public int Start;

		public int End;

		public int Previous;

		public bool IsDefined
		{
			get
			{
				return this.Start >= 0 && this.End >= 0;
			}
		}

		public int Index
		{
			get
			{
				return (this.Start >= this.End) ? this.End : this.Start;
			}
		}

		public int Length
		{
			get
			{
				return (this.Start >= this.End) ? (this.Start - this.End) : (this.End - this.Start);
			}
		}
	}
}
