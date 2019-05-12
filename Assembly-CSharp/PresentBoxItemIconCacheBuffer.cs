using System;

public sealed class PresentBoxItemIconCacheBuffer : AssetDataCacheBuffer
{
	private const int CACHE_ICON_NUM = 40;

	private static PresentBoxItemIconCacheBuffer instance;

	private PresentBoxItemIconCacheBuffer(int cacheSize) : base(cacheSize)
	{
	}

	public static PresentBoxItemIconCacheBuffer Instance()
	{
		if (PresentBoxItemIconCacheBuffer.instance == null)
		{
			PresentBoxItemIconCacheBuffer.instance = new PresentBoxItemIconCacheBuffer(40);
		}
		return PresentBoxItemIconCacheBuffer.instance;
	}
}
