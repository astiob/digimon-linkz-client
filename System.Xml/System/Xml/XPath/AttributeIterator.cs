using System;

namespace System.Xml.XPath
{
	internal class AttributeIterator : SimpleIterator
	{
		public AttributeIterator(BaseIterator iter) : base(iter)
		{
		}

		private AttributeIterator(AttributeIterator other) : base(other, true)
		{
		}

		public override XPathNodeIterator Clone()
		{
			return new AttributeIterator(this);
		}

		public override bool MoveNextCore()
		{
			if (this.CurrentPosition == 0)
			{
				if (this._nav.MoveToFirstAttribute())
				{
					return true;
				}
			}
			else if (this._nav.MoveToNextAttribute())
			{
				return true;
			}
			return false;
		}
	}
}
