using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdElementValidationState : XsdValidationState
	{
		private readonly XmlSchemaElement element;

		public XsdElementValidationState(XmlSchemaElement element, XsdParticleStateManager manager) : base(manager)
		{
			this.element = element;
		}

		private string Name
		{
			get
			{
				return this.element.QualifiedName.Name;
			}
		}

		private string NS
		{
			get
			{
				return this.element.QualifiedName.Namespace;
			}
		}

		public override void GetExpectedParticles(ArrayList al)
		{
			XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)base.MemberwiseClone();
			decimal num = this.element.ValidatedMinOccurs - base.Occured;
			xmlSchemaElement.MinOccurs = ((!(num > 0m)) ? 0m : num);
			if (this.element.ValidatedMaxOccurs == 79228162514264337593543950335m)
			{
				xmlSchemaElement.MaxOccursString = "unbounded";
			}
			else
			{
				xmlSchemaElement.MaxOccurs = this.element.ValidatedMaxOccurs - base.Occured;
			}
			al.Add(xmlSchemaElement);
		}

		public override XsdValidationState EvaluateStartElement(string name, string ns)
		{
			if (this.Name == name && this.NS == ns && !this.element.IsAbstract)
			{
				return this.CheckOccurence(this.element);
			}
			for (int i = 0; i < this.element.SubstitutingElements.Count; i++)
			{
				XmlSchemaElement xmlSchemaElement = (XmlSchemaElement)this.element.SubstitutingElements[i];
				if (xmlSchemaElement.QualifiedName.Name == name && xmlSchemaElement.QualifiedName.Namespace == ns)
				{
					return this.CheckOccurence(xmlSchemaElement);
				}
			}
			return XsdValidationState.Invalid;
		}

		private XsdValidationState CheckOccurence(XmlSchemaElement maybeSubstituted)
		{
			base.OccuredInternal++;
			base.Manager.CurrentElement = maybeSubstituted;
			if (base.Occured > this.element.ValidatedMaxOccurs)
			{
				return XsdValidationState.Invalid;
			}
			if (base.Occured == this.element.ValidatedMaxOccurs)
			{
				return base.Manager.Create(XmlSchemaParticle.Empty);
			}
			return this;
		}

		public override bool EvaluateEndElement()
		{
			return this.EvaluateIsEmptiable();
		}

		internal override bool EvaluateIsEmptiable()
		{
			return this.element.ValidatedMinOccurs <= base.Occured && this.element.ValidatedMaxOccurs >= base.Occured;
		}
	}
}
