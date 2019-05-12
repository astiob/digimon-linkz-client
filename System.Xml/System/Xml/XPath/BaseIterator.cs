using System;

namespace System.Xml.XPath
{
	internal abstract class BaseIterator : XPathNodeIterator
	{
		private IXmlNamespaceResolver _nsm;

		private int position;

		internal BaseIterator(BaseIterator other)
		{
			this._nsm = other._nsm;
			this.position = other.position;
		}

		internal BaseIterator(IXmlNamespaceResolver nsm)
		{
			this._nsm = nsm;
		}

		public IXmlNamespaceResolver NamespaceManager
		{
			get
			{
				return this._nsm;
			}
			set
			{
				this._nsm = value;
			}
		}

		public virtual bool ReverseAxis
		{
			get
			{
				return false;
			}
		}

		public int ComparablePosition
		{
			get
			{
				if (this.ReverseAxis)
				{
					int num = this.Count - this.CurrentPosition + 1;
					return (num >= 1) ? num : 1;
				}
				return this.CurrentPosition;
			}
		}

		public override int CurrentPosition
		{
			get
			{
				return this.position;
			}
		}

		internal void SetPosition(int pos)
		{
			this.position = pos;
		}

		public override bool MoveNext()
		{
			if (!this.MoveNextCore())
			{
				return false;
			}
			this.position++;
			return true;
		}

		public abstract bool MoveNextCore();

		internal XPathNavigator PeekNext()
		{
			XPathNodeIterator xpathNodeIterator = this.Clone();
			return (!xpathNodeIterator.MoveNext()) ? null : xpathNodeIterator.Current;
		}

		public override string ToString()
		{
			if (this.Current != null)
			{
				return string.Concat(new object[]
				{
					this.Current.NodeType.ToString(),
					"[",
					this.CurrentPosition,
					"] : ",
					this.Current.Name,
					" = ",
					this.Current.Value
				});
			}
			return string.Concat(new object[]
			{
				base.GetType().ToString(),
				"[",
				this.CurrentPosition,
				"]"
			});
		}
	}
}
