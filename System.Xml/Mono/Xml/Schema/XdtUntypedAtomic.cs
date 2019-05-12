using System;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XdtUntypedAtomic : XdtAnyAtomicType
	{
		internal XdtUntypedAtomic()
		{
		}

		public override XmlTypeCode TypeCode
		{
			get
			{
				return XmlTypeCode.UntypedAtomic;
			}
		}
	}
}
