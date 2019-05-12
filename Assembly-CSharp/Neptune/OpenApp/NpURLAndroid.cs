using System;
using UnityEngine;

namespace Neptune.OpenApp
{
	public class NpURLAndroid
	{
		public static void OpenUrl(string urlString)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.NPEtcetera"))
					{
						androidJavaClass2.CallStatic("openWeb", new object[]
						{
							urlString,
							@static
						});
					}
				}
			}
		}

		public static void OpenAppPageInAppStore(string applicationId)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.NPEtcetera"))
					{
						androidJavaClass2.CallStatic("goToMarket", new object[]
						{
							applicationId,
							@static
						});
					}
				}
			}
		}

		public static bool OpenLINEApp(string text)
		{
			bool result = false;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.NPEtcetera"))
					{
						result = androidJavaClass2.CallStatic<bool>("openLINEApp", new object[]
						{
							text,
							@static
						});
					}
				}
			}
			return result;
		}

		public static bool OpenAppScheme(string packageName, string scheme)
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.NPEtcetera"))
					{
						result = androidJavaClass2.CallStatic<bool>("openAppScheme", new object[]
						{
							packageName,
							scheme,
							@static
						});
					}
				}
			}
			return result;
		}
	}
}
