using System;

namespace UnityEngine
{
	public class iPhoneSettings
	{
		[Obsolete("verticalOrientation property is deprecated. Please use Screen.orientation == ScreenOrientation.Portrait instead.", false)]
		public static bool verticalOrientation
		{
			get
			{
				return false;
			}
		}

		[Obsolete("screenCanDarken property is deprecated. Please use (Screen.sleepTimeout != SleepTimeout.NeverSleep) instead.", false)]
		public static bool screenCanDarken
		{
			get
			{
				return false;
			}
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", false)]
		public static void StartLocationServiceUpdates(float desiredAccuracyInMeters, float updateDistanceInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", false)]
		public static void StartLocationServiceUpdates(float desiredAccuracyInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters);
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.", false)]
		public static void StartLocationServiceUpdates()
		{
			Input.location.Start();
		}

		[Obsolete("StopLocationServiceUpdates method is deprecated. Please use Input.location.Stop instead.", false)]
		public static void StopLocationServiceUpdates()
		{
			Input.location.Stop();
		}

		[Obsolete("locationServiceEnabledByUser property is deprecated. Please use Input.location.isEnabledByUser instead.", false)]
		public static bool locationServiceEnabledByUser
		{
			get
			{
				return Input.location.isEnabledByUser;
			}
		}
	}
}
