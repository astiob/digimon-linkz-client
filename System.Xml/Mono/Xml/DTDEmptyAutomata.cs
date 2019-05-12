using System;

namespace Mono.Xml
{
	internal class DTDEmptyAutomata : DTDAutomata
	{
		public DTDEmptyAutomata(DTDObjectModel root) : base(root)
		{
		}

		public override DTDAutomata TryEndElement()
		{
			return this;
		}

		public override DTDAutomata TryStartElement(string name)
		{
			return base.Root.Invalid;
		}

		public override bool Emptiable
		{
			get
			{
				return true;
			}
		}
	}
}
