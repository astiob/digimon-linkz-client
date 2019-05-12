using System;

namespace Neptune.OpenApp
{
	public class NpURL
	{
		public static void OpenUrl(string urlString)
		{
			NpURLAndroid.OpenUrl(urlString);
		}

		public static void OpenAppPageInAppStore(string applicationId)
		{
			NpURLAndroid.OpenAppPageInAppStore(applicationId);
		}

		public static void OpenReviewPageInAppStore(string applicationId)
		{
			NpURLAndroid.OpenAppPageInAppStore(applicationId);
		}

		public static bool OpenLINEApp(string text)
		{
			return NpURLAndroid.OpenLINEApp(text);
		}

		public static bool OpenAppScheme(string appId, string scheme)
		{
			return NpURLAndroid.OpenAppScheme(appId, scheme);
		}
	}
}
