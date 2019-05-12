using System;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdEntity : XsdName
	{
		internal XsdEntity()
		{
		}

		public override XmlTokenizedType TokenizedType
		{
			get
			{
				return XmlTokenizedType.ENTITY;
			}
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.Entity;
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
