using System;

namespace CROOZ.Chopin.Core
{
	public class NpHashConvert
	{
		public static byte[] GetBytes(string data, int mode)
		{
			switch (mode)
			{
			case 0:
			{
				\uE00F uE00F = new \uE00F();
				return uE00F.\uE002(data);
			}
			case 1:
			{
				\uE010 uE = new \uE010();
				return uE.\uE002(data);
			}
			case 2:
			{
				\uE011 uE2 = new \uE011();
				return uE2.\uE002(data);
			}
			case 3:
			{
				\uE011 uE3 = new \uE011();
				return uE3.\uE002(data);
			}
			default:
				return null;
			}
		}
	}
}
