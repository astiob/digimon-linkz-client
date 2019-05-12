using System;

namespace System.Globalization
{
	internal class CCFixed
	{
		public static DateTime ToDateTime(int date)
		{
			long ticks = (long)(date - 1) * 864000000000L;
			return new DateTime(ticks);
		}

		public static DateTime ToDateTime(int date, int hour, int minute, int second, double milliseconds)
		{
			return CCFixed.ToDateTime(date).AddHours((double)hour).AddMinutes((double)minute).AddSeconds((double)second).AddMilliseconds(milliseconds);
		}

		public static int FromDateTime(DateTime time)
		{
			return 1 + (int)(time.Ticks / 864000000000L);
		}

		public static DayOfWeek day_of_week(int date)
		{
			return (DayOfWeek)CCMath.mod(date, 7);
		}

		public static int kday_on_or_before(int date, int k)
		{
			return date - (int)CCFixed.day_of_week(date - k);
		}

		public static int kday_on_or_after(int date, int k)
		{
			return CCFixed.kday_on_or_before(date + 6, k);
		}

		public static int kd_nearest(int date, int k)
		{
			return CCFixed.kday_on_or_before(date + 3, k);
		}

		public static int kday_after(int date, int k)
		{
			return CCFixed.kday_on_or_before(date + 7, k);
		}

		public static int kday_before(int date, int k)
		{
			return CCFixed.kday_on_or_before(date - 1, k);
		}
	}
}
