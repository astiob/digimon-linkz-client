using System;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class AppRunningEvent : AnalyticsEvent
	{
		public const string kEventAppRunning = "appRunning";

		public const string kDuration = "duration";

		public const string kClicks = "clicks";

		public const string kKeyPresses = "keys";

		public AppRunningEvent(long runningDuration, long keyStrokes, long clicks) : base("appRunning", CloudEventFlags.None)
		{
			if (clicks > 0L)
			{
				base.SetParameter("clicks", clicks);
			}
			if (keyStrokes > 0L)
			{
				base.SetParameter("keys", keyStrokes);
			}
			base.SetParameter("duration", runningDuration);
		}
	}
}
