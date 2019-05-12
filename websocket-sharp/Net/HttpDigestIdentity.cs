using System;
using System.Collections.Specialized;
using System.Security.Principal;

namespace WebSocketSharp.Net
{
	public class HttpDigestIdentity : GenericIdentity
	{
		private NameValueCollection _params;

		internal HttpDigestIdentity(NameValueCollection authParams) : base(authParams["username"], "Digest")
		{
			this._params = authParams;
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

		public string Uri
		{
			get
			{
				return this._params["uri"];
			}
		}

		internal bool IsValid(string password, string realm, string method, string entity)
		{
			NameValueCollection nameValueCollection = new NameValueCollection(this._params);
			nameValueCollection["password"] = password;
			nameValueCollection["realm"] = realm;
			nameValueCollection["method"] = method;
			nameValueCollection["entity"] = entity;
			return this._params["response"] == HttpUtility.CreateRequestDigest(nameValueCollection);
		}
	}
}
