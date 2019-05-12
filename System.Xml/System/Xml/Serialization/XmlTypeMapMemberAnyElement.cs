using System;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMemberAnyElement : XmlTypeMapMemberExpandable
	{
		public bool IsElementDefined(string name, string ns)
		{
			foreach (object obj in base.ElementInfo)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
				if (xmlTypeMapElementInfo.IsUnnamedAnyElement)
				{
					return true;
				}
				if (xmlTypeMapElementInfo.ElementName == name && xmlTypeMapElementInfo.Namespace == ns)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsDefaultAny
		{
			get
			{
				foreach (object obj in base.ElementInfo)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
					if (xmlTypeMapElementInfo.IsUnnamedAnyElement)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool CanBeText
		{
			get
			{
				return base.ElementInfo.Count > 0 && ((XmlTypeMapElementInfo)base.ElementInfo[0]).IsTextElement;
			}
		}
	}
}
