using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class XPathSorters : IComparer
	{
		private readonly ArrayList _rgSorters = new ArrayList();

		int IComparer.Compare(object o1, object o2)
		{
			XPathSortElement xpathSortElement = (XPathSortElement)o1;
			XPathSortElement xpathSortElement2 = (XPathSortElement)o2;
			for (int i = 0; i < this._rgSorters.Count; i++)
			{
				XPathSorter xpathSorter = (XPathSorter)this._rgSorters[i];
				int num = xpathSorter.Compare(xpathSortElement.Values[i], xpathSortElement2.Values[i]);
				if (num != 0)
				{
					return num;
				}
			}
			XmlNodeOrder xmlNodeOrder = xpathSortElement.Navigator.ComparePosition(xpathSortElement2.Navigator);
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

		public void Add(object expr, IComparer cmp)
		{
			this._rgSorters.Add(new XPathSorter(expr, cmp));
		}

		public void Add(object expr, XmlSortOrder orderSort, XmlCaseOrder orderCase, string lang, XmlDataType dataType)
		{
			this._rgSorters.Add(new XPathSorter(expr, orderSort, orderCase, lang, dataType));
		}

		public void CopyFrom(XPathSorter[] sorters)
		{
			this._rgSorters.Clear();
			this._rgSorters.AddRange(sorters);
		}

		public BaseIterator Sort(BaseIterator iter)
		{
			ArrayList rgElts = this.ToSortElementList(iter);
			return this.Sort(rgElts, iter.NamespaceManager);
		}

		private ArrayList ToSortElementList(BaseIterator iter)
		{
			ArrayList arrayList = new ArrayList();
			int count = this._rgSorters.Count;
			while (iter.MoveNext())
			{
				XPathSortElement xpathSortElement = new XPathSortElement();
				xpathSortElement.Navigator = iter.Current.Clone();
				xpathSortElement.Values = new object[count];
				for (int i = 0; i < this._rgSorters.Count; i++)
				{
					XPathSorter xpathSorter = (XPathSorter)this._rgSorters[i];
					xpathSortElement.Values[i] = xpathSorter.Evaluate(iter);
				}
				arrayList.Add(xpathSortElement);
			}
			return arrayList;
		}

		public BaseIterator Sort(ArrayList rgElts, IXmlNamespaceResolver nsm)
		{
			rgElts.Sort(this);
			XPathNavigator[] array = new XPathNavigator[rgElts.Count];
			for (int i = 0; i < rgElts.Count; i++)
			{
				XPathSortElement xpathSortElement = (XPathSortElement)rgElts[i];
				array[i] = xpathSortElement.Navigator;
			}
			return new ListIterator(array, nsm);
		}
	}
}
