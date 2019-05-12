using System;

namespace System.Xml.XPath
{
	internal class FollowingSiblingIterator : SimpleIterator
	{
		public FollowingSiblingIterator(BaseIterator iter) : base(iter)
		{
		}

		private FollowingSiblingIterator(FollowingSiblingIterator other) : base(other, true)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new FollowingSiblingIterator(this);
		}

		public override bool MoveNextCore()
		{
			XPathNodeType nodeType = this._nav.NodeType;
			return nodeType != XPathNodeType.Attribute && nodeType != XPathNodeType.Namespace && this._nav.MoveToNext();
		}
	}
}
