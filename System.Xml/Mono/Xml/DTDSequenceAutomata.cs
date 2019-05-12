using System;

namespace Mono.Xml
{
	internal class DTDSequenceAutomata : DTDAutomata
	{
		private DTDAutomata left;

		private DTDAutomata right;

		private bool hasComputedEmptiable;

		private bool cachedEmptiable;

		public DTDSequenceAutomata(DTDObjectModel root, DTDAutomata left, DTDAutomata right) : base(root)
		{
			this.left = left;
			this.right = right;
		}

		public DTDAutomata Left
		{
			get
			{
				return this.left;
			}
		}

		public DTDAutomata Right
		{
			get
			{
				return this.right;
			}
		}

		public override DTDAutomata TryStartElement(string name)
		{
			DTDAutomata dtdautomata = this.left.TryStartElement(name);
			DTDAutomata dtdautomata2 = this.right.TryStartElement(name);
			if (dtdautomata == base.Root.Invalid)
			{
				return (!this.left.Emptiable) ? dtdautomata : dtdautomata2;
			}
			DTDAutomata dtdautomata3 = dtdautomata.MakeSequence(this.right);
			if (this.left.Emptiable)
			{
				return dtdautomata2.MakeChoice(dtdautomata3);
			}
			return dtdautomata3;
		}

		public override DTDAutomata TryEndElement()
		{
			return (!this.left.Emptiable) ? base.Root.Invalid : this.right;
		}

		public override bool Emptiable
		{
			get
			{
				if (!this.hasComputedEmptiable)
				{
					this.cachedEmptiable = (this.left.Emptiable && this.right.Emptiable);
					this.hasComputedEmptiable = true;
				}
				return this.cachedEmptiable;
			}
		}
	}
}
