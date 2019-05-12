using Mono.Xml.XPath;
using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XslModedTemplateTable
	{
		private ArrayList unnamedTemplates = new ArrayList();

		private XmlQualifiedName mode;

		private bool sorted;

		public XslModedTemplateTable(XmlQualifiedName mode)
		{
			if (mode == null)
			{
				throw new InvalidOperationException();
			}
			this.mode = mode;
		}

		public XmlQualifiedName Mode
		{
			get
			{
				return this.mode;
			}
		}

		public void Add(XslTemplate t)
		{
			if (!double.IsNaN(t.Priority))
			{
				this.unnamedTemplates.Add(new XslModedTemplateTable.TemplateWithPriority(t, t.Priority));
			}
			else
			{
				this.Add(t, t.Match);
			}
		}

		public void Add(XslTemplate t, Pattern p)
		{
			if (p is UnionPattern)
			{
				this.Add(t, ((UnionPattern)p).p0);
				this.Add(t, ((UnionPattern)p).p1);
				return;
			}
			this.unnamedTemplates.Add(new XslModedTemplateTable.TemplateWithPriority(t, p));
		}

		public XslTemplate FindMatch(XPathNavigator node, XslTransformProcessor p)
		{
			if (!this.sorted)
			{
				this.unnamedTemplates.Sort();
				this.unnamedTemplates.Reverse();
				this.sorted = true;
			}
			for (int i = 0; i < this.unnamedTemplates.Count; i++)
			{
				XslModedTemplateTable.TemplateWithPriority templateWithPriority = (XslModedTemplateTable.TemplateWithPriority)this.unnamedTemplates[i];
				if (templateWithPriority.Matches(node, p))
				{
					return templateWithPriority.Template;
				}
			}
			return null;
		}

		private class TemplateWithPriority : IComparable
		{
			public readonly double Priority;

			public readonly XslTemplate Template;

			public readonly Pattern Pattern;

			public readonly int TemplateID;

			public TemplateWithPriority(XslTemplate t, Pattern p)
			{
				this.Template = t;
				this.Pattern = p;
				this.Priority = p.DefaultPriority;
				this.TemplateID = t.Id;
			}

			public TemplateWithPriority(XslTemplate t, double p)
			{
				this.Template = t;
				this.Pattern = t.Match;
				this.Priority = p;
				this.TemplateID = t.Id;
			}

			public int CompareTo(object o)
			{
				XslModedTemplateTable.TemplateWithPriority templateWithPriority = (XslModedTemplateTable.TemplateWithPriority)o;
				int num = this.Priority.CompareTo(templateWithPriority.Priority);
				if (num != 0)
				{
					return num;
				}
				return this.TemplateID.CompareTo(templateWithPriority.TemplateID);
			}

			public bool Matches(XPathNavigator n, XslTransformProcessor p)
			{
				return p.Matches(this.Pattern, n);
			}
		}
	}
}
