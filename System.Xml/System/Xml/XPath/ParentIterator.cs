using System;

namespace System.Xml.XPath
{
	internal class ParentIterator : SimpleIterator
	{
		private bool canMove;

		public ParentIterator(BaseIterator iter) : base(iter)
		{
			this.canMove = this._nav.MoveToParent();
		}

		private ParentIterator(ParentIterator other, bool dummy) : base(other, true)
		{
			this.canMove = other.canMove;
		}

		public ParentIterator(XPathNavigator nav, IXmlNamespaceResolver nsm) : base(nav, nsm)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new ParentIterator(this, true);
		}

		public override bool MoveNextCore()
		{
			if (!this.canMove)
			{
				return false;
			}
			this.canMove = false;
			return true;
		}
	}
}
