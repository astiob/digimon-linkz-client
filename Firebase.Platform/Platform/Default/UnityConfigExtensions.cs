using System;
using System.IO;
using UnityEngine;

namespace Firebase.Platform.Default
{
	internal class UnityConfigExtensions : AppConfigExtensions
	{
		private static UnityConfigExtensions _instance = new UnityConfigExtensions();

		public static IAppConfigExtensions DefaultInstance
		{
			get
			{
				return UnityConfigExtensions._instance;
			}
		}

		public override string GetWriteablePath(IFirebaseAppPlatform app)
		{
			return FirebaseHandler.RunOnMainThread<string>(() => Application.persistentDataPath);
		}

		public override void SetEditorP12FileName(IFirebaseAppPlatform app, string p12Filename)
		{
			string text = p12Filename;
			if (!string.IsNullOrEmpty(text) && !File.Exists(text))
			{
				text = string.Concat(new object[]
				{
					Application.dataPath,
					Path.DirectorySeparatorChar,
					"Editor Default Resources",
					Path.DirectorySeparatorChar,
					text
				});
			}
			if (!string.IsNullOrEmpty(text) && !File.Exists(text))
			{
				FirebaseLogger.LogMessage(PlatformLogLevel.Warning, p12Filename + " was not found.  Also looked in " + text);
			}
			base.SetEditorP12FileName(app, text);
		}
	}
}
