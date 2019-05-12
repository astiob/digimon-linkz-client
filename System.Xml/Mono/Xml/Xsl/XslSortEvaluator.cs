using System;
using System.Collections;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XslSortEvaluator
	{
		private XPathExpression select;

		private Sort[] sorterTemplates;

		private XPathSorter[] sorters;

		private XPathSorters sortRunner;

		private bool isSorterContextDependent;

		public XslSortEvaluator(XPathExpression select, Sort[] sorterTemplates)
		{
			this.select = select;
			this.sorterTemplates = sorterTemplates;
			this.PopulateConstantSorters();
			this.sortRunner = new XPathSorters();
		}

		private void PopulateConstantSorters()
		{
			this.sorters = new XPathSorter[this.sorterTemplates.Length];
			for (int i = 0; i < this.sorterTemplates.Length; i++)
			{
				Sort sort = this.sorterTemplates[i];
				if (sort.IsContextDependent)
				{
					this.isSorterContextDependent = true;
				}
				else
				{
					this.sorters[i] = sort.ToXPathSorter(null);
				}
			}
		}

		public BaseIterator SortedSelect(XslTransformProcessor p)
		{
			if (this.isSorterContextDependent)
			{
				for (int i = 0; i < this.sorters.Length; i++)
				{
					if (this.sorterTemplates[i].IsContextDependent)
					{
						this.sorters[i] = this.sorterTemplates[i].ToXPathSorter(p);
					}
				}
			}
			BaseIterator baseIterator = (BaseIterator)p.Select(this.select);
			p.PushNodeset(baseIterator);
			p.PushForEachContext();
			ArrayList arrayList = new ArrayList(baseIterator.Count);
			while (baseIterator.MoveNext())
			{
				XPathSortElement xpathSortElement = new XPathSortElement();
				xpathSortElement.Navigator = baseIterator.Current.Clone();
				xpathSortElement.Values = new object[this.sorters.Length];
				for (int j = 0; j < this.sorters.Length; j++)
				{
					xpathSortElement.Values[j] = this.sorters[j].Evaluate(baseIterator);
				}
				arrayList.Add(xpathSortElement);
			}
			p.PopForEachContext();
			p.PopNodeset();
			this.sortRunner.CopyFrom(this.sorters);
			return this.sortRunner.Sort(arrayList, baseIterator.NamespaceManager);
		}
	}
}
