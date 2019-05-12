using System;

namespace System.Xml.XPath
{
	internal class ExprLiteral : Expression
	{
		protected string _value;

		public ExprLiteral(string value)
		{
			this._value = value;
		}

		public string Value
		{
			get
			{
				return this._value;
			}
		}

		public override string ToString()
		{
			return "'" + this._value + "'";
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.String;
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

		public override string StaticValueAsString
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

		public override string EvaluateString(BaseIterator iter)
		{
			return this._value;
		}
	}
}
