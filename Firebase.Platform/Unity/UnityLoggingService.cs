using Firebase.Platform;
using System;

namespace Firebase.Unity
{
	internal class UnityLoggingService : ILoggingService
	{
		private static UnityLoggingService _instance = new UnityLoggingService();

		public static UnityLoggingService Instance
		{
			get
			{
				return UnityLoggingService._instance;
			}
		}

		public void LogMessage(PlatformLogLevel level, string message)
		{
			FirebaseLogger.LogMessage(level, message);
		}
	}
}
