using System;
using UnityEngine;

public class NpDebugLogAndroid
{
	public static void EnableNativeDebugLog(bool isEnabled)
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpLog"))
		{
			androidJavaClass.CallStatic("enableNativeDebugLog", new object[]
			{
				isEnabled
			});
		}
	}
}
