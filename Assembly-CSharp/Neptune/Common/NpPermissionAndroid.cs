using System;
using UnityEngine;

namespace Neptune.Common
{
	public class NpPermissionAndroid : NpSingleton<NpPermission>
	{
		public static void EnableDebugLog(bool enable)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpPermissionManager"))
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

		public static void RequestPermissions(ManifestPermission manifestPermission)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpPermissionManager"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("requestPermissions", new object[]
					{
						(int)manifestPermission
					});
				}
			}
		}
	}
}
