using System;

namespace UnityEngine.Timeline
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TrackBindingTypeAttribute : Attribute
	{
		public readonly Type type;

		public TrackBindingTypeAttribute(Type type)
		{
			this.type = type;
		}
	}
}
