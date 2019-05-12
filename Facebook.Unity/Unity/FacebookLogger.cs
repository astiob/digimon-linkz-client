using System;
using UnityEngine;

namespace Facebook.Unity
{
	internal static class FacebookLogger
	{
		internal static IFacebookLogger Instance { private get; set; } = new FacebookLogger.DebugLogger();

		public static void Log(string msg)
		{
			FacebookLogger.Instance.Log(msg);
		}

		public static void Log(string format, params string[] args)
		{
			FacebookLogger.Log(string.Format(format, args));
		}

		public static void Info(string msg)
		{
			FacebookLogger.Instance.Info(msg);
		}

		public static void Info(string format, params string[] args)
		{
			FacebookLogger.Info(string.Format(format, args));
		}

		public static void Warn(string msg)
		{
			FacebookLogger.Instance.Warn(msg);
		}

		public static void Warn(string format, params string[] args)
		{
			FacebookLogger.Warn(string.Format(format, args));
		}

		public static void Error(string msg)
		{
			FacebookLogger.Instance.Error(msg);
		}

		public static void Error(string format, params string[] args)
		{
			FacebookLogger.Error(string.Format(format, args));
		}

		private class DebugLogger : IFacebookLogger
		{
			public void Log(string msg)
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log(msg);
				}
			}

			public void Info(string msg)
			{
				Debug.Log(msg);
			}

			public void Warn(string msg)
			{
				Debug.LogWarning(msg);
			}

			public void Error(string msg)
			{
				Debug.LogError(msg);
			}
		}
	}
}
