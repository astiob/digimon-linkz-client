using System;
using UnityEngine;

namespace TerminalIdentifier
{
	public static class PlatformUserID
	{
		private static string GetAndroidId()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure");
			string static2 = androidJavaClass2.GetStatic<string>("ANDROID_ID");
			return androidJavaClass2.CallStatic<string>("getString", new object[]
			{
				androidJavaObject,
				static2
			});
		}

		public static string GetId()
		{
			return PlatformUserID.GetAndroidId();
		}
	}
}
