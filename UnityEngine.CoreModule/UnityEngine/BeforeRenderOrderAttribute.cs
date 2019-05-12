using System;

namespace UnityEngine
{
	[AttributeUsage(AttributeTargets.Method)]
	public class BeforeRenderOrderAttribute : Attribute
	{
		public BeforeRenderOrderAttribute(int order)
		{
			this.order = order;
		}

		public int order { get; private set; }
	}
}
