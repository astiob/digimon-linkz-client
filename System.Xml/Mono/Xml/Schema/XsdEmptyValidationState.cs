using System;
using System.Collections;

namespace Mono.Xml.Schema
{
	internal class XsdEmptyValidationState : XsdValidationState
	{
		public XsdEmptyValidationState(XsdParticleStateManager manager) : base(manager)
		{
		}

		public override void GetExpectedParticles(ArrayList al)
		{
		}

		public override XsdValidationState EvaluateStartElement(string name, string ns)
		{
			return XsdValidationState.Invalid;
		}

		public override bool EvaluateEndElement()
		{
			return true;
		}

		internal override bool EvaluateIsEmptiable()
		{
			return true;
		}
	}
}
