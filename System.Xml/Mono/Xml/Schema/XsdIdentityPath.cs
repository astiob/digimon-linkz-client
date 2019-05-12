using System;

namespace Mono.Xml.Schema
{
	internal class XsdIdentityPath
	{
		public XsdIdentityStep[] OrderedSteps;

		public bool Descendants;

		public bool IsAttribute
		{
			get
			{
				return this.OrderedSteps.Length != 0 && this.OrderedSteps[this.OrderedSteps.Length - 1].IsAttribute;
			}
		}
	}
}
