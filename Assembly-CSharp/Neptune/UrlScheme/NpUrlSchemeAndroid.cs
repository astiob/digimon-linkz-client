using System;
using UnityEngine;

namespace Neptune.UrlScheme
{
	public class NpUrlSchemeAndroid
	{
		public static bool IsUrlSchemeAction()
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpUrlSchemeData"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<bool>("isUrlSchemeAction", new object[0]);
				}
			}
			return result;
		}

		public static string GetScheme()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpUrlSchemeData"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<string>("getScheme", new object[0]);
				}
			}
			return result;
		}

		public static string GetHost()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpUrlSchemeData"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<string>("getHost", new object[0]);
				}
			}
			return result;
		}

		public static string GetPath()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpUrlSchemeData"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<string>("getPath", new object[0]);
				}
			}
			return result;
		}

		public static string GetQueryParamJson()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpUrlSchemeData"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					result = androidJavaObject.Call<string>("getQueryParamJson", new object[0]);
				}
			}
			return result;
		}

		public static void Clear()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.NpUrlSchemeData"))
			{
				using (AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]))
				{
					androidJavaObject.Call("clear", new object[0]);
				}
			}
		}
	}
}
