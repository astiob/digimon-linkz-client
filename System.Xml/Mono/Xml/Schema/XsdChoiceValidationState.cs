using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdChoiceValidationState : XsdValidationState
	{
		private readonly XmlSchemaChoice choice;

		private bool emptiable;

		private bool emptiableComputed;

		public XsdChoiceValidationState(XmlSchemaChoice choice, XsdParticleStateManager manager) : base(manager)
		{
			this.choice = choice;
		}

		public override void GetExpectedParticles(ArrayList al)
		{
			if (base.Occured < this.choice.ValidatedMaxOccurs)
			{
				foreach (XmlSchemaObject xmlSchemaObject in this.choice.CompiledItems)
				{
					XmlSchemaParticle value = (XmlSchemaParticle)xmlSchemaObject;
					al.Add(value);
				}
			}
		}

		public override XsdValidationState EvaluateStartElement(string localName, string ns)
		{
			this.emptiableComputed = false;
			bool flag = true;
			int i = 0;
			while (i < this.choice.CompiledItems.Count)
			{
				XmlSchemaParticle xsobj = (XmlSchemaParticle)this.choice.CompiledItems[i];
				XsdValidationState xsdValidationState = base.Manager.Create(xsobj);
				XsdValidationState xsdValidationState2 = xsdValidationState.EvaluateStartElement(localName, ns);
				if (xsdValidationState2 != XsdValidationState.Invalid)
				{
					base.OccuredInternal++;
					if (base.Occured > this.choice.ValidatedMaxOccurs)
					{
						return XsdValidationState.Invalid;
					}
					if (base.Occured == this.choice.ValidatedMaxOccurs)
					{
						return xsdValidationState2;
					}
					return base.Manager.MakeSequence(xsdValidationState2, this);
				}
				else
				{
					if (!this.emptiableComputed)
					{
						flag &= xsdValidationState.EvaluateIsEmptiable();
					}
					i++;
				}
			}
			if (!this.emptiableComputed)
			{
				if (flag)
				{
					this.emptiable = true;
				}
				if (!this.emptiable)
				{
					this.emptiable = (this.choice.ValidatedMinOccurs <= base.Occured);
				}
				this.emptiableComputed = true;
			}
			return XsdValidationState.Invalid;
		}

		public override bool EvaluateEndElement()
		{
			this.emptiableComputed = false;
			if (this.choice.ValidatedMinOccurs > base.Occured + 1)
			{
				return false;
			}
			if (this.choice.ValidatedMinOccurs <= base.Occured)
			{
				return true;
			}
			for (int i = 0; i < this.choice.CompiledItems.Count; i++)
			{
				XmlSchemaParticle xsobj = (XmlSchemaParticle)this.choice.CompiledItems[i];
				if (base.Manager.Create(xsobj).EvaluateIsEmptiable())
				{
					return true;
				}
			}
			return false;
		}

		internal override bool EvaluateIsEmptiable()
		{
			if (this.emptiableComputed)
			{
				return this.emptiable;
			}
			if (this.choice.ValidatedMaxOccurs < base.Occured)
			{
				return false;
			}
			if (this.choice.ValidatedMinOccurs > base.Occured + 1)
			{
				return false;
			}
			int num = base.Occured;
			while (num < this.choice.ValidatedMinOccurs)
			{
				bool flag = false;
				for (int i = 0; i < this.choice.CompiledItems.Count; i++)
				{
					XmlSchemaParticle xsobj = (XmlSchemaParticle)this.choice.CompiledItems[i];
					if (base.Manager.Create(xsobj).EvaluateIsEmptiable())
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				num++;
			}
			return true;
		}
	}
}
