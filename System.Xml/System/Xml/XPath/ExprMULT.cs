using System;

namespace System.Xml.XPath
{
	internal class ExprMULT : ExprNumeric
	{
		public ExprMULT(Expression left, Expression right) : base(left, right)
		{
		}

		protected override string Operator
		{
			get
			{
				return "*";
			}
		}

		public override double StaticValueAsNumber
		{
			get
			{
				return (!this.HasStaticValue) ? 0.0 : (this._left.StaticValueAsNumber * this._right.StaticValueAsNumber);
			}
		}

		public override double EvaluateNumber(BaseIterator iter)
		{
			return this._left.EvaluateNumber(iter) * this._right.EvaluateNumber(iter);
		}
	}
}
