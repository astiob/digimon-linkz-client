using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Advertisements
{
	internal sealed class UnityAdsInternal
	{
		public static event UnityAdsDelegate onCampaignsAvailable;

		public static event UnityAdsDelegate onCampaignsFetchFailed;

		public static event UnityAdsDelegate onShow;

		public static event UnityAdsDelegate onHide;

		public static event UnityAdsDelegate<string, bool> onVideoCompleted;

		public static event UnityAdsDelegate onVideoStarted;

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RegisterNative();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Init(string gameId, bool testModeEnabled, bool debugModeEnabled, string unityVersion);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Show(string zoneId, string rewardItemKey, string options);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CanShowAds(string zoneId);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetLogLevel(int logLevel);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCampaignDataURL(string url);

		public static void RemoveAllEventHandlers()
		{
			UnityAdsInternal.onCampaignsAvailable = null;
			UnityAdsInternal.onCampaignsFetchFailed = null;
			UnityAdsInternal.onShow = null;
			UnityAdsInternal.onHide = null;
			UnityAdsInternal.onVideoCompleted = null;
			UnityAdsInternal.onVideoStarted = null;
		}

		public static void CallUnityAdsCampaignsAvailable()
		{
			UnityAdsDelegate unityAdsDelegate = UnityAdsInternal.onCampaignsAvailable;
			if (unityAdsDelegate != null)
			{
				unityAdsDelegate();
			}
		}

		public static void CallUnityAdsCampaignsFetchFailed()
		{
			UnityAdsDelegate unityAdsDelegate = UnityAdsInternal.onCampaignsFetchFailed;
			if (unityAdsDelegate != null)
			{
				unityAdsDelegate();
			}
		}

		public static void CallUnityAdsShow()
		{
			UnityAdsDelegate unityAdsDelegate = UnityAdsInternal.onShow;
			if (unityAdsDelegate != null)
			{
				unityAdsDelegate();
			}
		}

		public static void CallUnityAdsHide()
		{
			UnityAdsDelegate unityAdsDelegate = UnityAdsInternal.onHide;
			if (unityAdsDelegate != null)
			{
				unityAdsDelegate();
			}
		}

		public static void CallUnityAdsVideoCompleted(string rewardItemKey, bool skipped)
		{
			UnityAdsDelegate<string, bool> unityAdsDelegate = UnityAdsInternal.onVideoCompleted;
			if (unityAdsDelegate != null)
			{
				unityAdsDelegate(rewardItemKey, skipped);
			}
		}

		public static void CallUnityAdsVideoStarted()
		{
			UnityAdsDelegate unityAdsDelegate = UnityAdsInternal.onVideoStarted;
			if (unityAdsDelegate != null)
			{
				unityAdsDelegate();
			}
		}
	}
}
