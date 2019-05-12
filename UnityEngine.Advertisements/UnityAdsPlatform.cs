using System;

namespace UnityEngine.Advertisements
{
	internal abstract class UnityAdsPlatform
	{
		public abstract void RegisterNative(string extensionPath);

		public abstract void Init(string gameId, bool testModeEnabled);

		public abstract bool CanShowAds(string zoneId);

		public abstract void SetCampaignDataURL(string url);

		public abstract void SetLogLevel(Advertisement.DebugLevel logLevel);

		public abstract bool Show(string zoneId, string gamerSid);
	}
}
