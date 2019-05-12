using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class XPathNavigatorComparer : IComparer, IEqualityComparer
	{
		public static XPathNavigatorComparer Instance = new XPathNavigatorComparer();

		private XPathNavigatorComparer()
		{
		}

		bool IEqualityComparer.Equals(object o1, object o2)
		{
			XPathNavigator xpathNavigator = o1 as XPathNavigator;
			XPathNavigator xpathNavigator2 = o2 as XPathNavigator;
			return xpathNavigator != null && xpathNavigator2 != null && xpathNavigator.IsSamePosition(xpathNavigator2);
		}

		int IEqualityComparer.GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}

		public int Compare(object o1, object o2)
		{
			XPathNavigator xpathNavigator = o1 as XPathNavigator;
			XPathNavigator xpathNavigator2 = o2 as XPathNavigator;
			if (xpathNavigator == null)
			{
				return -1;
			}
			if (xpathNavigator2 == null)
			{
				return 1;
			}
			XmlNodeOrder xmlNodeOrder = xpathNavigator.ComparePosition(xpathNavigator2);
			if (xmlNodeOrder == XmlNodeOrder.After)
			{
				return 1;
			}
			if (xmlNodeOrder != XmlNodeOrder.Same)
			{
				return -1;
			}
			return 0;
		}
	}
}
