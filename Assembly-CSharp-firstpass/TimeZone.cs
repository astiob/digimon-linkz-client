using System;
using UnityEngine;

public class TimeZone
{
	public static string GetTimezoneName()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.util.TimeZone");
		AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getDefault", new object[0]);
		return androidJavaObject.Call<string>("getID", new object[0]);
	}
}
