using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;

namespace WebSocketSharp.Net
{
	public sealed class HttpListenerRequest
	{
		private static byte[] _100continue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");

		private string[] _acceptTypes;

		private bool _chunked;

		private Encoding _contentEncoding;

		private long _contentLength;

		private bool _contentLengthWasSet;

		private HttpListenerContext _context;

		private CookieCollection _cookies;

		private WebHeaderCollection _headers;

		private Guid _identifier;

		private Stream _inputStream;

		private bool _keepAlive;

		private bool _keepAliveWasSet;

		private string _method;

		private NameValueCollection _queryString;

		private string _rawUrl;

		private Uri _referer;

		private Uri _url;

		private string[] _userLanguages;

		private Version _version;

		internal HttpListenerRequest(HttpListenerContext context)
		{
			this._context = context;
			this._contentLength = -1L;
			this._headers = new WebHeaderCollection();
			this._identifier = Guid.NewGuid();
			this._version = HttpVersion.Version10;
		}

		public string[] AcceptTypes
		{
			get
			{
				return this._acceptTypes;
			}
		}

		public int ClientCertificateError
		{
			get
			{
				return 0;
			}
		}

		public Encoding ContentEncoding
		{
			get
			{
				Encoding result;
				if ((result = this._contentEncoding) == null)
				{
					result = (this._contentEncoding = Encoding.Default);
				}
				return result;
			}
		}

		public long ContentLength64
		{
			get
			{
				return this._contentLength;
			}
		}

		public string ContentType
		{
			get
			{
				return this._headers["Content-Type"];
			}
		}

		public CookieCollection Cookies
		{
			get
			{
				CookieCollection result;
				if ((result = this._cookies) == null)
				{
					result = (this._cookies = this._headers.GetCookies(false));
				}
				return result;
			}
		}

		public bool HasEntityBody
		{
			get
			{
				return this._contentLength > 0L || this._chunked;
			}
		}

		public NameValueCollection Headers
		{
			get
			{
				return this._headers;
			}
		}

		public string HttpMethod
		{
			get
			{
				return this._method;
			}
		}

		public Stream InputStream
		{
			get
			{
				Stream result;
				if ((result = this._inputStream) == null)
				{
					result = (this._inputStream = ((!this.HasEntityBody) ? Stream.Null : this._context.Connection.GetRequestStream(this._chunked, this._contentLength)));
				}
				return result;
			}
		}

		public bool IsAuthenticated
		{
			get
			{
				IPrincipal user = this._context.User;
				return user != null && user.Identity.IsAuthenticated;
			}
		}

		public bool IsLocal
		{
			get
			{
				return this.RemoteEndPoint.Address.IsLocal();
			}
		}

		public bool IsSecureConnection
		{
			get
			{
				return this._context.Connection.IsSecure;
			}
		}

		public bool IsWebSocketRequest
		{
			get
			{
				return this._method == "GET" && this._version >= HttpVersion.Version11 && this._headers.Contains("Upgrade", "websocket") && this._headers.Contains("Connection", "Upgrade");
			}
		}

		public bool KeepAlive
		{
			get
			{
				if (!this._keepAliveWasSet)
				{
					this._keepAlive = (this._headers.Contains("Connection", "keep-alive") || this._version == HttpVersion.Version11 || (this._headers.Contains("Keep-Alive") && !this._headers.Contains("Keep-Alive", "closed")));
					this._keepAliveWasSet = true;
				}
				return this._keepAlive;
			}
		}

		public IPEndPoint LocalEndPoint
		{
			get
			{
				return this._context.Connection.LocalEndPoint;
			}
		}

		public Version ProtocolVersion
		{
			get
			{
				return this._version;
			}
		}

		public NameValueCollection QueryString
		{
			get
			{
				return this._queryString;
			}
		}

		public string RawUrl
		{
			get
			{
				return this._rawUrl;
			}
		}

		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return this._context.Connection.RemoteEndPoint;
			}
		}

		public Guid RequestTraceIdentifier
		{
			get
			{
				return this._identifier;
			}
		}

		public Uri Url
		{
			get
			{
				return this._url;
			}
		}

		public Uri UrlReferrer
		{
			get
			{
				return this._referer;
			}
		}

		public string UserAgent
		{
			get
			{
				return this._headers["User-Agent"];
			}
		}

		public string UserHostAddress
		{
			get
			{
				return this.LocalEndPoint.ToString();
			}
		}

		public string UserHostName
		{
			get
			{
				return this._headers["Host"];
			}
		}

		public string[] UserLanguages
		{
			get
			{
				return this._userLanguages;
			}
		}

		private void createQueryString(string query)
		{
			if (query == null || query.Length == 0)
			{
				this._queryString = new NameValueCollection(1);
				return;
			}
			this._queryString = new NameValueCollection();
			if (query[0] == '?')
			{
				query = query.Substring(1);
			}
			string[] array = query.Split(new char[]
			{
				'&'
			});
			foreach (string text in array)
			{
				int num = text.IndexOf('=');
				if (num == -1)
				{
					this._queryString.Add(null, HttpUtility.UrlDecode(text));
				}
				else
				{
					string name = HttpUtility.UrlDecode(text.Substring(0, num));
					string val = HttpUtility.UrlDecode(text.Substring(num + 1));
					this._queryString.Add(name, val);
				}
			}
		}

		internal void AddHeader(string header)
		{
			int num = header.IndexOf(':');
			if (num == -1)
			{
				this._context.ErrorMessage = "Invalid header";
				return;
			}
			string text = header.Substring(0, num).Trim();
			string text2 = header.Substring(num + 1).Trim();
			string a = text.ToLower(CultureInfo.InvariantCulture);
			this._headers.SetInternal(text, text2, false);
			if (a == "accept")
			{
				this._acceptTypes = text2.SplitHeaderValue(new char[]
				{
					','
				}).ToArray<string>();
				return;
			}
			if (a == "accept-language")
			{
				this._userLanguages = text2.Split(new char[]
				{
					','
				});
				return;
			}
			if (a == "content-length")
			{
				long num2;
				if (long.TryParse(text2, out num2) && num2 >= 0L)
				{
					this._contentLength = num2;
					this._contentLengthWasSet = true;
				}
				else
				{
					this._context.ErrorMessage = "Invalid Content-Length header";
				}
				return;
			}
			if (a == "content-type")
			{
				string[] array = text2.Split(new char[]
				{
					';'
				});
				foreach (string text3 in array)
				{
					string text4 = text3.Trim();
					if (text4.StartsWith("charset"))
					{
						string value = text4.GetValue("=");
						if (value != null && value.Length > 0)
						{
							try
							{
								this._contentEncoding = Encoding.GetEncoding(value);
							}
							catch
							{
								this._context.ErrorMessage = "Invalid Content-Type header";
							}
						}
						break;
					}
				}
				return;
			}
			if (a == "referer")
			{
				this._referer = text2.ToUri();
			}
		}

		internal void FinishInitialization()
		{
			string text = this._headers["Host"];
			bool flag = text == null || text.Length == 0;
			if (this._version > HttpVersion.Version10 && flag)
			{
				this._context.ErrorMessage = "Invalid Host header";
				return;
			}
			if (flag)
			{
				text = this.UserHostAddress;
			}
			Uri uri = this._rawUrl.ToUri();
			string text2;
			if (uri != null && uri.IsAbsoluteUri)
			{
				text = uri.Host;
				text2 = uri.PathAndQuery;
			}
			else
			{
				text2 = HttpUtility.UrlDecode(this._rawUrl);
			}
			int num = text.IndexOf(':');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			string text3 = (!this.IsWebSocketRequest) ? "http" : "ws";
			string text4 = string.Format("{0}://{1}:{2}{3}", new object[]
			{
				(!this.IsSecureConnection) ? text3 : (text3 + "s"),
				text,
				this.LocalEndPoint.Port,
				text2
			});
			if (!Uri.TryCreate(text4, UriKind.Absolute, out this._url))
			{
				this._context.ErrorMessage = "Invalid request url: " + text4;
				return;
			}
			this.createQueryString(this._url.Query);
			string text5 = this.Headers["Transfer-Encoding"];
			if (this._version >= HttpVersion.Version11 && text5 != null && text5.Length > 0)
			{
				this._chunked = (text5.ToLower() == "chunked");
				if (!this._chunked)
				{
					this._context.ErrorMessage = string.Empty;
					this._context.ErrorStatus = 501;
					return;
				}
			}
			if (!this._chunked && !this._contentLengthWasSet)
			{
				string a = this._method.ToLower();
				if (a == "post" || a == "put")
				{
					this._context.ErrorMessage = string.Empty;
					this._context.ErrorStatus = 411;
					return;
				}
			}
			string text6 = this.Headers["Expect"];
			if (text6 != null && text6.Length > 0 && text6.ToLower() == "100-continue")
			{
				ResponseStream responseStream = this._context.Connection.GetResponseStream();
				responseStream.InternalWrite(HttpListenerRequest._100continue, 0, HttpListenerRequest._100continue.Length);
			}
		}

		internal bool FlushInput()
		{
			if (!this.HasEntityBody)
			{
				return true;
			}
			int num = 2048;
			if (this._contentLength > 0L)
			{
				num = (int)Math.Min(this._contentLength, (long)num);
			}
			byte[] buffer = new byte[num];
			bool result;
			for (;;)
			{
				try
				{
					IAsyncResult asyncResult = this.InputStream.BeginRead(buffer, 0, num, null, null);
					if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(100))
					{
						result = false;
						break;
					}
					if (this.InputStream.EndRead(asyncResult) <= 0)
					{
						result = true;
						break;
					}
				}
				catch
				{
					result = false;
					break;
				}
			}
			return result;
		}

		internal void SetRequestLine(string requestLine)
		{
			string[] array = requestLine.Split(new char[]
			{
				' '
			}, 3);
			if (array.Length != 3)
			{
				this._context.ErrorMessage = "Invalid request line (parts)";
				return;
			}
			this._method = array[0];
			if (!this._method.IsToken())
			{
				this._context.ErrorMessage = "Invalid request line (method)";
				return;
			}
			this._rawUrl = array[1];
			if (array[2].Length != 8 || !array[2].StartsWith("HTTP/"))
			{
				this._context.ErrorMessage = "Invalid request line (version)";
				return;
			}
			try
			{
				this._version = new Version(array[2].Substring(5));
				if (this._version.Major < 1)
				{
					throw new Exception();
				}
			}
			catch
			{
				this._context.ErrorMessage = "Invalid request line (version)";
			}
		}

		public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
		{
			throw new NotImplementedException();
		}

		public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
		{
			throw new NotImplementedException();
		}

		public X509Certificate2 GetClientCertificate()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}\r\n", this._method, this._rawUrl, this._version);
			foreach (string text in this._headers.AllKeys)
			{
				stringBuilder.AppendFormat("{0}: {1}\r\n", text, this._headers[text]);
			}
			stringBuilder.Append("\r\n");
			return stringBuilder.ToString();
		}
	}
}
