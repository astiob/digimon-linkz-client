using Firebase.Platform;
using Firebase.Platform.Default;
using System;

namespace Firebase.Unity
{
	internal class UnityPlatformServices
	{
		public static void SetupServices()
		{
			Services.AppConfig = UnityConfigExtensions.DefaultInstance;
			Services.HttpFactory = UnityHttpFactoryService.Instance;
			Services.Logging = UnityLoggingService.Instance;
			Services.RootCerts = InstallRootCerts.Instance;
		}
	}
}
