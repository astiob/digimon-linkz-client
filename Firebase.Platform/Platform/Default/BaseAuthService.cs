using Firebase.Platform.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Firebase.Platform.Default
{
	internal class BaseAuthService : IAuthService
	{
		private static BaseAuthService _instance = new BaseAuthService();

		protected BaseAuthService()
		{
		}

		public static BaseAuthService BaseInstance
		{
			get
			{
				return BaseAuthService._instance;
			}
		}

		protected bool GetIsServiceAccountAuth(IFirebaseAppPlatform app)
		{
			if (app == null)
			{
				app = FirebaseHandler.AppUtils.GetDefaultInstance();
			}
			string editorP12FileName = Services.AppConfig.GetEditorP12FileName(app);
			string editorServiceAccountEmail = Services.AppConfig.GetEditorServiceAccountEmail(app);
			return !string.IsNullOrEmpty(editorP12FileName) && !string.IsNullOrEmpty(editorServiceAccountEmail) && File.Exists(editorP12FileName);
		}

		public virtual Task<string> GetTokenAsync(IFirebaseAppPlatform app, bool forceRefresh)
		{
			if (app == null)
			{
				app = FirebaseHandler.AppUtils.GetDefaultInstance();
			}
			TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
			if (this.GetIsServiceAccountAuth(app))
			{
				string editorP12FileName = Services.AppConfig.GetEditorP12FileName(app);
				string text = Services.AppConfig.GetEditorP12Password(app);
				string editorServiceAccountEmail = Services.AppConfig.GetEditorServiceAccountEmail(app);
				if (string.IsNullOrEmpty(text))
				{
					text = "notasecret";
				}
				X509Certificate2 certificate = new X509Certificate2(editorP12FileName, text, X509KeyStorageFlags.Exportable);
				ServiceAccountCredential serviceAccountCredential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(editorServiceAccountEmail)
				{
					Scopes = new List<string>(new string[]
					{
						"https://www.googleapis.com/auth/firebase.database",
						"https://www.googleapis.com/auth/userinfo.email"
					})
				}.FromCertificate(certificate));
				string accessTokenForRequest = serviceAccountCredential.GetAccessTokenForRequest();
				if (string.IsNullOrEmpty(accessTokenForRequest))
				{
					Services.Logging.LogMessage(PlatformLogLevel.Debug, "FirebaseDatabase: Error obtaining service credential. Attempting unauthenticated access");
					taskCompletionSource.SetResult(string.Empty);
				}
				else
				{
					string editorAuthUserId = Services.AppConfig.GetEditorAuthUserId(app);
					Dictionary<string, object> dictionary = null;
					if (!string.IsNullOrEmpty(editorAuthUserId))
					{
						dictionary = new Dictionary<string, object>();
						dictionary["uid"] = editorAuthUserId;
					}
					GAuthToken gauthToken = new GAuthToken(accessTokenForRequest, dictionary);
					taskCompletionSource.SetResult(gauthToken.SerializeToString());
				}
			}
			else
			{
				taskCompletionSource.SetResult(string.Empty);
			}
			return taskCompletionSource.Task;
		}

		public void GetTokenAsync(IFirebaseAppPlatform app, bool forceRefresh, IGetTokenCompletionListener listener)
		{
			this.GetTokenAsync(app, forceRefresh).ContinueWith(delegate(Task<string> task)
			{
				if (task.Exception != null)
				{
					Services.Logging.LogMessage(PlatformLogLevel.Error, task.Exception.ToString());
					listener.OnSuccess(string.Empty);
				}
				else
				{
					listener.OnSuccess(task.Result);
				}
			});
		}

		public virtual void AddTokenChangeListener(IFirebaseAppPlatform app, ITokenChangeListener listener)
		{
		}

		public virtual string GetCurrentUserId(IFirebaseAppPlatform app)
		{
			return string.Empty;
		}
	}
}
