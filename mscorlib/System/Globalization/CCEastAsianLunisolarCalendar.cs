using System;

namespace System.Globalization
{
	internal class CCEastAsianLunisolarCalendar
	{
		private const int initial_epact = 29;

		public static int fixed_from_dmy(int day, int month, int year)
		{
			throw new Exception("fixed_from_dmy");
		}

		public static int year_from_fixed(int date)
		{
			throw new Exception("year_from_fixed");
		}

		public static void my_from_fixed(out int month, out int year, int date)
		{
			throw new Exception("my_from_fixed");
		}

		public static void dmy_from_fixed(out int day, out int month, out int year, int date)
		{
			throw new Exception("dmy_from_fixed");
		}

		public static DateTime AddMonths(DateTime date, int months)
		{
			throw new Exception("AddMonths");
		}

		public static DateTime AddYears(DateTime date, int years)
		{
			throw new Exception("AddYears");
		}

		public static int GetDayOfMonth(DateTime date)
		{
			throw new Exception("GetDayOfMonth");
		}

		public static int GetDayOfYear(DateTime date)
		{
			throw new Exception("GetDayOfYear");
		}

		public static int GetDaysInMonth(int gyear, int month)
		{
			throw new Exception("GetDaysInMonth");
		}

		public static int GetDaysInYear(int year)
		{
			throw new Exception("GetDaysInYear");
		}

		public static int GetMonth(DateTime date)
		{
			throw new Exception("GetMonth");
		}

		public static bool IsLeapMonth(int gyear, int month)
		{
			int num = gyear % 19;
			bool flag = false;
			bool flag2 = false;
			double num2 = 0.0;
			for (int i = 0; i < num; i++)
			{
				int num3 = 0;
				for (int j = 1; j <= month; j++)
				{
					if (flag2)
					{
						num3 += 30;
						flag2 = false;
						if (i == num && j == month)
						{
							return true;
						}
					}
					else
					{
						num3 += ((!flag) ? 29 : 30);
						flag = !flag;
						num2 += 30.44;
						if (num2 - (double)num3 > 29.0)
						{
							flag2 = true;
						}
					}
				}
			}
			return false;
		}

		public static bool IsLeapYear(int gyear)
		{
			int num = gyear % 19;
			int num2 = num;
			switch (num2)
			{
			case 6:
			case 9:
			case 11:
				break;
			default:
				switch (num2)
				{
				case 0:
				case 3:
					break;
				default:
					switch (num2)
					{
					case 14:
					case 17:
						return true;
					}
					return false;
				}
				break;
			}
			return true;
		}

		public static DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			throw new Exception("ToDateTime");
		}
	}
}
