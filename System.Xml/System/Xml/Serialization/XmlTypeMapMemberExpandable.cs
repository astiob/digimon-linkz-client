using System;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMemberExpandable : XmlTypeMapMemberElement
	{
		private int _flatArrayIndex;

		public int FlatArrayIndex
		{
			get
			{
				return this._flatArrayIndex;
			}
			set
			{
				this._flatArrayIndex = value;
			}
		}
	}
}
