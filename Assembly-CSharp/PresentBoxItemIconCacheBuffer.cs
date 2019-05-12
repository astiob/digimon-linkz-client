using System;

public sealed class PresentBoxItemIconCacheBuffer : AssetDataCacheBuffer
{
	private const int CACHE_ICON_NUM = 300;

	private static PresentBoxItemIconCacheBuffer instance;

	private PresentBoxItemIconCacheBuffer(int cacheSize) : base(cacheSize)
	{
	}

	public static PresentBoxItemIconCacheBuffer Instance()
	{
		if (PresentBoxItemIconCacheBuffer.instance == null)
		{
			PresentBoxItemIconCacheBuffer.instance = new PresentBoxItemIconCacheBuffer(300);
		}
		return PresentBoxItemIconCacheBuffer.instance;
	}
}
