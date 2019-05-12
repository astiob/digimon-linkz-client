using System;

namespace System.Xml.XPath
{
	internal class ExprGT : RelationalExpr
	{
		public ExprGT(Expression left, Expression right) : base(left, right)
		{
		}

		protected override string Operator
		{
			get
			{
				return ">";
			}
		}

		public override bool Compare(double arg1, double arg2)
		{
			return arg1 > arg2;
		}
	}
}
