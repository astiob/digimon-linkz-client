using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class PositionAssertion : Expression
	{
		private Position pos;

		public PositionAssertion(Position pos)
		{
			this.pos = pos;
		}

		public Position Position
		{
			get
			{
				return this.pos;
			}
			set
			{
				this.pos = value;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			cmp.EmitPosition(this.pos);
		}

		public override void GetWidth(out int min, out int max)
		{
			min = (max = 0);
		}

		public override bool IsComplex()
		{
			return false;
		}

		public override AnchorInfo GetAnchorInfo(bool revers)
		{
			switch (this.pos)
			{
			case Position.StartOfString:
			case Position.StartOfLine:
			case Position.StartOfScan:
				return new AnchorInfo(this, 0, 0, this.pos);
			default:
				return new AnchorInfo(this, 0);
			}
		}
	}
}
