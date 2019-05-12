using System;

public sealed class TitleIconCacheBuffer : AssetDataCacheBuffer
{
	private const int CACHE_ICON_NUM = 4;

	private static TitleIconCacheBuffer instance;

	private TitleIconCacheBuffer(int cacheSize) : base(cacheSize)
	{
	}

	public static TitleIconCacheBuffer Instance()
	{
		if (TitleIconCacheBuffer.instance == null)
		{
			TitleIconCacheBuffer.instance = new TitleIconCacheBuffer(4);
		}
		return TitleIconCacheBuffer.instance;
	}

	public static void ClearCacheBuffer()
	{
		TitleIconCacheBuffer.instance = null;
	}
}
