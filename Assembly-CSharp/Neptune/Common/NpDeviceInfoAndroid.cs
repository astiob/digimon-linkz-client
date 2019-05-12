using System;
using UnityEngine;

namespace Neptune.Common
{
	public class NpDeviceInfoAndroid
	{
		private static bool isPushBackKey;

		public static string GetFilesDir()
		{
			string result = string.Empty;
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getFilesDir", new object[0]))
						{
							result = androidJavaObject.Call<string>("getCanonicalPath", new object[0]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = string.Empty;
			}
			return result;
		}

		public static string GetCacheDir()
		{
			string result = string.Empty;
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getCacheDir", new object[0]))
						{
							result = androidJavaObject.Call<string>("getCanonicalPath", new object[0]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = string.Empty;
			}
			return result;
		}

		public static string GetExternalFilesDir()
		{
			string result = string.Empty;
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getExternalFilesDir", null))
						{
							result = androidJavaObject.Call<string>("getCanonicalPath", new object[0]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = string.Empty;
			}
			return result;
		}

		public static string GetExternalCacheDir()
		{
			string result = string.Empty;
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						using (AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getExternalCacheDir", new object[0]))
						{
							result = androidJavaObject.Call<string>("getCanonicalPath", new object[0]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = string.Empty;
			}
			return result;
		}

		public static int GetHeepTotalMemory()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				result = androidJavaClass.CallStatic<int>("getHeepTotalMemory", new object[0]);
			}
			return result;
		}

		public static int GetHeepFreeMemory()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				result = androidJavaClass.CallStatic<int>("getHeepFreeMemory", new object[0]);
			}
			return result;
		}

		public static int GetHeepUsedMemory()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				result = androidJavaClass.CallStatic<int>("getHeepUsedMemory", new object[0]);
			}
			return result;
		}

		public static int GetDalvikHeepMaxMemory()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				result = androidJavaClass.CallStatic<int>("getDalvikHeepMaxMemory", new object[0]);
			}
			return result;
		}

		public static int GetSystemAvailMem()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						result = androidJavaClass.CallStatic<int>("getSystemAvailMem", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public static int GetSystemThreshold()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						result = androidJavaClass.CallStatic<int>("getSystemThreshold", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public static int GetMemoryUse()
		{
			int result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpGetMemory"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						result = androidJavaClass.CallStatic<int>("getMemoryUse", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public static void SetLowMemoryCallBack(string gameObjectName, INpOnLowMemoryListener listener)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					@static.Call("setLowMemoryCallback", new object[]
					{
						gameObjectName
					});
				}
			}
		}

		public static void ReleaseLowMemoryCallBack()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					@static.Call("setLowMemoryCallback", new object[]
					{
						string.Empty
					});
				}
			}
		}

		public static string GetOSVersion()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("getDeviceOSVersion", new object[0]);
			}
			return result;
		}

		public static string GetDeviceModel()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("getDeviceModel", new object[0]);
			}
			return result;
		}

		public static float GetFreeStorageSpace()
		{
			float result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				string s = androidJavaClass.CallStatic<string>("getFreeStorageSpace", new object[0]);
				float num = float.Parse(s);
				result = num;
			}
			return result;
		}

		public static float GetFreeStorageSpaceForSDCard()
		{
			float result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				string s = androidJavaClass.CallStatic<string>("getFreeStorageSpaceForSDCard", new object[0]);
				float num = float.Parse(s);
				result = num;
			}
			return result;
		}

		public static bool GetIsSDCardEnable()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<bool>("getIsSDCardEnable", new object[0]);
			}
			return result;
		}

		public static string GetPlatform()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("getPlatform", new object[0]);
			}
			return result;
		}

		public static string GetLocaleLanguageAndCountry()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("getLocaleLanguageAndCountry", new object[0]);
			}
			return result;
		}

		public static string GetLocaleLanguageCode()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("getLanguageCode", new object[0]);
			}
			return result;
		}

		public static string GetAppVersion()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						result = androidJavaClass.CallStatic<string>("getVersion", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}

		public static string GetAppVersionCode()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						result = androidJavaClass.CallStatic<int>("getVersionCode", new object[]
						{
							@static
						}).ToString();
					}
				}
			}
			return result;
		}

		public static string GetGuid()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.MainActivity"))
			{
				result = androidJavaClass.CallStatic<string>("getGuid", new object[0]);
			}
			return result;
		}

		public static string GetLocaleCountryCode()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("getCountryCode", new object[0]);
			}
			return result;
		}

		public static string GetUuid()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				result = androidJavaClass.CallStatic<string>("generateUUID", new object[0]);
			}
			return result;
		}

		public static void PushBackKey(string title, string message)
		{
			if (Application.platform == RuntimePlatform.Android && (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu)) && !NpDeviceInfoAndroid.isPushBackKey)
			{
				NpDeviceInfo.Popup(title, message);
				NpDeviceInfoAndroid.isPushBackKey = true;
			}
		}

		private static void cancelBackKey()
		{
			NpDeviceInfoAndroid.isPushBackKey = false;
		}

		public static void Popup(string title, string message)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					using (AndroidJavaObject @static = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity"))
					{
						androidJavaClass.CallStatic("popup", new object[]
						{
							@static,
							title,
							message
						});
					}
				}
			}
		}

		public static void executeGetCancelKey()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NPDevice"))
			{
				for (;;)
				{
					int num = androidJavaClass.CallStatic<int>("getIsCancelKey", new object[0]);
					if (num == 1)
					{
						break;
					}
					if (num == 2)
					{
						goto Block_3;
					}
				}
				goto IL_3F;
				Block_3:
				NpDeviceInfoAndroid.cancelBackKey();
				IL_3F:
				androidJavaClass.CallStatic("setIsCancelKey", new object[]
				{
					0
				});
			}
		}

		private enum DEVICE_POPUP_STATE
		{
			POPUP_INIT,
			POPUP_OK,
			POPUP_CANCEL
		}
	}
}
