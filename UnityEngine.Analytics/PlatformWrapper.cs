using System;

namespace UnityEngine.Analytics
{
	internal static class PlatformWrapper
	{
		public static IPlatformWrapper platform
		{
			get
			{
				return new BasePlatformWrapper();
			}
		}
	}
}
