using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdAllValidationState : XsdValidationState
	{
		private readonly XmlSchemaAll all;

		private ArrayList consumed = new ArrayList();

		public XsdAllValidationState(XmlSchemaAll all, XsdParticleStateManager manager) : base(manager)
		{
			this.all = all;
		}

		public override void GetExpectedParticles(ArrayList al)
		{
			foreach (XmlSchemaObject xmlSchemaObject in this.all.CompiledItems)
			{
				XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
				if (!this.consumed.Contains(xmlSchemaParticle))
				{
					al.Add(xmlSchemaParticle);
				}
			}
		}

		public override XsdValidationState EvaluateStartElement(string localName, string ns)
		{
			if (this.all.CompiledItems.Count == 0)
			{
				return XsdValidationState.Invalid;
			}
			int i = 0;
			while (i < this.all.CompiledItems.Count)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.all.CompiledItems[i];
				if (xmlSchemaElement.QualifiedName.Name == localName && xmlSchemaElement.QualifiedName.Namespace == ns)
				{
					if (this.consumed.Contains(xmlSchemaElement))
					{
						return XsdValidationState.Invalid;
					}
					this.consumed.Add(xmlSchemaElement);
					base.Manager.CurrentElement = xmlSchemaElement;
					base.OccuredInternal = 1;
					return this;
				}
				else
				{
					i++;
				}
			}
			return XsdValidationState.Invalid;
		}

		public override bool EvaluateEndElement()
		{
			if (this.all.Emptiable || this.all.ValidatedMinOccurs == 0m)
			{
				return true;
			}
			if (this.all.ValidatedMinOccurs > 0m && this.consumed.Count == 0)
			{
				return false;
			}
			if (this.all.CompiledItems.Count == this.consumed.Count)
			{
				return true;
			}
			for (int i = 0; i < this.all.CompiledItems.Count; i++)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.all.CompiledItems[i];
				if (xmlSchemaElement.ValidatedMinOccurs > 0m && !this.consumed.Contains(xmlSchemaElement))
				{
					return false;
				}
			}
			return true;
		}

		internal override bool EvaluateIsEmptiable()
		{
			if (this.all.Emptiable || this.all.ValidatedMinOccurs == 0m)
			{
				return true;
			}
			for (int i = 0; i < this.all.CompiledItems.Count; i++)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.all.CompiledItems[i];
				if (xmlSchemaElement.ValidatedMinOccurs > 0m && !this.consumed.Contains(xmlSchemaElement))
				{
					return false;
				}
			}
			return true;
		}
	}
}
