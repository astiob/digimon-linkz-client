using System;

namespace System.Xml.XPath
{
	internal class ExprFilter : NodeSet
	{
		internal Expression expr;

		internal Expression pred;

		public ExprFilter(Expression expr, Expression pred)
		{
			this.expr = expr;
			this.pred = pred;
		}

		public override Expression Optimize()
		{
			this.expr = this.expr.Optimize();
			this.pred = this.pred.Optimize();
			return this;
		}

		internal Expression LeftHandSide
		{
			get
			{
				return this.expr;
			}
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.expr.ToString(),
				")[",
				this.pred.ToString(),
				"]"
			});
		}

		public override object Evaluate(BaseIterator iter)
		{
			BaseIterator iter2 = this.expr.EvaluateNodeSet(iter);
			return new PredicateIterator(iter2, this.pred);
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return this.expr.EvaluatedNodeType;
			}
		}

		internal override bool IsPositional
		{
			get
			{
				return this.pred.ReturnType == XPathResultType.Number || this.expr.IsPositional || this.pred.IsPositional;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.expr.Peer && this.pred.Peer;
			}
		}

		internal override bool Subtree
		{
			get
			{
				NodeSet nodeSet = this.expr as NodeSet;
				return nodeSet != null && nodeSet.Subtree;
			}
		}
	}
}
