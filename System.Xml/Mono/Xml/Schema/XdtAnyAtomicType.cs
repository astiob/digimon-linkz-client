using System;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XdtAnyAtomicType : XsdAnySimpleType
	{
		internal XdtAnyAtomicType()
		{
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.AnyAtomicType;
			}
		}
	}
}
