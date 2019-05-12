using System;
using UnityEngine;

public class UserSystemSettings : MonoBehaviour
{
	private const string androidPluginName = "com.trc.android.plugin.usersystemsettings.UserSystemSettings";

	public static bool GetAccelerometerRotation()
	{
		bool result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.usersystemsettings.UserSystemSettings"))
		{
			result = androidJavaClass.CallStatic<bool>("GetAccelerometerRotation", new object[0]);
		}
		return result;
	}

	public static long GetDeviceFreeSpace()
	{
		long result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.usersystemsettings.UserSystemSettings"))
		{
			result = androidJavaClass.CallStatic<long>("GetDeviceFreeSpace", new object[0]);
		}
		return result;
	}

	public static long GetSDCardFreeSpace()
	{
		long result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.usersystemsettings.UserSystemSettings"))
		{
			result = androidJavaClass.CallStatic<long>("GetSDCardFreeSpace", new object[0]);
		}
		return result;
	}

	public static long GetFreeSpace(string SearchPath)
	{
		long result;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.trc.android.plugin.usersystemsettings.UserSystemSettings"))
		{
			result = androidJavaClass.CallStatic<long>("GetFreeSpace", new object[]
			{
				SearchPath
			});
		}
		return result;
	}
}
