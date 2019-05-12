using System;

namespace Firebase.Platform
{
	internal interface IAppConfigExtensions
	{
		string GetWriteablePath(IFirebaseAppPlatform app);

		void SetDatabaseUrl(IFirebaseAppPlatform app, string databaseUrl);

		string GetDatabaseUrl(IFirebaseAppPlatform app);

		void SetEditorP12Password(IFirebaseAppPlatform app, string p12Password);

		string GetEditorP12Password(IFirebaseAppPlatform app);

		void SetEditorP12FileName(IFirebaseAppPlatform app, string p12Filename);

		string GetEditorP12FileName(IFirebaseAppPlatform app);

		void SetEditorServiceAccountEmail(IFirebaseAppPlatform app, string email);

		string GetEditorServiceAccountEmail(IFirebaseAppPlatform app);

		void SetEditorAuthUserId(IFirebaseAppPlatform app, string uid);

		string GetEditorAuthUserId(IFirebaseAppPlatform app);

		void SetCertPemFile(IFirebaseAppPlatform app, string certName);

		string GetCertPemFile(IFirebaseAppPlatform app);

		void SetCertUpdateUrl(IFirebaseAppPlatform app, Uri certUrl);

		Uri GetCertUpdateUrl(IFirebaseAppPlatform app);
	}
}
