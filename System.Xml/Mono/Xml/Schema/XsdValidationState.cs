using System;
using System.Collections;

namespace Mono.Xml.Schema
{
	internal abstract class XsdValidationState
	{
		private static XsdInvalidValidationState invalid = new XsdInvalidValidationState(null);

		private int occured;

		private readonly XsdParticleStateManager manager;

		public XsdValidationState(XsdParticleStateManager manager)
		{
			this.manager = manager;
		}

		public static XsdInvalidValidationState Invalid
		{
			get
			{
				return XsdValidationState.invalid;
			}
		}

		public abstract XsdValidationState EvaluateStartElement(string localName, string ns);

		public abstract bool EvaluateEndElement();

		internal abstract bool EvaluateIsEmptiable();

		public abstract void GetExpectedParticles(ArrayList al);

		public XsdParticleStateManager Manager
		{
			get
			{
				return this.manager;
			}
		}

		public int Occured
		{
			get
			{
				return this.occured;
			}
		}

		internal int OccuredInternal
		{
			get
			{
				return this.occured;
			}
			set
			{
				this.occured = value;
			}
		}
	}
}
