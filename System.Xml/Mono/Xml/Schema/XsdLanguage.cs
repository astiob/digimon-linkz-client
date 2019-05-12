using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdLanguage : XsdToken
	{
		internal XsdLanguage()
		{
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
				return XmlTypeCode.Language;
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
