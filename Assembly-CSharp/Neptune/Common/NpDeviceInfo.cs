using System;
using System.Threading;
using UnityEngine;

namespace Neptune.Common
{
	public class NpDeviceInfo
	{
		public static string GetFilesDir()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetFilesDir();
		}

		public static string GetCacheDir()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetCacheDir();
		}

		public static string GetExternalFilesDir()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetExternalFilesDir();
		}

		public static string GetExternalCacheDir()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetExternalCacheDir();
		}

		public static int GetHeepTotalMemory()
		{
			return NpDeviceInfoAndroid.GetHeepTotalMemory();
		}

		public static int GetHeepFreeMemory()
		{
			return NpDeviceInfoAndroid.GetHeepFreeMemory();
		}

		public static int GetHeepUsedMemory()
		{
			return NpDeviceInfoAndroid.GetHeepUsedMemory();
		}

		public static int GetDalvikHeepMaxMemory()
		{
			return NpDeviceInfoAndroid.GetDalvikHeepMaxMemory();
		}

		public static int GetSystemAvailMem()
		{
			return NpDeviceInfoAndroid.GetSystemAvailMem();
		}

		public static int GetSystemThreshold()
		{
			return NpDeviceInfoAndroid.GetSystemThreshold();
		}

		public static int GetInactiveMem()
		{
			return 0;
		}

		public static int GetTotalFreeMem()
		{
			return NpDeviceInfoAndroid.GetSystemAvailMem();
		}

		public static int GetMemoryUse()
		{
			return NpDeviceInfoAndroid.GetMemoryUse();
		}

		public static void SetLowMemoryCallBack(string gameObjectName, INpOnLowMemoryListener listener)
		{
			if (listener == null)
			{
				global::Debug.LogError("INpOnLowMemoryListener is Null.");
				return;
			}
			NpDeviceInfoAndroid.SetLowMemoryCallBack(gameObjectName, listener);
		}

		public static void ReleaseLowMemoryCallBack()
		{
			NpDeviceInfoAndroid.ReleaseLowMemoryCallBack();
		}

		public static string GetOSVersion()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetOSVersion();
		}

		public static string GetDeviceModel()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetDeviceModel();
		}

		public static float GetFreeStorageSpace()
		{
			return NpDeviceInfoAndroid.GetFreeStorageSpace();
		}

		public static float GetFreeStorageSpaceForSDCard()
		{
			return NpDeviceInfoAndroid.GetFreeStorageSpaceForSDCard();
		}

		public static bool GetIsSDCardEnable()
		{
			return NpDeviceInfoAndroid.GetIsSDCardEnable();
		}

		public static string GetPlatform()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetPlatform();
		}

		public static string GetLocaleLanguageAndCountry()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetLocaleLanguageAndCountry();
		}

		public static string GetLocaleLanguageCode()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetLocaleLanguageCode();
		}

		public static string GetAppVersion()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetAppVersion();
		}

		public static string GetAppVersionCode()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetAppVersionCode();
		}

		public static string GetLocaleCountryCode()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetLocaleCountryCode();
		}

		public static string GetGuid()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetGuid();
		}

		public static string GetAdid()
		{
			string empty = string.Empty;
			return NpDeviceInfoAndroid.GetGuid();
		}

		public static string GenerateUUID()
		{
			return NpDeviceInfoAndroid.GetUuid();
		}

		public static void PushBackKey(string title, string message)
		{
			NpDeviceInfoAndroid.PushBackKey(title, message);
		}

		public static void DeviceExit()
		{
			Application.Quit();
		}

		public static void Popup(string title, string message)
		{
			NpDeviceInfoAndroid.Popup(title, message);
			Thread thread = new Thread(delegate()
			{
				NpDeviceInfo.executeGetCancelKey();
			});
			thread.Start();
		}

		private static void executeGetCancelKey()
		{
			NpDeviceInfoAndroid.executeGetCancelKey();
		}

		public static string randomStr(int length)
		{
			string text = string.Empty;
			char[] array = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
			while (text.Length < length)
			{
				text += array[UnityEngine.Random.Range(0, array.Length)];
			}
			return text;
		}
	}
}
