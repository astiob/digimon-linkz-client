using System;
using UnityEngine;

namespace Facebook.Unity
{
	internal static class FBUnityUtility
	{
		private static string currentDeviceIdentifier;

		private static IAsyncRequestStringWrapper asyncRequestStringWrapper;

		public static string UnityDeviceIdentifier
		{
			get
			{
				if (string.IsNullOrEmpty(FBUnityUtility.currentDeviceIdentifier))
				{
					FBUnityUtility.currentDeviceIdentifier = SystemInfo.deviceUniqueIdentifier;
				}
				return FBUnityUtility.currentDeviceIdentifier;
			}
			set
			{
				FBUnityUtility.currentDeviceIdentifier = value;
			}
		}

		public static IAsyncRequestStringWrapper AsyncRequestStringWrapper
		{
			get
			{
				if (FBUnityUtility.asyncRequestStringWrapper == null)
				{
					FBUnityUtility.asyncRequestStringWrapper = new AsyncRequestStringWrapper();
				}
				return FBUnityUtility.asyncRequestStringWrapper;
			}
			set
			{
				FBUnityUtility.asyncRequestStringWrapper = value;
			}
		}
	}
}
