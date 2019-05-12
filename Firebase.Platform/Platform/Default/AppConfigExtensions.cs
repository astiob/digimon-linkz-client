using System;
using System.Collections.Generic;

namespace Firebase.Platform.Default
{
	internal class AppConfigExtensions : IAppConfigExtensions
	{
		private static readonly Uri DefaultUpdateUrl = new Uri("https://www.gstatic.com/firebase/ssl/roots.pem");

		private static readonly string Default = "DEFAULT";

		private static readonly object Sync = new object();

		private static AppConfigExtensions _instance = new AppConfigExtensions();

		private static readonly Dictionary<int, Dictionary<string, string>> SStringState = new Dictionary<int, Dictionary<string, string>>();

		protected AppConfigExtensions()
		{
		}

		public static IAppConfigExtensions Instance
		{
			get
			{
				return AppConfigExtensions._instance;
			}
		}

		public virtual string GetWriteablePath(IFirebaseAppPlatform app)
		{
			return string.Empty;
		}

		public virtual void SetDatabaseUrl(IFirebaseAppPlatform app, string databaseUrl)
		{
			AppConfigExtensions.SetState<string>(app, 0, databaseUrl, AppConfigExtensions.SStringState);
		}

		public virtual string GetDatabaseUrl(IFirebaseAppPlatform app)
		{
			string text = AppConfigExtensions.GetState<string>(app, 0, AppConfigExtensions.SStringState);
			if (string.IsNullOrEmpty(text) && app != null)
			{
				Uri databaseUrl = app.DatabaseUrl;
				text = ((!(databaseUrl != null)) ? null : databaseUrl.ToString());
			}
			return text;
		}

		public virtual void SetEditorP12Password(IFirebaseAppPlatform app, string p12Password)
		{
			AppConfigExtensions.SetState<string>(app, 2, p12Password, AppConfigExtensions.SStringState);
		}

		public virtual string GetEditorP12Password(IFirebaseAppPlatform app)
		{
			return AppConfigExtensions.GetState<string>(app, 2, AppConfigExtensions.SStringState);
		}

		public virtual void SetEditorP12FileName(IFirebaseAppPlatform app, string p12Filename)
		{
			AppConfigExtensions.SetState<string>(app, 1, p12Filename, AppConfigExtensions.SStringState);
		}

		public virtual string GetEditorP12FileName(IFirebaseAppPlatform app)
		{
			return AppConfigExtensions.GetState<string>(app, 1, AppConfigExtensions.SStringState);
		}

		public virtual void SetEditorServiceAccountEmail(IFirebaseAppPlatform app, string email)
		{
			AppConfigExtensions.SetState<string>(app, 3, email, AppConfigExtensions.SStringState);
		}

		public virtual string GetEditorServiceAccountEmail(IFirebaseAppPlatform app)
		{
			return AppConfigExtensions.GetState<string>(app, 3, AppConfigExtensions.SStringState);
		}

		public virtual void SetEditorAuthUserId(IFirebaseAppPlatform app, string uid)
		{
			AppConfigExtensions.SetState<string>(app, 4, uid, AppConfigExtensions.SStringState);
		}

		public virtual string GetEditorAuthUserId(IFirebaseAppPlatform app)
		{
			string text = Services.Auth.GetCurrentUserId(app);
			if (string.IsNullOrEmpty(text))
			{
				text = AppConfigExtensions.GetState<string>(app, 4, AppConfigExtensions.SStringState);
			}
			return text;
		}

		public virtual void SetCertPemFile(IFirebaseAppPlatform app, string certName)
		{
			AppConfigExtensions.SetState<string>(app, 5, certName, AppConfigExtensions.SStringState);
		}

		public virtual string GetCertPemFile(IFirebaseAppPlatform app)
		{
			return AppConfigExtensions.GetState<string>(app, 5, AppConfigExtensions.SStringState);
		}

		public void SetCertUpdateUrl(IFirebaseAppPlatform app, Uri certUrl)
		{
			AppConfigExtensions.SetState<string>(app, 6, certUrl.ToString(), AppConfigExtensions.SStringState);
		}

		public Uri GetCertUpdateUrl(IFirebaseAppPlatform app)
		{
			string state = AppConfigExtensions.GetState<string>(app, 6, AppConfigExtensions.SStringState);
			if (string.IsNullOrEmpty(state))
			{
				return AppConfigExtensions.DefaultUpdateUrl;
			}
			return new Uri(state);
		}

		private static T GetState<T>(IFirebaseAppPlatform app, int state, Dictionary<int, Dictionary<string, T>> store)
		{
			if (app == null)
			{
				app = FirebaseHandler.AppUtils.GetDefaultInstance();
			}
			object sync = AppConfigExtensions.Sync;
			T result;
			lock (sync)
			{
				string text = app.Name;
				if (string.IsNullOrEmpty(text))
				{
					text = AppConfigExtensions.Default;
				}
				Dictionary<string, T> dictionary;
				if (!store.TryGetValue(state, out dictionary))
				{
					dictionary = new Dictionary<string, T>();
					store[state] = dictionary;
				}
				T t;
				if (!dictionary.TryGetValue(text, out t))
				{
					result = default(T);
				}
				else
				{
					result = t;
				}
			}
			return result;
		}

		private static void SetState<T>(IFirebaseAppPlatform app, int state, T value, Dictionary<int, Dictionary<string, T>> store)
		{
			if (app == null)
			{
				app = FirebaseHandler.AppUtils.GetDefaultInstance();
			}
			object sync = AppConfigExtensions.Sync;
			lock (sync)
			{
				string text = app.Name;
				if (string.IsNullOrEmpty(text))
				{
					text = AppConfigExtensions.Default;
				}
				Dictionary<string, T> dictionary;
				if (!store.TryGetValue(state, out dictionary))
				{
					dictionary = new Dictionary<string, T>();
					store[state] = dictionary;
				}
				dictionary[text] = value;
			}
		}

		private enum ExtraStringState
		{
			DatabaseUrl,
			P12FileName,
			P12Password,
			ServiceAccountEmail,
			AuthUserId,
			CertTxtFileName,
			WebCertUpdateUrl
		}
	}
}
