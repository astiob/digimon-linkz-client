using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdNormalizedString : XsdString
	{
		internal XsdNormalizedString()
		{
			this.WhitespaceValue = XsdWhitespaceFacet.Replace;
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.CDATA;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.NormalizedString;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}
	}
}
