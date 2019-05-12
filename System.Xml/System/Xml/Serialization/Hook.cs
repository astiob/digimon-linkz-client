using System;

namespace System.Xml.Serialization
{
	[XmlType("hook")]
	internal class Hook
	{
		[XmlAttribute("type")]
		public HookType HookType;

		[XmlElement("select")]
		public Select Select;

		[XmlElement("insertBefore")]
		public string InsertBefore;

		[XmlElement("insertAfter")]
		public string InsertAfter;

		[XmlElement("replace")]
		public string Replace;

		public string GetCode(HookAction action)
		{
			if (action == HookAction.InsertBefore)
			{
				return this.InsertBefore;
			}
			if (action == HookAction.InsertAfter)
			{
				return this.InsertAfter;
			}
			return this.Replace;
		}
	}
}
