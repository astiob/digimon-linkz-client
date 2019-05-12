using System;

namespace System.Xml.XPath
{
	internal class ExprNE : EqualityExpr
	{
		public ExprNE(Expression left, Expression right) : base(left, right, false)
		{
		}

		protected override string Operator
		{
			get
			{
				return "!=";
			}
		}
	}
}
