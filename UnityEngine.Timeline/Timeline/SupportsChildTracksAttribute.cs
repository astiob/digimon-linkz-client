using System;

namespace UnityEngine.Timeline
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	internal class SupportsChildTracksAttribute : Attribute
	{
		public readonly Type childType;

		public readonly int levels;

		public SupportsChildTracksAttribute(Type childType = null, int levels = 2147483647)
		{
			this.childType = childType;
			this.levels = levels;
		}
	}
}
