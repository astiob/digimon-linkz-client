using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class SortedIterator : BaseIterator
	{
		private ArrayList list;

		public SortedIterator(BaseIterator iter) : base(iter.NamespaceManager)
		{
			this.list = new ArrayList();
			while (iter.MoveNext())
			{
				XPathNavigator xpathNavigator = iter.Current;
				this.list.Add(xpathNavigator.Clone());
			}
			if (this.list.Count == 0)
			{
				return;
			}
			XPathNavigator xpathNavigator2 = (XPathNavigator)this.list[0];
			this.list.Sort(XPathNavigatorComparer.Instance);
			for (int i = 1; i < this.list.Count; i++)
			{
				XPathNavigator xpathNavigator3 = (XPathNavigator)this.list[i];
				if (xpathNavigator2.IsSamePosition(xpathNavigator3))
				{
					this.list.RemoveAt(i);
					i--;
				}
				else
				{
					xpathNavigator2 = xpathNavigator3;
				}
			}
		}

		public SortedIterator(SortedIterator other) : base(other)
		{
			this.list = other.list;
			base.SetPosition(other.CurrentPosition);
		}

		public override XPathNodeIterator Clone()
		{
			return new SortedIterator(this);
		}

		public override bool MoveNextCore()
		{
			return this.CurrentPosition < this.list.Count;
		}

		public override XPathNavigator Current
		{
			get
			{
				return (this.CurrentPosition != 0) ? ((XPathNavigator)this.list[this.CurrentPosition - 1]) : null;
			}
		}

		public override int Count
		{
			get
			{
				return this.list.Count;
			}
		}
	}
}
