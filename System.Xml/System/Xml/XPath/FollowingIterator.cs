using System;

namespace System.Xml.XPath
{
	internal class FollowingIterator : SimpleIterator
	{
		private bool _finished;

		public FollowingIterator(BaseIterator iter) : base(iter)
		{
		}

		private FollowingIterator(FollowingIterator other) : base(other, true)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new FollowingIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this._finished)
			{
				return false;
			}
			bool flag = true;
			if (this.CurrentPosition == 0)
			{
				flag = false;
				XPathNodeType nodeType = this._nav.NodeType;
				if (nodeType != XPathNodeType.Attribute && nodeType != XPathNodeType.Namespace)
				{
					if (this._nav.MoveToNext())
					{
						return true;
					}
					while (this._nav.MoveToParent())
					{
						if (this._nav.MoveToNext())
						{
							return true;
						}
					}
				}
				else
				{
					this._nav.MoveToParent();
					flag = true;
				}
			}
			if (flag)
			{
				if (this._nav.MoveToFirstChild())
				{
					return true;
				}
				while (!this._nav.MoveToNext())
				{
					if (!this._nav.MoveToParent())
					{
						goto IL_C6;
					}
				}
				return true;
			}
			IL_C6:
			this._finished = true;
			return false;
		}
	}
}
