using System;

namespace System.Xml.XPath
{
	internal class ParensIterator : BaseIterator
	{
		private BaseIterator _iter;

		public ParensIterator(BaseIterator iter) : base(iter.NamespaceManager)
		{
			this._iter = iter;
		}

		private ParensIterator(ParensIterator other) : base(other)
		{
			this._iter = (BaseIterator)other._iter.Clone();
		}

		public override XPathNodeIterator Clone()
		{
			return new ParensIterator(this);
		}

		public override bool MoveNextCore()
		{
			return this._iter.MoveNext();
		}

		public override XPathNavigator Current
		{
			get
			{
				return this._iter.Current;
			}
		}

		public override int Count
		{
			get
			{
				return this._iter.Count;
			}
		}
	}
}
