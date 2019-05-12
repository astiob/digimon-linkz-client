using System;
using UnityEngine;

namespace Neptune.Push
{
	public class NpPushAndroid
	{
		public static void GetDeviceToken(string callBackReceiveObjName, string senderId)
		{
			string text = "OnGetDeviceToken";
			string text2 = "OnPushError";
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						androidJavaClass2.CallStatic("init", new object[]
						{
							callBackReceiveObjName,
							senderId,
							@static
						});
						androidJavaClass2.CallStatic("getDeviceToken", new object[]
						{
							text,
							text2,
							@static
						});
					}
				}
			}
		}

		public static void LocalPushSend(string alertBody, int second)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						androidJavaClass2.CallStatic("sendLocalMessage", new object[]
						{
							alertBody,
							second,
							@static
						});
					}
				}
			}
		}

		public static void LocalPushSend(int type, string alertBody, int second)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						androidJavaClass2.CallStatic("sendLocalMessage", new object[]
						{
							type,
							alertBody,
							second,
							@static
						});
					}
				}
			}
		}

		public static void LocalPushSendRequestCode(string alertBody, int second, int requestCode)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						androidJavaClass2.CallStatic("sendLocalMessageRequestCode", new object[]
						{
							requestCode,
							alertBody,
							second,
							@static
						});
					}
				}
			}
		}

		public static void CancelAllLocalNotifications()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						androidJavaClass2.CallStatic("cancelAllNotification", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void CancelLocalNotifications(int requestCode)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						androidJavaClass2.CallStatic("cancelNotification", new object[]
						{
							@static,
							requestCode
						});
					}
				}
			}
		}

		public static string GetRegisterNotification()
		{
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.push.NPPushMessager"))
					{
						result = androidJavaClass2.CallStatic<string>("getRegisterNotification", new object[]
						{
							@static
						});
					}
				}
			}
			return result;
		}
	}
}
