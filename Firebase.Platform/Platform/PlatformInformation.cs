using Firebase.Unity;
using System;
using System.Threading;
using UnityEngine;

namespace Firebase.Platform
{
	internal static class PlatformInformation
	{
		private static string runtimeVersion;

		internal static bool IsAndroid
		{
			get
			{
				return Application.platform == RuntimePlatform.Android;
			}
		}

		internal static bool IsIOS
		{
			get
			{
				return Application.platform == RuntimePlatform.IPhonePlayer;
			}
		}

		internal static string DefaultConfigLocation
		{
			get
			{
				return FirebaseHandler.RunOnMainThread<string>(() => Application.streamingAssetsPath);
			}
		}

		internal static SynchronizationContext SynchronizationContext
		{
			get
			{
				return UnitySynchronizationContext.Instance;
			}
		}

		internal static float RealtimeSinceStartup
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		internal static float RealtimeSinceStartupSafe { get; set; }

		internal static string RuntimeName
		{
			get
			{
				return "unity";
			}
		}

		internal static string RuntimeVersion
		{
			get
			{
				if (PlatformInformation.runtimeVersion == null)
				{
					PlatformInformation.runtimeVersion = FirebaseHandler.RunOnMainThread<string>(() => Application.unityVersion);
				}
				return PlatformInformation.runtimeVersion;
			}
		}
	}
}
