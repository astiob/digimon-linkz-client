using System;

namespace System.Xml.XPath
{
	internal class SimpleSlashIterator : BaseIterator
	{
		private NodeSet _expr;

		private BaseIterator _left;

		private BaseIterator _right;

		private XPathNavigator _current;

		public SimpleSlashIterator(BaseIterator left, NodeSet expr) : base(left.NamespaceManager)
		{
			this._left = left;
			this._expr = expr;
		}

		private SimpleSlashIterator(SimpleSlashIterator other) : base(other)
		{
			this._expr = other._expr;
			this._left = (BaseIterator)other._left.Clone();
			if (other._right != null)
			{
				this._right = (BaseIterator)other._right.Clone();
			}
		}

		public override XPathNodeIterator Clone()
		{
			return new SimpleSlashIterator(this);
		}

		public override bool MoveNextCore()
		{
			while (this._right == null || !this._right.MoveNext())
			{
				if (!this._left.MoveNext())
				{
					return false;
				}
				this._right = this._expr.EvaluateNodeSet(this._left);
			}
			if (this._current == null)
			{
				this._current = this._right.Current.Clone();
			}
			else if (!this._current.MoveTo(this._right.Current))
			{
				this._current = this._right.Current.Clone();
			}
			return true;
		}

		public override XPathNavigator Current
		{
			get
			{
				return this._current;
			}
		}
	}
}
