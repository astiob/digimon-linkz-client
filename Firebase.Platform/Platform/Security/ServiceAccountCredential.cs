using Google.MiniJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Firebase.Platform.Security
{
	internal class ServiceAccountCredential : ServiceCredential
	{
		protected static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public ServiceAccountCredential(ServiceAccountCredential.Initializer initializer) : base(initializer)
		{
			this.Id = initializer.Id;
			this.User = initializer.User;
			this.Scopes = initializer.Scopes.ToArray();
			this.Key = initializer.Key;
		}

		public string Id { get; private set; }

		public string User { get; private set; }

		public string[] Scopes { get; private set; }

		public RSACryptoServiceProvider Key { get; private set; }

		private void SendOAuth(ServiceAccountCredential.OAuthRequest request)
		{
			Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("Sending OAuth request to {0}", base.TokenServerUrl));
			FirebaseHttpRequest firebaseHttpRequest = Services.HttpFactory.OpenConnection(new Uri(base.TokenServerUrl));
			firebaseHttpRequest.SetRequestMethod("POST");
			firebaseHttpRequest.SetRequestProperty(FirebaseHttpRequest.HeaderContentType, "application/x-www-form-urlencoded");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("grant_type=");
			stringBuilder.Append("urn:ietf:params:oauth:grant-type:jwt-bearer");
			stringBuilder.Append("&");
			stringBuilder.Append("assertion=");
			stringBuilder.Append(request.Assertion);
			byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			firebaseHttpRequest.OutputStream.Write(bytes, 0, bytes.Length);
			if (firebaseHttpRequest.ResponseCode >= 200 && firebaseHttpRequest.ResponseCode < 300)
			{
				Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("OAuth request to {0} completed with code {1}", base.TokenServerUrl, firebaseHttpRequest.ResponseCode.ToString()));
				StreamReader streamReader = new StreamReader(firebaseHttpRequest.InputStream);
				request.ResponseBody = streamReader.ReadToEnd();
				Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("OAuth request to {0} completed with msg {1}", base.TokenServerUrl, request.ResponseBody));
			}
			else
			{
				Services.Logging.LogMessage(PlatformLogLevel.Error, string.Format("OAuth request to {0} failed with code {1}", base.TokenServerUrl, firebaseHttpRequest.ResponseCode.ToString()));
				StreamReader streamReader2 = new StreamReader(firebaseHttpRequest.ErrorStream);
				string arg = streamReader2.ReadToEnd();
				Services.Logging.LogMessage(PlatformLogLevel.Debug, string.Format("OAuth request to {0} failed with msg {1}", base.TokenServerUrl, arg));
				request.ResponseBody = null;
			}
		}

		public override string GetAccessTokenForRequestSync(CancellationToken taskCancellationToken)
		{
			string assertion = this.CreateAssertionFromPayload(this.CreatePayload());
			ServiceAccountCredential.OAuthRequest oauthRequest = new ServiceAccountCredential.OAuthRequest
			{
				Assertion = assertion
			};
			this.SendOAuth(oauthRequest);
			if (string.IsNullOrEmpty(oauthRequest.ResponseBody))
			{
				return null;
			}
			IDictionary<string, object> dictionary = (IDictionary<string, object>)Json.Deserialize(oauthRequest.ResponseBody);
			object obj = null;
			if (dictionary != null)
			{
				dictionary.TryGetValue("access_token", out obj);
			}
			return (obj == null) ? null : obj.ToString();
		}

		private string CreateAssertionFromPayload(string serializedPayload)
		{
			string value = "{\"alg\":\"RS256\",\"typ\":\"JWT\"}";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.UrlSafeBase64Encode(value)).Append(".").Append(this.UrlSafeBase64Encode(serializedPayload));
			string value2 = this.UrlSafeBase64Encode(this.Key.SignData(Encoding.ASCII.GetBytes(stringBuilder.ToString()), "SHA256"));
			stringBuilder.Append(".").Append(value2);
			return stringBuilder.ToString();
		}

		private string CreatePayload()
		{
			int num = (int)(base.Clock.UtcNow - ServiceAccountCredential.UnixEpoch).TotalSeconds;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["iss"] = this.Id;
			dictionary["scope"] = string.Join(" ", this.Scopes);
			dictionary["aud"] = "https://accounts.google.com/o/oauth2/token";
			dictionary["exp"] = num + 3600;
			dictionary["iat"] = num;
			return Json.Serialize(dictionary);
		}

		private string UrlSafeBase64Encode(string value)
		{
			return this.UrlSafeBase64Encode(Encoding.UTF8.GetBytes(value));
		}

		private string UrlSafeBase64Encode(byte[] bytes)
		{
			return Convert.ToBase64String(bytes).Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
		}

		private class UploadCompleted
		{
			public UploadValuesCompletedEventArgs args;
		}

		private class OAuthRequest
		{
			public string Assertion;

			public string ResponseBody;
		}

		internal new class Initializer : ServiceCredential.Initializer
		{
			public Initializer(string id) : this(id, "https://accounts.google.com/o/oauth2/token")
			{
			}

			public Initializer(string id, string tokenServerUrl) : base(tokenServerUrl)
			{
				this.Id = id;
				this.Scopes = new List<string>();
			}

			public string Id { get; private set; }

			public string User { get; set; }

			public List<string> Scopes { get; set; }

			public RSACryptoServiceProvider Key { get; set; }

			public ServiceAccountCredential.Initializer FromCertificate(X509Certificate2 certificate)
			{
				RSACryptoServiceProvider rsacryptoServiceProvider = (RSACryptoServiceProvider)certificate.PrivateKey;
				byte[] keyBlob = rsacryptoServiceProvider.ExportCspBlob(true);
				this.Key = new RSACryptoServiceProvider();
				this.Key.ImportCspBlob(keyBlob);
				return this;
			}
		}
	}
}
