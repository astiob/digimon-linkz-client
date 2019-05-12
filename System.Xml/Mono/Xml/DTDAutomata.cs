using System;

namespace Mono.Xml
{
	internal abstract class DTDAutomata
	{
		private DTDObjectModel root;

		public DTDAutomata(DTDObjectModel root)
		{
			this.root = root;
		}

		public DTDObjectModel Root
		{
			get
			{
				return this.root;
			}
		}

		public DTDAutomata MakeChoice(DTDAutomata other)
		{
			if (this == this.Root.Invalid)
			{
				return other;
			}
			if (other == this.Root.Invalid)
			{
				return this;
			}
			if (this == this.Root.Empty && other == this.Root.Empty)
			{
				return this;
			}
			if (this == this.Root.Any && other == this.Root.Any)
			{
				return this;
			}
			if (other == this.Root.Empty)
			{
				return this.Root.Factory.Choice(other, this);
			}
			return this.Root.Factory.Choice(this, other);
		}

		public DTDAutomata MakeSequence(DTDAutomata other)
		{
			if (this == this.Root.Invalid || other == this.Root.Invalid)
			{
				return this.Root.Invalid;
			}
			if (this == this.Root.Empty)
			{
				return other;
			}
			if (other == this.Root.Empty)
			{
				return this;
			}
			return this.Root.Factory.Sequence(this, other);
		}

		public abstract DTDAutomata TryStartElement(string name);

		public virtual DTDAutomata TryEndElement()
		{
			return this.Root.Invalid;
		}

		public virtual bool Emptiable
		{
			get
			{
				return false;
			}
		}
	}
}
