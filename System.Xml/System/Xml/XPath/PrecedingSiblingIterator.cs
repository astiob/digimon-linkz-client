using System;

namespace System.Xml.XPath
{
	internal class PrecedingSiblingIterator : SimpleIterator
	{
		private bool finished;

		private bool started;

		private XPathNavigator startPosition;

		public PrecedingSiblingIterator(BaseIterator iter) : base(iter)
		{
			this.startPosition = iter.Current.Clone();
		}

		private PrecedingSiblingIterator(PrecedingSiblingIterator other) : base(other, true)
		{
			this.startPosition = other.startPosition;
			this.started = other.started;
			this.finished = other.finished;
		}

		public override XPathNodeIterator Clone()
		{
			return new PrecedingSiblingIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this.finished)
			{
				return false;
			}
			if (!this.started)
			{
				this.started = true;
				XPathNodeType nodeType = this._nav.NodeType;
				if (nodeType == XPathNodeType.Attribute || nodeType == XPathNodeType.Namespace)
				{
					this.finished = true;
					return false;
				}
				this._nav.MoveToFirst();
				if (!this._nav.IsSamePosition(this.startPosition))
				{
					return true;
				}
			}
			else if (!this._nav.MoveToNext())
			{
				this.finished = true;
				return false;
			}
			if (this._nav.ComparePosition(this.startPosition) != XmlNodeOrder.Before)
			{
				this.finished = true;
				return false;
			}
			return true;
		}

		public override bool ReverseAxis
		{
			get
			{
				return true;
			}
		}
	}
}
