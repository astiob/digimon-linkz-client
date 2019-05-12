using System;

namespace System.Xml.XPath
{
	internal class NullIterator : SelfIterator
	{
		public NullIterator(BaseIterator iter) : base(iter)
		{
		}

		public NullIterator(XPathNavigator nav) : this(nav, null)
		{
		}

		public NullIterator(XPathNavigator nav, IXmlNamespaceResolver nsm) : base(nav, nsm)
		{
		}

		private NullIterator(NullIterator other) : base(other, true)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new NullIterator(this);
		}

		public override bool MoveNextCore()
		{
			return false;
		}

		public override int CurrentPosition
		{
			get
			{
				return 1;
			}
		}

		public override XPathNavigator Current
		{
			get
			{
				return this._nav;
			}
		}
	}
}
