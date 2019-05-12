using System;

namespace System.Globalization
{
	internal class CCMath
	{
		public static double round(double x)
		{
			return Math.Floor(x + 0.5);
		}

		public static double mod(double x, double y)
		{
			return x - y * Math.Floor(x / y);
		}

		public static int div(int x, int y)
		{
			return (int)Math.Floor((double)x / (double)y);
		}

		public static int mod(int x, int y)
		{
			return x - y * CCMath.div(x, y);
		}

		public static int div_mod(out int remainder, int x, int y)
		{
			int num = CCMath.div(x, y);
			remainder = x - y * num;
			return num;
		}

		public static int signum(double x)
		{
			if (x < 0.0)
			{
				return -1;
			}
			if (x == 0.0)
			{
				return 0;
			}
			return 1;
		}

		public static int signum(int x)
		{
			if (x < 0)
			{
				return -1;
			}
			if (x == 0)
			{
				return 0;
			}
			return 1;
		}

		public static double amod(double x, double y)
		{
			double num = CCMath.mod(x, y);
			return (num != 0.0) ? num : y;
		}

		public static int amod(int x, int y)
		{
			int num = CCMath.mod(x, y);
			return (num != 0) ? num : y;
		}
	}
}
