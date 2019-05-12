using System;
using System.Collections;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdAnyValidationState : XsdValidationState
	{
		private readonly XmlSchemaAny any;

		public XsdAnyValidationState(XmlSchemaAny any, XsdParticleStateManager manager) : base(manager)
		{
			this.any = any;
		}

		public override void GetExpectedParticles(ArrayList al)
		{
			al.Add(this.any);
		}

		public override XsdValidationState EvaluateStartElement(string name, string ns)
		{
			if (!this.MatchesNamespace(ns))
			{
				return XsdValidationState.Invalid;
			}
			base.OccuredInternal++;
			base.Manager.SetProcessContents(this.any.ResolvedProcessContents);
			if (base.Occured > this.any.ValidatedMaxOccurs)
			{
				return XsdValidationState.Invalid;
			}
			if (base.Occured == this.any.ValidatedMaxOccurs)
			{
				return base.Manager.Create(XmlSchemaParticle.Empty);
			}
			return this;
		}

		private bool MatchesNamespace(string ns)
		{
			if (this.any.HasValueAny)
			{
				return true;
			}
			if (this.any.HasValueLocal && ns == string.Empty)
			{
				return true;
			}
			if (this.any.HasValueOther && (this.any.TargetNamespace == string.Empty || this.any.TargetNamespace != ns))
			{
				return true;
			}
			if (this.any.HasValueTargetNamespace && this.any.TargetNamespace == ns)
			{
				return true;
			}
			for (int i = 0; i < this.any.ResolvedNamespaces.Count; i++)
			{
				if (this.any.ResolvedNamespaces[i] == ns)
				{
					return true;
				}
			}
			return false;
		}

		public override bool EvaluateEndElement()
		{
			return this.EvaluateIsEmptiable();
		}

		internal override bool EvaluateIsEmptiable()
		{
			return this.any.ValidatedMinOccurs <= base.Occured && this.any.ValidatedMaxOccurs >= base.Occured;
		}
	}
}
