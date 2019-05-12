using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	internal class SessionConfig : ISessionConfig
	{
		public const string kConfigEtag = "config_etag";

		private const string kEventsEndPointUrl = "https://api.uca.cloud.unity3d.com/v1/events";

		private const string kConfigEndPointUrl = "https://config.uca.cloud.unity3d.com";

		private const string kAnalytics = "analytics";

		private const string kEnabled = "enabled";

		private const string kSupports = "supports";

		private const string kEventsEndPoint = "events_end_point";

		private const string kConfigEndPoint = "config_end_point";

		private const string kLimits = "limits";

		private const string kMaxAppIdSize = "max_app_id_size";

		private const string kMaxUserIdSize = "max_user_id_size";

		private const string kMaxCurrencySize = "max_currency_size";

		private const string kMaxProductIdSize = "max_product_id_size";

		private const string kMaxCustomEventNameSize = "max_custom_event_name_size";

		private const string kSession = "session";

		private const string kResumeTimeout = "resume_timeout_in_sec";

		private const string kDispatchRetryInSec = "dispatch_retry_in_sec";

		private const string kGrouping = "grouping";

		private const string kMaxNumberInQueue = "max_num_in_queue";

		private const string kMaxNumberInGroup = "max_num_in_group";

		private const string kMinWaitNumberInGroup = "min_wait_num_in_group";

		private const string kMaxTimeoutInSec = "max_timeout_in_sec";

		private const string kEvents = "events";

		private const string kCustomEvent = "custom_event";

		private const string kCustomEventMaxEventPerHour = "max_event_per_hour";

		private const string kCustomEventMaxNumberOfItems = "max_num_of_items";

		private const string kCustomEventMaxTotalItemsSize = "max_total_items_size";

		private bool m_AnalyticsEnabled = true;

		private List<string> m_Supports;

		private string m_EventsEndPoint = "https://api.uca.cloud.unity3d.com/v1/events";

		private string m_ConfigEndPoint = "https://config.uca.cloud.unity3d.com";

		private int m_RequestNumberOfEvents = 1;

		private int m_MaxNumberOfEventInGroup = 30;

		private int m_MaxTimeoutForGrouping = 300;

		private int m_MaxNumberOfEventInQueue = 300;

		private int m_ResumeTimeoutInMillSeconds = 1800000;

		private int[] m_DispatcherWaitTimeInSeconds = new int[]
		{
			1,
			2,
			30,
			60,
			120,
			300
		};

		private int m_MaxAppIdSize = 100;

		private int m_MaxUserIdSize = 100;

		private int m_MaxCurrencySize = 20;

		private int m_MaxProductIdSize = 100;

		private int m_MaxCustomEventNameSize = 100;

		private int m_CustomEventMaxEventPerHour = 100;

		private int m_CustomEventMaxNumberOfItems = 10;

		private int m_CustomEventMaxTotalItemsSize = 500;

		private IPlatformWrapper m_PlatformWrapper;

		public SessionConfig(IPlatformWrapper platformWrapper, string configUrl, string eventUrl)
		{
			this.m_PlatformWrapper = platformWrapper;
			if (!string.IsNullOrEmpty(configUrl))
			{
				this.m_ConfigEndPoint = configUrl;
			}
			if (!string.IsNullOrEmpty(eventUrl))
			{
				this.m_EventsEndPoint = eventUrl;
			}
		}

		public bool analyticsEnabled
		{
			get
			{
				return this.m_AnalyticsEnabled;
			}
		}

		public string eventsEndPoint
		{
			get
			{
				return this.m_EventsEndPoint;
			}
		}

		public string configEndPoint
		{
			get
			{
				return this.m_ConfigEndPoint;
			}
		}

		public int requestNumberOfEvents
		{
			get
			{
				return this.m_RequestNumberOfEvents;
			}
		}

		public int maxNumberOfEventInGroup
		{
			get
			{
				return this.m_MaxNumberOfEventInGroup;
			}
		}

		public int maxTimeoutForGrouping
		{
			get
			{
				return this.m_MaxTimeoutForGrouping;
			}
		}

		public int maxNumberOfEventInQueue
		{
			get
			{
				return this.m_MaxNumberOfEventInQueue;
			}
		}

		public int resumeTimeoutInMillSeconds
		{
			get
			{
				return this.m_ResumeTimeoutInMillSeconds;
			}
		}

		public int[] dispatcherWaitTimeInSeconds
		{
			get
			{
				return this.m_DispatcherWaitTimeInSeconds;
			}
		}

		public int customEventMaxEventPerHour
		{
			get
			{
				return this.m_CustomEventMaxEventPerHour;
			}
		}

		public int maxAppIdSize
		{
			get
			{
				return this.m_MaxAppIdSize;
			}
		}

		public int maxUserIdSize
		{
			get
			{
				return this.m_MaxUserIdSize;
			}
		}

		public int maxCustomEventNameSize
		{
			get
			{
				return this.m_MaxCustomEventNameSize;
			}
		}

		public int maxCurrencySize
		{
			get
			{
				return this.m_MaxCurrencySize;
			}
		}

		public int maxProductIdSize
		{
			get
			{
				return this.m_MaxProductIdSize;
			}
		}

		public int customEventMaxNumberOfItems
		{
			get
			{
				return this.m_CustomEventMaxNumberOfItems;
			}
		}

		public int customEventMaxTotalItemsSize
		{
			get
			{
				return this.m_CustomEventMaxTotalItemsSize;
			}
		}

		public bool Restore(string jsonData)
		{
			if (string.IsNullOrEmpty(jsonData))
			{
				return false;
			}
			try
			{
				SessionValues sessionValues = new SessionValues(jsonData);
				SessionValues sessionValues2 = sessionValues.TryGetValue("analytics");
				if (sessionValues2 != null)
				{
					this.RestoreAnalyticsConfig(sessionValues2);
				}
				return true;
			}
			catch (ArgumentNullException)
			{
			}
			catch (InvalidCastException)
			{
			}
			return false;
		}

		public void RestoreAnalyticsConfig(SessionValues analyticsVal)
		{
			this.m_AnalyticsEnabled = analyticsVal.TryGetBool("enabled", this.m_AnalyticsEnabled);
			this.m_EventsEndPoint = analyticsVal.TryGetString("events_end_point", this.m_EventsEndPoint);
			this.m_Supports = analyticsVal.TryGetStringList("supports", null);
			if (this.m_AnalyticsEnabled && this.m_Supports != null && this.m_Supports.Count != 0)
			{
				string platformName = this.m_PlatformWrapper.platformName;
				bool analyticsEnabled = false;
				foreach (string value in this.m_Supports)
				{
					if (platformName.Equals(value))
					{
						analyticsEnabled = true;
						break;
					}
				}
				this.m_AnalyticsEnabled = analyticsEnabled;
			}
			SessionValues sessionValues = analyticsVal.TryGetValue("limits");
			if (sessionValues != null)
			{
				this.RestoreLimitsConfig(sessionValues);
			}
			SessionValues sessionValues2 = analyticsVal.TryGetValue("session");
			if (sessionValues2 != null)
			{
				this.RestoreSessionConfig(sessionValues2);
			}
			SessionValues sessionValues3 = analyticsVal.TryGetValue("events");
			if (sessionValues3 != null)
			{
				this.RestoreEventsConfig(sessionValues3);
			}
		}

		public void RestoreEventsConfig(SessionValues eventsVal)
		{
			SessionValues sessionValues = eventsVal.TryGetValue("custom_event");
			if (sessionValues != null)
			{
				this.RestoreCustomEventConfig(sessionValues);
			}
		}

		public void RestoreCustomEventConfig(SessionValues customEventVal)
		{
			this.m_CustomEventMaxEventPerHour = customEventVal.TryGetInt("max_event_per_hour", this.m_CustomEventMaxEventPerHour);
			this.m_CustomEventMaxNumberOfItems = customEventVal.TryGetInt("max_num_of_items", this.m_CustomEventMaxNumberOfItems);
			this.m_CustomEventMaxTotalItemsSize = customEventVal.TryGetInt("max_total_items_size", this.m_CustomEventMaxTotalItemsSize);
		}

		public void RestoreSessionConfig(SessionValues sessionVal)
		{
			int num = sessionVal.TryGetInt("resume_timeout_in_sec", 0);
			if (num != 0)
			{
				this.m_ResumeTimeoutInMillSeconds = num * 1000;
			}
			this.m_DispatcherWaitTimeInSeconds = sessionVal.TryGetIntArray("dispatch_retry_in_sec", this.m_DispatcherWaitTimeInSeconds);
			SessionValues sessionValues = sessionVal.TryGetValue("grouping");
			if (sessionValues != null)
			{
				this.RestoreGroupingConfig(sessionValues);
			}
		}

		public void RestoreGroupingConfig(SessionValues groupingVal)
		{
			this.m_MaxNumberOfEventInQueue = groupingVal.TryGetInt("max_num_in_queue", this.m_MaxNumberOfEventInQueue);
			this.m_MaxNumberOfEventInGroup = groupingVal.TryGetInt("max_num_in_group", this.m_MaxNumberOfEventInGroup);
			this.m_RequestNumberOfEvents = groupingVal.TryGetInt("min_wait_num_in_group", this.m_RequestNumberOfEvents);
			this.m_MaxTimeoutForGrouping = groupingVal.TryGetInt("max_timeout_in_sec", this.m_MaxTimeoutForGrouping);
		}

		public void RestoreLimitsConfig(SessionValues stringLimitsVal)
		{
			this.m_MaxAppIdSize = stringLimitsVal.TryGetInt("max_app_id_size", this.m_MaxAppIdSize);
			this.m_MaxUserIdSize = stringLimitsVal.TryGetInt("max_user_id_size", this.m_MaxUserIdSize);
			this.m_MaxCurrencySize = stringLimitsVal.TryGetInt("max_currency_size", this.m_MaxCurrencySize);
			this.m_MaxProductIdSize = stringLimitsVal.TryGetInt("max_product_id_size", this.m_MaxProductIdSize);
			this.m_MaxCustomEventNameSize = stringLimitsVal.TryGetInt("max_custom_event_name_size", this.m_MaxCustomEventNameSize);
		}
	}
}
