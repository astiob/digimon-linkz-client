using System;

namespace System.Xml.XPath
{
	internal class PrecedingIterator : SimpleIterator
	{
		private bool finished;

		private bool started;

		private XPathNavigator startPosition;

		public PrecedingIterator(BaseIterator iter) : base(iter)
		{
			this.startPosition = iter.Current.Clone();
		}

		private PrecedingIterator(PrecedingIterator other) : base(other, true)
		{
			this.startPosition = other.startPosition;
			this.started = other.started;
			this.finished = other.finished;
		}

		public override XPathNodeIterator Clone()
		{
			return new PrecedingIterator(this);
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
				this._nav.MoveToRoot();
			}
			bool flag = true;
			while (flag)
			{
				if (!this._nav.MoveToFirstChild())
				{
					while (!this._nav.MoveToNext())
					{
						if (!this._nav.MoveToParent())
						{
							this.finished = true;
							return false;
						}
					}
				}
				if (!this._nav.IsDescendant(this.startPosition))
				{
					break;
				}
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
