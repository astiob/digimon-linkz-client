using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	[AttributeUsage(AttributeTargets.Class)]
	public class DefaultExecutionOrder : Attribute
	{
		public DefaultExecutionOrder(int order)
		{
			this.order = order;
		}

		public int order { get; private set; }
	}
}
