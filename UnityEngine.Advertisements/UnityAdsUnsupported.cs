using System;

namespace UnityEngine.Advertisements
{
	internal class UnityAdsUnsupported : UnityAdsPlatform
	{
		public override void RegisterNative(string extensionPath)
		{
		}

		public override void Init(string gameId, bool testModeEnabled)
		{
		}

		public override bool CanShowAds(string zoneId)
		{
			return false;
		}

		public override void SetCampaignDataURL(string url)
		{
		}

		public override void SetLogLevel(Advertisement.DebugLevel logLevel)
		{
		}

		public override bool Show(string zoneId, string gamerSid)
		{
			return false;
		}
	}
}
