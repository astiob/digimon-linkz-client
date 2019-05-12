using System;
using System.Diagnostics;

namespace Mono.Security.Protocol.Tls
{
	internal class DebugHelper
	{
		private static bool isInitialized;

		[Conditional("DEBUG")]
		public static void Initialize()
		{
			if (!DebugHelper.isInitialized)
			{
				DebugHelper.isInitialized = true;
			}
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string format, params object[] args)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message, byte[] buffer)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteBuffer(byte[] buffer)
		{
		}

		[Conditional("DEBUG")]
		public static void WriteBuffer(byte[] buffer, int index, int length)
		{
			for (int i = index; i < length; i += 16)
			{
				int num = (length - i < 16) ? (length - i) : 16;
				string str = string.Empty;
				for (int j = 0; j < num; j++)
				{
					str = str + buffer[i + j].ToString("x2") + " ";
				}
			}
		}
	}
}
