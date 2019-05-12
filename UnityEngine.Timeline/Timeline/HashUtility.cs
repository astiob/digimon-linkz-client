using System;

namespace UnityEngine.Timeline
{
	internal static class HashUtility
	{
		public static int CombineHash(this int h1, int h2)
		{
			return h1 ^ (int)((long)h2 + (long)((ulong)-1640531527) + (long)((long)h1 << 6) + (long)(h1 >> 2));
		}

		public static int CombineHash(int h1, int h2, int h3)
		{
			return h1.CombineHash(h2).CombineHash(h3);
		}

		public static int CombineHash(int h1, int h2, int h3, int h4)
		{
			return HashUtility.CombineHash(h1, h2, h3).CombineHash(h4);
		}

		public static int CombineHash(int h1, int h2, int h3, int h4, int h5)
		{
			return HashUtility.CombineHash(h1, h2, h3, h4).CombineHash(h5);
		}

		public static int CombineHash(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return HashUtility.CombineHash(h1, h2, h3, h4, h5).CombineHash(h6);
		}

		public static int CombineHash(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return HashUtility.CombineHash(h1, h2, h3, h4, h5, h6).CombineHash(h7);
		}

		public static int CombineHash(int[] hashes)
		{
			int result;
			if (hashes == null || hashes.Length == 0)
			{
				result = 0;
			}
			else
			{
				int num = hashes[0];
				for (int i = 1; i < hashes.Length; i++)
				{
					num = num.CombineHash(hashes[i]);
				}
				result = num;
			}
			return result;
		}
	}
}
