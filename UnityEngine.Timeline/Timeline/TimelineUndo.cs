using System;
using System.Diagnostics;

namespace UnityEngine.Timeline
{
	internal static class TimelineUndo
	{
		public static void PushDestroyUndo(TimelineAsset timeline, Object thingToDirty, Object objectToDestroy, string operation)
		{
			if (objectToDestroy != null)
			{
				Object.Destroy(objectToDestroy);
			}
		}

		[Conditional("UNITY_EDITOR")]
		public static void PushUndo(Object thingToDirty, string operation)
		{
		}
	}
}
