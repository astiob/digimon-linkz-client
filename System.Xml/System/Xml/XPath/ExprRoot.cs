using System;

namespace System.Xml.XPath
{
	internal class ExprRoot : NodeSet
	{
		public override string ToString()
		{
			return string.Empty;
		}

		public override object Evaluate(BaseIterator iter)
		{
			if (iter.CurrentPosition == 0)
			{
				iter = (BaseIterator)iter.Clone();
				iter.MoveNext();
			}
			XPathNavigator xpathNavigator = iter.Current.Clone();
			xpathNavigator.MoveToRoot();
			return new SelfIterator(xpathNavigator, iter.NamespaceManager);
		}

		internal override XPathNodeType EvaluatedNodeType
		{
			get
			{
				return XPathNodeType.Root;
			}
		}

		internal override bool Peer
		{
			get
			{
				return true;
			}
		}

		internal override bool Subtree
		{
			get
			{
				return false;
			}
		}
	}
}
