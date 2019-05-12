using System;

namespace System.Xml.XPath
{
	internal class AxisIterator : BaseIterator
	{
		private BaseIterator _iter;

		private NodeTest _test;

		public AxisIterator(BaseIterator iter, NodeTest test) : base(iter.NamespaceManager)
		{
			this._iter = iter;
			this._test = test;
		}

		private AxisIterator(AxisIterator other) : base(other)
		{
			this._iter = (BaseIterator)other._iter.Clone();
			this._test = other._test;
		}

		public override XPathNodeIterator Clone()
		{
			return new AxisIterator(this);
		}

		public override bool MoveNextCore()
		{
			while (this._iter.MoveNext())
			{
				if (this._test.Match(base.NamespaceManager, this._iter.Current))
				{
					return true;
				}
			}
			return false;
		}

		public override XPathNavigator Current
		{
			get
			{
				return (this.CurrentPosition != 0) ? this._iter.Current : null;
			}
		}

		public override bool ReverseAxis
		{
			get
			{
				return this._iter.ReverseAxis;
			}
		}
	}
}
