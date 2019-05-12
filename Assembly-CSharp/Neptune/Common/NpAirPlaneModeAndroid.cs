using System;
using UnityEngine;

namespace Neptune.Common
{
	public class NpAirPlaneModeAndroid
	{
		public static void EnableDebugLog(bool enable)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpAirPlaneMode"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("enableDebugLog", new object[]
					{
						enable
					});
				}
			}
		}

		public static bool GetAirPlaneMode()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpAirPlaneMode"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<bool>("getAirPlaneMode", new object[0]);
				}
			}
			return result;
		}

		public static void SetAirPlaneModeRecieverEnable(bool enable)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpAirPlaneMode"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("setAirPlaneModeRecieverEnable", new object[]
					{
						enable
					});
				}
			}
		}

		public static bool GetAirPlanModeSwitching()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpAirPlaneMode"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<bool>("getAirPlanModeSwitching", new object[0]);
				}
			}
			return result;
		}
	}
}
