using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class XPathIteratorComparer : IComparer
	{
		public static XPathIteratorComparer Instance = new XPathIteratorComparer();

		private XPathIteratorComparer()
		{
		}

		public int Compare(object o1, object o2)
		{
			XPathNodeIterator xpathNodeIterator = o1 as XPathNodeIterator;
			XPathNodeIterator xpathNodeIterator2 = o2 as XPathNodeIterator;
			if (xpathNodeIterator == null)
			{
				return -1;
			}
			if (xpathNodeIterator2 == null)
			{
				return 1;
			}
			XmlNodeOrder xmlNodeOrder = xpathNodeIterator.Current.ComparePosition(xpathNodeIterator2.Current);
			if (xmlNodeOrder == XmlNodeOrder.After)
			{
				return -1;
			}
			if (xmlNodeOrder != XmlNodeOrder.Same)
			{
				return 1;
			}
			return 0;
		}
	}
}
