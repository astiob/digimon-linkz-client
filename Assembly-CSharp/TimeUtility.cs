using Master;
using System;

public static class TimeUtility
{
	private const int DAY_SEC = 86400;

	private const int HOUR_SEC = 3600;

	private const int MINUTE_SEC = 60;

	public static string ToBuildTime(this string buildTimeSeconds)
	{
		return buildTimeSeconds.ToInt32().ToBuildTime();
	}

	public static string ToBuildTime(this int buildTimeSeconds)
	{
		string result = string.Empty;
		int num;
		int num2;
		int num3;
		int num4;
		TimeUtility.GetTimeParts(buildTimeSeconds, out num, out num2, out num3, out num4);
		if (1 <= num)
		{
			if (1 <= num2)
			{
				result = string.Format(StringMaster.GetString("SystemTimeDh"), num, num2);
			}
			else
			{
				result = string.Format(StringMaster.GetString("SystemTimeD"), num);
			}
		}
		else if (1 <= num2)
		{
			if (1 <= num3)
			{
				result = string.Format(StringMaster.GetString("SystemTimeHM"), num2, num3);
			}
			else
			{
				result = string.Format(StringMaster.GetString("SystemTimeH"), num2);
			}
		}
		else if (1 <= num3)
		{
			if (1 <= num4)
			{
				result = string.Format(StringMaster.GetString("SystemTimeMS"), num3, num4);
			}
			else
			{
				result = string.Format(StringMaster.GetString("SystemTimeM"), num3);
			}
		}
		else
		{
			result = string.Format(StringMaster.GetString("SystemTimeS"), num4);
		}
		return result;
	}

	public static string ToStaminaRecoveryTime(int countDown)
	{
		int num;
		int num2;
		int num3;
		int num4;
		TimeUtility.GetTimeParts(countDown, out num, out num2, out num3, out num4);
		string arg = string.Empty;
		if (0 < num2)
		{
			arg = string.Format(StringMaster.GetString("SystemTimeHMS"), num2, num3, num4);
		}
		else if (0 < num3)
		{
			arg = string.Format(StringMaster.GetString("SystemTimeMS"), num3, num4);
		}
		else if (0 < num4)
		{
			arg = string.Format(StringMaster.GetString("SystemTimeS"), num4);
		}
		return string.Format(StringMaster.GetString("StaminaRecoveryTime"), arg);
	}

	public static DateTime ToJPLocalDateTime(DateTime jpDateTime)
	{
		DateTime dateTime = default(DateTime);
		DateTime dateTime2 = default(DateTime);
		dateTime2 = jpDateTime;
		DateTime dateTime3 = new DateTime(dateTime2.AddHours(-9.0).Ticks, DateTimeKind.Utc);
		return dateTime3.ToLocalTime();
	}

	public static DateTime DateTimeDifferenceServer(DateTime dateTime)
	{
		int num = dateTime.Day - ServerDateTime.Now.Day;
		int num2 = dateTime.Month - ServerDateTime.Now.Month;
		int num3 = dateTime.Year - ServerDateTime.Now.Year;
		DateTime result = dateTime;
		if (num != 0)
		{
			result = result.AddDays((double)(-(double)num));
		}
		if (num2 != 0)
		{
			result = result.AddMonths(-num2);
		}
		if (num3 != 0)
		{
			result = result.AddYears(-num3);
		}
		return result;
	}

	private static void GetTimeParts(int totalTime, out int day, out int hour, out int minutes, out int seconds)
	{
		day = totalTime / 86400;
		int num = totalTime - 86400 * day;
		hour = num / 3600;
		num -= 3600 * hour;
		minutes = num / 60;
		seconds = num - 60 * minutes;
	}
}
