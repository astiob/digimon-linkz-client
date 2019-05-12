using System;

namespace UnityEngine.Analytics
{
	[EnumCase(EnumCase.Styles.Snake)]
	public enum AcquisitionSource
	{
		None,
		Store,
		Earned,
		Promotion,
		Gift,
		RewardedAd,
		TimedReward,
		SocialReward
	}
}
