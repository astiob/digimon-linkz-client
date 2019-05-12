using SimpleJson;
using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal abstract class AnalyticsEvent
	{
		public const string kEventType = "type";

		public const string kApplicationId = "appid";

		public const string kUserId = "userid";

		public const string kSessionId = "sessionid";

		public const string kTimeStamp = "ts";

		public const string kPlatformName = "platform";

		public const string kSdkVersion = "sdk_ver";

		public const string kEvents = "events";

		public const string kCommonHeader = "header";

		public const string kDebugDevice = "debug_device";

		public const string kEventAppStart = "appStart";

		private Dictionary<string, object> m_Parameters;

		private CloudEventFlags m_Flags;

		private AnalyticsEvent()
		{
		}

		protected AnalyticsEvent(string eventName, CloudEventFlags eventFlags)
		{
			this.m_Flags = eventFlags;
			this.m_Parameters = new Dictionary<string, object>();
			this.m_Parameters["type"] = eventName;
			this.m_Parameters["ts"] = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
		}

		public CloudEventFlags flags
		{
			get
			{
				return this.m_Flags;
			}
		}

		protected void SetParameter(string key, object value)
		{
			this.m_Parameters[key] = value;
		}

		public IDictionary<string, object> GetParams()
		{
			return this.m_Parameters;
		}

		public string ToStringJSON()
		{
			return SimpleJson.SerializeObject(this.m_Parameters);
		}

		public bool HighPriority
		{
			get
			{
				return (this.m_Flags & CloudEventFlags.HighPriority) != CloudEventFlags.None;
			}
		}

		public bool CacheImmediately
		{
			get
			{
				return (this.m_Flags & CloudEventFlags.CacheImmediately) != CloudEventFlags.None;
			}
		}
	}
}
