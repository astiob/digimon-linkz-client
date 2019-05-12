using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class CustomEvent : AnalyticsEvent
	{
		public const string kEventCustom = "custom";

		public const string kCustomEventName = "name";

		public const string kCustomParams = "custom_params";

		public const string kCustomEventId = "eventid";

		public const string kCustomEventCount = "custom_event_count";

		public const string kCustomEventTime = "custom_event_time";

		public const string kUnityCustomEventNamePrefix = "unity.";

		public const string kEventSceneLoad = "unity.sceneLoad";

		public const string kSceneLevelNumber = "level_num";

		public const string kSceneLevelName = "level_name";

		public const string kSceneTotalLevels = "total_levels";

		public CustomEvent(string customEventName) : base("custom", CloudEventFlags.None)
		{
			base.SetParameter("name", customEventName);
		}

		public void SetEventData(IDictionary<string, object> eventData)
		{
			if (eventData != null)
			{
				base.SetParameter("custom_params", eventData);
			}
		}
	}
}
