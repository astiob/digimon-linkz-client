using System;

public sealed class MissionBannerCacheBuffer : AssetDataCacheBuffer
{
	private const int CACHE_ICON_NUM = 2;

	private static MissionBannerCacheBuffer instance;

	private MissionBannerCacheBuffer(int cacheSize) : base(cacheSize)
	{
	}

	public static MissionBannerCacheBuffer Instance()
	{
		if (MissionBannerCacheBuffer.instance == null)
		{
			MissionBannerCacheBuffer.instance = new MissionBannerCacheBuffer(2);
		}
		return MissionBannerCacheBuffer.instance;
	}

	public static void ClearCacheBuffer()
	{
		MissionBannerCacheBuffer.instance = null;
	}
}
