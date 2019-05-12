using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class ListIterator : BaseIterator
	{
		private IList _list;

		public ListIterator(BaseIterator iter, IList list) : base(iter.NamespaceManager)
		{
			this._list = list;
		}

		public ListIterator(IList list, IXmlNamespaceResolver nsm) : base(nsm)
		{
			this._list = list;
		}

		private ListIterator(ListIterator other) : base(other)
		{
			this._list = other._list;
		}

		public override XPathNodeIterator Clone()
		{
			return new ListIterator(this);
		}

		public override bool MoveNextCore()
		{
			return this.CurrentPosition < this._list.Count;
		}

		public override XPathNavigator Current
		{
			get
			{
				if (this._list.Count == 0 || this.CurrentPosition == 0)
				{
					return null;
				}
				return (XPathNavigator)this._list[this.CurrentPosition - 1];
			}
		}

		public override int Count
		{
			get
			{
				return this._list.Count;
			}
		}
	}
}
