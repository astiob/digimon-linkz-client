using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal class Repetition : CompositeExpression
	{
		private int min;

		private int max;

		private bool lazy;

		public Repetition(int min, int max, bool lazy)
		{
			base.Expressions.Add(null);
			this.min = min;
			this.max = max;
			this.lazy = lazy;
		}

		public Expression Expression
		{
			get
			{
				return base.Expressions[0];
			}
			set
			{
				base.Expressions[0] = value;
			}
		}

		public int Minimum
		{
			get
			{
				return this.min;
			}
			set
			{
				this.min = value;
			}
		}

		public int Maximum
		{
			get
			{
				return this.max;
			}
			set
			{
				this.max = value;
			}
		}

		public bool Lazy
		{
			get
			{
				return this.lazy;
			}
			set
			{
				this.lazy = value;
			}
		}

		public override void Compile(ICompiler cmp, bool reverse)
		{
			if (this.Expression.IsComplex())
			{
				LinkRef linkRef = cmp.NewLink();
				cmp.EmitRepeat(this.min, this.max, this.lazy, linkRef);
				this.Expression.Compile(cmp, reverse);
				cmp.EmitUntil(linkRef);
			}
			else
			{
				LinkRef linkRef2 = cmp.NewLink();
				cmp.EmitFastRepeat(this.min, this.max, this.lazy, linkRef2);
				this.Expression.Compile(cmp, reverse);
				cmp.EmitTrue();
				cmp.ResolveLink(linkRef2);
			}
		}

		public override void GetWidth(out int min, out int max)
		{
			this.Expression.GetWidth(out min, out max);
			min *= this.min;
			if (max == 2147483647 || this.max == 65535)
			{
				max = int.MaxValue;
			}
			else
			{
				max *= this.max;
			}
		}

		public override AnchorInfo GetAnchorInfo(bool reverse)
		{
			int fixedWidth = base.GetFixedWidth();
			if (this.Minimum == 0)
			{
				return new AnchorInfo(this, fixedWidth);
			}
			AnchorInfo anchorInfo = this.Expression.GetAnchorInfo(reverse);
			if (anchorInfo.IsPosition)
			{
				return new AnchorInfo(this, anchorInfo.Offset, fixedWidth, anchorInfo.Position);
			}
			if (!anchorInfo.IsSubstring)
			{
				return new AnchorInfo(this, fixedWidth);
			}
			if (anchorInfo.IsComplete)
			{
				string substring = anchorInfo.Substring;
				StringBuilder stringBuilder = new StringBuilder(substring);
				for (int i = 1; i < this.Minimum; i++)
				{
					stringBuilder.Append(substring);
				}
				return new AnchorInfo(this, 0, fixedWidth, stringBuilder.ToString(), anchorInfo.IgnoreCase);
			}
			return new AnchorInfo(this, anchorInfo.Offset, fixedWidth, anchorInfo.Substring, anchorInfo.IgnoreCase);
		}
	}
}
