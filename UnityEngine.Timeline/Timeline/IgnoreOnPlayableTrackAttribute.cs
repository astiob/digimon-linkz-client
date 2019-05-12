using System;

namespace UnityEngine.Timeline
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	internal class IgnoreOnPlayableTrackAttribute : Attribute
	{
	}
}
