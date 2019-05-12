using Google.MiniJSON;
using System;
using System.Collections.Generic;
using System.IO;

namespace Firebase.Platform.Security
{
	internal class GAuthToken
	{
		private const string TokenPrefix = "gauth|";

		private const string AuthKey = "auth";

		private const string TokenKey = "token";

		private readonly IDictionary<string, object> _auth;

		private readonly string _token;

		public GAuthToken(string token, IDictionary<string, object> auth)
		{
			this._token = token;
			this._auth = auth;
		}

		public static GAuthToken TryParseFromString(string rawToken)
		{
			if (!rawToken.StartsWith("gauth|"))
			{
				return null;
			}
			string json = rawToken.Substring("gauth|".Length);
			GAuthToken result;
			try
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)Json.Deserialize(json);
				string token = (string)dictionary["token"];
				IDictionary<string, object> auth = (IDictionary<string, object>)dictionary["auth"];
				result = new GAuthToken(token, auth);
			}
			catch (IOException innerException)
			{
				throw new Exception("Failed to parse gauth token", innerException);
			}
			return result;
		}

		public virtual string SerializeToString()
		{
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["token"] = this._token;
			dictionary["auth"] = this._auth;
			string result;
			try
			{
				string str = Json.Serialize(dictionary);
				result = "gauth|" + str;
			}
			catch (IOException innerException)
			{
				throw new Exception("Failed to serialize gauth token", innerException);
			}
			return result;
		}

		public virtual string GetToken()
		{
			return this._token;
		}

		public virtual IDictionary<string, object> GetAuth()
		{
			return this._auth;
		}
	}
}
