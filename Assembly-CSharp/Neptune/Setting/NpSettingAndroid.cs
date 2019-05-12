using System;
using UnityEngine;

namespace Neptune.Setting
{
	public class NpSettingAndroid
	{
		private static string[] actionPackageArray = new string[]
		{
			"android.settings.SETTINGS",
			"android.settings.APN_SETTINGS",
			"android.settings.LOCATION_SOURCE_SETTINGS",
			"android.settings.WIRELESS_SETTINGS",
			"android.settings.WIFI_SETTINGS",
			"android.settings.AIRPLANE_MODE_SETTINGS",
			"android.settings.BLUETOOTH_SETTINGS",
			"android.settings.DATE_SETTINGS",
			"android.settings.DEVICE_INFO_SETTINGS",
			"android.settings.DISPLAY_SETTINGS",
			"android.settings.INTERNAL_STORAGE_SETTINGS"
		};

		private static string GetActionPackage(SettingViewAction action)
		{
			return NpSettingAndroid.actionPackageArray[(int)action];
		}

		public static void OpenAppSettingView(string package)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri"))
				{
					using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[0]))
					{
						using (AndroidJavaObject androidJavaObject2 = androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[]
						{
							"package:" + package
						}))
						{
							androidJavaObject.Call<AndroidJavaObject>("setAction", new object[]
							{
								"android.settings.APPLICATION_DETAILS_SETTINGS"
							});
							androidJavaObject.Call<AndroidJavaObject>("setData", new object[]
							{
								androidJavaObject2
							});
							androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity").Call("startActivity", new object[]
							{
								androidJavaObject
							});
						}
					}
				}
			}
		}

		public static void OpenSettingView(SettingViewAction action)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[0]))
				{
					androidJavaObject.Call<AndroidJavaObject>("setAction", new object[]
					{
						NpSettingAndroid.GetActionPackage(action)
					});
					androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity").Call("startActivity", new object[]
					{
						androidJavaObject
					});
				}
			}
		}
	}
}
