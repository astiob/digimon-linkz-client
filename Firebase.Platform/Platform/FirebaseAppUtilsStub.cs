using System;

namespace Firebase.Platform
{
	internal class FirebaseAppUtilsStub : IFirebaseAppUtils
	{
		private static FirebaseAppUtilsStub _instance = new FirebaseAppUtilsStub();

		public static FirebaseAppUtilsStub Instance
		{
			get
			{
				return FirebaseAppUtilsStub._instance;
			}
		}

		public void TranslateDllNotFoundException(Action action)
		{
			action();
		}

		public void PollCallbacks()
		{
		}

		public IFirebaseAppPlatform GetDefaultInstance()
		{
			return null;
		}

		public string GetDefaultInstanceName()
		{
			return "__FIRAPP_DEFAULT";
		}

		public PlatformLogLevel GetLogLevel()
		{
			return PlatformLogLevel.Debug;
		}
	}
}
