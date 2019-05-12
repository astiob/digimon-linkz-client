using System;
using System.Collections.Specialized;
using System.Security.Principal;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	internal class AuthenticationResponse
	{
		private uint _nonceCount;

		private NameValueCollection _params;

		private string _scheme;

		private AuthenticationResponse(string authScheme, NameValueCollection authParams)
		{
			this._scheme = authScheme;
			this._params = authParams;
		}

		internal AuthenticationResponse(NetworkCredential credentials) : this("Basic", new NameValueCollection(), credentials, 0u)
		{
		}

		internal AuthenticationResponse(AuthenticationChallenge challenge, NetworkCredential credentials, uint nonceCount) : this(challenge.Scheme, challenge.Params, credentials, nonceCount)
		{
		}

		internal AuthenticationResponse(string authScheme, NameValueCollection authParams, NetworkCredential credentials, uint nonceCount)
		{
			this._scheme = authScheme.ToLower();
			this._params = authParams;
			this._params["username"] = credentials.UserName;
			this._params["password"] = credentials.Password;
			this._params["uri"] = credentials.Domain;
			this._nonceCount = nonceCount;
			if (this._scheme == "digest")
			{
				this.initAsDigest();
			}
		}

		internal uint NonceCount
		{
			get
			{
				return (this._nonceCount >= uint.MaxValue) ? 0u : this._nonceCount;
			}
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

		public string Cnonce
		{
			get
			{
				return this._params["cnonce"];
			}
		}

		public string Nc
		{
			get
			{
				return this._params["nc"];
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

		public string Password
		{
			get
			{
				return this._params["password"];
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

		public string Response
		{
			get
			{
				return this._params["response"];
			}
		}

		public string Scheme
		{
			get
			{
				return this._scheme;
			}
		}

		public string Uri
		{
			get
			{
				return this._params["uri"];
			}
		}

		public string UserName
		{
			get
			{
				return this._params["username"];
			}
		}

		private static bool contains(string[] array, string item)
		{
			foreach (string text in array)
			{
				if (text.Trim().ToLower() == item)
				{
					return true;
				}
			}
			return false;
		}

		private void initAsDigest()
		{
			string text = this._params["qop"];
			if (text != null)
			{
				string text2 = "auth";
				if (AuthenticationResponse.contains(text.Split(new char[]
				{
					','
				}), text2))
				{
					this._params["qop"] = text2;
					this._params["nc"] = string.Format("{0:x8}", this._nonceCount += 1u);
					this._params["cnonce"] = HttpUtility.CreateNonceValue();
				}
				else
				{
					this._params["qop"] = null;
				}
			}
			this._params["method"] = "GET";
			this._params["response"] = HttpUtility.CreateRequestDigest(this._params);
		}

		public static AuthenticationResponse Parse(string value)
		{
			try
			{
				string[] array = value.Split(new char[]
				{
					' '
				}, 2);
				if (array.Length != 2)
				{
					return null;
				}
				string text = array[0].ToLower();
				return (!(text == "basic")) ? ((!(text == "digest")) ? null : new AuthenticationResponse(text, array[1].ParseAuthParams())) : new AuthenticationResponse(text, array[1].ParseBasicAuthResponseParams());
			}
			catch
			{
			}
			return null;
		}

		public IIdentity ToIdentity()
		{
			IIdentity result;
			if (this._scheme == "basic")
			{
				IIdentity identity = new HttpBasicIdentity(this._params["username"], this._params["password"]);
				result = identity;
			}
			else
			{
				result = ((!(this._scheme == "digest")) ? null : new HttpDigestIdentity(this._params));
			}
			return result;
		}

		public override string ToString()
		{
			return (!(this._scheme == "basic")) ? ((!(this._scheme == "digest")) ? string.Empty : HttpUtility.CreateDigestAuthCredentials(this._params)) : HttpUtility.CreateBasicAuthCredentials(this._params["username"], this._params["password"]);
		}
	}
}
