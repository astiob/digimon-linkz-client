using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune.Push
{
	public class NpPush
	{
		private string mCallBackReceiveObjName;

		public NpPush(GameObject callBackReceiveObj, NpPush.INpPushListener iNpPushListener)
		{
			this.mCallBackReceiveObjName = callBackReceiveObj.name;
			global::Debug.Log(string.Format("CallBackGoName = {0}", this.mCallBackReceiveObjName));
		}

		public void GetDeviceToken(string senderId)
		{
			NpPushAndroid.GetDeviceToken(this.mCallBackReceiveObjName, senderId);
		}

		public static bool IsPushNotificationEnable()
		{
			return true;
		}

		[Obsolete("Please use here LocalPushSendRequestCode( string alertBody, int badgeCount, int second, int requestCode )")]
		public static void LocalPushSend(string alertBody, int badgeCount, int second)
		{
			NpPushAndroid.LocalPushSend(alertBody, second);
		}

		public static void LocalPushSendRequestCode(string alertBody, int badgeCount, int second, int requestCode)
		{
			NpPushAndroid.LocalPushSendRequestCode(alertBody, second, requestCode);
		}

		[Obsolete("Please use here LocalPushSendRequestCode( string alertBody, int badgeCount, int second, int requestCode )")]
		public static void LocalPushSend(int type, string alertBody, int badgeCount, int second)
		{
			NpPushAndroid.LocalPushSend(type, alertBody, second);
		}

		public static void CancelAllLocalNotifications()
		{
			NpPushAndroid.CancelAllLocalNotifications();
		}

		public static void CancelLocalNotifications(int requestCode)
		{
			NpPushAndroid.CancelLocalNotifications(requestCode);
		}

		public static List<int> GetRegisterNotification()
		{
			string text = string.Empty;
			List<int> list = new List<int>();
			try
			{
				text = NpPushAndroid.GetRegisterNotification();
				if (string.IsNullOrEmpty(text))
				{
					return null;
				}
				string[] array = text.Split(new char[]
				{
					','
				});
				foreach (string text2 in array)
				{
					try
					{
						list.Add(int.Parse(text2));
					}
					catch (FormatException)
					{
						global::Debug.Log("GetRegisterNotification : Invalid parameter requestCode = " + text2);
						list = null;
						break;
					}
				}
			}
			catch (Exception)
			{
				global::Debug.Log("GetRegisterNotification : unexpected error.");
				list = null;
			}
			return list;
		}

		public static void BadgeNumberChange(int badgeNumber)
		{
		}

		public static string GetMessage()
		{
			return string.Empty;
		}

		public enum PUSH_INTERVAL
		{
			LOCAL_NOTIFICATION_INTERVAL_NONE,
			LOCAL_NOTIFICATION_INTERVAL_HALF_HOUR,
			LOCAL_NOTIFICATION_INTERVAL_HOUR,
			LOCAL_NOTIFICATION_INTERVAL_HALF_DAY,
			LOCAL_NOTIFICATION_INTERVAL_DAY,
			LOCAL_NOTIFICATION_INTERVAL_WEEK
		}

		public interface INpPushListener
		{
			void OnGetDeviceToken(string deviceToken);

			void OnPushError(string errMsg);
		}
	}
}
