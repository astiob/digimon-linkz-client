using System;
using System.Collections;

namespace Mono.Xml.Schema
{
	internal class XsdInvalidValidationState : XsdValidationState
	{
		internal XsdInvalidValidationState(XsdParticleStateManager manager) : base(manager)
		{
		}

		public override void GetExpectedParticles(ArrayList al)
		{
		}

		public override XsdValidationState EvaluateStartElement(string name, string ns)
		{
			return this;
		}

		public override bool EvaluateEndElement()
		{
			return false;
		}

		internal override bool EvaluateIsEmptiable()
		{
			return false;
		}
	}
}
