using System;
using UnityEngine;

namespace com.adjust.sdk
{
	public class Adjust : MonoBehaviour
	{
		private const string errorMessage = "adjust: SDK not started. Start it manually using the 'start' method.";

		private static IAdjust instance;

		private static Action<string> deferredDeeplinkDelegate;

		private static Action<AdjustEventSuccess> eventSuccessDelegate;

		private static Action<AdjustEventFailure> eventFailureDelegate;

		private static Action<AdjustSessionSuccess> sessionSuccessDelegate;

		private static Action<AdjustSessionFailure> sessionFailureDelegate;

		private static Action<AdjustAttribution> attributionChangedDelegate;

		public bool startManually = true;

		public bool eventBuffering;

		public bool printAttribution = true;

		public bool sendInBackground;

		public bool launchDeferredDeeplink = true;

		public string appToken = "{Your App Token}";

		public AdjustLogLevel logLevel = AdjustLogLevel.Info;

		public AdjustEnvironment environment;

		private void Awake()
		{
			if (Adjust.instance != null)
			{
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			if (!this.startManually)
			{
				AdjustConfig adjustConfig = new AdjustConfig(this.appToken, this.environment, this.logLevel == AdjustLogLevel.Suppress);
				adjustConfig.setLogLevel(this.logLevel);
				adjustConfig.setSendInBackground(this.sendInBackground);
				adjustConfig.setEventBufferingEnabled(this.eventBuffering);
				adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
				if (this.printAttribution)
				{
					adjustConfig.setEventSuccessDelegate(new Action<AdjustEventSuccess>(this.EventSuccessCallback), "Adjust");
					adjustConfig.setEventFailureDelegate(new Action<AdjustEventFailure>(this.EventFailureCallback), "Adjust");
					adjustConfig.setSessionSuccessDelegate(new Action<AdjustSessionSuccess>(this.SessionSuccessCallback), "Adjust");
					adjustConfig.setSessionFailureDelegate(new Action<AdjustSessionFailure>(this.SessionFailureCallback), "Adjust");
					adjustConfig.setDeferredDeeplinkDelegate(new Action<string>(this.DeferredDeeplinkCallback), "Adjust");
					adjustConfig.setAttributionChangedDelegate(new Action<AdjustAttribution>(this.AttributionChangedCallback), "Adjust");
				}
				Adjust.start(adjustConfig);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (Adjust.instance == null)
			{
				return;
			}
			if (pauseStatus)
			{
				Adjust.instance.onPause();
			}
			else
			{
				Adjust.instance.onResume();
			}
		}

		public static void start(AdjustConfig adjustConfig)
		{
			if (Adjust.instance != null)
			{
				global::Debug.Log("adjust: Error, SDK already started.");
				return;
			}
			if (adjustConfig == null)
			{
				global::Debug.Log("adjust: Missing config to start.");
				return;
			}
			Adjust.instance = new AdjustAndroid();
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
				return;
			}
			Adjust.eventSuccessDelegate = adjustConfig.getEventSuccessDelegate();
			Adjust.eventFailureDelegate = adjustConfig.getEventFailureDelegate();
			Adjust.sessionSuccessDelegate = adjustConfig.getSessionSuccessDelegate();
			Adjust.sessionFailureDelegate = adjustConfig.getSessionFailureDelegate();
			Adjust.deferredDeeplinkDelegate = adjustConfig.getDeferredDeeplinkDelegate();
			Adjust.attributionChangedDelegate = adjustConfig.getAttributionChangedDelegate();
			Adjust.instance.start(adjustConfig);
		}

		public static void trackEvent(AdjustEvent adjustEvent)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (adjustEvent == null)
			{
				global::Debug.Log("adjust: Missing event to track.");
				return;
			}
			Adjust.instance.trackEvent(adjustEvent);
		}

		public static void setEnabled(bool enabled)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			Adjust.instance.setEnabled(enabled);
		}

		public static bool isEnabled()
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return false;
			}
			return Adjust.instance.isEnabled();
		}

		public static void setOfflineMode(bool enabled)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			Adjust.instance.setOfflineMode(enabled);
		}

		public static void sendFirstPackages()
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			Adjust.instance.sendFirstPackages();
		}

		public static void addSessionPartnerParameter(string key, string value)
		{
			AdjustAndroid.addSessionPartnerParameter(key, value);
		}

		public static void addSessionCallbackParameter(string key, string value)
		{
			AdjustAndroid.addSessionCallbackParameter(key, value);
		}

		public static void removeSessionPartnerParameter(string key)
		{
			AdjustAndroid.removeSessionPartnerParameter(key);
		}

		public static void removeSessionCallbackParameter(string key)
		{
			AdjustAndroid.removeSessionCallbackParameter(key);
		}

		public static void resetSessionPartnerParameters()
		{
			AdjustAndroid.resetSessionPartnerParameters();
		}

		public static void resetSessionCallbackParameters()
		{
			AdjustAndroid.resetSessionCallbackParameters();
		}

		public static string getAdid()
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return null;
			}
			return Adjust.instance.getAdid();
		}

		public static AdjustAttribution getAttribution()
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return null;
			}
			return Adjust.instance.getAttribution();
		}

		public static void setDeviceToken(string deviceToken)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			Adjust.instance.setDeviceToken(deviceToken);
		}

		public static string getIdfa()
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return null;
			}
			return Adjust.instance.getIdfa();
		}

		public static void setReferrer(string referrer)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			Adjust.instance.setReferrer(referrer);
		}

		public static void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			Adjust.instance.getGoogleAdId(onDeviceIdsRead);
		}

		public void GetNativeAttribution(string attributionData)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (Adjust.attributionChangedDelegate == null)
			{
				global::Debug.Log("adjust: Attribution changed delegate was not set.");
				return;
			}
			AdjustAttribution obj = new AdjustAttribution(attributionData);
			Adjust.attributionChangedDelegate(obj);
		}

		public void GetNativeEventSuccess(string eventSuccessData)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (Adjust.eventSuccessDelegate == null)
			{
				global::Debug.Log("adjust: Event success delegate was not set.");
				return;
			}
			AdjustEventSuccess obj = new AdjustEventSuccess(eventSuccessData);
			Adjust.eventSuccessDelegate(obj);
		}

		public void GetNativeEventFailure(string eventFailureData)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (Adjust.eventFailureDelegate == null)
			{
				global::Debug.Log("adjust: Event failure delegate was not set.");
				return;
			}
			AdjustEventFailure obj = new AdjustEventFailure(eventFailureData);
			Adjust.eventFailureDelegate(obj);
		}

		public void GetNativeSessionSuccess(string sessionSuccessData)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (Adjust.sessionSuccessDelegate == null)
			{
				global::Debug.Log("adjust: Session success delegate was not set.");
				return;
			}
			AdjustSessionSuccess obj = new AdjustSessionSuccess(sessionSuccessData);
			Adjust.sessionSuccessDelegate(obj);
		}

		public void GetNativeSessionFailure(string sessionFailureData)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (Adjust.sessionFailureDelegate == null)
			{
				global::Debug.Log("adjust: Session failure delegate was not set.");
				return;
			}
			AdjustSessionFailure obj = new AdjustSessionFailure(sessionFailureData);
			Adjust.sessionFailureDelegate(obj);
		}

		public void GetNativeDeferredDeeplink(string deeplinkURL)
		{
			if (Adjust.instance == null)
			{
				global::Debug.Log("adjust: SDK not started. Start it manually using the 'start' method.");
				return;
			}
			if (Adjust.deferredDeeplinkDelegate == null)
			{
				global::Debug.Log("adjust: Deferred deeplink delegate was not set.");
				return;
			}
			Adjust.deferredDeeplinkDelegate(deeplinkURL);
		}

		private void AttributionChangedCallback(AdjustAttribution attributionData)
		{
			global::Debug.Log("Attribution changed!");
			if (attributionData.trackerName != null)
			{
				global::Debug.Log("Tracker name: " + attributionData.trackerName);
			}
			if (attributionData.trackerToken != null)
			{
				global::Debug.Log("Tracker token: " + attributionData.trackerToken);
			}
			if (attributionData.network != null)
			{
				global::Debug.Log("Network: " + attributionData.network);
			}
			if (attributionData.campaign != null)
			{
				global::Debug.Log("Campaign: " + attributionData.campaign);
			}
			if (attributionData.adgroup != null)
			{
				global::Debug.Log("Adgroup: " + attributionData.adgroup);
			}
			if (attributionData.creative != null)
			{
				global::Debug.Log("Creative: " + attributionData.creative);
			}
			if (attributionData.clickLabel != null)
			{
				global::Debug.Log("Click label: " + attributionData.clickLabel);
			}
			if (attributionData.adid != null)
			{
				global::Debug.Log("ADID: " + attributionData.adid);
			}
		}

		private void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
		{
			global::Debug.Log("Event tracked successfully!");
			if (eventSuccessData.Message != null)
			{
				global::Debug.Log("Message: " + eventSuccessData.Message);
			}
			if (eventSuccessData.Timestamp != null)
			{
				global::Debug.Log("Timestamp: " + eventSuccessData.Timestamp);
			}
			if (eventSuccessData.Adid != null)
			{
				global::Debug.Log("Adid: " + eventSuccessData.Adid);
			}
			if (eventSuccessData.EventToken != null)
			{
				global::Debug.Log("EventToken: " + eventSuccessData.EventToken);
			}
			if (eventSuccessData.JsonResponse != null)
			{
				global::Debug.Log("JsonResponse: " + eventSuccessData.GetJsonResponse());
			}
		}

		private void EventFailureCallback(AdjustEventFailure eventFailureData)
		{
			global::Debug.Log("Event tracking failed!");
			if (eventFailureData.Message != null)
			{
				global::Debug.Log("Message: " + eventFailureData.Message);
			}
			if (eventFailureData.Timestamp != null)
			{
				global::Debug.Log("Timestamp: " + eventFailureData.Timestamp);
			}
			if (eventFailureData.Adid != null)
			{
				global::Debug.Log("Adid: " + eventFailureData.Adid);
			}
			if (eventFailureData.EventToken != null)
			{
				global::Debug.Log("EventToken: " + eventFailureData.EventToken);
			}
			global::Debug.Log("WillRetry: " + eventFailureData.WillRetry.ToString());
			if (eventFailureData.JsonResponse != null)
			{
				global::Debug.Log("JsonResponse: " + eventFailureData.GetJsonResponse());
			}
		}

		private void SessionSuccessCallback(AdjustSessionSuccess sessionSuccessData)
		{
			global::Debug.Log("Session tracked successfully!");
			if (sessionSuccessData.Message != null)
			{
				global::Debug.Log("Message: " + sessionSuccessData.Message);
			}
			if (sessionSuccessData.Timestamp != null)
			{
				global::Debug.Log("Timestamp: " + sessionSuccessData.Timestamp);
			}
			if (sessionSuccessData.Adid != null)
			{
				global::Debug.Log("Adid: " + sessionSuccessData.Adid);
			}
			if (sessionSuccessData.JsonResponse != null)
			{
				global::Debug.Log("JsonResponse: " + sessionSuccessData.GetJsonResponse());
			}
		}

		private void SessionFailureCallback(AdjustSessionFailure sessionFailureData)
		{
			global::Debug.Log("Session tracking failed!");
			if (sessionFailureData.Message != null)
			{
				global::Debug.Log("Message: " + sessionFailureData.Message);
			}
			if (sessionFailureData.Timestamp != null)
			{
				global::Debug.Log("Timestamp: " + sessionFailureData.Timestamp);
			}
			if (sessionFailureData.Adid != null)
			{
				global::Debug.Log("Adid: " + sessionFailureData.Adid);
			}
			global::Debug.Log("WillRetry: " + sessionFailureData.WillRetry.ToString());
			if (sessionFailureData.JsonResponse != null)
			{
				global::Debug.Log("JsonResponse: " + sessionFailureData.GetJsonResponse());
			}
		}

		private void DeferredDeeplinkCallback(string deeplinkURL)
		{
			global::Debug.Log("Deferred deeplink reported!");
			if (deeplinkURL != null)
			{
				global::Debug.Log("Deeplink URL: " + deeplinkURL);
			}
			else
			{
				global::Debug.Log("Deeplink URL is null!");
			}
		}
	}
}
