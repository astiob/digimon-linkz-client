using SimpleJson;
using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	internal class SessionInfo
	{
		public SessionInfo(string appId, string userId, string platformName)
		{
			this.appId = appId;
			this.userId = userId;
			this.sdkVersion = UnityAnalyticsVersion.sdkVersion;
			this.debugDevice = false;
			this.platformName = platformName;
		}

		public string appId { get; set; }

		public string userId { get; set; }

		public long sessionId { get; set; }

		public string sdkVersion { get; set; }

		public bool debugDevice { get; set; }

		public string platformName { get; set; }

		public string ToStringJson()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["appid"] = this.appId;
			dictionary["userid"] = this.userId;
			dictionary["sessionid"] = this.sessionId;
			dictionary["platform"] = this.platformName;
			dictionary["sdk_ver"] = this.sdkVersion;
			if (this.debugDevice)
			{
				dictionary["debug_device"] = this.debugDevice;
			}
			return SimpleJson.SerializeObject(dictionary);
		}
	}
}
