using System;

namespace System.Globalization
{
	internal class CCGregorianCalendar
	{
		private const int epoch = 1;

		public static bool is_leap_year(int year)
		{
			if (CCMath.mod(year, 4) != 0)
			{
				return false;
			}
			int num = CCMath.mod(year, 400);
			return num != 100 && num != 200 && num != 300;
		}

		public static int fixed_from_dmy(int day, int month, int year)
		{
			int num = 0;
			num += 365 * (year - 1);
			num += CCMath.div(year - 1, 4);
			num -= CCMath.div(year - 1, 100);
			num += CCMath.div(year - 1, 400);
			num += CCMath.div(367 * month - 362, 12);
			if (month > 2)
			{
				num += ((!CCGregorianCalendar.is_leap_year(year)) ? -2 : -1);
			}
			return num + day;
		}

		public static int year_from_fixed(int date)
		{
			int x = date - 1;
			int num = CCMath.div_mod(out x, x, 146097);
			int num2 = CCMath.div_mod(out x, x, 36524);
			int num3 = CCMath.div_mod(out x, x, 1461);
			int num4 = CCMath.div(x, 365);
			int num5 = 400 * num + 100 * num2 + 4 * num3 + num4;
			return (num2 != 4 && num4 != 4) ? (num5 + 1) : num5;
		}

		public static void my_from_fixed(out int month, out int year, int date)
		{
			year = CCGregorianCalendar.year_from_fixed(date);
			int num = date - CCGregorianCalendar.fixed_from_dmy(1, 1, year);
			int num2;
			if (date < CCGregorianCalendar.fixed_from_dmy(1, 3, year))
			{
				num2 = 0;
			}
			else if (CCGregorianCalendar.is_leap_year(year))
			{
				num2 = 1;
			}
			else
			{
				num2 = 2;
			}
			month = CCMath.div(12 * (num + num2) + 373, 367);
		}

		public static void dmy_from_fixed(out int day, out int month, out int year, int date)
		{
			CCGregorianCalendar.my_from_fixed(out month, out year, date);
			day = date - CCGregorianCalendar.fixed_from_dmy(1, month, year) + 1;
		}

		public static int month_from_fixed(int date)
		{
			int result;
			int num;
			CCGregorianCalendar.my_from_fixed(out result, out num, date);
			return result;
		}

		public static int day_from_fixed(int date)
		{
			int result;
			int num;
			int num2;
			CCGregorianCalendar.dmy_from_fixed(out result, out num, out num2, date);
			return result;
		}

		public static int date_difference(int dayA, int monthA, int yearA, int dayB, int monthB, int yearB)
		{
			return CCGregorianCalendar.fixed_from_dmy(dayB, monthB, yearB) - CCGregorianCalendar.fixed_from_dmy(dayA, monthA, yearA);
		}

		public static int day_number(int day, int month, int year)
		{
			return CCGregorianCalendar.date_difference(31, 12, year - 1, day, month, year);
		}

		public static int days_remaining(int day, int month, int year)
		{
			return CCGregorianCalendar.date_difference(day, month, year, 31, 12, year);
		}

		public static DateTime AddMonths(DateTime time, int months)
		{
			int date = CCFixed.FromDateTime(time);
			int num;
			int num2;
			int num3;
			CCGregorianCalendar.dmy_from_fixed(out num, out num2, out num3, date);
			num2 += months;
			num3 += CCMath.div_mod(out num2, num2, 12);
			int daysInMonth = CCGregorianCalendar.GetDaysInMonth(num3, num2);
			if (num > daysInMonth)
			{
				num = daysInMonth;
			}
			date = CCGregorianCalendar.fixed_from_dmy(num, num2, num3);
			return CCFixed.ToDateTime(date).Add(time.TimeOfDay);
		}

		public static DateTime AddYears(DateTime time, int years)
		{
			int date = CCFixed.FromDateTime(time);
			int num;
			int month;
			int num2;
			CCGregorianCalendar.dmy_from_fixed(out num, out month, out num2, date);
			num2 += years;
			int daysInMonth = CCGregorianCalendar.GetDaysInMonth(num2, month);
			if (num > daysInMonth)
			{
				num = daysInMonth;
			}
			date = CCGregorianCalendar.fixed_from_dmy(num, month, num2);
			return CCFixed.ToDateTime(date).Add(time.TimeOfDay);
		}

		public static int GetDayOfMonth(DateTime time)
		{
			return CCGregorianCalendar.day_from_fixed(CCFixed.FromDateTime(time));
		}

		public static int GetDayOfYear(DateTime time)
		{
			int num = CCFixed.FromDateTime(time);
			int year = CCGregorianCalendar.year_from_fixed(num);
			int num2 = CCGregorianCalendar.fixed_from_dmy(1, 1, year);
			return num - num2 + 1;
		}

		public static int GetDaysInMonth(int year, int month)
		{
			int num = CCGregorianCalendar.fixed_from_dmy(1, month, year);
			int num2 = CCGregorianCalendar.fixed_from_dmy(1, month + 1, year);
			return num2 - num;
		}

		public static int GetDaysInYear(int year)
		{
			int num = CCGregorianCalendar.fixed_from_dmy(1, 1, year);
			int num2 = CCGregorianCalendar.fixed_from_dmy(1, 1, year + 1);
			return num2 - num;
		}

		public static int GetMonth(DateTime time)
		{
			return CCGregorianCalendar.month_from_fixed(CCFixed.FromDateTime(time));
		}

		public static int GetYear(DateTime time)
		{
			return CCGregorianCalendar.year_from_fixed(CCFixed.FromDateTime(time));
		}

		public static bool IsLeapDay(int year, int month, int day)
		{
			return CCGregorianCalendar.is_leap_year(year) && month == 2 && day == 29;
		}

		public static DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int milliseconds)
		{
			return CCFixed.ToDateTime(CCGregorianCalendar.fixed_from_dmy(day, month, year), hour, minute, second, (double)milliseconds);
		}

		public enum Month
		{
			january = 1,
			february,
			march,
			april,
			may,
			june,
			july,
			august,
			september,
			october,
			november,
			december
		}
	}
}
