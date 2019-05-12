using System;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMemberFlatList : XmlTypeMapMemberExpandable
	{
		private ListMap _listMap;

		public ListMap ListMap
		{
			get
			{
				return this._listMap;
			}
			set
			{
				this._listMap = value;
			}
		}
	}
}
