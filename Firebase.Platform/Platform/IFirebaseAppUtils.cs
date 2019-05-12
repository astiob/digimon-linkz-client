using System;

namespace Firebase.Platform
{
	internal interface IFirebaseAppUtils
	{
		void TranslateDllNotFoundException(Action action);

		void PollCallbacks();

		IFirebaseAppPlatform GetDefaultInstance();

		string GetDefaultInstanceName();

		PlatformLogLevel GetLogLevel();
	}
}
