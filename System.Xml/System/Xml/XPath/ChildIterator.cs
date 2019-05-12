using System;

namespace System.Xml.XPath
{
	internal class ChildIterator : BaseIterator
	{
		private XPathNavigator _nav;

		public ChildIterator(BaseIterator iter) : base(iter.NamespaceManager)
		{
			this._nav = ((iter.CurrentPosition != 0) ? iter.Current : iter.PeekNext());
			if (this._nav != null && this._nav.HasChildren)
			{
				this._nav = this._nav.Clone();
			}
			else
			{
				this._nav = null;
			}
		}

		private ChildIterator(ChildIterator other) : base(other)
		{
			this._nav = ((other._nav != null) ? other._nav.Clone() : null);
		}

		public override XPathNodeIterator Clone()
		{
			return new ChildIterator(this);
		}

		public override bool MoveNextCore()
		{
			return this._nav != null && ((this.CurrentPosition != 0) ? this._nav.MoveToNext() : this._nav.MoveToFirstChild());
		}

		public override XPathNavigator Current
		{
			get
			{
				if (this.CurrentPosition == 0)
				{
					return null;
				}
				return this._nav;
			}
		}
	}
}
