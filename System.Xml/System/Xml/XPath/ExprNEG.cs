using System;

namespace System.Xml.XPath
{
	internal class ExprNEG : Expression
	{
		private Expression _expr;

		public ExprNEG(Expression expr)
		{
			this._expr = expr;
		}

		public override string ToString()
		{
			return "- " + this._expr.ToString();
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
			this._expr = this._expr.Optimize();
			return this.HasStaticValue ? new ExprNumber(this.StaticValueAsNumber) : this;
		}

		internal override bool Peer
		{
			get
			{
				return this._expr.Peer;
			}
		}

		public override bool HasStaticValue
		{
			get
			{
				return this._expr.HasStaticValue;
			}
		}

		public override double StaticValueAsNumber
		{
			get
			{
				return (!this._expr.HasStaticValue) ? 0.0 : (-1.0 * this._expr.StaticValueAsNumber);
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			return -this._expr.EvaluateNumber(iter);
		}

		public override double EvaluateNumber(BaseIterator iter)
		{
			return -this._expr.EvaluateNumber(iter);
		}

		internal override bool IsPositional
		{
			get
			{
				return this._expr.IsPositional;
			}
		}
	}
}
