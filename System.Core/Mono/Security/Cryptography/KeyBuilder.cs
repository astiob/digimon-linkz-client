using System;
using System.Security.Cryptography;

namespace Mono.Security.Cryptography
{
	public sealed class KeyBuilder
	{
		private static RandomNumberGenerator rng;

		private KeyBuilder()
		{
		}

		private static RandomNumberGenerator Rng
		{
			get
			{
				if (KeyBuilder.rng == null)
				{
					KeyBuilder.rng = RandomNumberGenerator.Create();
				}
				return KeyBuilder.rng;
			}
		}

		public static byte[] Key(int size)
		{
			byte[] array = new byte[size];
			KeyBuilder.Rng.GetBytes(array);
			return array;
		}

		public static byte[] IV(int size)
		{
			byte[] array = new byte[size];
			KeyBuilder.Rng.GetBytes(array);
			return array;
		}
	}
}
