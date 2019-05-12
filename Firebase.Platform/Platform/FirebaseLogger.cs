using System;
using UnityEngine;

namespace Firebase.Platform
{
	internal class FirebaseLogger
	{
		internal static void LogMessage(PlatformLogLevel logLevel, string message)
		{
			PlatformLogLevel logLevel2 = FirebaseHandler.AppUtils.GetLogLevel();
			if (logLevel < logLevel2)
			{
				return;
			}
			switch (logLevel)
			{
			case PlatformLogLevel.Verbose:
			case PlatformLogLevel.Debug:
			case PlatformLogLevel.Info:
				Debug.Log(message);
				break;
			case PlatformLogLevel.Warning:
				Debug.LogWarning(message);
				break;
			case PlatformLogLevel.Error:
				Debug.LogError(message);
				break;
			}
		}
	}
}
