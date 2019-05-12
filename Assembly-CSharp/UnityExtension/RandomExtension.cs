using System;
using UnityEngine;

namespace UnityExtension
{
	public static class RandomExtension
	{
		private static bool Bool()
		{
			return UnityEngine.Random.Range(0, 1) == 0;
		}

		public static int IntPlusMinus()
		{
			if (RandomExtension.Bool())
			{
				return -1;
			}
			return 1;
		}

		public static int IntPlusMinus(System.Random randomSource)
		{
			int num;
			if (randomSource != null)
			{
				num = randomSource.Next(2);
			}
			else
			{
				num = UnityEngine.Random.Range(0, 2);
			}
			int result = 1;
			if (num == 0)
			{
				result = -1;
			}
			return result;
		}

		public static bool Switch(float percentage, System.Random randomSource = null)
		{
			float num = UnityEngine.Random.Range(0f, 1f);
			return num <= percentage;
		}
	}
}
