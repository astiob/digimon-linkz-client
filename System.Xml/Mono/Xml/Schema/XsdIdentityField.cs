using System;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdIdentityField
	{
		private XsdIdentityPath[] fieldPaths;

		private int index;

		public XsdIdentityField(XmlSchemaXPath field, int index)
		{
			this.index = index;
			this.fieldPaths = field.CompiledExpression;
		}

		public XsdIdentityPath[] Paths
		{
			get
			{
				return this.fieldPaths;
			}
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}
	}
}
