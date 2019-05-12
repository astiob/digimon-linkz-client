using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdSequenceValidationState : XsdValidationState
	{
		private readonly XmlSchemaSequence seq;

		private int current;

		private XsdValidationState currentAutomata;

		private bool emptiable;

		public XsdSequenceValidationState(XmlSchemaSequence sequence, XsdParticleStateManager manager) : base(manager)
		{
			this.seq = sequence;
			this.current = -1;
		}

		public override void GetExpectedParticles(ArrayList al)
		{
			if (this.currentAutomata == null)
			{
				foreach (XmlSchemaObject xmlSchemaObject in this.seq.CompiledItems)
				{
					XmlSchemaParticle xmlSchemaParticle = (XmlSchemaParticle)xmlSchemaObject;
					al.Add(xmlSchemaParticle);
					if (!xmlSchemaParticle.ValidateIsEmptiable())
					{
						break;
					}
				}
				return;
			}
			if (this.currentAutomata != null)
			{
				this.currentAutomata.GetExpectedParticles(al);
				if (!this.currentAutomata.EvaluateIsEmptiable())
				{
					return;
				}
				for (int i = this.current + 1; i < this.seq.CompiledItems.Count; i++)
				{
					XmlSchemaParticle xmlSchemaParticle2 = this.seq.CompiledItems[i] as XmlSchemaParticle;
					al.Add(xmlSchemaParticle2);
					if (!xmlSchemaParticle2.ValidateIsEmptiable())
					{
						break;
					}
				}
			}
			if (base.Occured + 1 == this.seq.ValidatedMaxOccurs)
			{
				return;
			}
			for (int j = 0; j <= this.current; j++)
			{
				al.Add(this.seq.CompiledItems[j]);
			}
		}

		public override XsdValidationState EvaluateStartElement(string name, string ns)
		{
			if (this.seq.CompiledItems.Count == 0)
			{
				return XsdValidationState.Invalid;
			}
			int num = (this.current >= 0) ? this.current : 0;
			XsdValidationState xsdValidationState = this.currentAutomata;
			bool flag = false;
			XsdValidationState xsdValidationState2;
			for (;;)
			{
				if (xsdValidationState == null)
				{
					xsdValidationState = base.Manager.Create(this.seq.CompiledItems[num] as XmlSchemaParticle);
					flag = true;
				}
				if (xsdValidationState is XsdEmptyValidationState && this.seq.CompiledItems.Count == num + 1 && base.Occured == this.seq.ValidatedMaxOccurs)
				{
					break;
				}
				xsdValidationState2 = xsdValidationState.EvaluateStartElement(name, ns);
				if (xsdValidationState2 != XsdValidationState.Invalid)
				{
					goto IL_E1;
				}
				if (!xsdValidationState.EvaluateIsEmptiable())
				{
					goto Block_8;
				}
				num++;
				if (num > this.current && flag && this.current >= 0)
				{
					goto Block_13;
				}
				if (this.seq.CompiledItems.Count > num)
				{
					xsdValidationState = base.Manager.Create(this.seq.CompiledItems[num] as XmlSchemaParticle);
				}
				else
				{
					if (this.current < 0)
					{
						goto Block_15;
					}
					num = 0;
					xsdValidationState = null;
				}
			}
			return XsdValidationState.Invalid;
			Block_8:
			this.emptiable = false;
			return XsdValidationState.Invalid;
			IL_E1:
			this.current = num;
			this.currentAutomata = xsdValidationState2;
			if (flag)
			{
				base.OccuredInternal++;
				if (base.Occured > this.seq.ValidatedMaxOccurs)
				{
					return XsdValidationState.Invalid;
				}
			}
			return this;
			Block_13:
			return XsdValidationState.Invalid;
			Block_15:
			return XsdValidationState.Invalid;
		}

		public override bool EvaluateEndElement()
		{
			if (this.seq.ValidatedMinOccurs > base.Occured + 1)
			{
				return false;
			}
			if (this.seq.CompiledItems.Count == 0)
			{
				return true;
			}
			if (this.currentAutomata == null && this.seq.ValidatedMinOccurs <= base.Occured)
			{
				return true;
			}
			int num = (this.current >= 0) ? this.current : 0;
			XsdValidationState xsdValidationState = this.currentAutomata;
			if (xsdValidationState == null)
			{
				xsdValidationState = base.Manager.Create(this.seq.CompiledItems[num] as XmlSchemaParticle);
			}
			while (xsdValidationState != null)
			{
				if (!xsdValidationState.EvaluateEndElement() && !xsdValidationState.EvaluateIsEmptiable())
				{
					return false;
				}
				num++;
				if (this.seq.CompiledItems.Count > num)
				{
					xsdValidationState = base.Manager.Create(this.seq.CompiledItems[num] as XmlSchemaParticle);
				}
				else
				{
					xsdValidationState = null;
				}
			}
			if (this.current < 0)
			{
				base.OccuredInternal++;
			}
			return this.seq.ValidatedMinOccurs <= base.Occured && this.seq.ValidatedMaxOccurs >= base.Occured;
		}

		internal override bool EvaluateIsEmptiable()
		{
			if (this.seq.ValidatedMinOccurs > base.Occured + 1)
			{
				return false;
			}
			if (this.seq.ValidatedMinOccurs == 0m && this.currentAutomata == null)
			{
				return true;
			}
			if (this.emptiable)
			{
				return true;
			}
			if (this.seq.CompiledItems.Count == 0)
			{
				return true;
			}
			int num = (this.current >= 0) ? this.current : 0;
			XsdValidationState xsdValidationState = this.currentAutomata;
			if (xsdValidationState == null)
			{
				xsdValidationState = base.Manager.Create(this.seq.CompiledItems[num] as XmlSchemaParticle);
			}
			while (xsdValidationState != null)
			{
				if (!xsdValidationState.EvaluateIsEmptiable())
				{
					return false;
				}
				num++;
				if (this.seq.CompiledItems.Count > num)
				{
					xsdValidationState = base.Manager.Create(this.seq.CompiledItems[num] as XmlSchemaParticle);
				}
				else
				{
					xsdValidationState = null;
				}
			}
			this.emptiable = true;
			return true;
		}
	}
}
