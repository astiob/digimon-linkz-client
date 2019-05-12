using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class SlashIterator : BaseIterator
	{
		private BaseIterator _iterLeft;

		private BaseIterator _iterRight;

		private NodeSet _expr;

		private SortedList _iterList;

		private bool _finished;

		private BaseIterator _nextIterRight;

		public SlashIterator(BaseIterator iter, NodeSet expr) : base(iter.NamespaceManager)
		{
			this._iterLeft = iter;
			this._expr = expr;
		}

		private SlashIterator(SlashIterator other) : base(other)
		{
			this._iterLeft = (BaseIterator)other._iterLeft.Clone();
			if (other._iterRight != null)
			{
				this._iterRight = (BaseIterator)other._iterRight.Clone();
			}
			this._expr = other._expr;
			if (other._iterList != null)
			{
				this._iterList = (SortedList)other._iterList.Clone();
			}
			this._finished = other._finished;
			if (other._nextIterRight != null)
			{
				this._nextIterRight = (BaseIterator)other._nextIterRight.Clone();
			}
		}

		public override XPathNodeIterator Clone()
		{
			return new SlashIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this._finished)
			{
				return false;
			}
			if (this._iterRight == null)
			{
				if (!this._iterLeft.MoveNext())
				{
					return false;
				}
				this._iterRight = this._expr.EvaluateNodeSet(this._iterLeft);
				this._iterList = new SortedList(XPathIteratorComparer.Instance);
			}
			while (!this._iterRight.MoveNext())
			{
				if (this._iterList.Count > 0)
				{
					int index = this._iterList.Count - 1;
					this._iterRight = (BaseIterator)this._iterList.GetByIndex(index);
					this._iterList.RemoveAt(index);
					break;
				}
				if (this._nextIterRight != null)
				{
					this._iterRight = this._nextIterRight;
					this._nextIterRight = null;
					break;
				}
				if (!this._iterLeft.MoveNext())
				{
					this._finished = true;
					return false;
				}
				this._iterRight = this._expr.EvaluateNodeSet(this._iterLeft);
			}
			bool flag = true;
			while (flag)
			{
				flag = false;
				if (this._nextIterRight == null)
				{
					bool flag2 = false;
					while (this._nextIterRight == null || !this._nextIterRight.MoveNext())
					{
						if (!this._iterLeft.MoveNext())
						{
							flag2 = true;
							break;
						}
						this._nextIterRight = this._expr.EvaluateNodeSet(this._iterLeft);
					}
					if (flag2)
					{
						this._nextIterRight = null;
					}
				}
				if (this._nextIterRight != null)
				{
					XmlNodeOrder xmlNodeOrder = this._iterRight.Current.ComparePosition(this._nextIterRight.Current);
					if (xmlNodeOrder != XmlNodeOrder.After)
					{
						if (xmlNodeOrder == XmlNodeOrder.Same)
						{
							if (!this._nextIterRight.MoveNext())
							{
								this._nextIterRight = null;
							}
							else
							{
								int count = this._iterList.Count;
								this._iterList[this._nextIterRight] = this._nextIterRight;
								if (count != this._iterList.Count)
								{
									this._nextIterRight = (BaseIterator)this._iterList.GetByIndex(count);
									this._iterList.RemoveAt(count);
								}
							}
							flag = true;
						}
					}
					else
					{
						this._iterList[this._iterRight] = this._iterRight;
						this._iterRight = this._nextIterRight;
						this._nextIterRight = null;
						flag = true;
					}
				}
			}
			return true;
		}

		public override XPathNavigator Current
		{
			get
			{
				return (this.CurrentPosition != 0) ? this._iterRight.Current : null;
			}
		}
	}
}
