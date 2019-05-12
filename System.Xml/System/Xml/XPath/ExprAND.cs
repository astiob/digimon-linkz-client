using System;

namespace System.Xml.XPath
{
	internal class ExprAND : ExprBoolean
	{
		public ExprAND(Expression left, Expression right) : base(left, right)
		{
		}

		protected override string Operator
		{
			get
			{
				return "and";
			}
		}

		public override bool StaticValueAsBoolean
		{
			get
			{
				return this.HasStaticValue && this._left.StaticValueAsBoolean && this._right.StaticValueAsBoolean;
			}
		}

		public override bool EvaluateBoolean(BaseIterator iter)
		{
			return this._left.EvaluateBoolean(iter) && this._right.EvaluateBoolean(iter);
		}
	}
}
