using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class AncestorOrSelfIterator : SimpleIterator
	{
		private int currentPosition;

		private ArrayList navigators;

		private XPathNavigator startPosition;

		public AncestorOrSelfIterator(BaseIterator iter) : base(iter)
		{
			this.startPosition = iter.Current.Clone();
		}

		private AncestorOrSelfIterator(AncestorOrSelfIterator other) : base(other, true)
		{
			this.startPosition = other.startPosition;
			if (other.navigators != null)
			{
				this.navigators = (ArrayList)other.navigators.Clone();
			}
			this.currentPosition = other.currentPosition;
		}

		public override XPathNodeIterator Clone()
		{
			return new AncestorOrSelfIterator(this);
		}

		private void CollectResults()
		{
			this.navigators = new ArrayList();
			XPathNavigator xpathNavigator = this.startPosition.Clone();
			if (!xpathNavigator.MoveToParent())
			{
				return;
			}
			while (xpathNavigator.NodeType != XPathNodeType.Root)
			{
				this.navigators.Add(xpathNavigator.Clone());
				xpathNavigator.MoveToParent();
			}
			this.currentPosition = this.navigators.Count;
		}

		public override bool MoveNextCore()
		{
			if (this.navigators == null)
			{
				this.CollectResults();
				if (this.startPosition.NodeType != XPathNodeType.Root)
				{
					this._nav.MoveToRoot();
					return true;
				}
			}
			if (this.currentPosition == -1)
			{
				return false;
			}
			if (this.currentPosition-- == 0)
			{
				this._nav.MoveTo(this.startPosition);
				return true;
			}
			this._nav.MoveTo((XPathNavigator)this.navigators[this.currentPosition]);
			return true;
		}

		public override bool ReverseAxis
		{
			get
			{
				return true;
			}
		}

		public override int Count
		{
			get
			{
				if (this.navigators == null)
				{
					this.CollectResults();
				}
				return this.navigators.Count + 1;
			}
		}
	}
}
