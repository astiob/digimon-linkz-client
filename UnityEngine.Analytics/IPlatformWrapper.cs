using System;

namespace UnityEngine.Analytics
{
	internal interface IPlatformWrapper
	{
		string appVersion { get; }

		string appBundleIdentifier { get; }

		string appInstallMode { get; }

		bool isRootedOrJailbroken { get; }

		bool isNetworkReachable { get; }

		bool isWebPlayer { get; }

		bool isEditor { get; }

		bool isAndroid { get; }

		int levelCount { get; }

		int loadedLevel { get; }

		string loadedLevelName { get; }

		string persistentDataPath { get; }

		string platformName { get; }

		string unityVersion { get; }

		string licenseType { get; }

		string deviceModel { get; }

		string deviceUniqueIdentifier { get; }

		string operatingSystem { get; }

		string processorType { get; }

		int systemMemorySize { get; }

		long randomNumber { get; }
	}
}
