using System;
using System.Collections;

namespace Mono.Xml.Schema
{
	internal class XsdKeyEntryFieldCollection : CollectionBase
	{
		public XsdKeyEntryField this[int i]
		{
			get
			{
				return (XsdKeyEntryField)base.List[i];
			}
			set
			{
				base.List[i] = value;
			}
		}

		public int Add(XsdKeyEntryField value)
		{
			return base.List.Add(value);
		}
	}
}
