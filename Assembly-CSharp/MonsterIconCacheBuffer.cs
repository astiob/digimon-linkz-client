using System;

public sealed class MonsterIconCacheBuffer : AssetDataCacheBuffer
{
	private const int CACHE_ICON_NUM = 80;

	private static MonsterIconCacheBuffer instance;

	private MonsterIconCacheBuffer(int cacheSize) : base(cacheSize)
	{
	}

	public static MonsterIconCacheBuffer Instance()
	{
		if (MonsterIconCacheBuffer.instance == null)
		{
			MonsterIconCacheBuffer.instance = new MonsterIconCacheBuffer(80);
		}
		return MonsterIconCacheBuffer.instance;
	}
}
