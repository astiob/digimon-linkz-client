using System;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
	[RequiredByNativeCode]
	public enum AnalyticsSessionState
	{
		kSessionStopped,
		kSessionStarted,
		kSessionPaused,
		kSessionResumed
	}
}
