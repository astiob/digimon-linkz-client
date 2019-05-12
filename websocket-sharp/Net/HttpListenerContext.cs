using System;
using System.Security.Principal;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Net
{
	public sealed class HttpListenerContext
	{
		private HttpConnection _connection;

		private string _error;

		private int _errorStatus;

		private HttpListenerRequest _request;

		private HttpListenerResponse _response;

		private IPrincipal _user;

		internal HttpListener Listener;

		internal HttpListenerContext(HttpConnection connection)
		{
			this._connection = connection;
			this._errorStatus = 400;
			this._request = new HttpListenerRequest(this);
			this._response = new HttpListenerResponse(this);
		}

		internal HttpConnection Connection
		{
			get
			{
				return this._connection;
			}
		}

		internal string ErrorMessage
		{
			get
			{
				return this._error;
			}
			set
			{
				this._error = value;
			}
		}

		internal int ErrorStatus
		{
			get
			{
				return this._errorStatus;
			}
			set
			{
				this._errorStatus = value;
			}
		}

		internal bool HaveError
		{
			get
			{
				return this._error != null && this._error.Length > 0;
			}
		}

		public HttpListenerRequest Request
		{
			get
			{
				return this._request;
			}
		}

		public HttpListenerResponse Response
		{
			get
			{
				return this._response;
			}
		}

		public IPrincipal User
		{
			get
			{
				return this._user;
			}
		}

		internal void SetUser(AuthenticationSchemes expectedScheme, string realm, Func<IIdentity, NetworkCredential> credentialsFinder)
		{
			AuthenticationResponse authenticationResponse = AuthenticationResponse.Parse(this._request.Headers["Authorization"]);
			if (authenticationResponse == null)
			{
				return;
			}
			IIdentity identity = authenticationResponse.ToIdentity();
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

		public HttpListenerWebSocketContext AcceptWebSocket(Logger logger)
		{
			return new HttpListenerWebSocketContext(this, logger);
		}
	}
}
