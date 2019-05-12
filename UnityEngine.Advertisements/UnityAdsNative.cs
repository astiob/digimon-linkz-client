using System;

namespace UnityEngine.Advertisements
{
	internal class UnityAdsNative : UnityAdsPlatform
	{
		private bool s_FetchCompleted;

		public override void RegisterNative(string extensionPath)
		{
			UnityAdsInternal.RegisterNative();
		}

		public override void Init(string gameId, bool testModeEnabled)
		{
			UnityAdsInternal.onCampaignsAvailable += this.OnFetchCompleted;
			UnityAdsInternal.onCampaignsFetchFailed += this.OnFetchFailed;
			UnityAdsInternal.Init(gameId, testModeEnabled, (Advertisement.debugLevel & Advertisement.DebugLevel.Debug) != Advertisement.DebugLevel.None, Application.unityVersion);
		}

		public override bool CanShowAds(string zoneId)
		{
			return this.s_FetchCompleted && UnityAdsInternal.CanShowAds(zoneId);
		}

		public override void SetLogLevel(Advertisement.DebugLevel logLevel)
		{
			UnityAdsInternal.SetLogLevel((int)logLevel);
		}

		public override void SetCampaignDataURL(string url)
		{
			UnityAdsInternal.SetCampaignDataURL(url);
		}

		public override bool Show(string zoneId, string gamerSid)
		{
			string options = null;
			if (gamerSid != null)
			{
				options = "sid:" + gamerSid;
			}
			return UnityAdsInternal.Show(zoneId, string.Empty, options);
		}

		public void OnFetchCompleted()
		{
			this.s_FetchCompleted = true;
		}

		public void OnFetchFailed()
		{
			this.s_FetchCompleted = false;
		}
	}
}
