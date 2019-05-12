using System;

public class NpUtil
{
	public static DateTime MsTimestampToDateTime(long timestamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return dateTime.AddMilliseconds((double)timestamp).ToLocalTime();
	}

	public static DateTime SecTimestampToDateTime(long timestamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return dateTime.AddSeconds((double)timestamp).ToLocalTime();
	}
}
