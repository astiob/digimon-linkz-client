using System;

namespace System.Globalization
{
	internal class CCJulianCalendar
	{
		private const int epoch = -1;

		public static bool is_leap_year(int year)
		{
			return CCMath.mod(year, 4) == ((year <= 0) ? 3 : 0);
		}

		public static int fixed_from_dmy(int day, int month, int year)
		{
			int num = (year >= 0) ? year : (year + 1);
			int num2 = -2;
			num2 += 365 * (num - 1);
			num2 += CCMath.div(num - 1, 4);
			num2 += CCMath.div(367 * month - 362, 12);
			if (month > 2)
			{
				num2 += ((!CCJulianCalendar.is_leap_year(year)) ? -2 : -1);
			}
			return num2 + day;
		}

		public static int year_from_fixed(int date)
		{
			int num = CCMath.div(4 * (date - -1) + 1464, 1461);
			return (num > 0) ? num : (num - 1);
		}

		public static void my_from_fixed(out int month, out int year, int date)
		{
			year = CCJulianCalendar.year_from_fixed(date);
			int num = date - CCJulianCalendar.fixed_from_dmy(1, 1, year);
			int num2;
			if (date < CCJulianCalendar.fixed_from_dmy(1, 3, year))
			{
				num2 = 0;
			}
			else if (CCJulianCalendar.is_leap_year(year))
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
			CCJulianCalendar.my_from_fixed(out month, out year, date);
			day = date - CCJulianCalendar.fixed_from_dmy(1, month, year) + 1;
		}

		public static int month_from_fixed(int date)
		{
			int result;
			int num;
			CCJulianCalendar.my_from_fixed(out result, out num, date);
			return result;
		}

		public static int day_from_fixed(int date)
		{
			int result;
			int num;
			int num2;
			CCJulianCalendar.dmy_from_fixed(out result, out num, out num2, date);
			return result;
		}

		public static int date_difference(int dayA, int monthA, int yearA, int dayB, int monthB, int yearB)
		{
			return CCJulianCalendar.fixed_from_dmy(dayB, monthB, yearB) - CCJulianCalendar.fixed_from_dmy(dayA, monthA, yearA);
		}

		public static int day_number(int day, int month, int year)
		{
			return CCJulianCalendar.date_difference(31, 12, year - 1, day, month, year);
		}

		public static int days_remaining(int day, int month, int year)
		{
			return CCJulianCalendar.date_difference(day, month, year, 31, 12, year);
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
