using System;

namespace System.Xml.XPath
{
	internal abstract class ExprBoolean : ExprBinary
	{
		public ExprBoolean(Expression left, Expression right) : base(left, right)
		{
		}

		public override Expression Optimize()
		{
			base.Optimize();
			if (!this.HasStaticValue)
			{
				return this;
			}
			if (this.StaticValueAsBoolean)
			{
				return new XPathFunctionTrue(null);
			}
			return new XPathFunctionFalse(null);
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Boolean;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			return this.EvaluateBoolean(iter);
		}

		public override double EvaluateNumber(BaseIterator iter)
		{
			return (double)((!this.EvaluateBoolean(iter)) ? 0 : 1);
		}

		public override string EvaluateString(BaseIterator iter)
		{
			return (!this.EvaluateBoolean(iter)) ? "false" : "true";
		}
	}
}
