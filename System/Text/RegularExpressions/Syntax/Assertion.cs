using System;

namespace System.Text.RegularExpressions.Syntax
{
	internal abstract class Assertion : CompositeExpression
	{
		public Assertion()
		{
			base.Expressions.Add(null);
			base.Expressions.Add(null);
		}

		public Expression TrueExpression
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

		public Expression FalseExpression
		{
			get
			{
				return base.Expressions[1];
			}
			set
			{
				base.Expressions[1] = value;
			}
		}

		public override void GetWidth(out int min, out int max)
		{
			base.GetWidth(out min, out max, 2);
			if (this.TrueExpression == null || this.FalseExpression == null)
			{
				min = 0;
			}
		}
	}
}
