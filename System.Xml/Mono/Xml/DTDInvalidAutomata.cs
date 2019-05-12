using System;

namespace Mono.Xml
{
	internal class DTDInvalidAutomata : DTDAutomata
	{
		public DTDInvalidAutomata(DTDObjectModel root) : base(root)
		{
		}

		public override DTDAutomata TryEndElement()
		{
			return this;
		}

		public override DTDAutomata TryStartElement(string name)
		{
			return this;
		}
	}
}
