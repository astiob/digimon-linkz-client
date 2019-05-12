using System;

namespace System.Xml.XPath
{
	internal class ExprSLASH2 : NodeSet
	{
		public Expression left;

		public NodeSet right;

		private static NodeTest DescendantOrSelfStar = new NodeTypeTest(Axes.DescendantOrSelf, XPathNodeType.All);

		public ExprSLASH2(Expression left, NodeSet right)
		{
			this.left = left;
			this.right = right;
		}

		public override Expression Optimize()
		{
			this.left = this.left.Optimize();
			this.right = (NodeSet)this.right.Optimize();
			NodeTest nodeTest = this.right as NodeTest;
			if (nodeTest != null && nodeTest.Axis.Axis == Axes.Child)
			{
				NodeNameTest nodeNameTest = nodeTest as NodeNameTest;
				if (nodeNameTest != null)
				{
					return new ExprSLASH(this.left, new NodeNameTest(nodeNameTest, Axes.Descendant));
				}
				NodeTypeTest nodeTypeTest = nodeTest as NodeTypeTest;
				if (nodeTypeTest != null)
				{
					return new ExprSLASH(this.left, new NodeTypeTest(nodeTypeTest, Axes.Descendant));
				}
			}
			return this;
		}

		public override string ToString()
		{
			return this.left.ToString() + "//" + this.right.ToString();
		}

		public override object Evaluate(BaseIterator iter)
		{
			BaseIterator iter2 = this.left.EvaluateNodeSet(iter);
			if (this.left.Peer && !this.left.RequireSorting)
			{
				iter2 = new SimpleSlashIterator(iter2, ExprSLASH2.DescendantOrSelfStar);
			}
			else
			{
				BaseIterator baseIterator = new SlashIterator(iter2, ExprSLASH2.DescendantOrSelfStar);
				iter2 = ((!this.left.RequireSorting) ? baseIterator : new SortedIterator(baseIterator));
			}
			SlashIterator iter3 = new SlashIterator(iter2, this.right);
			return new SortedIterator(iter3);
		}

		public override bool RequireSorting
		{
			get
			{
				return this.left.RequireSorting || this.right.RequireSorting;
			}
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return this.right.EvaluatedNodeType;
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
				return false;
			}
		}

		internal override bool Subtree
		{
			get
			{
				NodeSet nodeSet = this.left as NodeSet;
				return nodeSet != null && nodeSet.Subtree && this.right.Subtree;
			}
		}
	}
}
