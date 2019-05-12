using System;

namespace Mono.Xml
{
	internal class DTDChoiceAutomata : DTDAutomata
	{
		private DTDAutomata left;

		private DTDAutomata right;

		private bool hasComputedEmptiable;

		private bool cachedEmptiable;

		public DTDChoiceAutomata(DTDObjectModel root, DTDAutomata left, DTDAutomata right) : base(root)
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
			return this.left.TryStartElement(name).MakeChoice(this.right.TryStartElement(name));
		}

		public override DTDAutomata TryEndElement()
		{
			return this.left.TryEndElement().MakeChoice(this.right.TryEndElement());
		}

		public override bool Emptiable
		{
			get
			{
				if (!this.hasComputedEmptiable)
				{
					this.cachedEmptiable = (this.left.Emptiable || this.right.Emptiable);
					this.hasComputedEmptiable = true;
				}
				return this.cachedEmptiable;
			}
		}
	}
}
