using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class AncestorIterator : SimpleIterator
	{
		private int currentPosition;

		private ArrayList navigators;

		private XPathNavigator startPosition;

		public AncestorIterator(BaseIterator iter) : base(iter)
		{
			this.startPosition = iter.Current.Clone();
		}

		private AncestorIterator(AncestorIterator other) : base(other, true)
		{
			this.startPosition = other.startPosition;
			if (other.navigators != null)
			{
				this.navigators = other.navigators;
			}
			this.currentPosition = other.currentPosition;
		}

		public override XPathNodeIterator Clone()
		{
			return new AncestorIterator(this);
		}

		private void CollectResults()
		{
			this.navigators = new ArrayList();
			XPathNavigator xpathNavigator = this.startPosition.Clone();
			while (xpathNavigator.NodeType != XPathNodeType.Root && xpathNavigator.MoveToParent())
			{
				this.navigators.Add(xpathNavigator.Clone());
			}
			this.currentPosition = this.navigators.Count;
		}

		public override bool MoveNextCore()
		{
			if (this.navigators == null)
			{
				this.CollectResults();
			}
			if (this.currentPosition == 0)
			{
				return false;
			}
			this._nav.MoveTo((XPathNavigator)this.navigators[--this.currentPosition]);
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
				return this.navigators.Count;
			}
		}
	}
}
