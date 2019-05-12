using System;

namespace Firebase.Platform
{
	internal class DebugLogger : ILoggingService
	{
		private static DebugLogger _instance = new DebugLogger();

		public static DebugLogger Instance
		{
			get
			{
				return DebugLogger._instance;
			}
		}

		public void LogMessage(PlatformLogLevel level, string message)
		{
		}
	}
}
