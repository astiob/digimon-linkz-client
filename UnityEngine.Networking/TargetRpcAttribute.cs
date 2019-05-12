using System;

namespace UnityEngine.Networking
{
	[AttributeUsage(AttributeTargets.Method)]
	public class TargetRpcAttribute : Attribute
	{
		public int channel = 0;
	}
}
