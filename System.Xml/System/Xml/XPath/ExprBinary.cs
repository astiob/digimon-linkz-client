using System;

namespace System.Xml.XPath
{
	internal abstract class ExprBinary : Expression
	{
		protected Expression _left;

		protected Expression _right;

		public ExprBinary(Expression left, Expression right)
		{
			this._left = left;
			this._right = right;
		}

		public override Expression Optimize()
		{
			this._left = this._left.Optimize();
			this._right = this._right.Optimize();
			return this;
		}

		public override bool HasStaticValue
		{
			get
			{
				return this._left.HasStaticValue && this._right.HasStaticValue;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this._left.ToString(),
				' ',
				this.Operator,
				' ',
				this._right.ToString()
			});
		}

		protected abstract string Operator { get; }

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				if (this._left.EvaluatedNodeType == this._right.EvaluatedNodeType)
				{
					return this._left.EvaluatedNodeType;
				}
				return XPathNodeType.All;
			}
		}

		internal override bool IsPositional
		{
			get
			{
				return this._left.IsPositional || this._right.IsPositional;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this._left.Peer && this._right.Peer;
			}
		}
	}
}
