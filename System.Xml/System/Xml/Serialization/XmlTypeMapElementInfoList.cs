using System;
using System.Collections;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapElementInfoList : ArrayList
	{
		public int IndexOfElement(string name, string namspace)
		{
			for (int i = 0; i < this.Count; i++)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)base[i];
				if (xmlTypeMapElementInfo.ElementName == name && xmlTypeMapElementInfo.Namespace == namspace)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
