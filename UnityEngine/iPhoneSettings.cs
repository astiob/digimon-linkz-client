using System;

namespace UnityEngine
{
	public sealed class iPhoneSettings
	{
		[Obsolete("verticalOrientation property is deprecated. Please use Screen.orientation == ScreenOrientation.Portrait instead.")]
		public static bool verticalOrientation
		{
			get
			{
				return false;
			}
		}

		[Obsolete("screenCanDarken property is deprecated. Please use (Screen.sleepTimeout != SleepTimeout.NeverSleep) instead.")]
		public static bool screenCanDarken
		{
			get
			{
				return false;
			}
		}

		[Obsolete("locationServiceEnabledByUser property is deprecated. Please use Input.location.isEnabledByUser instead.")]
		public static bool locationServiceEnabledByUser
		{
			get
			{
				return Input.location.isEnabledByUser;
			}
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.")]
		public static void StartLocationServiceUpdates(float desiredAccuracyInMeters, float updateDistanceInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.")]
		public static void StartLocationServiceUpdates(float desiredAccuracyInMeters)
		{
			Input.location.Start(desiredAccuracyInMeters);
		}

		[Obsolete("StartLocationServiceUpdates method is deprecated. Please use Input.location.Start instead.")]
		public static void StartLocationServiceUpdates()
		{
			Input.location.Start();
		}

		[Obsolete("StopLocationServiceUpdates method is deprecated. Please use Input.location.Stop instead.")]
		public static void StopLocationServiceUpdates()
		{
			Input.location.Stop();
		}
	}
}
