using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdIDRefs : XsdName
	{
		internal XsdIDRefs()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.IDREFS;
			}
		}

		[MonoTODO]
		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Item;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(string[]);
			}
		}

		public override object ParseValue(string value, XmlNameTable nt, IXmlNamespaceResolver nsmgr)
		{
			return this.GetValidatedArray(value, nt);
		}

		internal override ValueType ParseValueType(string s, XmlNameTable nameTable, IXmlNamespaceResolver nsmgr)
		{
			return new StringArrayValueType(this.GetValidatedArray(s, nameTable));
		}

		private string[] GetValidatedArray(string value, XmlNameTable nt)
		{
			string[] array = base.ParseListValue(value, nt);
			for (int i = 0; i < array.Length; i++)
			{
				XmlConvert.VerifyNCName(array[i]);
			}
			return array;
		}
	}
}
