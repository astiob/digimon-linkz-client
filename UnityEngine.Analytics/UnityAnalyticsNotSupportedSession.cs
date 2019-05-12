using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	internal class UnityAnalyticsNotSupportedSession : IUnityAnalyticsSession
	{
		public string GetPathName()
		{
			return "UnsupportedPlatform";
		}

		public AnalyticsResult StartWithAppId(string appId, ICloudService cloudService, string configUrl, string eventUrl)
		{
			return AnalyticsResult.UnsupportedPlatform;
		}

		public AnalyticsResult SetCustomUserId(string userId)
		{
			return AnalyticsResult.UnsupportedPlatform;
		}

		public AnalyticsResult SetUserGender(Gender gender)
		{
			return AnalyticsResult.UnsupportedPlatform;
		}

		public AnalyticsResult SetUserBirthYear(int birthYear)
		{
			return AnalyticsResult.UnsupportedPlatform;
		}

		public AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			return AnalyticsResult.UnsupportedPlatform;
		}

		public AnalyticsResult SendCustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			return AnalyticsResult.UnsupportedPlatform;
		}
	}
}
