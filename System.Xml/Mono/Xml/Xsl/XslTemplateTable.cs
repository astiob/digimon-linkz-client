using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Mono.Xml.Xsl
{
	internal class XslTemplateTable
	{
		private Hashtable templateTables = new Hashtable();

		private Hashtable namedTemplates = new Hashtable();

		private XslStylesheet parent;

		public XslTemplateTable(XslStylesheet parent)
		{
			this.parent = parent;
		}

		public Hashtable TemplateTables
		{
			get
			{
				return this.templateTables;
			}
		}

		public XslModedTemplateTable this[XmlQualifiedName mode]
		{
			get
			{
				return this.templateTables[mode] as XslModedTemplateTable;
			}
		}

		public void Add(XslTemplate template)
		{
			if (template.Name != XmlQualifiedName.Empty)
			{
				if (this.namedTemplates[template.Name] != null)
				{
					throw new InvalidOperationException("Named template " + template.Name + " is already registered.");
				}
				this.namedTemplates[template.Name] = template;
			}
			if (template.Match == null)
			{
				return;
			}
			XslModedTemplateTable xslModedTemplateTable = this[template.Mode];
			if (xslModedTemplateTable == null)
			{
				xslModedTemplateTable = new XslModedTemplateTable(template.Mode);
				this.Add(xslModedTemplateTable);
			}
			xslModedTemplateTable.Add(template);
		}

		public void Add(XslModedTemplateTable table)
		{
			if (this[table.Mode] != null)
			{
				throw new InvalidOperationException("Mode " + table.Mode + " is already registered.");
			}
			this.templateTables.Add(table.Mode, table);
		}

		public XslTemplate FindMatch(XPathNavigator node, XmlQualifiedName mode, XslTransformProcessor p)
		{
			if (this[mode] != null)
			{
				XslTemplate xslTemplate = this[mode].FindMatch(node, p);
				if (xslTemplate != null)
				{
					return xslTemplate;
				}
			}
			for (int i = this.parent.Imports.Count - 1; i >= 0; i--)
			{
				XslStylesheet xslStylesheet = (XslStylesheet)this.parent.Imports[i];
				XslTemplate xslTemplate = xslStylesheet.Templates.FindMatch(node, mode, p);
				if (xslTemplate != null)
				{
					return xslTemplate;
				}
			}
			return null;
		}

		public XslTemplate FindTemplate(XmlQualifiedName name)
		{
			XslTemplate xslTemplate = (XslTemplate)this.namedTemplates[name];
			if (xslTemplate != null)
			{
				return xslTemplate;
			}
			for (int i = this.parent.Imports.Count - 1; i >= 0; i--)
			{
				XslStylesheet xslStylesheet = (XslStylesheet)this.parent.Imports[i];
				xslTemplate = xslStylesheet.Templates.FindTemplate(name);
				if (xslTemplate != null)
				{
					return xslTemplate;
				}
			}
			return null;
		}
	}
}
