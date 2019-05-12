using System;

namespace System.Xml.XPath
{
	internal class ExprNumber : Expression
	{
		protected double _value;

		public ExprNumber(double value)
		{
			this._value = value;
		}

		public override string ToString()
		{
			return this._value.ToString();
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		internal override bool Peer
		{
			get
			{
				return true;
			}
		}

		public override bool HasStaticValue
		{
			get
			{
				return true;
			}
		}

		public override double StaticValueAsNumber
		{
			get
			{
				return XPathFunctions.ToNumber(this._value);
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			return this._value;
		}

		public override double EvaluateNumber(BaseIterator iter)
		{
			return this._value;
		}

		internal override bool IsPositional
		{
			get
			{
				return false;
			}
		}
	}
}
