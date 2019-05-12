using System;

namespace System.Xml.XPath
{
	internal class ExprEQ : EqualityExpr
	{
		public ExprEQ(Expression left, Expression right) : base(left, right, true)
		{
		}

		protected override string Operator
		{
			get
			{
				return "=";
			}
		}
	}
}
