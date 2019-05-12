using System;
using UnityEngine;

namespace com.adjust.sdk
{
	public class AdjustAndroid : IAdjust
	{
		private const string sdkPrefix = "unity4.11.0";

		private static bool launchDeferredDeeplink = true;

		private static AndroidJavaClass ajcAdjust;

		private AndroidJavaObject ajoCurrentActivity;

		private AdjustAndroid.DeferredDeeplinkListener onDeferredDeeplinkListener;

		private AdjustAndroid.AttributionChangeListener onAttributionChangedListener;

		private AdjustAndroid.EventTrackingFailedListener onEventTrackingFailedListener;

		private AdjustAndroid.EventTrackingSucceededListener onEventTrackingSucceededListener;

		private AdjustAndroid.SessionTrackingFailedListener onSessionTrackingFailedListener;

		private AdjustAndroid.SessionTrackingSucceededListener onSessionTrackingSucceededListener;

		public AdjustAndroid()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.ajoCurrentActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		public void start(AdjustConfig adjustConfig)
		{
			AndroidJavaObject androidJavaObject = (adjustConfig.environment != AdjustEnvironment.Sandbox) ? new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_PRODUCTION") : new AndroidJavaClass("com.adjust.sdk.AdjustConfig").GetStatic<AndroidJavaObject>("ENVIRONMENT_SANDBOX");
			bool? allowSuppressLogLevel = adjustConfig.allowSuppressLogLevel;
			AndroidJavaObject androidJavaObject2;
			if (allowSuppressLogLevel != null)
			{
				androidJavaObject2 = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", new object[]
				{
					this.ajoCurrentActivity,
					adjustConfig.appToken,
					androidJavaObject,
					adjustConfig.allowSuppressLogLevel
				});
			}
			else
			{
				androidJavaObject2 = new AndroidJavaObject("com.adjust.sdk.AdjustConfig", new object[]
				{
					this.ajoCurrentActivity,
					adjustConfig.appToken,
					androidJavaObject
				});
			}
			AdjustAndroid.launchDeferredDeeplink = adjustConfig.launchDeferredDeeplink;
			AdjustLogLevel? logLevel = adjustConfig.logLevel;
			if (logLevel != null)
			{
				AndroidJavaObject @static;
				if (adjustConfig.logLevel.Value.uppercaseToString().Equals("SUPPRESS"))
				{
					@static = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>("SUPRESS");
				}
				else
				{
					@static = new AndroidJavaClass("com.adjust.sdk.LogLevel").GetStatic<AndroidJavaObject>(adjustConfig.logLevel.Value.uppercaseToString());
				}
				if (@static != null)
				{
					androidJavaObject2.Call("setLogLevel", new object[]
					{
						@static
					});
				}
			}
			double? delayStart = adjustConfig.delayStart;
			if (delayStart != null)
			{
				androidJavaObject2.Call("setDelayStart", new object[]
				{
					adjustConfig.delayStart
				});
			}
			bool? eventBufferingEnabled = adjustConfig.eventBufferingEnabled;
			if (eventBufferingEnabled != null)
			{
				AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.Boolean", new object[]
				{
					adjustConfig.eventBufferingEnabled.Value
				});
				androidJavaObject2.Call("setEventBufferingEnabled", new object[]
				{
					androidJavaObject3
				});
			}
			bool? sendInBackground = adjustConfig.sendInBackground;
			if (sendInBackground != null)
			{
				androidJavaObject2.Call("setSendInBackground", new object[]
				{
					adjustConfig.sendInBackground.Value
				});
			}
			if (adjustConfig.userAgent != null)
			{
				androidJavaObject2.Call("setUserAgent", new object[]
				{
					adjustConfig.userAgent
				});
			}
			if (!string.IsNullOrEmpty(adjustConfig.processName))
			{
				androidJavaObject2.Call("setProcessName", new object[]
				{
					adjustConfig.processName
				});
			}
			if (adjustConfig.defaultTracker != null)
			{
				androidJavaObject2.Call("setDefaultTracker", new object[]
				{
					adjustConfig.defaultTracker
				});
			}
			if (adjustConfig.attributionChangedDelegate != null)
			{
				this.onAttributionChangedListener = new AdjustAndroid.AttributionChangeListener(adjustConfig.attributionChangedDelegate);
				androidJavaObject2.Call("setOnAttributionChangedListener", new object[]
				{
					this.onAttributionChangedListener
				});
			}
			if (adjustConfig.eventSuccessDelegate != null)
			{
				this.onEventTrackingSucceededListener = new AdjustAndroid.EventTrackingSucceededListener(adjustConfig.eventSuccessDelegate);
				androidJavaObject2.Call("setOnEventTrackingSucceededListener", new object[]
				{
					this.onEventTrackingSucceededListener
				});
			}
			if (adjustConfig.eventFailureDelegate != null)
			{
				this.onEventTrackingFailedListener = new AdjustAndroid.EventTrackingFailedListener(adjustConfig.eventFailureDelegate);
				androidJavaObject2.Call("setOnEventTrackingFailedListener", new object[]
				{
					this.onEventTrackingFailedListener
				});
			}
			if (adjustConfig.sessionSuccessDelegate != null)
			{
				this.onSessionTrackingSucceededListener = new AdjustAndroid.SessionTrackingSucceededListener(adjustConfig.sessionSuccessDelegate);
				androidJavaObject2.Call("setOnSessionTrackingSucceededListener", new object[]
				{
					this.onSessionTrackingSucceededListener
				});
			}
			if (adjustConfig.sessionFailureDelegate != null)
			{
				this.onSessionTrackingFailedListener = new AdjustAndroid.SessionTrackingFailedListener(adjustConfig.sessionFailureDelegate);
				androidJavaObject2.Call("setOnSessionTrackingFailedListener", new object[]
				{
					this.onSessionTrackingFailedListener
				});
			}
			if (adjustConfig.deferredDeeplinkDelegate != null)
			{
				this.onDeferredDeeplinkListener = new AdjustAndroid.DeferredDeeplinkListener(adjustConfig.deferredDeeplinkDelegate);
				androidJavaObject2.Call("setOnDeeplinkResponseListener", new object[]
				{
					this.onDeferredDeeplinkListener
				});
			}
			androidJavaObject2.Call("setSdkPrefix", new object[]
			{
				"unity4.11.0"
			});
			AdjustAndroid.ajcAdjust.CallStatic("onCreate", new object[]
			{
				androidJavaObject2
			});
			AdjustAndroid.ajcAdjust.CallStatic("onResume", new object[0]);
		}

		public void trackEvent(AdjustEvent adjustEvent)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.adjust.sdk.AdjustEvent", new object[]
			{
				adjustEvent.eventToken
			});
			double? revenue = adjustEvent.revenue;
			if (revenue != null)
			{
				AndroidJavaObject androidJavaObject2 = androidJavaObject;
				string methodName = "setRevenue";
				object[] array = new object[2];
				int num = 0;
				double? revenue2 = adjustEvent.revenue;
				array[num] = revenue2.Value;
				array[1] = adjustEvent.currency;
				androidJavaObject2.Call(methodName, array);
			}
			if (adjustEvent.callbackList != null)
			{
				for (int i = 0; i < adjustEvent.callbackList.Count; i += 2)
				{
					string text = adjustEvent.callbackList[i];
					string text2 = adjustEvent.callbackList[i + 1];
					androidJavaObject.Call("addCallbackParameter", new object[]
					{
						text,
						text2
					});
				}
			}
			if (adjustEvent.partnerList != null)
			{
				for (int j = 0; j < adjustEvent.partnerList.Count; j += 2)
				{
					string text3 = adjustEvent.partnerList[j];
					string text4 = adjustEvent.partnerList[j + 1];
					androidJavaObject.Call("addPartnerParameter", new object[]
					{
						text3,
						text4
					});
				}
			}
			if (adjustEvent.transactionId != null)
			{
				androidJavaObject.Call("setOrderId", new object[]
				{
					adjustEvent.transactionId
				});
			}
			AdjustAndroid.ajcAdjust.CallStatic("trackEvent", new object[]
			{
				androidJavaObject
			});
		}

		public bool isEnabled()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<bool>("isEnabled", new object[0]);
		}

		public void setEnabled(bool enabled)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setEnabled", new object[]
			{
				enabled
			});
		}

		public void setOfflineMode(bool enabled)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setOfflineMode", new object[]
			{
				enabled
			});
		}

		public void sendFirstPackages()
		{
			AdjustAndroid.ajcAdjust.CallStatic("sendFirstPackages", new object[0]);
		}

		public void setDeviceToken(string deviceToken)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setPushToken", new object[]
			{
				deviceToken
			});
		}

		public string getAdid()
		{
			return AdjustAndroid.ajcAdjust.CallStatic<string>("getAdid", new object[0]);
		}

		public AdjustAttribution getAttribution()
		{
			try
			{
				AndroidJavaObject androidJavaObject = AdjustAndroid.ajcAdjust.CallStatic<AndroidJavaObject>("getAttribution", new object[0]);
				if (androidJavaObject == null)
				{
					return null;
				}
				return new AdjustAttribution
				{
					trackerName = androidJavaObject.Get<string>(AdjustUtils.KeyTrackerName),
					trackerToken = androidJavaObject.Get<string>(AdjustUtils.KeyTrackerToken),
					network = androidJavaObject.Get<string>(AdjustUtils.KeyNetwork),
					campaign = androidJavaObject.Get<string>(AdjustUtils.KeyCampaign),
					adgroup = androidJavaObject.Get<string>(AdjustUtils.KeyAdgroup),
					creative = androidJavaObject.Get<string>(AdjustUtils.KeyCreative),
					clickLabel = androidJavaObject.Get<string>(AdjustUtils.KeyClickLabel),
					adid = androidJavaObject.Get<string>(AdjustUtils.KeyAdid)
				};
			}
			catch (Exception)
			{
			}
			return null;
		}

		public static void addSessionPartnerParameter(string key, string value)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("addSessionPartnerParameter", new object[]
			{
				key,
				value
			});
		}

		public static void addSessionCallbackParameter(string key, string value)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("addSessionCallbackParameter", new object[]
			{
				key,
				value
			});
		}

		public static void removeSessionPartnerParameter(string key)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("removeSessionPartnerParameter", new object[]
			{
				key
			});
		}

		public static void removeSessionCallbackParameter(string key)
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("removeSessionCallbackParameter", new object[]
			{
				key
			});
		}

		public static void resetSessionPartnerParameters()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("resetSessionPartnerParameters", new object[0]);
		}

		public static void resetSessionCallbackParameters()
		{
			if (AdjustAndroid.ajcAdjust == null)
			{
				AdjustAndroid.ajcAdjust = new AndroidJavaClass("com.adjust.sdk.Adjust");
			}
			AdjustAndroid.ajcAdjust.CallStatic("resetSessionCallbackParameters", new object[0]);
		}

		public void onPause()
		{
			AdjustAndroid.ajcAdjust.CallStatic("onPause", new object[0]);
		}

		public void onResume()
		{
			AdjustAndroid.ajcAdjust.CallStatic("onResume", new object[0]);
		}

		public void setReferrer(string referrer)
		{
			AdjustAndroid.ajcAdjust.CallStatic("setReferrer", new object[]
			{
				referrer
			});
		}

		public void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			AdjustAndroid.DeviceIdsReadListener deviceIdsReadListener = new AdjustAndroid.DeviceIdsReadListener(onDeviceIdsRead);
			AdjustAndroid.ajcAdjust.CallStatic("getGoogleAdId", new object[]
			{
				this.ajoCurrentActivity,
				deviceIdsReadListener
			});
		}

		public string getIdfa()
		{
			return null;
		}

		private class AttributionChangeListener : AndroidJavaProxy
		{
			private Action<AdjustAttribution> callback;

			public AttributionChangeListener(Action<AdjustAttribution> pCallback) : base("com.adjust.sdk.OnAttributionChangedListener")
			{
				this.callback = pCallback;
			}

			public void onAttributionChanged(AndroidJavaObject attribution)
			{
				if (this.callback == null)
				{
					return;
				}
				AdjustAttribution adjustAttribution = new AdjustAttribution();
				adjustAttribution.trackerName = attribution.Get<string>(AdjustUtils.KeyTrackerName);
				adjustAttribution.trackerToken = attribution.Get<string>(AdjustUtils.KeyTrackerToken);
				adjustAttribution.network = attribution.Get<string>(AdjustUtils.KeyNetwork);
				adjustAttribution.campaign = attribution.Get<string>(AdjustUtils.KeyCampaign);
				adjustAttribution.adgroup = attribution.Get<string>(AdjustUtils.KeyAdgroup);
				adjustAttribution.creative = attribution.Get<string>(AdjustUtils.KeyCreative);
				adjustAttribution.clickLabel = attribution.Get<string>(AdjustUtils.KeyClickLabel);
				adjustAttribution.adid = attribution.Get<string>(AdjustUtils.KeyAdid);
				this.callback(adjustAttribution);
			}
		}

		private class DeferredDeeplinkListener : AndroidJavaProxy
		{
			private Action<string> callback;

			public DeferredDeeplinkListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeeplinkResponseListener")
			{
				this.callback = pCallback;
			}

			public bool launchReceivedDeeplink(AndroidJavaObject deeplink)
			{
				if (this.callback == null)
				{
					return AdjustAndroid.launchDeferredDeeplink;
				}
				string obj = deeplink.Call<string>("toString", new object[0]);
				this.callback(obj);
				return AdjustAndroid.launchDeferredDeeplink;
			}
		}

		private class EventTrackingSucceededListener : AndroidJavaProxy
		{
			private Action<AdjustEventSuccess> callback;

			public EventTrackingSucceededListener(Action<AdjustEventSuccess> pCallback) : base("com.adjust.sdk.OnEventTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedEventTrackingSucceeded(AndroidJavaObject eventSuccessData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (eventSuccessData == null)
				{
					return;
				}
				AdjustEventSuccess adjustEventSuccess = new AdjustEventSuccess();
				adjustEventSuccess.Adid = eventSuccessData.Get<string>(AdjustUtils.KeyAdid);
				adjustEventSuccess.Message = eventSuccessData.Get<string>(AdjustUtils.KeyMessage);
				adjustEventSuccess.Timestamp = eventSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
				adjustEventSuccess.EventToken = eventSuccessData.Get<string>(AdjustUtils.KeyEventToken);
				try
				{
					AndroidJavaObject androidJavaObject = eventSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustEventSuccess.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustEventSuccess);
			}
		}

		private class EventTrackingFailedListener : AndroidJavaProxy
		{
			private Action<AdjustEventFailure> callback;

			public EventTrackingFailedListener(Action<AdjustEventFailure> pCallback) : base("com.adjust.sdk.OnEventTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedEventTrackingFailed(AndroidJavaObject eventFailureData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (eventFailureData == null)
				{
					return;
				}
				AdjustEventFailure adjustEventFailure = new AdjustEventFailure();
				adjustEventFailure.Adid = eventFailureData.Get<string>(AdjustUtils.KeyAdid);
				adjustEventFailure.Message = eventFailureData.Get<string>(AdjustUtils.KeyMessage);
				adjustEventFailure.WillRetry = eventFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
				adjustEventFailure.Timestamp = eventFailureData.Get<string>(AdjustUtils.KeyTimestamp);
				adjustEventFailure.EventToken = eventFailureData.Get<string>(AdjustUtils.KeyEventToken);
				try
				{
					AndroidJavaObject androidJavaObject = eventFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustEventFailure.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustEventFailure);
			}
		}

		private class SessionTrackingSucceededListener : AndroidJavaProxy
		{
			private Action<AdjustSessionSuccess> callback;

			public SessionTrackingSucceededListener(Action<AdjustSessionSuccess> pCallback) : base("com.adjust.sdk.OnSessionTrackingSucceededListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedSessionTrackingSucceeded(AndroidJavaObject sessionSuccessData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (sessionSuccessData == null)
				{
					return;
				}
				AdjustSessionSuccess adjustSessionSuccess = new AdjustSessionSuccess();
				adjustSessionSuccess.Adid = sessionSuccessData.Get<string>(AdjustUtils.KeyAdid);
				adjustSessionSuccess.Message = sessionSuccessData.Get<string>(AdjustUtils.KeyMessage);
				adjustSessionSuccess.Timestamp = sessionSuccessData.Get<string>(AdjustUtils.KeyTimestamp);
				try
				{
					AndroidJavaObject androidJavaObject = sessionSuccessData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustSessionSuccess.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustSessionSuccess);
			}
		}

		private class SessionTrackingFailedListener : AndroidJavaProxy
		{
			private Action<AdjustSessionFailure> callback;

			public SessionTrackingFailedListener(Action<AdjustSessionFailure> pCallback) : base("com.adjust.sdk.OnSessionTrackingFailedListener")
			{
				this.callback = pCallback;
			}

			public void onFinishedSessionTrackingFailed(AndroidJavaObject sessionFailureData)
			{
				if (this.callback == null)
				{
					return;
				}
				if (sessionFailureData == null)
				{
					return;
				}
				AdjustSessionFailure adjustSessionFailure = new AdjustSessionFailure();
				adjustSessionFailure.Adid = sessionFailureData.Get<string>(AdjustUtils.KeyAdid);
				adjustSessionFailure.Message = sessionFailureData.Get<string>(AdjustUtils.KeyMessage);
				adjustSessionFailure.WillRetry = sessionFailureData.Get<bool>(AdjustUtils.KeyWillRetry);
				adjustSessionFailure.Timestamp = sessionFailureData.Get<string>(AdjustUtils.KeyTimestamp);
				try
				{
					AndroidJavaObject androidJavaObject = sessionFailureData.Get<AndroidJavaObject>(AdjustUtils.KeyJsonResponse);
					string jsonResponseString = androidJavaObject.Call<string>("toString", new object[0]);
					adjustSessionFailure.BuildJsonResponseFromString(jsonResponseString);
				}
				catch (Exception)
				{
				}
				this.callback(adjustSessionFailure);
			}
		}

		private class DeviceIdsReadListener : AndroidJavaProxy
		{
			private Action<string> onPlayAdIdReadCallback;

			public DeviceIdsReadListener(Action<string> pCallback) : base("com.adjust.sdk.OnDeviceIdsRead")
			{
				this.onPlayAdIdReadCallback = pCallback;
			}

			public void onGoogleAdIdRead(string playAdId)
			{
				if (this.onPlayAdIdReadCallback == null)
				{
					return;
				}
				this.onPlayAdIdReadCallback(playAdId);
			}

			public void onGoogleAdIdRead(AndroidJavaObject ajoAdId)
			{
				if (ajoAdId == null)
				{
					string playAdId = null;
					this.onGoogleAdIdRead(playAdId);
					return;
				}
				this.onGoogleAdIdRead(ajoAdId.Call<string>("toString", new object[0]));
			}
		}
	}
}
