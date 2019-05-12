using System;

namespace System.Xml.XPath
{
	internal class DescendantIterator : SimpleIterator
	{
		private int _depth;

		private bool _finished;

		public DescendantIterator(BaseIterator iter) : base(iter)
		{
		}

		private DescendantIterator(DescendantIterator other) : base(other, true)
		{
			this._depth = other._depth;
			this._finished = other._finished;
		}

		public override XPathNodeIterator Clone()
		{
			return new DescendantIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this._finished)
			{
				return false;
			}
			if (this._nav.MoveToFirstChild())
			{
				this._depth++;
				return true;
			}
			while (this._depth != 0)
			{
				if (this._nav.MoveToNext())
				{
					return true;
				}
				if (!this._nav.MoveToParent())
				{
					throw new XPathException("Current node is removed while it should not be, or there are some bugs in the XPathNavigator implementation class: " + this._nav.GetType());
				}
				this._depth--;
			}
			this._finished = true;
			return false;
		}
	}
}
