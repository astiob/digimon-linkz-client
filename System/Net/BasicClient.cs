using System;

namespace System.Net
{
	internal class BasicClient : IAuthenticationModule
	{
		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || challenge == null)
			{
				return null;
			}
			string text = challenge.Trim();
			if (text.ToLower().IndexOf("basic") == -1)
			{
				return null;
			}
			return BasicClient.InternalAuthenticate(webRequest, credentials);
		}

		private static byte[] GetBytes(string str)
		{
			int i = str.Length;
			byte[] array = new byte[i];
			for (i--; i >= 0; i--)
			{
				array[i] = (byte)str[i];
			}
			return array;
		}

		private static Authorization InternalAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null || credentials == null)
			{
				return null;
			}
			NetworkCredential credential = credentials.GetCredential(httpWebRequest.AuthUri, "basic");
			if (credential == null)
			{
				return null;
			}
			string userName = credential.UserName;
			if (userName == null || userName == string.Empty)
			{
				return null;
			}
			string password = credential.Password;
			string domain = credential.Domain;
			byte[] bytes;
			if (domain == null || domain == string.Empty || domain.Trim() == string.Empty)
			{
				bytes = BasicClient.GetBytes(userName + ":" + password);
			}
			else
			{
				bytes = BasicClient.GetBytes(string.Concat(new string[]
				{
					domain,
					"\\",
					userName,
					":",
					password
				}));
			}
			string token = "Basic " + Convert.ToBase64String(bytes);
			return new Authorization(token);
		}

		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			return BasicClient.InternalAuthenticate(webRequest, credentials);
		}

		public string AuthenticationType
		{
			get
			{
				return "Basic";
			}
		}

		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}
	}
}
