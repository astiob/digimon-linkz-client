using System;

namespace System.Globalization
{
	internal class CCHebrewCalendar
	{
		private const int epoch = -1373427;

		public static bool is_leap_year(int year)
		{
			return CCMath.mod(7 * year + 1, 19) < 7;
		}

		public static int last_month_of_year(int year)
		{
			return (!CCHebrewCalendar.is_leap_year(year)) ? 12 : 13;
		}

		public static int elapsed_days(int year)
		{
			int num = CCMath.div(235 * year - 234, 19);
			int num3;
			int num2 = CCMath.div_mod(out num3, num, 1080);
			int x = 204 + 793 * num3;
			int x2 = 11 + 12 * num + 793 * num2 + CCMath.div(x, 1080);
			int num4 = 29 * num + CCMath.div(x2, 24);
			if (CCMath.mod(3 * (num4 + 1), 7) < 3)
			{
				num4++;
			}
			return num4;
		}

		public static int new_year_delay(int year)
		{
			int num = CCHebrewCalendar.elapsed_days(year);
			int num2 = CCHebrewCalendar.elapsed_days(year + 1);
			if (num2 - num == 356)
			{
				return 2;
			}
			int num3 = CCHebrewCalendar.elapsed_days(year - 1);
			if (num - num3 == 382)
			{
				return 1;
			}
			return 0;
		}

		public static int last_day_of_month(int month, int year)
		{
			if (month < 1 || month > 13)
			{
				throw new ArgumentOutOfRangeException("month", "Month should be between One and Thirteen.");
			}
			switch (month)
			{
			case 2:
				return 29;
			case 4:
				return 29;
			case 6:
				return 29;
			case 8:
				if (!CCHebrewCalendar.long_heshvan(year))
				{
					return 29;
				}
				break;
			case 9:
				if (CCHebrewCalendar.short_kislev(year))
				{
					return 29;
				}
				break;
			case 10:
				return 29;
			case 12:
				if (!CCHebrewCalendar.is_leap_year(year))
				{
					return 29;
				}
				break;
			case 13:
				return 29;
			}
			return 30;
		}

		public static bool long_heshvan(int year)
		{
			return CCMath.mod(CCHebrewCalendar.days_in_year(year), 10) == 5;
		}

		public static bool short_kislev(int year)
		{
			return CCMath.mod(CCHebrewCalendar.days_in_year(year), 10) == 3;
		}

		public static int days_in_year(int year)
		{
			return CCHebrewCalendar.fixed_from_dmy(1, 7, year + 1) - CCHebrewCalendar.fixed_from_dmy(1, 7, year);
		}

		public static int fixed_from_dmy(int day, int month, int year)
		{
			int num = -1373428;
			num += CCHebrewCalendar.elapsed_days(year);
			num += CCHebrewCalendar.new_year_delay(year);
			if (month < 7)
			{
				int num2 = CCHebrewCalendar.last_month_of_year(year);
				for (int i = 7; i <= num2; i++)
				{
					num += CCHebrewCalendar.last_day_of_month(i, year);
				}
				for (int i = 1; i < month; i++)
				{
					num += CCHebrewCalendar.last_day_of_month(i, year);
				}
			}
			else
			{
				for (int i = 7; i < month; i++)
				{
					num += CCHebrewCalendar.last_day_of_month(i, year);
				}
			}
			return num + day;
		}

		public static int year_from_fixed(int date)
		{
			int num = (int)Math.Floor((double)(date - -1373427) / 365.24682220597794);
			int num2 = num;
			while (date >= CCHebrewCalendar.fixed_from_dmy(1, 7, num2))
			{
				num2++;
			}
			return num2 - 1;
		}

		public static void my_from_fixed(out int month, out int year, int date)
		{
			year = CCHebrewCalendar.year_from_fixed(date);
			int num = (date >= CCHebrewCalendar.fixed_from_dmy(1, 1, year)) ? 1 : 7;
			month = num;
			while (date > CCHebrewCalendar.fixed_from_dmy(CCHebrewCalendar.last_day_of_month(month, year), month, year))
			{
				month++;
			}
		}

		public static void dmy_from_fixed(out int day, out int month, out int year, int date)
		{
			CCHebrewCalendar.my_from_fixed(out month, out year, date);
			day = date - CCHebrewCalendar.fixed_from_dmy(1, month, year) + 1;
		}

		public static int month_from_fixed(int date)
		{
			int result;
			int num;
			CCHebrewCalendar.my_from_fixed(out result, out num, date);
			return result;
		}

		public static int day_from_fixed(int date)
		{
			int result;
			int num;
			int num2;
			CCHebrewCalendar.dmy_from_fixed(out result, out num, out num2, date);
			return result;
		}

		public static int date_difference(int dayA, int monthA, int yearA, int dayB, int monthB, int yearB)
		{
			return CCHebrewCalendar.fixed_from_dmy(dayB, monthB, yearB) - CCHebrewCalendar.fixed_from_dmy(dayA, monthA, yearA);
		}

		public static int day_number(int day, int month, int year)
		{
			return CCHebrewCalendar.date_difference(1, 7, year, day, month, year) + 1;
		}

		public static int days_remaining(int day, int month, int year)
		{
			return CCHebrewCalendar.date_difference(day, month, year, 1, 7, year + 1) - 1;
		}

		public enum Month
		{
			nisan = 1,
			iyyar,
			sivan,
			tammuz,
			av,
			elul,
			tishri,
			heshvan,
			kislev,
			teveth,
			shevat,
			adar,
			adar_I = 12,
			adar_II
		}
	}
}
