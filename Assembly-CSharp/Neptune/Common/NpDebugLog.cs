using System;
using System.IO;

namespace Neptune.Common
{
	public class NpDebugLog
	{
		public static void Dump(byte[] bytes)
		{
		}

		public static void Dump(Stream stream)
		{
		}

		public static void Log(string log)
		{
		}

		public static void EnableNativeDebugLog(bool isEnabled)
		{
			NpDebugLogAndroid.EnableNativeDebugLog(isEnabled);
		}
	}
}
