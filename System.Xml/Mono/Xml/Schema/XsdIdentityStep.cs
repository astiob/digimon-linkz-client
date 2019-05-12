using System;

namespace Mono.Xml.Schema
{
	internal class XsdIdentityStep
	{
		public bool IsCurrent;

		public bool IsAttribute;

		public bool IsAnyName;

		public string NsName;

		public string Name;

		public string Namespace = string.Empty;
	}
}
