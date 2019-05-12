using System;

namespace Firebase.Platform
{
	internal class FirebaseAppUtils : IFirebaseAppUtils
	{
		private static FirebaseAppUtils instance = new FirebaseAppUtils();

		public static FirebaseAppUtils Instance
		{
			get
			{
				return FirebaseAppUtils.instance;
			}
		}

		public void TranslateDllNotFoundException(Action action)
		{
			FirebaseApp.TranslateDllNotFoundException(action);
		}

		public void PollCallbacks()
		{
			AppUtil.PollCallbacks();
		}

		public IFirebaseAppPlatform GetDefaultInstance()
		{
			return FirebaseApp.DefaultInstance.AppPlatform;
		}

		public string GetDefaultInstanceName()
		{
			return FirebaseApp.DefaultName;
		}

		public PlatformLogLevel GetLogLevel()
		{
			LogLevel logLevel;
			try
			{
				logLevel = FirebaseApp.LogLevel;
			}
			catch (InitializationException)
			{
				logLevel = LogLevel.Debug;
			}
			return FirebaseApp.ConvertLogLevel(logLevel);
		}
	}
}
