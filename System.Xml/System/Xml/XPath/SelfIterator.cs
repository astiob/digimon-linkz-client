using System;

namespace System.Xml.XPath
{
	internal class SelfIterator : SimpleIterator
	{
		public SelfIterator(BaseIterator iter) : base(iter)
		{
		}

		public SelfIterator(XPathNavigator nav, IXmlNamespaceResolver nsm) : base(nav, nsm)
		{
		}

		protected SelfIterator(SelfIterator other, bool clone) : base(other, true)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new SelfIterator(this, true);
		}

		public override bool MoveNextCore()
		{
			return this.CurrentPosition == 0;
		}

		public override XPathNavigator Current
		{
			get
			{
				return (this.CurrentPosition != 0) ? this._nav : null;
			}
		}
	}
}
