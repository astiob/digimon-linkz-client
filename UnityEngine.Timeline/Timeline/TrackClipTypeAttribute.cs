using System;

namespace UnityEngine.Timeline
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class TrackClipTypeAttribute : Attribute
	{
		public readonly Type inspectedType;

		public TrackClipTypeAttribute(Type clipClass)
		{
			this.inspectedType = clipClass;
		}
	}
}
