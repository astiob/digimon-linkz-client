using System;

namespace System.Xml.XPath
{
	internal abstract class ExprNumeric : ExprBinary
	{
		public ExprNumeric(Expression left, Expression right) : base(left, right)
		{
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		public override Expression Optimize()
		{
			base.Optimize();
			return this.HasStaticValue ? new ExprNumber(this.StaticValueAsNumber) : this;
		}

		public override object Evaluate(BaseIterator iter)
		{
			return this.EvaluateNumber(iter);
		}
	}
}
