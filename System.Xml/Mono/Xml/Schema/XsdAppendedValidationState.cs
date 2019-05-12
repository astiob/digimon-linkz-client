using System;
using System.Collections;

namespace Mono.Xml.Schema
{
	internal class XsdAppendedValidationState : XsdValidationState
	{
		private XsdValidationState head;

		private XsdValidationState rest;

		public XsdAppendedValidationState(XsdParticleStateManager manager, XsdValidationState head, XsdValidationState rest) : base(manager)
		{
			this.head = head;
			this.rest = rest;
		}

		public override void GetExpectedParticles(ArrayList al)
		{
			this.head.GetExpectedParticles(al);
			this.rest.GetExpectedParticles(al);
		}

		public override XsdValidationState EvaluateStartElement(string name, string ns)
		{
			XsdValidationState xsdValidationState = this.head.EvaluateStartElement(name, ns);
			if (xsdValidationState != XsdValidationState.Invalid)
			{
				this.head = xsdValidationState;
				return (!(xsdValidationState is XsdEmptyValidationState)) ? this : this.rest;
			}
			if (!this.head.EvaluateIsEmptiable())
			{
				return XsdValidationState.Invalid;
			}
			return this.rest.EvaluateStartElement(name, ns);
		}

		public override bool EvaluateEndElement()
		{
			if (this.head.EvaluateEndElement())
			{
				return this.rest.EvaluateIsEmptiable();
			}
			return this.head.EvaluateIsEmptiable() && this.rest.EvaluateEndElement();
		}

		internal override bool EvaluateIsEmptiable()
		{
			return this.head.EvaluateIsEmptiable() && this.rest.EvaluateIsEmptiable();
		}
	}
}
