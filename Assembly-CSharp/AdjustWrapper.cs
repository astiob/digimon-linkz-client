using com.adjust.sdk;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AdjustWrapper : MonoBehaviour
{
	private static GameObject go;

	private static AdjustWrapper instance;

	private string errorMessage = "adjust: SDK not started. Start it manually using the 'start' method.";

	private IAdjust adjust;

	private Action<string> deferredDeeplinkDelegate;

	private Action<AdjustEventSuccess> eventSuccessDelegate;

	private Action<AdjustEventFailure> eventFailureDelegate;

	private Action<AdjustSessionSuccess> sessionSuccessDelegate;

	private Action<AdjustSessionFailure> sessionFailureDelegate;

	private Action<AdjustAttribution> attributionChangedDelegate;

	private bool startManually;

	private bool eventBuffering;

	private bool printAttribution = true;

	private bool sendInBackground;

	private bool launchDeferredDeeplink = true;

	private string appToken = "tceqx3zd3rb4";

	private AdjustLogLevel logLevel = AdjustLogLevel.Info;

	private AdjustEnvironment environment = AdjustEnvironment.Production;

	public static string EVENT_ID_FINISH_TUTORIAL = "t4tvq2";

	public static string EVENT_ID_SEND_PAYMENT = "ws02z4";

	public static string EVENT_ID_CONFIRM_GDPR = "6ld0k0";

	public static AdjustWrapper Instance
	{
		get
		{
			if (AdjustWrapper.instance == null)
			{
				AdjustWrapper.go = new GameObject("AdjustWrapper");
				UnityEngine.Object.DontDestroyOnLoad(AdjustWrapper.go);
				AdjustWrapper.instance = AdjustWrapper.go.AddComponent<AdjustWrapper>();
			}
			return AdjustWrapper.instance;
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (this.adjust == null)
		{
			return;
		}
		if (pauseStatus)
		{
			this.adjust.onPause();
		}
		else
		{
			this.adjust.onResume();
		}
	}

	public void StartAdjust()
	{
		if (this.adjust != null)
		{
			return;
		}
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
			this.start(adjustConfig);
			this.getGoogleAdId(delegate(string googleAdId)
			{
				global::Debug.Log("googleAdId:" + googleAdId);
			});
			string adid = this.getAdid();
			if (adid != null)
			{
				global::Debug.Log("adid:" + adid);
			}
		}
	}

	public void TrackEvent(string eventToken)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		AdjustEvent adjustEvent = new AdjustEvent(eventToken);
		this.adjust.trackEvent(adjustEvent);
	}

	public void TrackEvent(string eventToken, double amount, string currency)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		AdjustEvent adjustEvent = new AdjustEvent(eventToken);
		adjustEvent.setRevenue(amount, currency);
		this.adjust.trackEvent(adjustEvent);
	}

	public void TrackEventGDPR(Dictionary<string, bool> agreements)
	{
		if (this.adjust == null || agreements == null || 0 >= agreements.Count)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		AdjustEvent adjustEvent = new AdjustEvent(AdjustWrapper.EVENT_ID_CONFIRM_GDPR);
		foreach (KeyValuePair<string, bool> keyValuePair in agreements)
		{
			string value = (!keyValuePair.Value) ? "0" : "1";
			adjustEvent.addCallbackParameter(keyValuePair.Key, value);
		}
		Adjust.trackEvent(adjustEvent);
	}

	public void start(AdjustConfig adjustConfig)
	{
		if (this.adjust != null)
		{
			global::Debug.Log("adjust: Error, SDK already started.");
			return;
		}
		if (adjustConfig == null)
		{
			global::Debug.Log("adjust: Missing config to start.");
			return;
		}
		this.adjust = new AdjustAndroid();
		if (this.adjust == null)
		{
			global::Debug.Log("adjust: SDK can only be used in Android, iOS, Windows Phone 8 or Windows Store apps.");
			return;
		}
		this.eventSuccessDelegate = adjustConfig.getEventSuccessDelegate();
		this.eventFailureDelegate = adjustConfig.getEventFailureDelegate();
		this.sessionSuccessDelegate = adjustConfig.getSessionSuccessDelegate();
		this.sessionFailureDelegate = adjustConfig.getSessionFailureDelegate();
		this.deferredDeeplinkDelegate = adjustConfig.getDeferredDeeplinkDelegate();
		this.attributionChangedDelegate = adjustConfig.getAttributionChangedDelegate();
		this.adjust.start(adjustConfig);
	}

	public void trackEvent(AdjustEvent adjustEvent)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (adjustEvent == null)
		{
			global::Debug.Log("adjust: Missing event to track.");
			return;
		}
		this.adjust.trackEvent(adjustEvent);
	}

	public void setEnabled(bool enabled)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		this.adjust.setEnabled(enabled);
	}

	public bool isEnabled()
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return false;
		}
		return this.adjust.isEnabled();
	}

	public void setOfflineMode(bool enabled)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		this.adjust.setOfflineMode(enabled);
	}

	public void sendFirstPackages()
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		this.adjust.sendFirstPackages();
	}

	public void addSessionPartnerParameter(string key, string value)
	{
		AdjustAndroid.addSessionPartnerParameter(key, value);
	}

	public void addSessionCallbackParameter(string key, string value)
	{
		AdjustAndroid.addSessionCallbackParameter(key, value);
	}

	public void removeSessionPartnerParameter(string key)
	{
		AdjustAndroid.removeSessionPartnerParameter(key);
	}

	public void removeSessionCallbackParameter(string key)
	{
		AdjustAndroid.removeSessionCallbackParameter(key);
	}

	public void resetSessionPartnerParameters()
	{
		AdjustAndroid.resetSessionPartnerParameters();
	}

	public void resetSessionCallbackParameters()
	{
		AdjustAndroid.resetSessionCallbackParameters();
	}

	public string getAdid()
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return null;
		}
		return this.adjust.getAdid();
	}

	public AdjustAttribution getAttribution()
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return null;
		}
		return this.adjust.getAttribution();
	}

	public void setDeviceToken(string deviceToken)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		this.adjust.setDeviceToken(deviceToken);
	}

	public string getIdfa()
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return null;
		}
		return this.adjust.getIdfa();
	}

	public void setReferrer(string referrer)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		this.adjust.setReferrer(referrer);
	}

	public void getGoogleAdId(Action<string> onDeviceIdsRead)
	{
		if (this.adjust == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		this.adjust.getGoogleAdId(onDeviceIdsRead);
	}

	public void runAttributionChangedDictionary(Dictionary<string, string> dicAttributionData)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.attributionChangedDelegate == null)
		{
			global::Debug.Log("adjust: Attribution changed delegate was not set.");
			return;
		}
		AdjustAttribution obj = new AdjustAttribution(dicAttributionData);
		this.attributionChangedDelegate(obj);
	}

	public void GetNativeAttribution(string attributionData)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.attributionChangedDelegate == null)
		{
			global::Debug.Log("adjust: Attribution changed delegate was not set.");
			return;
		}
		AdjustAttribution obj = new AdjustAttribution(attributionData);
		this.attributionChangedDelegate(obj);
	}

	public void GetNativeEventSuccess(string eventSuccessData)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.eventSuccessDelegate == null)
		{
			global::Debug.Log("adjust: Event success delegate was not set.");
			return;
		}
		AdjustEventSuccess obj = new AdjustEventSuccess(eventSuccessData);
		this.eventSuccessDelegate(obj);
	}

	public void GetNativeEventFailure(string eventFailureData)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.eventFailureDelegate == null)
		{
			global::Debug.Log("adjust: Event failure delegate was not set.");
			return;
		}
		AdjustEventFailure obj = new AdjustEventFailure(eventFailureData);
		this.eventFailureDelegate(obj);
	}

	public void GetNativeSessionSuccess(string sessionSuccessData)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.sessionSuccessDelegate == null)
		{
			global::Debug.Log("adjust: Session success delegate was not set.");
			return;
		}
		AdjustSessionSuccess obj = new AdjustSessionSuccess(sessionSuccessData);
		this.sessionSuccessDelegate(obj);
	}

	public void GetNativeSessionFailure(string sessionFailureData)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.sessionFailureDelegate == null)
		{
			global::Debug.Log("adjust: Session failure delegate was not set.");
			return;
		}
		AdjustSessionFailure obj = new AdjustSessionFailure(sessionFailureData);
		this.sessionFailureDelegate(obj);
	}

	public void GetNativeDeferredDeeplink(string deeplinkURL)
	{
		if (AdjustWrapper.instance == null)
		{
			global::Debug.Log(this.errorMessage);
			return;
		}
		if (this.deferredDeeplinkDelegate == null)
		{
			global::Debug.Log("adjust: Deferred deeplink delegate was not set.");
			return;
		}
		this.deferredDeeplinkDelegate(deeplinkURL);
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
