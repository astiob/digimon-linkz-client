using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdQName : XsdName
	{
		internal XsdQName()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.QName;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.QName;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(XmlQualifiedName);
			}
		}

		public override object ParseValue(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			if (nameTable == null)
			{
				throw new ArgumentNullException("name table");
			}
			if (nsmgr == null)
			{
				throw new ArgumentNullException("namespace manager");
			}
			XmlQualifiedName xmlQualifiedName = XmlQualifiedName.Parse(s, nsmgr, true);
			nameTable.Add(xmlQualifiedName.Name);
			nameTable.Add(xmlQualifiedName.Namespace);
			return xmlQualifiedName;
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new QNameValueType(this.ParseValue(s, nameTable, nsmgr) as XmlQualifiedName);
		}
	}
}
