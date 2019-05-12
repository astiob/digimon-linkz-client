using System;

namespace System.Xml.XPath
{
	internal abstract class SimpleIterator : BaseIterator
	{
		protected readonly XPathNavigator _nav;

		protected XPathNavigator _current;

		private bool skipfirst;

		public SimpleIterator(BaseIterator iter) : base(iter.NamespaceManager)
		{
			if (iter.CurrentPosition == 0)
			{
				this.skipfirst = true;
				iter.MoveNext();
			}
			if (iter.CurrentPosition > 0)
			{
				this._nav = iter.Current.Clone();
			}
		}

		protected SimpleIterator(SimpleIterator other, bool clone) : base(other)
		{
			if (other._nav != null)
			{
				this._nav = ((!clone) ? other._nav : other._nav.Clone());
			}
			this.skipfirst = other.skipfirst;
		}

		public SimpleIterator(XPathNavigator nav, IXmlNamespaceResolver nsm) : base(nsm)
		{
			this._nav = nav.Clone();
		}

		public override bool MoveNext()
		{
			if (!this.skipfirst)
			{
				return base.MoveNext();
			}
			if (this._nav == null)
			{
				return false;
			}
			this.skipfirst = false;
			base.SetPosition(1);
			return true;
		}

		public override XPathNavigator Current
		{
			get
			{
				if (this.CurrentPosition == 0)
				{
					return null;
				}
				this._current = this._nav;
				return this._current;
			}
		}
	}
}
