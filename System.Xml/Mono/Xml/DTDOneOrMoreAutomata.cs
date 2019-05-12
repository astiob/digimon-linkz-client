using System;

namespace Mono.Xml
{
	internal class DTDOneOrMoreAutomata : DTDAutomata
	{
		private DTDAutomata children;

		public DTDOneOrMoreAutomata(DTDObjectModel root, DTDAutomata children) : base(root)
		{
			this.children = children;
		}

		public DTDAutomata Children
		{
			get
			{
				return this.children;
			}
		}

		public override DTDAutomata TryStartElement(string name)
		{
			DTDAutomata dtdautomata = this.children.TryStartElement(name);
			if (dtdautomata != base.Root.Invalid)
			{
				return dtdautomata.MakeSequence(base.Root.Empty.MakeChoice(this));
			}
			return base.Root.Invalid;
		}

		public override DTDAutomata TryEndElement()
		{
			return (!this.Emptiable) ? base.Root.Invalid : this.children.TryEndElement();
		}
	}
}
