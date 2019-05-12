using System;
using System.Collections.Generic;
using UnityEngine.Cloud.Service;

namespace UnityEngine.Analytics
{
	internal class SessionImpl : IGameObserverListener, IUnityAnalyticsSession, ICloudServiceListener
	{
		private const string kValuesFileName = "values";

		private const string kConfigFileName = "config";

		private const string kSessionPauseTime = "session_pause_time";

		private const string kUserId = "user_id";

		private const long kCustomEventLimitTimeInMillSec = 3600000L;

		private SessionConfig m_SessionConfig;

		private SessionValues m_SessionValues;

		private IPlatformWrapper m_PlatformWrapper;

		private ICloudService m_CloudService;

		private CloudServiceConfig m_CloudServiceConfig;

		private SessionInfo m_SessionInfo;

		private string m_UserId;

		private string m_AppId;

		private long m_SessionStartTime;

		private long m_SessionPauseTime;

		private long m_AppRunningSentTime;

		private long m_AppSessionDuration;

		private long m_KeyStrokes;

		private long m_ClickCount;

		public SessionImpl(IPlatformWrapper platformWrapper)
		{
			this.m_PlatformWrapper = platformWrapper;
			this.state = SessionImpl.State.Stopped;
			Application.OnAdvertisingIdentifierCallback = (AdvertisingIdentifierCallback)Delegate.Combine(Application.OnAdvertisingIdentifierCallback, new AdvertisingIdentifierCallback(this.OnAdvertisingIdentifierCallback));
		}

		public SessionImpl.State state { get; private set; }

		public static bool IsAnalyticsSupportedForPlatform()
		{
			RuntimePlatform platform = Application.platform;
			switch (platform)
			{
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.WindowsWebPlayer:
			case RuntimePlatform.PS3:
			case RuntimePlatform.XBOX360:
				break;
			default:
				switch (platform)
				{
				case RuntimePlatform.PSP2:
				case RuntimePlatform.PS4:
				case RuntimePlatform.PSM:
				case RuntimePlatform.XboxOne:
					break;
				default:
					return true;
				}
				break;
			}
			return false;
		}

		public string GetPathName()
		{
			return this.m_CloudService.serviceFolderName;
		}

		public AnalyticsResult StartWithAppId(string appId, ICloudService cloudService, string configUrl, string eventUrl)
		{
			if (string.IsNullOrEmpty(appId))
			{
				return AnalyticsResult.InvalidData;
			}
			if (this.state == SessionImpl.State.Running)
			{
				return AnalyticsResult.Ok;
			}
			this.m_CloudService = cloudService;
			this.m_AppId = appId;
			AnalyticsResult analyticsResult = this.Initialize(configUrl, eventUrl);
			if (analyticsResult != AnalyticsResult.Ok)
			{
				return analyticsResult;
			}
			if (!this.isStringWithinLimit(appId, (long)this.m_SessionConfig.maxAppIdSize))
			{
				return AnalyticsResult.SizeLimitReached;
			}
			this.state = SessionImpl.State.Running;
			if (this.m_SessionConfig.analyticsEnabled)
			{
				bool newSession = true;
				this.m_SessionPauseTime = this.m_SessionValues.TryGetLong("session_pause_time", 0L);
				if (this.m_SessionPauseTime != 0L)
				{
					newSession = this.SessionElapsedSincePause();
				}
				this.StartSession(newSession);
				this.OnLevelWasLoaded(this.m_PlatformWrapper.loadedLevel);
			}
			return AnalyticsResult.Ok;
		}

		public AnalyticsResult SetCustomUserId(string customUserId)
		{
			if (this.state != SessionImpl.State.Running)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (string.IsNullOrEmpty(customUserId))
			{
				return AnalyticsResult.InvalidData;
			}
			if (!this.isStringWithinLimit(customUserId, (long)this.m_SessionConfig.maxUserIdSize))
			{
				return AnalyticsResult.SizeLimitReached;
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return AnalyticsResult.AnalyticsDisabled;
			}
			if (!this.m_SessionValues.TryGetString("custom_userid", string.Empty).Equals(customUserId))
			{
				UserInfoEvent userInfoEvent = new UserInfoEvent();
				userInfoEvent.SetCustomUserId(customUserId);
				this.m_SessionValues.PutItem("custom_userid", customUserId);
				this.QueueEvent(userInfoEvent);
			}
			return AnalyticsResult.Ok;
		}

		public AnalyticsResult SetAdvertisingId(string advertisingId, bool trackingEnabled)
		{
			if (this.state != SessionImpl.State.Running)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (string.IsNullOrEmpty(advertisingId))
			{
				return AnalyticsResult.InvalidData;
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return AnalyticsResult.AnalyticsDisabled;
			}
			if (!this.m_SessionValues.TryGetString("adsid", string.Empty).Equals(advertisingId))
			{
				DeviceInfoEvent deviceInfoEvent = new DeviceInfoEvent();
				deviceInfoEvent.SetAdvertisingId(advertisingId);
				deviceInfoEvent.SetAdvertisingTracking(trackingEnabled);
				this.m_SessionValues.PutItem("adsid", advertisingId);
				this.m_SessionValues.PutItem("ads_tracking", trackingEnabled);
				deviceInfoEvent.SetChanged(new List<string>
				{
					"adsid"
				});
				this.QueueEvent(deviceInfoEvent);
			}
			return AnalyticsResult.Ok;
		}

		public AnalyticsResult SetUserGender(Gender gender)
		{
			if (this.state != SessionImpl.State.Running)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return AnalyticsResult.AnalyticsDisabled;
			}
			string text = (gender != Gender.Male) ? ((gender != Gender.Female) ? "U" : "F") : "M";
			if (!this.m_SessionValues.TryGetString("sex", string.Empty).Equals(text))
			{
				UserInfoEvent userInfoEvent = new UserInfoEvent();
				userInfoEvent.SetUserGender(text);
				this.m_SessionValues.PutItem("sex", text);
				this.QueueEvent(userInfoEvent);
			}
			return AnalyticsResult.Ok;
		}

		public AnalyticsResult SetUserBirthYear(int birthYear)
		{
			if (this.state != SessionImpl.State.Running)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return AnalyticsResult.AnalyticsDisabled;
			}
			if (this.m_SessionValues.TryGetLong("birth_year", -1L) != (long)birthYear)
			{
				UserInfoEvent userInfoEvent = new UserInfoEvent();
				userInfoEvent.SetUserBirthYear(birthYear);
				this.m_SessionValues.PutItem("birth_year", birthYear);
				this.QueueEvent(userInfoEvent);
			}
			return AnalyticsResult.Ok;
		}

		public AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			if (this.state != SessionImpl.State.Running)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(currency))
			{
				return AnalyticsResult.InvalidData;
			}
			if (!this.isStringWithinLimit(productId, (long)this.m_SessionConfig.maxProductIdSize) || !this.isStringWithinLimit(currency, (long)this.m_SessionConfig.maxCurrencySize))
			{
				return AnalyticsResult.SizeLimitReached;
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return AnalyticsResult.AnalyticsDisabled;
			}
			TransactionEvent playRMEvent = new TransactionEvent(productId, amount, currency, receiptPurchaseData, signature, this.GenerateTransactionId());
			this.QueueEvent(playRMEvent);
			return AnalyticsResult.Ok;
		}

		public AnalyticsResult SendCustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			if (this.state != SessionImpl.State.Running)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (string.IsNullOrEmpty(customEventName))
			{
				return AnalyticsResult.InvalidData;
			}
			if (!this.isStringWithinLimit(customEventName, (long)this.m_SessionConfig.maxCustomEventNameSize))
			{
				return AnalyticsResult.SizeLimitReached;
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return AnalyticsResult.AnalyticsDisabled;
			}
			if (eventData != null)
			{
				bool flag = true;
				if (eventData.Count > this.m_SessionConfig.customEventMaxNumberOfItems)
				{
					return AnalyticsResult.TooManyItems;
				}
				if (this.CalculateCustomEventSize(eventData, out flag) > (long)this.m_SessionConfig.customEventMaxTotalItemsSize)
				{
					return AnalyticsResult.SizeLimitReached;
				}
				if (!flag)
				{
					return AnalyticsResult.InvalidData;
				}
			}
			if (!this.HandleCustomEventCount((long)this.m_SessionConfig.customEventMaxEventPerHour))
			{
				return AnalyticsResult.TooManyRequests;
			}
			CustomEvent customEvent = new CustomEvent(customEventName);
			customEvent.SetEventData(eventData);
			this.QueueEvent(customEvent);
			return AnalyticsResult.Ok;
		}

		private bool isStringWithinLimit(string input, long sizeLimit)
		{
			return (long)input.Length <= sizeLimit;
		}

		public void OnAppPause()
		{
			if (this.state != SessionImpl.State.Running)
			{
				return;
			}
			this.state = SessionImpl.State.Paused;
			this.PauseSession();
		}

		public void OnAppQuit()
		{
			if (this.state == SessionImpl.State.Stopped)
			{
				return;
			}
			if (this.state == SessionImpl.State.Running)
			{
				this.OnAppPause();
			}
			this.state = SessionImpl.State.Stopped;
			this.StopSession();
		}

		public void OnAppResume()
		{
			if (this.state != SessionImpl.State.Paused || !this.m_SessionConfig.analyticsEnabled)
			{
				return;
			}
			this.m_AppRunningSentTime = 0L;
			this.state = SessionImpl.State.Running;
			this.StartSession(this.SessionElapsedSincePause());
		}

		public void OnClick()
		{
			this.m_ClickCount += 1L;
		}

		public void OnKey()
		{
			this.m_KeyStrokes += 1L;
		}

		public void OnLevelWasLoaded(int level)
		{
			if (this.state == SessionImpl.State.Stopped || !this.m_SessionConfig.analyticsEnabled)
			{
				return;
			}
			if (this.m_SessionPauseTime > 0L)
			{
				long num = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
				int num2;
				if (this.m_AppRunningSentTime > 0L)
				{
					num2 = (int)((num - this.m_AppRunningSentTime) / 1000L);
				}
				else
				{
					num2 = (int)((num - this.m_SessionPauseTime) / 1000L);
				}
				if (num2 > 0)
				{
					this.QueueAppRunningEvent(num2);
				}
			}
			Dictionary<string, object> eventData = new Dictionary<string, object>
			{
				{
					"level_num",
					level
				},
				{
					"total_levels",
					this.m_PlatformWrapper.levelCount
				},
				{
					"level_name",
					this.m_PlatformWrapper.loadedLevelName
				}
			};
			CustomEvent customEvent = new CustomEvent("unity.sceneLoad");
			customEvent.SetEventData(eventData);
			this.QueueEvent(customEvent);
		}

		private void ResetSessionValues()
		{
			this.m_ClickCount = 0L;
			this.m_KeyStrokes = 0L;
			this.m_AppSessionDuration = 0L;
		}

		public void SaveSessionValues()
		{
			this.m_SessionValues.PutItems(new Dictionary<string, object>
			{
				{
					"clicks",
					this.m_ClickCount
				},
				{
					"keys",
					this.m_KeyStrokes
				},
				{
					"duration",
					this.m_AppSessionDuration
				}
			});
		}

		private void RestoreSessionValues()
		{
			this.m_ClickCount = this.m_SessionValues.TryGetLong("clicks", 0L);
			this.m_KeyStrokes = this.m_SessionValues.TryGetLong("keys", 0L);
			this.m_AppSessionDuration = this.m_SessionValues.TryGetLong("duration", 0L);
		}

		private AnalyticsResult Initialize(string configUrl, string eventUrl)
		{
			this.m_CloudService.Initialize(this.m_AppId);
			this.m_SessionValues = new SessionValues(this.m_CloudService, "values");
			string userId = this.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return AnalyticsResult.InvalidData;
			}
			this.m_UserId = userId;
			this.m_SessionConfig = new SessionConfig(this.m_PlatformWrapper, configUrl, eventUrl);
			this.m_SessionInfo = new SessionInfo(this.m_AppId, this.m_UserId, this.m_PlatformWrapper.platformName);
			this.RestoreConfig();
			this.UpdateConfigFromServer();
			return AnalyticsResult.Ok;
		}

		private string GenerateUUID()
		{
			return Guid.NewGuid().ToString();
		}

		private string GetUserId()
		{
			string text = this.m_SessionValues.TryGetString("user_id", null);
			if (string.IsNullOrEmpty(text))
			{
				text = this.GenerateUUID();
				this.m_SessionValues.PutItem("user_id", text);
			}
			return text;
		}

		private void StartSession(bool newSession)
		{
			this.m_CloudService.ResetNetworkRetryIndex();
			if (newSession)
			{
				this.ResetSessionValues();
				this.SaveSessionValues();
				this.m_SessionInfo.sessionId = this.m_PlatformWrapper.randomNumber;
				this.m_SessionValues.PutItem("sessionid", this.m_SessionInfo.sessionId);
				this.m_CloudService.StartEventHandler(this.m_SessionInfo.ToStringJson(), this.m_SessionConfig.maxNumberOfEventInQueue, this.m_SessionConfig.maxTimeoutForGrouping);
				AnalyticsEvent playRMEvent = new AppStartEvent();
				this.QueueEvent(playRMEvent);
			}
			else
			{
				this.RestoreSessionValues();
				long num = this.m_SessionValues.TryGetLong("sessionid", 0L);
				if (num == 0L)
				{
					this.m_SessionInfo.sessionId = this.m_PlatformWrapper.randomNumber;
					this.m_SessionValues.PutItem("sessionid", this.m_SessionInfo.sessionId);
				}
				else
				{
					this.m_SessionInfo.sessionId = num;
				}
				this.m_CloudService.StartEventHandler(this.m_SessionInfo.ToStringJson(), this.m_SessionConfig.maxNumberOfEventInQueue, this.m_SessionConfig.maxTimeoutForGrouping);
			}
			this.LookForVersionChange();
			this.m_SessionStartTime = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
			this.m_SessionPauseTime = this.m_SessionStartTime;
			this.m_SessionValues.PutItems(new Dictionary<string, object>
			{
				{
					"session_pause_time",
					this.m_SessionPauseTime
				},
				{
					"sessionid",
					this.m_SessionInfo.sessionId
				}
			});
		}

		private void PauseSession()
		{
			if (this.m_SessionConfig.analyticsEnabled)
			{
				this.HandleSessionPause();
			}
			this.m_CloudService.PauseEventHandler(true);
		}

		private void StopSession()
		{
			this.m_CloudService.StopEventHandler();
			this.m_CloudService.StopEventDispatcher();
		}

		private void HandleSessionPause()
		{
			long num = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
			int num2;
			if (this.m_AppRunningSentTime > 0L)
			{
				num2 = (int)((num - this.m_AppRunningSentTime) / 1000L);
			}
			else
			{
				num2 = (int)((num - this.m_SessionPauseTime) / 1000L);
			}
			if (num2 > 0)
			{
				this.QueueAppRunningEvent(num2);
			}
			this.m_SessionPauseTime = num;
			this.m_SessionValues.PutItem("session_pause_time", this.m_SessionPauseTime);
			this.SaveSessionValues();
		}

		private bool SessionElapsedSincePause()
		{
			long num = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
			long num2 = num - this.m_SessionPauseTime;
			return num2 > (long)this.m_SessionConfig.resumeTimeoutInMillSeconds;
		}

		private void QueueEvent(AnalyticsEvent playRMEvent)
		{
			this.m_CloudService.QueueEvent(playRMEvent.ToStringJSON(), playRMEvent.flags);
		}

		private void QueueAppRunningEvent(int delayInSeconds)
		{
			this.m_AppSessionDuration += (long)delayInSeconds;
			this.SaveSessionValues();
			AnalyticsEvent playRMEvent = new AppRunningEvent(this.m_AppSessionDuration, this.m_KeyStrokes, this.m_ClickCount);
			this.QueueEvent(playRMEvent);
			this.m_AppRunningSentTime = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
		}

		private long GenerateTransactionId()
		{
			long num = this.m_SessionValues.TryGetLong("transactionid", 0L) + 1L;
			this.m_SessionValues.PutItem("transactionid", num);
			return num;
		}

		private long CalculateCustomEventSize(IDictionary<string, object> dict, out bool validData)
		{
			long num = 0L;
			foreach (KeyValuePair<string, object> keyValuePair in dict)
			{
				object value = keyValuePair.Value;
				if (value == null)
				{
					num += (long)keyValuePair.Key.Length;
				}
				else
				{
					if (!Utils.isJSONPrimitive(value))
					{
						validData = false;
						return 0L;
					}
					num += (long)keyValuePair.Key.Length;
					num += (long)value.ToString().Length;
				}
			}
			validData = true;
			return num;
		}

		private bool HandleCustomEventCount(long limitCountPerHour)
		{
			long num = SystemClock.ToUnixTimeMilliseconds(DateTime.UtcNow);
			long num2 = this.m_SessionValues.TryGetLong("custom_event_time", num - 3600000L);
			long num3 = num - num2;
			long num4;
			if (num3 >= 3600000L)
			{
				num4 = 1L;
				this.m_SessionValues.PutItem("custom_event_time", num, false);
			}
			else
			{
				num4 = this.m_SessionValues.TryGetLong("custom_event_count", 0L) + 1L;
				if (num4 > limitCountPerHour)
				{
					return false;
				}
			}
			this.m_SessionValues.PutItem("custom_event_count", num4, false);
			return true;
		}

		private void OnAdvertisingIdentifierCallback(string advertisingId, bool trackingEnabled)
		{
			this.SetAdvertisingId(advertisingId, trackingEnabled);
		}

		private bool IsDebugDevice()
		{
			return this.m_PlatformWrapper.isEditor || Debug.isDebugBuild;
		}

		private bool IsPlatformChangedValue(string typeName, out string outValue)
		{
			string text = this.m_SessionValues.TryGetString(typeName, null);
			outValue = (string)this.GetPlatformInfo(typeName);
			if (outValue != null && text != null)
			{
				return !outValue.Equals(text);
			}
			return outValue != null;
		}

		private bool IsPlatformChangedValue(string typeName, out bool outValue)
		{
			bool flag = this.m_SessionValues.TryGetBool(typeName, false);
			object platformInfo = this.GetPlatformInfo(typeName);
			if (platformInfo == null)
			{
				outValue = false;
			}
			else
			{
				outValue = (bool)platformInfo;
			}
			return outValue != flag;
		}

		private object GetPlatformInfo(string typeName)
		{
			if (typeName != null)
			{
				if (SessionImpl.<>f__switch$map0 == null)
				{
					SessionImpl.<>f__switch$map0 = new Dictionary<string, int>(2)
					{
						{
							"sdk_ver",
							0
						},
						{
							"debug_build",
							1
						}
					};
				}
				int num;
				if (SessionImpl.<>f__switch$map0.TryGetValue(typeName, out num))
				{
					if (num == 0)
					{
						return UnityAnalyticsVersion.sdkVersion;
					}
					if (num == 1)
					{
						return Debug.isDebugBuild;
					}
				}
			}
			if (this.m_PlatformWrapper == null)
			{
				return null;
			}
			switch (typeName)
			{
			case "license_type":
				return this.m_PlatformWrapper.licenseType;
			case "os_ver":
				return this.m_PlatformWrapper.operatingSystem;
			case "app_ver":
				return this.m_PlatformWrapper.appVersion;
			case "app_name":
				return this.m_PlatformWrapper.appBundleIdentifier;
			case "app_install_mode":
				return this.m_PlatformWrapper.appInstallMode;
			case "rooted_jailbroken":
				return this.m_PlatformWrapper.isRootedOrJailbroken;
			}
			return null;
		}

		private void UpdateAdvertisingInfo()
		{
			string unityAdsId = UnityAnalyticsManager.unityAdsId;
			if (string.IsNullOrEmpty(unityAdsId))
			{
				return;
			}
			this.m_SessionValues.PutItem("adsid", unityAdsId);
			this.m_SessionValues.PutItem("ads_tracking", UnityAnalyticsManager.unityAdsTrackingEnabled);
		}

		private void LookForVersionChange()
		{
			string text;
			bool flag = this.IsPlatformChangedValue("os_ver", out text);
			string text2;
			bool flag2 = this.IsPlatformChangedValue("app_ver", out text2);
			string value;
			bool flag3 = this.IsPlatformChangedValue("sdk_ver", out value);
			bool flag4 = false;
			bool flag5 = this.IsPlatformChangedValue("rooted_jailbroken", out flag4);
			bool flag6 = false;
			bool flag7 = this.IsPlatformChangedValue("debug_build", out flag6);
			if (flag2 || flag || flag3 || flag5 || flag7)
			{
				DeviceInfoEvent deviceInfoEvent = new DeviceInfoEvent();
				deviceInfoEvent.SetDeviceMake(this.m_PlatformWrapper.platformName);
				deviceInfoEvent.SetDeviceModel(this.m_PlatformWrapper.deviceModel);
				deviceInfoEvent.SetProcessorType(this.m_PlatformWrapper.processorType);
				deviceInfoEvent.SetSystemMemorySize(this.m_PlatformWrapper.systemMemorySize.ToString());
				deviceInfoEvent.SetGameEngineVersion(this.m_PlatformWrapper.unityVersion);
				deviceInfoEvent.SetAppName((string)this.GetPlatformInfo("app_name"));
				deviceInfoEvent.SetAppInstallMode((string)this.GetPlatformInfo("app_install_mode"));
				deviceInfoEvent.SetIsRootedOrJailbroken(flag4);
				deviceInfoEvent.SetIsDebugBuild(flag6);
				deviceInfoEvent.SetLicenseType((string)this.GetPlatformInfo("license_type"));
				deviceInfoEvent.SetOSVersion(text);
				string text3 = this.m_SessionValues.TryGetString("adsid", string.Empty);
				this.UpdateAdvertisingInfo();
				if (!string.IsNullOrEmpty(text3))
				{
					deviceInfoEvent.SetAdvertisingId(text3);
					deviceInfoEvent.SetAdvertisingTracking(this.m_SessionValues.TryGetBool("ads_tracking", true));
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["sdk_ver"] = value;
				dictionary["os_ver"] = text;
				dictionary["rooted_jailbroken"] = flag4;
				dictionary["debug_build"] = flag6;
				deviceInfoEvent.SetDeviceId(this.m_PlatformWrapper.deviceUniqueIdentifier);
				if (text2 != null)
				{
					dictionary["app_ver"] = text2;
					deviceInfoEvent.SetAppVersion(text2);
				}
				this.m_SessionValues.PutItems(dictionary);
				List<string> list = new List<string>();
				if (flag2)
				{
					list.Add("app_ver");
				}
				if (flag)
				{
					list.Add("os_ver");
				}
				if (flag3)
				{
					list.Add("sdk_ver");
				}
				if (flag5)
				{
					list.Add("rooted_jailbroken");
				}
				if (flag7)
				{
					list.Add("debug_build");
				}
				deviceInfoEvent.SetChanged(list);
				this.QueueEvent(deviceInfoEvent);
			}
		}

		private void PrepareCloudServiceConfig()
		{
			this.m_CloudServiceConfig = new CloudServiceConfig();
			this.m_CloudServiceConfig.sessionHeaderName = "header";
			this.m_CloudServiceConfig.eventsHeaderName = "events";
			this.m_CloudServiceConfig.eventsEndPoint = this.m_SessionConfig.eventsEndPoint;
			this.m_CloudServiceConfig.maxNumberOfEventInGroup = this.m_SessionConfig.maxNumberOfEventInGroup;
			this.m_CloudServiceConfig.networkFailureRetryTimeoutInSec = this.m_SessionConfig.dispatcherWaitTimeInSeconds;
		}

		private void RestoreConfig()
		{
			string jsonData = this.m_CloudService.RestoreFile("config");
			this.m_SessionConfig.Restore(jsonData);
		}

		private void UpdateConfigFromServer()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{
					"platform",
					this.m_PlatformWrapper.platformName
				},
				{
					"sdk_ver",
					UnityAnalyticsVersion.sdkVersion
				},
				{
					"userid",
					this.m_UserId
				}
			};
			string value = this.m_SessionValues.TryGetString("config_etag", null);
			if (!string.IsNullOrEmpty(value))
			{
				dictionary["If-None-Match"] = value;
			}
			string url = this.m_SessionConfig.configEndPoint + "/" + this.m_AppId + ".json";
			if (Application.isWebPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
			{
				dictionary = null;
			}
			this.m_CloudService.SaveFileFromServer("config", url, dictionary, this, new Action<string, string, bool, int>(this.OnDoneSaveFileFromServer));
		}

		public void OnDoneSaveFileFromServer(string fileName, string etag, bool fileUpdated, int responseStatus)
		{
			if (!string.IsNullOrEmpty(etag))
			{
				this.m_SessionValues.PutItem("config_etag", etag);
			}
			if (!this.m_SessionConfig.analyticsEnabled)
			{
				return;
			}
			if (fileUpdated)
			{
				this.RestoreConfig();
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (this.IsDebugDevice())
			{
				dictionary["debug_device"] = "true";
			}
			this.PrepareCloudServiceConfig();
			this.m_CloudService.StartEventDispatcher(this.m_CloudServiceConfig, dictionary);
		}

		public enum State
		{
			Stopped,
			Running,
			Paused
		}
	}
}
