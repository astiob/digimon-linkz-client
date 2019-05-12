using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdIdentitySelector
	{
		private XsdIdentityPath[] selectorPaths;

		private ArrayList fields = new ArrayList();

		private XsdIdentityField[] cachedFields;

		public XsdIdentitySelector(XmlSchemaXPath selector)
		{
			this.selectorPaths = selector.CompiledExpression;
		}

		public XsdIdentityPath[] Paths
		{
			get
			{
				return this.selectorPaths;
			}
		}

		public void AddField(XsdIdentityField field)
		{
			this.cachedFields = null;
			this.fields.Add(field);
		}

		public XsdIdentityField[] Fields
		{
			get
			{
				if (this.cachedFields == null)
				{
					this.cachedFields = (this.fields.ToArray(typeof(XsdIdentityField)) as XsdIdentityField[]);
				}
				return this.cachedFields;
			}
		}
	}
}
