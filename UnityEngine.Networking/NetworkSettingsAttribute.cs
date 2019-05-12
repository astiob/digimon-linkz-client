using System;

namespace UnityEngine.Networking
{
	[AttributeUsage(AttributeTargets.Class)]
	public class NetworkSettingsAttribute : Attribute
	{
		public int channel = 0;

		public float sendInterval = 0.1f;
	}
}
