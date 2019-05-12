using System;

namespace System.Xml.XPath
{
	internal class BooleanConstant : Expression
	{
		private bool _value;

		public BooleanConstant(bool value)
		{
			this._value = value;
		}

		public override string ToString()
		{
			return (!this._value) ? "false()" : "true()";
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Boolean;
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

		public override bool StaticValueAsBoolean
		{
			get
			{
				return this._value;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			return this._value;
		}

		public override bool EvaluateBoolean(BaseIterator iter)
		{
			return this._value;
		}
	}
}
