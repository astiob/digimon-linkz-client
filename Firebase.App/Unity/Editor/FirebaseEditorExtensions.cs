using Firebase.Platform;
using System;

namespace Firebase.Unity.Editor
{
	public static class FirebaseEditorExtensions
	{
		public static void SetEditorDatabaseUrl(this FirebaseApp app, string databaseUrl)
		{
			if (app == null)
			{
				throw new ArgumentException("FirebaseApp was not initialized properly.  Please create the app before setting the database url.");
			}
			Services.AppConfig.SetDatabaseUrl(app.AppPlatform, databaseUrl);
		}

		public static void SetEditorDatabaseUrl(this FirebaseApp app, Uri databaseUrl)
		{
			Services.AppConfig.SetDatabaseUrl(app.AppPlatform, databaseUrl.ToString());
		}

		public static string GetEditorDatabaseUrl(this FirebaseApp app)
		{
			return Services.AppConfig.GetDatabaseUrl(app.AppPlatform);
		}

		public static void SetEditorP12Password(this FirebaseApp app, string p12Password)
		{
			Services.AppConfig.SetEditorP12Password(app.AppPlatform, p12Password);
		}

		public static string GetEditorP12Password(this FirebaseApp app)
		{
			return Services.AppConfig.GetEditorP12Password(app.AppPlatform);
		}

		public static void SetEditorP12FileName(this FirebaseApp app, string p12Filename)
		{
			Services.AppConfig.SetEditorP12FileName(app.AppPlatform, p12Filename);
		}

		public static string GetEditorP12FileName(this FirebaseApp app)
		{
			return Services.AppConfig.GetEditorP12FileName(app.AppPlatform);
		}

		public static void SetEditorServiceAccountEmail(this FirebaseApp app, string email)
		{
			Services.AppConfig.SetEditorServiceAccountEmail(app.AppPlatform, email);
		}

		public static string GetEditorServiceAccountEmail(this FirebaseApp app)
		{
			return Services.AppConfig.GetEditorServiceAccountEmail(app.AppPlatform);
		}

		public static void SetEditorAuthUserId(this FirebaseApp app, string uid)
		{
			Services.AppConfig.SetEditorAuthUserId(app.AppPlatform, uid);
		}

		public static string GetEditorAuthUserId(this FirebaseApp app)
		{
			string text = Services.Auth.GetCurrentUserId(app.AppPlatform);
			if (string.IsNullOrEmpty(text))
			{
				text = Services.AppConfig.GetEditorAuthUserId(app.AppPlatform);
			}
			return text;
		}

		public static void SetCertPemFile(this FirebaseApp app, string certName)
		{
			Services.AppConfig.SetCertPemFile(app.AppPlatform, certName);
		}

		public static string GetCertPemFile(this FirebaseApp app)
		{
			return Services.AppConfig.GetCertPemFile(app.AppPlatform);
		}
	}
}
