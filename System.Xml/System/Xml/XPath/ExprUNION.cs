using System;

namespace System.Xml.XPath
{
	internal class ExprUNION : NodeSet
	{
		internal Expression left;

		internal Expression right;

		public ExprUNION(Expression left, Expression right)
		{
			this.left = left;
			this.right = right;
		}

		public override Expression Optimize()
		{
			this.left = this.left.Optimize();
			this.right = this.right.Optimize();
			return this;
		}

		public override string ToString()
		{
			return this.left.ToString() + " | " + this.right.ToString();
		}

		public override object Evaluate(BaseIterator iter)
		{
			BaseIterator baseIterator = this.left.EvaluateNodeSet(iter);
			BaseIterator baseIterator2 = this.right.EvaluateNodeSet(iter);
			return new UnionIterator(iter, baseIterator, baseIterator2);
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return (this.left.EvaluatedNodeType != this.right.EvaluatedNodeType) ? XPathNodeType.All : this.left.EvaluatedNodeType;
			}
		}

		internal override bool IsPositional
		{
			get
			{
				return this.left.IsPositional || this.right.IsPositional;
			}
		}

		internal override bool Peer
		{
			get
			{
				return this.left.Peer && this.right.Peer;
			}
		}

		internal override bool Subtree
		{
			get
			{
				NodeSet nodeSet = this.left as NodeSet;
				NodeSet nodeSet2 = this.right as NodeSet;
				return nodeSet != null && nodeSet2 != null && nodeSet.Subtree && nodeSet2.Subtree;
			}
		}
	}
}
