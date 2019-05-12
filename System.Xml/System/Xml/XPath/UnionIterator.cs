using System;

namespace System.Xml.XPath
{
	internal class UnionIterator : BaseIterator
	{
		private BaseIterator _left;

		private BaseIterator _right;

		private bool keepLeft;

		private bool keepRight;

		private XPathNavigator _current;

		public UnionIterator(BaseIterator iter, BaseIterator left, BaseIterator right) : base(iter.NamespaceManager)
		{
			this._left = left;
			this._right = right;
		}

		private UnionIterator(UnionIterator other) : base(other)
		{
			this._left = (BaseIterator)other._left.Clone();
			this._right = (BaseIterator)other._right.Clone();
			this.keepLeft = other.keepLeft;
			this.keepRight = other.keepRight;
			if (other._current != null)
			{
				this._current = other._current.Clone();
			}
		}

		public override XPathNodeIterator Clone()
		{
			return new UnionIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (!this.keepLeft)
			{
				this.keepLeft = this._left.MoveNext();
			}
			if (!this.keepRight)
			{
				this.keepRight = this._right.MoveNext();
			}
			if (!this.keepLeft && !this.keepRight)
			{
				return false;
			}
			if (!this.keepRight)
			{
				this.keepLeft = false;
				this.SetCurrent(this._left);
				return true;
			}
			if (!this.keepLeft)
			{
				this.keepRight = false;
				this.SetCurrent(this._right);
				return true;
			}
			switch (this._left.Current.ComparePosition(this._right.Current))
			{
			case XmlNodeOrder.Before:
			case XmlNodeOrder.Unknown:
				this.keepLeft = false;
				this.SetCurrent(this._left);
				return true;
			case XmlNodeOrder.After:
				this.keepRight = false;
				this.SetCurrent(this._right);
				return true;
			case XmlNodeOrder.Same:
				this.keepLeft = (this.keepRight = false);
				this.SetCurrent(this._right);
				return true;
			default:
				throw new InvalidOperationException("Should not happen.");
			}
		}

		private void SetCurrent(XPathNodeIterator iter)
		{
			if (this._current == null)
			{
				this._current = iter.Current.Clone();
			}
			else if (!this._current.MoveTo(iter.Current))
			{
				this._current = iter.Current.Clone();
			}
		}

		public override XPathNavigator Current
		{
			get
			{
				return (this.CurrentPosition <= 0) ? null : this._current;
			}
		}
	}
}
