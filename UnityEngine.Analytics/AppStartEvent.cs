using System;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class AppStartEvent : AnalyticsEvent
	{
		public AppStartEvent() : base("appStart", CloudEventFlags.HighPriority)
		{
		}
	}
}
