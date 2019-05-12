using CROOZ.Chopin.Core;
using System;

namespace Neptune.Common
{
	public class NpHash
	{
		public static string GetHashString(string text, NpHashMode mode)
		{
			byte[] bytes = NpHashConvert.GetBytes(text, (int)mode);
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}

		public static string GetHashPath(string path, NpHashMode mode)
		{
			byte[] bytes = NpHashConvert.GetBytes(path, (int)mode);
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}
	}
}
