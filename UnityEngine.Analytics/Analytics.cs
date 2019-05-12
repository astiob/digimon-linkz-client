using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	public static class Analytics
	{
		private static IUnityAnalyticsSession s_SessionImpl;

		[RuntimeInitializeOnLoadMethod]
		internal static void AutoStartUnityAnalytics()
		{
			if (!Application.isWebPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer)
			{
				Analytics.InternalStartSession(Application.cloudProjectId);
			}
		}

		internal static AnalyticsResult InternalStartSession(string appId)
		{
			string configUrl = null;
			string eventUrl = null;
			bool flag = UnityAnalyticsManager.enabled && Application.isHumanControllingUs;
			if (UnityAnalyticsManager.testMode)
			{
				configUrl = UnityAnalyticsManager.testConfigUrl;
				eventUrl = UnityAnalyticsManager.testEventUrl;
				flag = true;
			}
			if (flag)
			{
				return Analytics.InternalStartSession(appId, new AnalyticsCloudService(), configUrl, eventUrl);
			}
			return AnalyticsResult.AnalyticsDisabled;
		}

		internal static AnalyticsResult InternalStartSession(string appId, ICloudService cloudService, string configUrl, string eventUrl)
		{
			return Analytics.GetSingleton().StartWithAppId(appId, cloudService, configUrl, eventUrl);
		}

		internal static string GetPathName()
		{
			if (Analytics.s_SessionImpl == null)
			{
				return "NotInitialized";
			}
			return Analytics.s_SessionImpl.GetPathName();
		}

		public static AnalyticsResult SetUserId(string userId)
		{
			if (Analytics.s_SessionImpl == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return Analytics.s_SessionImpl.SetCustomUserId(userId);
		}

		public static AnalyticsResult SetUserGender(Gender gender)
		{
			if (Analytics.s_SessionImpl == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return Analytics.s_SessionImpl.SetUserGender(gender);
		}

		public static AnalyticsResult SetUserBirthYear(int birthYear)
		{
			if (Analytics.s_SessionImpl == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return Analytics.s_SessionImpl.SetUserBirthYear(birthYear);
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency)
		{
			if (Analytics.s_SessionImpl == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return Analytics.s_SessionImpl.Transaction(productId, amount, currency, null, null);
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			if (Analytics.s_SessionImpl == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return Analytics.s_SessionImpl.Transaction(productId, amount, currency, receiptPurchaseData, signature);
		}

		public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			if (Analytics.s_SessionImpl == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return Analytics.s_SessionImpl.SendCustomEvent(customEventName, eventData);
		}

		private static IUnityAnalyticsSession GetSingleton()
		{
			if (Analytics.s_SessionImpl == null)
			{
				if (SessionImpl.IsAnalyticsSupportedForPlatform())
				{
					IPlatformWrapper platform = PlatformWrapper.platform;
					Analytics.s_SessionImpl = new SessionImpl(platform);
					GameObserver.CreateComponent(platform, (SessionImpl)Analytics.s_SessionImpl);
				}
				else
				{
					Analytics.s_SessionImpl = new UnityAnalyticsNotSupportedSession();
				}
			}
			return Analytics.s_SessionImpl;
		}
	}
}
