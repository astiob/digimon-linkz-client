using System;

namespace System.Xml.XPath
{
	internal abstract class NodeTest : NodeSet
	{
		protected AxisSpecifier _axis;

		public NodeTest(Axes axis)
		{
			this._axis = new AxisSpecifier(axis);
		}

		public abstract bool Match(IXmlNamespaceResolver nsm, XPathNavigator nav);

		public AxisSpecifier Axis
		{
			get
			{
				return this._axis;
			}
		}

		public override object Evaluate(BaseIterator iter)
		{
			BaseIterator iter2 = this._axis.Evaluate(iter);
			return new AxisIterator(iter2, this);
		}

		public abstract void GetInfo(out string name, out string ns, out XPathNodeType nodetype, IXmlNamespaceResolver nsm);

		public override bool RequireSorting
		{
			get
			{
				switch (this._axis.Axis)
				{
				case Axes.Ancestor:
				case Axes.AncestorOrSelf:
				case Axes.Attribute:
				case Axes.Namespace:
				case Axes.Preceding:
				case Axes.PrecedingSibling:
					return true;
				}
				return false;
			}
		}

		internal override bool Peer
		{
			get
			{
				switch (this._axis.Axis)
				{
				case Axes.Ancestor:
				case Axes.AncestorOrSelf:
				case Axes.Descendant:
				case Axes.DescendantOrSelf:
				case Axes.Following:
				case Axes.Preceding:
					return false;
				}
				return true;
			}
		}

		internal override bool Subtree
		{
			get
			{
				switch (this._axis.Axis)
				{
				case Axes.Ancestor:
				case Axes.AncestorOrSelf:
				case Axes.Following:
				case Axes.FollowingSibling:
				case Axes.Parent:
				case Axes.Preceding:
				case Axes.PrecedingSibling:
					return false;
				}
				return true;
			}
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return this._axis.NodeType;
			}
		}
	}
}
