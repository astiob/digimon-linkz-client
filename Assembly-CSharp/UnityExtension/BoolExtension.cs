using System;

namespace UnityExtension
{
	public static class BoolExtension
	{
		public static bool AllMachValue(bool checkValue, params bool[] flag)
		{
			foreach (bool flag2 in flag)
			{
				if (flag2 != checkValue)
				{
					return false;
				}
			}
			return true;
		}

		public static bool Inverse(bool source, bool isInverse)
		{
			if (isInverse)
			{
				return !source;
			}
			return source;
		}
	}
}
