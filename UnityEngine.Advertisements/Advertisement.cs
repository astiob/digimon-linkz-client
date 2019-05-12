using System;

namespace UnityEngine.Advertisements
{
	public static class Advertisement
	{
		private static bool s_Initialized = false;

		private static bool s_Showing = false;

		private static ShowOptions s_ShowOptions = null;

		private static bool s_ResultDelivered = false;

		private static UnityAdsPlatform s_Implementation = null;

		private static string s_ExtensionPath = null;

		private static Advertisement.DebugLevel _debugLevel = (!Debug.isDebugBuild) ? (Advertisement.DebugLevel.Error | Advertisement.DebugLevel.Warning | Advertisement.DebugLevel.Info) : (Advertisement.DebugLevel.Error | Advertisement.DebugLevel.Warning | Advertisement.DebugLevel.Info | Advertisement.DebugLevel.Debug);

		[RuntimeInitializeOnLoadMethod]
		private static void LoadRuntime()
		{
			if (Advertisement.s_Implementation == null)
			{
				if (Application.isEditor)
				{
					return;
				}
				if (Advertisement.isSupported)
				{
					Advertisement.s_Implementation = new UnityAdsNative();
				}
				else
				{
					Advertisement.s_Implementation = new UnityAdsUnsupported();
				}
				Advertisement.Load();
			}
		}

		public static void LoadEditor(string extensionPath)
		{
			if (Advertisement.s_Implementation == null)
			{
				Advertisement.s_ExtensionPath = extensionPath;
				Advertisement.s_Implementation = new UnityAdsEditor();
				Advertisement.Load();
			}
		}

		private static void Load()
		{
			if (Advertisement.s_Implementation != null && Advertisement.isSupported)
			{
				Advertisement.RegisterNative();
				if (Advertisement.initializeOnStartup)
				{
					Advertisement.Initialize(Advertisement.gameId, Advertisement.testMode);
				}
			}
		}

		private static bool initializeOnStartup
		{
			get
			{
				return UnityAdsManager.initializeOnStartup;
			}
		}

		private static void RegisterNative()
		{
			Advertisement.s_Implementation.RegisterNative(Advertisement.s_ExtensionPath);
		}

		public static Advertisement.DebugLevel debugLevel
		{
			get
			{
				return Advertisement._debugLevel;
			}
			set
			{
				Advertisement._debugLevel = value;
				Advertisement.s_Implementation.SetLogLevel(Advertisement._debugLevel);
			}
		}

		public static bool isSupported
		{
			get
			{
				return (Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) && Advertisement.IsEnabled();
			}
		}

		private static bool IsEnabled()
		{
			return UnityAdsManager.enabled && UnityAdsManager.IsPlatformEnabled(Application.platform);
		}

		public static bool isInitialized
		{
			get
			{
				return Advertisement.s_Initialized;
			}
		}

		public static void Initialize(string appId)
		{
			Advertisement.Initialize(appId, false);
		}

		public static void Initialize(string appId, bool testMode)
		{
			if (!Advertisement.s_Initialized)
			{
				Advertisement.s_Initialized = true;
				UnityAdsInternal.onHide += Advertisement.OnHide;
				UnityAdsInternal.onShow += Advertisement.OnShow;
				UnityAdsInternal.onVideoCompleted += Advertisement.OnVideoCompleted;
				Advertisement.s_Implementation.Init(appId, testMode);
			}
		}

		public static void Show()
		{
			Advertisement.Show(null, null);
		}

		public static void Show(string zoneId)
		{
			Advertisement.Show(zoneId, null);
		}

		public static void Show(string zoneId, ShowOptions options)
		{
			if (!Advertisement.s_Showing && Advertisement.s_Implementation.Show(zoneId, (options == null) ? null : options.gamerSid))
			{
				Advertisement.s_Showing = true;
				Advertisement.s_ResultDelivered = false;
				Advertisement.s_ShowOptions = options;
			}
			else if (options != null && options.resultCallback != null)
			{
				options.resultCallback(ShowResult.Failed);
			}
		}

		public static bool IsReady()
		{
			return Advertisement.IsReady(string.Empty);
		}

		public static bool IsReady(string zoneId)
		{
			return Advertisement.s_Implementation.CanShowAds(zoneId);
		}

		public static bool isShowing
		{
			get
			{
				return Advertisement.s_Showing;
			}
		}

		public static void SetCampaignDataURL(string url)
		{
			Advertisement.s_Implementation.SetCampaignDataURL(url);
		}

		private static void OnHide()
		{
			Advertisement.s_Showing = false;
			Advertisement.DeliverResult(ShowResult.Skipped);
		}

		private static void OnShow()
		{
			Advertisement.s_Showing = true;
		}

		private static void OnVideoCompleted(string rewardItemKey, bool skipped)
		{
			Advertisement.DeliverResult((!skipped) ? ShowResult.Finished : ShowResult.Skipped);
		}

		public static bool testMode
		{
			get
			{
				return UnityAdsManager.testMode;
			}
		}

		public static string gameId
		{
			get
			{
				return UnityAdsManager.GetGameId(Application.platform);
			}
		}

		private static void DeliverResult(ShowResult result)
		{
			if (!Advertisement.s_ResultDelivered && Advertisement.s_ShowOptions != null && Advertisement.s_ShowOptions.resultCallback != null)
			{
				Advertisement.s_ResultDelivered = true;
				Advertisement.s_ShowOptions.resultCallback(result);
			}
		}

		[Flags]
		public enum DebugLevel
		{
			None = 0,
			Error = 1,
			Warning = 2,
			Info = 4,
			Debug = 8
		}
	}
}
