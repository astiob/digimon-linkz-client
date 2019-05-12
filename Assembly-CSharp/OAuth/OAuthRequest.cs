using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace OAuth
{
	public class OAuthRequest
	{
		public virtual OAuthSignatureMethod SignatureMethod { get; set; }

		public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

		public virtual OAuthRequestType Type { get; set; }

		public virtual string Method { get; set; }

		public virtual string Realm { get; set; }

		public virtual string ConsumerKey { get; set; }

		public virtual string ConsumerSecret { get; set; }

		public virtual string Token { get; set; }

		public virtual string TokenSecret { get; set; }

		public virtual string Verifier { get; set; }

		public virtual string ClientUsername { get; set; }

		public virtual string ClientPassword { get; set; }

		public virtual string CallbackUrl { get; set; }

		public virtual string Version { get; set; }

		public virtual string SessionHandle { get; set; }

		public virtual string RequestUrl { get; set; }

		public string GetAuthorizationHeader(NameValueCollection parameters)
		{
			WebParameterCollection parameters2 = new WebParameterCollection(parameters);
			return this.GetAuthorizationHeader(parameters2);
		}

		public string GetAuthorizationHeader(IDictionary<string, string> parameters)
		{
			WebParameterCollection parameters2 = new WebParameterCollection(parameters);
			return this.GetAuthorizationHeader(parameters2);
		}

		public string GetAuthorizationHeader()
		{
			WebParameterCollection parameters = new WebParameterCollection(0);
			return this.GetAuthorizationHeader(parameters);
		}

		public string GetAuthorizationHeader(WebParameterCollection parameters)
		{
			switch (this.Type)
			{
			case OAuthRequestType.RequestToken:
				this.ValidateRequestState();
				return this.GetSignatureAuthorizationHeader(parameters);
			case OAuthRequestType.AccessToken:
				this.ValidateAccessRequestState();
				return this.GetSignatureAuthorizationHeader(parameters);
			case OAuthRequestType.ProtectedResource:
				this.ValidateProtectedResourceState();
				return this.GetSignatureAuthorizationHeader(parameters);
			case OAuthRequestType.ClientAuthentication:
				this.ValidateClientAuthAccessRequestState();
				return this.GetClientSignatureAuthorizationHeader(parameters);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private string GetSignatureAuthorizationHeader(WebParameterCollection parameters)
		{
			string newSignature = this.GetNewSignature(parameters);
			parameters.Add("oauth_signature", newSignature);
			return this.WriteAuthorizationHeader(parameters);
		}

		private string GetClientSignatureAuthorizationHeader(WebParameterCollection parameters)
		{
			string newSignatureXAuth = this.GetNewSignatureXAuth(parameters);
			parameters.Add("oauth_signature", newSignatureXAuth);
			return this.WriteAuthorizationHeader(parameters);
		}

		private string WriteAuthorizationHeader(WebParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder("OAuth ");
			if (!OAuthRequest.IsNullOrBlank(this.Realm))
			{
				stringBuilder.AppendFormat("realm=\"{0}\",", OAuthTools.UrlEncodeRelaxed(this.Realm));
			}
			parameters.Sort((WebParameter l, WebParameter r) => l.Name.CompareTo(r.Name));
			int num = 0;
			foreach (WebParameter webParameter in parameters.Where((WebParameter parameter) => !OAuthRequest.IsNullOrBlank(parameter.Name) && !OAuthRequest.IsNullOrBlank(parameter.Value) && (parameter.Name.StartsWith("oauth_") || parameter.Name.StartsWith("x_auth_"))))
			{
				num++;
				string format = (num >= parameters.Count) ? "{0}=\"{1}\"" : "{0}=\"{1}\",";
				stringBuilder.AppendFormat(format, webParameter.Name, webParameter.Value);
			}
			return stringBuilder.ToString();
		}

		public string GetAuthorizationQuery(NameValueCollection parameters)
		{
			WebParameterCollection parameters2 = new WebParameterCollection(parameters);
			return this.GetAuthorizationQuery(parameters2);
		}

		public string GetAuthorizationQuery(IDictionary<string, string> parameters)
		{
			WebParameterCollection parameters2 = new WebParameterCollection(parameters);
			return this.GetAuthorizationQuery(parameters2);
		}

		public string GetAuthorizationQuery()
		{
			WebParameterCollection parameters = new WebParameterCollection(0);
			return this.GetAuthorizationQuery(parameters);
		}

		private string GetAuthorizationQuery(WebParameterCollection parameters)
		{
			switch (this.Type)
			{
			case OAuthRequestType.RequestToken:
				this.ValidateRequestState();
				return this.GetSignatureAuthorizationQuery(parameters);
			case OAuthRequestType.AccessToken:
				this.ValidateAccessRequestState();
				return this.GetSignatureAuthorizationQuery(parameters);
			case OAuthRequestType.ProtectedResource:
				this.ValidateProtectedResourceState();
				return this.GetSignatureAuthorizationQuery(parameters);
			case OAuthRequestType.ClientAuthentication:
				this.ValidateClientAuthAccessRequestState();
				return this.GetClientSignatureAuthorizationQuery(parameters);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private string GetSignatureAuthorizationQuery(WebParameterCollection parameters)
		{
			string newSignature = this.GetNewSignature(parameters);
			parameters.Add("oauth_signature", newSignature);
			return OAuthRequest.WriteAuthorizationQuery(parameters);
		}

		private string GetClientSignatureAuthorizationQuery(WebParameterCollection parameters)
		{
			string newSignatureXAuth = this.GetNewSignatureXAuth(parameters);
			parameters.Add("oauth_signature", newSignatureXAuth);
			return OAuthRequest.WriteAuthorizationQuery(parameters);
		}

		private static string WriteAuthorizationQuery(WebParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			parameters.Sort((WebParameter l, WebParameter r) => l.Name.CompareTo(r.Name));
			int num = 0;
			foreach (WebParameter webParameter in parameters.Where((WebParameter parameter) => !OAuthRequest.IsNullOrBlank(parameter.Name) && !OAuthRequest.IsNullOrBlank(parameter.Value) && (parameter.Name.StartsWith("oauth_") || parameter.Name.StartsWith("x_auth_"))))
			{
				num++;
				string format = (num >= parameters.Count) ? "{0}={1}" : "{0}={1}&";
				stringBuilder.AppendFormat(format, webParameter.Name, webParameter.Value);
			}
			return stringBuilder.ToString();
		}

		private string GetNewSignature(WebParameterCollection parameters)
		{
			string timestamp = OAuthTools.GetTimestamp();
			string nonce = OAuthTools.GetNonce();
			this.AddAuthParameters(parameters, timestamp, nonce);
			string signatureBase = OAuthTools.ConcatenateRequestElements(this.Method.ToUpperInvariant(), this.RequestUrl, parameters);
			return OAuthTools.GetSignature(this.SignatureMethod, this.SignatureTreatment, signatureBase, this.ConsumerSecret, this.TokenSecret);
		}

		private string GetNewSignatureXAuth(WebParameterCollection parameters)
		{
			string timestamp = OAuthTools.GetTimestamp();
			string nonce = OAuthTools.GetNonce();
			this.AddXAuthParameters(parameters, timestamp, nonce);
			string signatureBase = OAuthTools.ConcatenateRequestElements(this.Method.ToUpperInvariant(), this.RequestUrl, parameters);
			return OAuthTools.GetSignature(this.SignatureMethod, this.SignatureTreatment, signatureBase, this.ConsumerSecret, this.TokenSecret);
		}

		public static OAuthRequest ForRequestToken(string consumerKey, string consumerSecret)
		{
			return new OAuthRequest
			{
				Method = "GET",
				Type = OAuthRequestType.RequestToken,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				SignatureTreatment = OAuthSignatureTreatment.Escaped,
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret
			};
		}

		public static OAuthRequest ForRequestToken(string consumerKey, string consumerSecret, string callbackUrl)
		{
			OAuthRequest oauthRequest = OAuthRequest.ForRequestToken(consumerKey, consumerSecret);
			oauthRequest.CallbackUrl = callbackUrl;
			return oauthRequest;
		}

		public static OAuthRequest ForAccessToken(string consumerKey, string consumerSecret, string requestToken, string requestTokenSecret)
		{
			return new OAuthRequest
			{
				Method = "GET",
				Type = OAuthRequestType.AccessToken,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				SignatureTreatment = OAuthSignatureTreatment.Escaped,
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret,
				Token = requestToken,
				TokenSecret = requestTokenSecret
			};
		}

		public static OAuthRequest ForAccessToken(string consumerKey, string consumerSecret, string requestToken, string requestTokenSecret, string verifier)
		{
			OAuthRequest oauthRequest = OAuthRequest.ForAccessToken(consumerKey, consumerSecret, requestToken, requestTokenSecret);
			oauthRequest.Verifier = verifier;
			return oauthRequest;
		}

		public static OAuthRequest ForAccessTokenRefresh(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string sessionHandle)
		{
			OAuthRequest oauthRequest = OAuthRequest.ForAccessToken(consumerKey, consumerSecret, accessToken, accessTokenSecret);
			oauthRequest.SessionHandle = sessionHandle;
			return oauthRequest;
		}

		public static OAuthRequest ForAccessTokenRefresh(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret, string sessionHandle, string verifier)
		{
			OAuthRequest oauthRequest = OAuthRequest.ForAccessToken(consumerKey, consumerSecret, accessToken, accessTokenSecret);
			oauthRequest.SessionHandle = sessionHandle;
			oauthRequest.Verifier = verifier;
			return oauthRequest;
		}

		public static OAuthRequest ForClientAuthentication(string consumerKey, string consumerSecret, string username, string password)
		{
			return new OAuthRequest
			{
				Method = "GET",
				Type = OAuthRequestType.ClientAuthentication,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				SignatureTreatment = OAuthSignatureTreatment.Escaped,
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret,
				ClientUsername = username,
				ClientPassword = password
			};
		}

		public static OAuthRequest ForProtectedResource(string method, string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
		{
			return new OAuthRequest
			{
				Method = (method ?? "GET"),
				Type = OAuthRequestType.ProtectedResource,
				SignatureMethod = OAuthSignatureMethod.HmacSha1,
				SignatureTreatment = OAuthSignatureTreatment.Escaped,
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret,
				Token = accessToken,
				TokenSecret = accessTokenSecret
			};
		}

		private void ValidateRequestState()
		{
			if (OAuthRequest.IsNullOrBlank(this.Method))
			{
				throw new ArgumentException("You must specify an HTTP method");
			}
			if (OAuthRequest.IsNullOrBlank(this.RequestUrl))
			{
				throw new ArgumentException("You must specify a request token URL");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerKey))
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerSecret))
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
		}

		private void ValidateAccessRequestState()
		{
			if (OAuthRequest.IsNullOrBlank(this.Method))
			{
				throw new ArgumentException("You must specify an HTTP method");
			}
			if (OAuthRequest.IsNullOrBlank(this.RequestUrl))
			{
				throw new ArgumentException("You must specify an access token URL");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerKey))
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerSecret))
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
			if (OAuthRequest.IsNullOrBlank(this.Token))
			{
				throw new ArgumentException("You must specify a token");
			}
		}

		private void ValidateClientAuthAccessRequestState()
		{
			if (OAuthRequest.IsNullOrBlank(this.Method))
			{
				throw new ArgumentException("You must specify an HTTP method");
			}
			if (OAuthRequest.IsNullOrBlank(this.RequestUrl))
			{
				throw new ArgumentException("You must specify an access token URL");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerKey))
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerSecret))
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
			if (OAuthRequest.IsNullOrBlank(this.ClientUsername) || OAuthRequest.IsNullOrBlank(this.ClientPassword))
			{
				throw new ArgumentException("You must specify user credentials");
			}
		}

		private void ValidateProtectedResourceState()
		{
			if (OAuthRequest.IsNullOrBlank(this.Method))
			{
				throw new ArgumentException("You must specify an HTTP method");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerKey))
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (OAuthRequest.IsNullOrBlank(this.ConsumerSecret))
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
		}

		private void AddAuthParameters(ICollection<WebParameter> parameters, string timestamp, string nonce)
		{
			WebParameterCollection webParameterCollection = new WebParameterCollection
			{
				new WebParameter("oauth_consumer_key", this.ConsumerKey),
				new WebParameter("oauth_nonce", nonce),
				new WebParameter("oauth_signature_method", OAuthRequest.ToRequestValue(this.SignatureMethod)),
				new WebParameter("oauth_timestamp", timestamp),
				new WebParameter("oauth_version", this.Version ?? "1.0")
			};
			if (!OAuthRequest.IsNullOrBlank(this.Token))
			{
				webParameterCollection.Add(new WebParameter("oauth_token", this.Token));
			}
			if (!OAuthRequest.IsNullOrBlank(this.CallbackUrl))
			{
				webParameterCollection.Add(new WebParameter("oauth_callback", this.CallbackUrl));
			}
			if (!OAuthRequest.IsNullOrBlank(this.Verifier))
			{
				webParameterCollection.Add(new WebParameter("oauth_verifier", this.Verifier));
			}
			if (!OAuthRequest.IsNullOrBlank(this.SessionHandle))
			{
				webParameterCollection.Add(new WebParameter("oauth_session_handle", this.SessionHandle));
			}
			foreach (WebParameter item in webParameterCollection)
			{
				parameters.Add(item);
			}
		}

		private void AddXAuthParameters(ICollection<WebParameter> parameters, string timestamp, string nonce)
		{
			WebParameterCollection webParameterCollection = new WebParameterCollection
			{
				new WebParameter("x_auth_username", this.ClientUsername),
				new WebParameter("x_auth_password", this.ClientPassword),
				new WebParameter("x_uuid", "TEST"),
				new WebParameter("x_auth_mode", "client_auth"),
				new WebParameter("oauth_consumer_key", this.ConsumerKey),
				new WebParameter("oauth_signature_method", OAuthRequest.ToRequestValue(this.SignatureMethod)),
				new WebParameter("oauth_timestamp", timestamp),
				new WebParameter("oauth_nonce", nonce),
				new WebParameter("oauth_version", this.Version ?? "1.0")
			};
			foreach (WebParameter item in webParameterCollection)
			{
				parameters.Add(item);
			}
		}

		public static string ToRequestValue(OAuthSignatureMethod signatureMethod)
		{
			string text = signatureMethod.ToString().ToUpper();
			int num = text.IndexOf("SHA1");
			return (num <= -1) ? text : text.Insert(num, "-");
		}

		private static bool IsNullOrBlank(string value)
		{
			return string.IsNullOrEmpty(value) || (!string.IsNullOrEmpty(value) && value.Trim() == string.Empty);
		}
	}
}
