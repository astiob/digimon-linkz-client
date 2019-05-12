using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Analytics
{
	internal class UnityAnalyticsManager
	{
		public static extern string unityAdsId { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern bool unityAdsTrackingEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern string deviceUniqueIdentifier { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
