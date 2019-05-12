using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace WebSocketSharp.Net.WebSockets
{
	public class TcpListenerWebSocketContext : WebSocketContext
	{
		private TcpClient _client;

		private CookieCollection _cookies;

		private HandshakeRequest _request;

		private bool _secure;

		private WsStream _stream;

		private Uri _uri;

		private IPrincipal _user;

		private WebSocket _websocket;

		internal TcpListenerWebSocketContext(TcpClient client, X509Certificate cert, bool secure, Logger logger)
		{
			this._client = client;
			this._secure = secure;
			this._stream = WsStream.CreateServerStream(client, cert, secure);
			this._request = this._stream.ReadHandshake<HandshakeRequest>(new Func<string[], HandshakeRequest>(HandshakeRequest.Parse), 90000);
			this._websocket = new WebSocket(this, logger);
		}

		internal WsStream Stream
		{
			get
			{
				return this._stream;
			}
		}

		public override CookieCollection CookieCollection
		{
			get
			{
				CookieCollection result;
				if ((result = this._cookies) == null)
				{
					result = (this._cookies = this._request.Cookies);
				}
				return result;
			}
		}

		public override NameValueCollection Headers
		{
			get
			{
				return this._request.Headers;
			}
		}

		public override string Host
		{
			get
			{
				return this._request.Headers["Host"];
			}
		}

		public override bool IsAuthenticated
		{
			get
			{
				return this._user != null && this._user.Identity.IsAuthenticated;
			}
		}

		public override bool IsLocal
		{
			get
			{
				return this.UserEndPoint.Address.IsLocal();
			}
		}

		public override bool IsSecureConnection
		{
			get
			{
				return this._secure;
			}
		}

		public override bool IsWebSocketRequest
		{
			get
			{
				return this._request.IsWebSocketRequest;
			}
		}

		public override string Origin
		{
			get
			{
				return this._request.Headers["Origin"];
			}
		}

		public override string Path
		{
			get
			{
				return this._request.RequestUri.GetAbsolutePath();
			}
		}

		public override NameValueCollection QueryString
		{
			get
			{
				return this._request.QueryString;
			}
		}

		public override Uri RequestUri
		{
			get
			{
				Uri result;
				if ((result = this._uri) == null)
				{
					result = (this._uri = this.createRequestUri());
				}
				return result;
			}
		}

		public override string SecWebSocketKey
		{
			get
			{
				return this._request.Headers["Sec-WebSocket-Key"];
			}
		}

		public override IEnumerable<string> SecWebSocketProtocols
		{
			get
			{
				string protocols = this._request.Headers["Sec-WebSocket-Protocol"];
				if (protocols != null)
				{
					foreach (string protocol in protocols.Split(new char[]
					{
						','
					}))
					{
						yield return protocol.Trim();
					}
				}
				yield break;
			}
		}

		public override string SecWebSocketVersion
		{
			get
			{
				return this._request.Headers["Sec-WebSocket-Version"];
			}
		}

		public override IPEndPoint ServerEndPoint
		{
			get
			{
				return (IPEndPoint)this._client.Client.LocalEndPoint;
			}
		}

		public override IPrincipal User
		{
			get
			{
				return this._user;
			}
		}

		public override IPEndPoint UserEndPoint
		{
			get
			{
				return (IPEndPoint)this._client.Client.RemoteEndPoint;
			}
		}

		public override WebSocket WebSocket
		{
			get
			{
				return this._websocket;
			}
		}

		private Uri createRequestUri()
		{
			string arg = (!this._secure) ? "ws" : "wss";
			string arg2 = this._request.Headers["Host"];
			Uri requestUri = this._request.RequestUri;
			string arg3 = (!requestUri.IsAbsoluteUri) ? HttpUtility.UrlDecode(this._request.RawUrl) : requestUri.PathAndQuery;
			return string.Format("{0}://{1}{2}", arg, arg2, arg3).ToUri();
		}

		internal void Close()
		{
			this._stream.Close();
			this._client.Close();
		}

		internal void Close(HttpStatusCode code)
		{
			this._websocket.Close(HandshakeResponse.CreateCloseResponse(code));
		}

		internal void SendAuthChallenge(string challenge)
		{
			HandshakeResponse handshakeResponse = new HandshakeResponse(HttpStatusCode.Unauthorized);
			handshakeResponse.Headers["WWW-Authenticate"] = challenge;
			this._stream.WriteHandshake(handshakeResponse);
			this._request = this._stream.ReadHandshake<HandshakeRequest>(new Func<string[], HandshakeRequest>(HandshakeRequest.Parse), 15000);
		}

		internal void SetUser(AuthenticationSchemes expectedScheme, string realm, Func<IIdentity, NetworkCredential> credentialsFinder)
		{
			AuthenticationResponse authResponse = this._request.AuthResponse;
			if (authResponse == null)
			{
				return;
			}
			IIdentity identity = authResponse.ToIdentity();
			if (identity == null)
			{
				return;
			}
			NetworkCredential networkCredential = null;
			try
			{
				networkCredential = credentialsFinder(identity);
			}
			catch
			{
			}
			if (networkCredential == null)
			{
				return;
			}
			bool flag = (expectedScheme != AuthenticationSchemes.Basic) ? (expectedScheme == AuthenticationSchemes.Digest && ((HttpDigestIdentity)identity).IsValid(networkCredential.Password, realm, this._request.HttpMethod, null)) : (((HttpBasicIdentity)identity).Password == networkCredential.Password);
			if (flag)
			{
				this._user = new GenericPrincipal(identity, networkCredential.Roles);
			}
		}

		public override string ToString()
		{
			return this._request.ToString();
		}
	}
}
