using System;

namespace UnityEngine.Analytics
{
	internal static class UnityAnalyticsVersion
	{
		private static string kSdkVersion = "u" + Application.unityVersion;

		public static string sdkVersion
		{
			get
			{
				return UnityAnalyticsVersion.kSdkVersion;
			}
		}
	}
}
