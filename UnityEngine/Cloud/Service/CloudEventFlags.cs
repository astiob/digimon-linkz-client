using System;

namespace UnityEngine.Cloud.Service
{
	[Flags]
	internal enum CloudEventFlags
	{
		None = 0,
		HighPriority = 1,
		CacheImmediately = 2
	}
}
