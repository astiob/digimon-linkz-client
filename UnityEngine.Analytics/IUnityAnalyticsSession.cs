using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	internal interface IUnityAnalyticsSession
	{
		string GetPathName();

		AnalyticsResult StartWithAppId(string appId, ICloudService cloudService, string configUrl, string eventUrl);

		AnalyticsResult SetCustomUserId(string userId);

		AnalyticsResult SetUserGender(Gender gender);

		AnalyticsResult SetUserBirthYear(int birthYear);

		AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature);

		AnalyticsResult SendCustomEvent(string customEventName, IDictionary<string, object> eventData);
	}
}
