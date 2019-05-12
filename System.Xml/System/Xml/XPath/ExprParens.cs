using System;

namespace System.Xml.XPath
{
	internal class ExprParens : Expression
	{
		protected Expression _expr;

		public ExprParens(Expression expr)
		{
			this._expr = expr;
		}

		public override Expression Optimize()
		{
			this._expr.Optimize();
			return this;
		}

		public override bool HasStaticValue
		{
			get
			{
				return this._expr.HasStaticValue;
			}
		}

		public override object StaticValue
		{
			get
			{
				return this._expr.StaticValue;
			}
		}

		public override string StaticValueAsString
		{
			get
			{
				return this._expr.StaticValueAsString;
			}
		}

		public override double StaticValueAsNumber
		{
			get
			{
				return this._expr.StaticValueAsNumber;
			}
		}

		public override bool StaticValueAsBoolean
		{
			get
			{
				return this._expr.StaticValueAsBoolean;
			}
		}

		public override string ToString()
		{
			return "(" + this._expr.ToString() + ")";
		}

		public override XPathResultType ReturnType
		{
			get
			{
				return this._expr.ReturnType;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			object obj = this._expr.Evaluate(iter);
			XPathNodeIterator xpathNodeIterator = obj as XPathNodeIterator;
			BaseIterator baseIterator = xpathNodeIterator as BaseIterator;
			if (baseIterator == null && xpathNodeIterator != null)
			{
				baseIterator = new WrapperIterator(xpathNodeIterator, iter.NamespaceManager);
			}
			if (baseIterator != null)
			{
				return new ParensIterator(baseIterator);
			}
			return obj;
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return this._expr.EvaluatedNodeType;
			}
		}

		internal override bool IsPositional
		{
			get
			{
				return this._expr.IsPositional;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this._expr.Peer;
			}
		}
	}
}
