using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.Net
{
	/// <summary>Describes an incoming HTTP request to an <see cref="T:System.Net.HttpListener" /> object. This class cannot be inherited.</summary>
	public sealed class HttpListenerRequest
	{
		private string[] accept_types;

		private Encoding content_encoding;

		private long content_length;

		private bool cl_set;

		private CookieCollection cookies;

		private WebHeaderCollection headers;

		private string method;

		private Stream input_stream;

		private Version version;

		private System.Collections.Specialized.NameValueCollection query_string;

		private string raw_url;

		private Guid identifier;

		private System.Uri url;

		private System.Uri referrer;

		private string[] user_languages;

		private HttpListenerContext context;

		private bool is_chunked;

		private static byte[] _100continue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");

		private static readonly string[] no_body_methods = new string[]
		{
			"GET",
			"HEAD",
			"DELETE"
		};

		private static char[] separators = new char[]
		{
			' '
		};

		internal HttpListenerRequest(HttpListenerContext context)
		{
			this.context = context;
			this.headers = new WebHeaderCollection();
			this.input_stream = Stream.Null;
			this.version = HttpVersion.Version10;
		}

		internal void SetRequestLine(string req)
		{
			string[] array = req.Split(HttpListenerRequest.separators, 3);
			if (array.Length != 3)
			{
				this.context.ErrorMessage = "Invalid request line (parts).";
				return;
			}
			this.method = array[0];
			foreach (char c in this.method)
			{
				int num = (int)c;
				if ((num < 65 || num > 90) && (num <= 32 || c >= '\u007f' || c == '(' || c == ')' || c == '<' || c == '<' || c == '>' || c == '@' || c == ',' || c == ';' || c == ':' || c == '\\' || c == '"' || c == '/' || c == '[' || c == ']' || c == '?' || c == '=' || c == '{' || c == '}'))
				{
					this.context.ErrorMessage = "(Invalid verb)";
					return;
				}
			}
			this.raw_url = array[1];
			if (array[2].Length != 8 || !array[2].StartsWith("HTTP/"))
			{
				this.context.ErrorMessage = "Invalid request line (version).";
				return;
			}
			try
			{
				this.version = new Version(array[2].Substring(5));
				if (this.version.Major < 1)
				{
					throw new Exception();
				}
			}
			catch
			{
				this.context.ErrorMessage = "Invalid request line (version).";
			}
		}

		private void CreateQueryString(string query)
		{
			this.query_string = new System.Collections.Specialized.NameValueCollection();
			if (query == null || query.Length == 0)
			{
				return;
			}
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
					this.query_string.Add(null, HttpUtility.UrlDecode(text));
				}
				else
				{
					string name = HttpUtility.UrlDecode(text.Substring(0, num));
					string val = HttpUtility.UrlDecode(text.Substring(num + 1));
					this.query_string.Add(name, val);
				}
			}
		}

		internal void FinishInitialization()
		{
			string text = this.UserHostName;
			if (this.version > HttpVersion.Version10 && (text == null || text.Length == 0))
			{
				this.context.ErrorMessage = "Invalid host name";
				return;
			}
			System.Uri uri;
			string pathAndQuery;
			if (System.Uri.MaybeUri(this.raw_url) && System.Uri.TryCreate(this.raw_url, System.UriKind.Absolute, out uri))
			{
				pathAndQuery = uri.PathAndQuery;
			}
			else
			{
				pathAndQuery = this.raw_url;
			}
			if (text == null || text.Length == 0)
			{
				text = this.UserHostAddress;
			}
			if (uri != null)
			{
				text = uri.Host;
			}
			int num = text.IndexOf(':');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			string text2 = string.Format("{0}://{1}:{2}", (!this.IsSecureConnection) ? "http" : "https", text, this.LocalEndPoint.Port);
			if (!System.Uri.TryCreate(text2 + pathAndQuery, System.UriKind.Absolute, out this.url))
			{
				this.context.ErrorMessage = "Invalid url: " + text2 + pathAndQuery;
				return;
			}
			this.CreateQueryString(this.url.Query);
			string text3 = null;
			if (this.version >= HttpVersion.Version11)
			{
				text3 = this.Headers["Transfer-Encoding"];
				if (text3 != null && text3 != "chunked")
				{
					this.context.Connection.SendError(null, 501);
					return;
				}
			}
			this.is_chunked = (text3 == "chunked");
			foreach (string strB in HttpListenerRequest.no_body_methods)
			{
				if (string.Compare(this.method, strB, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return;
				}
			}
			if (!this.is_chunked && !this.cl_set)
			{
				this.context.Connection.SendError(null, 411);
				return;
			}
			if (this.is_chunked || this.content_length > 0L)
			{
				this.input_stream = this.context.Connection.GetRequestStream(this.is_chunked, this.content_length);
			}
			if (this.Headers["Expect"] == "100-continue")
			{
				ResponseStream responseStream = this.context.Connection.GetResponseStream();
				responseStream.InternalWrite(HttpListenerRequest._100continue, 0, HttpListenerRequest._100continue.Length);
			}
		}

		internal static string Unquote(string str)
		{
			int num = str.IndexOf('"');
			int num2 = str.LastIndexOf('"');
			if (num >= 0 && num2 >= 0)
			{
				str = str.Substring(num + 1, num2 - 1);
			}
			return str.Trim();
		}

		internal void AddHeader(string header)
		{
			int num = header.IndexOf(':');
			if (num == -1 || num == 0)
			{
				this.context.ErrorMessage = "Bad Request";
				this.context.ErrorStatus = 400;
				return;
			}
			string text = header.Substring(0, num).Trim();
			string text2 = header.Substring(num + 1).Trim();
			string text3 = text.ToLower(CultureInfo.InvariantCulture);
			this.headers.SetInternal(text, text2);
			string text4 = text3;
			switch (text4)
			{
			case "accept-language":
				this.user_languages = text2.Split(new char[]
				{
					','
				});
				break;
			case "accept":
				this.accept_types = text2.Split(new char[]
				{
					','
				});
				break;
			case "content-length":
				try
				{
					this.content_length = long.Parse(text2.Trim());
					if (this.content_length < 0L)
					{
						this.context.ErrorMessage = "Invalid Content-Length.";
					}
					this.cl_set = true;
				}
				catch
				{
					this.context.ErrorMessage = "Invalid Content-Length.";
				}
				break;
			case "referer":
				try
				{
					this.referrer = new System.Uri(text2);
				}
				catch
				{
					this.referrer = new System.Uri("http://someone.is.screwing.with.the.headers.com/");
				}
				break;
			case "cookie":
			{
				if (this.cookies == null)
				{
					this.cookies = new CookieCollection();
				}
				string[] array = text2.Split(new char[]
				{
					',',
					';'
				});
				Cookie cookie = null;
				int num3 = 0;
				foreach (string text5 in array)
				{
					string text6 = text5.Trim();
					if (text6.Length != 0)
					{
						if (text6.StartsWith("$Version"))
						{
							num3 = int.Parse(HttpListenerRequest.Unquote(text6.Substring(text6.IndexOf("=") + 1)));
						}
						else if (text6.StartsWith("$Path"))
						{
							if (cookie != null)
							{
								cookie.Path = text6.Substring(text6.IndexOf("=") + 1).Trim();
							}
						}
						else if (text6.StartsWith("$Domain"))
						{
							if (cookie != null)
							{
								cookie.Domain = text6.Substring(text6.IndexOf("=") + 1).Trim();
							}
						}
						else if (text6.StartsWith("$Port"))
						{
							if (cookie != null)
							{
								cookie.Port = text6.Substring(text6.IndexOf("=") + 1).Trim();
							}
						}
						else
						{
							if (cookie != null)
							{
								this.cookies.Add(cookie);
							}
							cookie = new Cookie();
							int num4 = text6.IndexOf("=");
							if (num4 > 0)
							{
								cookie.Name = text6.Substring(0, num4).Trim();
								cookie.Value = text6.Substring(num4 + 1).Trim();
							}
							else
							{
								cookie.Name = text6.Trim();
								cookie.Value = string.Empty;
							}
							cookie.Version = num3;
						}
					}
				}
				if (cookie != null)
				{
					this.cookies.Add(cookie);
				}
				break;
			}
			}
		}

		internal bool FlushInput()
		{
			if (!this.HasEntityBody)
			{
				return true;
			}
			int num = 2048;
			if (this.content_length > 0L)
			{
				num = (int)Math.Min(this.content_length, (long)num);
			}
			byte[] buffer = new byte[num];
			bool result;
			for (;;)
			{
				try
				{
					if (this.InputStream.Read(buffer, 0, num) <= 0)
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

		/// <summary>Gets the MIME types accepted by the client. </summary>
		/// <returns>A <see cref="T:System.String" /> array that contains the type names specified in the request's Accept header or null if the client request did not include an Accept header.</returns>
		public string[] AcceptTypes
		{
			get
			{
				return this.accept_types;
			}
		}

		/// <summary>Gets an error code that identifies a problem with the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> provided by the client.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains a Windows error code.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.KeyContainerPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Create" />
		/// </PermissionSet>
		[MonoTODO("Always returns 0")]
		public int ClientCertificateError
		{
			get
			{
				return 0;
			}
		}

		/// <summary>Gets the content encoding that can be used with data sent with the request</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> object suitable for use with the data in the <see cref="P:System.Net.HttpListenerRequest.InputStream" /> property.</returns>
		public Encoding ContentEncoding
		{
			get
			{
				if (this.content_encoding == null)
				{
					this.content_encoding = Encoding.Default;
				}
				return this.content_encoding;
			}
		}

		/// <summary>Gets the length of the body data included in the request.</summary>
		/// <returns>The value from the request's Content-Length header. This value is -1 if the content length is not known.</returns>
		public long ContentLength64
		{
			get
			{
				return this.content_length;
			}
		}

		/// <summary>Gets the MIME type of the body data included in the request.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the text of the request's Content-Type header.</returns>
		public string ContentType
		{
			get
			{
				return this.headers["content-type"];
			}
		}

		/// <summary>Gets the cookies sent with the request.</summary>
		/// <returns>A <see cref="T:System.Net.CookieCollection" /> that contains cookies that accompany the request. This property returns an empty collection if the request does not contain cookies.</returns>
		public CookieCollection Cookies
		{
			get
			{
				if (this.cookies == null)
				{
					this.cookies = new CookieCollection();
				}
				return this.cookies;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the request has associated body data.</summary>
		/// <returns>true if the request has associated body data; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public bool HasEntityBody
		{
			get
			{
				return this.content_length > 0L || this.is_chunked;
			}
		}

		/// <summary>Gets the collection of header name/value pairs sent in the request.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the HTTP headers included in the request.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public System.Collections.Specialized.NameValueCollection Headers
		{
			get
			{
				return this.headers;
			}
		}

		/// <summary>Gets the HTTP method specified by the client. </summary>
		/// <returns>A <see cref="T:System.String" /> that contains the method used in the request.</returns>
		public string HttpMethod
		{
			get
			{
				return this.method;
			}
		}

		/// <summary>Gets a stream that contains the body data sent by the client.</summary>
		/// <returns>A readable <see cref="T:System.IO.Stream" /> object that contains the bytes sent by the client in the body of the request. This property returns <see cref="F:System.IO.Stream.Null" /> if no data is sent with the request.</returns>
		public Stream InputStream
		{
			get
			{
				return this.input_stream;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the client sending this request is authenticated.</summary>
		/// <returns>true if the client was authenticated; otherwise, false.</returns>
		[MonoTODO("Always returns false")]
		public bool IsAuthenticated
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the request is sent from the local computer.</summary>
		/// <returns>true if the request originated on the same computer as the <see cref="T:System.Net.HttpListener" /> object that provided the request; otherwise, false.</returns>
		public bool IsLocal
		{
			get
			{
				return IPAddress.IsLoopback(this.RemoteEndPoint.Address);
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the TCP connection used to send the request is using the Secure Sockets Layer (SSL) protocol.</summary>
		/// <returns>true if the TCP connection is using SSL; otherwise, false.</returns>
		public bool IsSecureConnection
		{
			get
			{
				return this.context.Connection.IsSecure;
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the client requests a persistent connection.</summary>
		/// <returns>true if the connection should be kept open; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public bool KeepAlive
		{
			get
			{
				return false;
			}
		}

		/// <summary>Get the server IP address and port number to which the request is directed.</summary>
		/// <returns>An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address that the request is sent to.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IPEndPoint LocalEndPoint
		{
			get
			{
				return this.context.Connection.LocalEndPoint;
			}
		}

		/// <summary>Gets the HTTP version used by the requesting client.</summary>
		/// <returns>A <see cref="T:System.Version" /> that identifies the client's version of HTTP.</returns>
		public Version ProtocolVersion
		{
			get
			{
				return this.version;
			}
		}

		/// <summary>Gets the query string included in the request.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> object that contains the query data included in the request <see cref="P:System.Net.HttpListenerRequest.Url" />.</returns>
		public System.Collections.Specialized.NameValueCollection QueryString
		{
			get
			{
				return this.query_string;
			}
		}

		/// <summary>Gets the URL information (without the host and port) requested by the client.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the raw URL for this request.</returns>
		public string RawUrl
		{
			get
			{
				return this.raw_url;
			}
		}

		/// <summary>Gets the client IP address and port number from which the request originated.</summary>
		/// <returns>An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address and port number from which the request originated.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.context.Connection.RemoteEndPoint;
			}
		}

		/// <summary>Gets the request identifier of the incoming HTTP request.</summary>
		/// <returns>A <see cref="T:System.Guid" /> object that contains the identifier of the HTTP request.</returns>
		public Guid RequestTraceIdentifier
		{
			get
			{
				return this.identifier;
			}
		}

		/// <summary>Gets the <see cref="T:System.Uri" /> object requested by the client.</summary>
		/// <returns>A <see cref="T:System.Uri" /> object that identifies the resource requested by the client.</returns>
		public System.Uri Url
		{
			get
			{
				return this.url;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the resource that referred the client to the server.</summary>
		/// <returns>A <see cref="T:System.Uri" /> object that contains the text of the request's <see cref="F:System.Net.HttpRequestHeader.Referer" /> header, or null if the header was not included in the request.</returns>
		public System.Uri UrlReferrer
		{
			get
			{
				return this.referrer;
			}
		}

		/// <summary>Gets the user agent presented by the client.</summary>
		/// <returns>A <see cref="T:System.String" /> object that contains the text of the request's User-Agent header.</returns>
		public string UserAgent
		{
			get
			{
				return this.headers["user-agent"];
			}
		}

		/// <summary>Gets the server IP address and port number to which the request is directed.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the host address information.</returns>
		public string UserHostAddress
		{
			get
			{
				return this.LocalEndPoint.ToString();
			}
		}

		/// <summary>Gets the DNS name and, if provided, the port number specified by the client.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the text of the request's Host header.</returns>
		public string UserHostName
		{
			get
			{
				return this.headers["host"];
			}
		}

		/// <summary>Gets the natural languages that are preferred for the response.</summary>
		/// <returns>A <see cref="T:System.String" /> array that contains the languages specified in the request's <see cref="F:System.Net.HttpRequestHeader.AcceptLanguage" /> header or null if the client request did not include an <see cref="F:System.Net.HttpRequestHeader.AcceptLanguage" /> header.</returns>
		public string[] UserLanguages
		{
			get
			{
				return this.user_languages;
			}
		}

		/// <summary>Begins an asynchronous request for the client's X.509 v.3 certificate.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that indicates the status of the operation.</returns>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the callback delegate when the operation completes.</param>
		public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
		{
			return null;
		}

		/// <summary>Ends an asynchronous request for the client's X.509 v.3 certificate.</summary>
		/// <returns>The <see cref="T:System.IAsyncResult" /> object that is returned when the operation started.</returns>
		/// <param name="asyncResult">The pending request for the certificate.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not obtained by calling <see cref="M:System.Net.HttpListenerRequest.BeginGetClientCertificate(System.AsyncCallback,System.Object)" /><paramref name="e." /></exception>
		/// <exception cref="T:System.InvalidOperationException">This method was already called for the operation identified by <paramref name="asyncResult" />. </exception>
		public System.Security.Cryptography.X509Certificates.X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
		{
			return null;
		}

		/// <summary>Retrieves the client's X.509 v.3 certificate.</summary>
		/// <returns>A <see cref="N:System.Security.Cryptography.X509Certificates" /> object that contains the client's X.509 v.3 certificate.</returns>
		/// <exception cref="T:System.InvalidOperationException">A call to this method to retrieve the client's X.509 v.3 certificate is in progress and therefore another call to this method cannot be made.</exception>
		public System.Security.Cryptography.X509Certificates.X509Certificate2 GetClientCertificate()
		{
			return null;
		}
	}
}
