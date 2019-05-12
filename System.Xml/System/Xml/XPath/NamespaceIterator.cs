using System;

namespace System.Xml.XPath
{
	internal class NamespaceIterator : SimpleIterator
	{
		public NamespaceIterator(BaseIterator iter) : base(iter)
		{
		}

		private NamespaceIterator(NamespaceIterator other) : base(other, true)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new NamespaceIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this.CurrentPosition == 0)
			{
				if (this._nav.MoveToFirstNamespace())
				{
					return true;
				}
			}
			else if (this._nav.MoveToNextNamespace())
			{
				return true;
			}
			return false;
		}
	}
}
