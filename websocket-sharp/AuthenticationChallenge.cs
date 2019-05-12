using System;
using System.Collections.Specialized;

namespace WebSocketSharp
{
	internal class AuthenticationChallenge
	{
		private NameValueCollection _params;

		private string _scheme;

		internal AuthenticationChallenge(string authScheme, string authParams)
		{
			this._scheme = authScheme;
			this._params = authParams.ParseAuthParams();
		}

		internal NameValueCollection Params
		{
			get
			{
				return this._params;
			}
		}

		public string Algorithm
		{
			get
			{
				return this._params["algorithm"];
			}
		}

		public string Domain
		{
			get
			{
				return this._params["domain"];
			}
		}

		public string Nonce
		{
			get
			{
				return this._params["nonce"];
			}
		}

		public string Opaque
		{
			get
			{
				return this._params["opaque"];
			}
		}

		public string Qop
		{
			get
			{
				return this._params["qop"];
			}
		}

		public string Realm
		{
			get
			{
				return this._params["realm"];
			}
		}

		public string Scheme
		{
			get
			{
				return this._scheme;
			}
		}

		public string Stale
		{
			get
			{
				return this._params["stale"];
			}
		}

		public static AuthenticationChallenge Parse(string value)
		{
			string[] array = value.Split(new char[]
			{
				' '
			}, 2);
			string text = array[0].ToLower();
			return (!(text == "basic") && !(text == "digest")) ? null : new AuthenticationChallenge(text, array[1]);
		}
	}
}
