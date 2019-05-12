using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace System.Net
{
	/// <summary>Provides an HTTP-specific implementation of the <see cref="T:System.Net.WebResponse" /> class.</summary>
	[Serializable]
	public class HttpWebResponse : WebResponse, IDisposable, ISerializable
	{
		private System.Uri uri;

		private WebHeaderCollection webHeaders;

		private CookieCollection cookieCollection;

		private string method;

		private Version version;

		private HttpStatusCode statusCode;

		private string statusDescription;

		private long contentLength;

		private string contentType;

		private CookieContainer cookie_container;

		private bool disposed;

		private Stream stream;

		private string[] cookieExpiresFormats = new string[]
		{
			"r",
			"ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'",
			"ddd, dd'-'MMM'-'yy HH':'mm':'ss 'GMT'"
		};

		internal HttpWebResponse(System.Uri uri, string method, WebConnectionData data, CookieContainer container)
		{
			this.uri = uri;
			this.method = method;
			this.webHeaders = data.Headers;
			this.version = data.Version;
			this.statusCode = (HttpStatusCode)data.StatusCode;
			this.statusDescription = data.StatusDescription;
			this.stream = data.stream;
			this.contentLength = -1L;
			try
			{
				string text = this.webHeaders["Content-Length"];
				if (string.IsNullOrEmpty(text) || !long.TryParse(text, out this.contentLength))
				{
					this.contentLength = -1L;
				}
			}
			catch (Exception)
			{
				this.contentLength = -1L;
			}
			if (container != null)
			{
				this.cookie_container = container;
				this.FillCookies();
			}
			string a = this.webHeaders["Content-Encoding"];
			if (a == "gzip" && (data.request.AutomaticDecompression & DecompressionMethods.GZip) != DecompressionMethods.None)
			{
				this.stream = new System.IO.Compression.GZipStream(this.stream, System.IO.Compression.CompressionMode.Decompress);
			}
			else if (a == "deflate" && (data.request.AutomaticDecompression & DecompressionMethods.Deflate) != DecompressionMethods.None)
			{
				this.stream = new System.IO.Compression.DeflateStream(this.stream, System.IO.Compression.CompressionMode.Decompress);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpWebResponse" /> class from the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> instances.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that contains the information required to serialize the new <see cref="T:System.Net.HttpWebRequest" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.HttpWebRequest" />. </param>
		[Obsolete("Serialization is obsoleted for this type", false)]
		protected HttpWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.uri = (System.Uri)serializationInfo.GetValue("uri", typeof(System.Uri));
			this.contentLength = serializationInfo.GetInt64("contentLength");
			this.contentType = serializationInfo.GetString("contentType");
			this.method = serializationInfo.GetString("method");
			this.statusDescription = serializationInfo.GetString("statusDescription");
			this.cookieCollection = (CookieCollection)serializationInfo.GetValue("cookieCollection", typeof(CookieCollection));
			this.version = (Version)serializationInfo.GetValue("version", typeof(Version));
			this.statusCode = (HttpStatusCode)((int)serializationInfo.GetValue("statusCode", typeof(HttpStatusCode)));
		}

		/// <summary>Serializes this instance into the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object.</summary>
		/// <param name="serializationInfo">The object into which this <see cref="T:System.Net.HttpWebResponse" /> will be serialized. </param>
		/// <param name="streamingContext">The destination of the serialization. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.HttpWebResponse" />.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets the character set of the response.</summary>
		/// <returns>A string that contains the character set of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public string CharacterSet
		{
			get
			{
				string text = this.ContentType;
				if (text == null)
				{
					return "ISO-8859-1";
				}
				string text2 = text.ToLower();
				int num = text2.IndexOf("charset=");
				if (num == -1)
				{
					return "ISO-8859-1";
				}
				num += 8;
				int num2 = text2.IndexOf(';', num);
				return (num2 != -1) ? text.Substring(num, num2 - num) : text.Substring(num);
			}
		}

		/// <summary>Gets the method that is used to encode the body of the response.</summary>
		/// <returns>A string that describes the method that is used to encode the body of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string ContentEncoding
		{
			get
			{
				this.CheckDisposed();
				string text = this.webHeaders["Content-Encoding"];
				return (text == null) ? string.Empty : text;
			}
		}

		/// <summary>Gets the length of the content returned by the request.</summary>
		/// <returns>The number of bytes returned by the request. Content length does not include header information.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override long ContentLength
		{
			get
			{
				return this.contentLength;
			}
		}

		/// <summary>Gets the content type of the response.</summary>
		/// <returns>A string that contains the content type of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override string ContentType
		{
			get
			{
				this.CheckDisposed();
				if (this.contentType == null)
				{
					this.contentType = this.webHeaders["Content-Type"];
				}
				return this.contentType;
			}
		}

		/// <summary>Gets or sets the cookies that are associated with this response.</summary>
		/// <returns>A <see cref="T:System.Net.CookieCollection" /> that contains the cookies that are associated with this response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public CookieCollection Cookies
		{
			get
			{
				this.CheckDisposed();
				if (this.cookieCollection == null)
				{
					this.cookieCollection = new CookieCollection();
				}
				return this.cookieCollection;
			}
			set
			{
				this.CheckDisposed();
				this.cookieCollection = value;
			}
		}

		/// <summary>Gets the headers that are associated with this response from the server.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the header information returned with the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.webHeaders;
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether both client and server were authenticated.</summary>
		/// <returns>true if mutual authentication occurred; otherwise, false.</returns>
		[MonoTODO]
		public override bool IsMutuallyAuthenticated
		{
			get
			{
				throw HttpWebResponse.GetMustImplement();
			}
		}

		/// <summary>Gets the last date and time that the contents of the response were modified.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the date and time that the contents of the response were modified.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public DateTime LastModified
		{
			get
			{
				this.CheckDisposed();
				DateTime result;
				try
				{
					string dateStr = this.webHeaders["Last-Modified"];
					result = MonoHttpDate.Parse(dateStr);
				}
				catch (Exception)
				{
					result = DateTime.Now;
				}
				return result;
			}
		}

		/// <summary>Gets the method that is used to return the response.</summary>
		/// <returns>A string that contains the HTTP method that is used to return the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string Method
		{
			get
			{
				this.CheckDisposed();
				return this.method;
			}
		}

		/// <summary>Gets the version of the HTTP protocol that is used in the response.</summary>
		/// <returns>A <see cref="T:System.Version" /> that contains the HTTP protocol version of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public Version ProtocolVersion
		{
			get
			{
				this.CheckDisposed();
				return this.version;
			}
		}

		/// <summary>Gets the URI of the Internet resource that responded to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the Internet resource that responded to the request.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public override System.Uri ResponseUri
		{
			get
			{
				this.CheckDisposed();
				return this.uri;
			}
		}

		/// <summary>Gets the name of the server that sent the response.</summary>
		/// <returns>A string that contains the name of the server that sent the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string Server
		{
			get
			{
				this.CheckDisposed();
				return this.webHeaders["Server"];
			}
		}

		/// <summary>Gets the status of the response.</summary>
		/// <returns>One of the <see cref="T:System.Net.HttpStatusCode" /> values.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public HttpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
		}

		/// <summary>Gets the status description returned with the response.</summary>
		/// <returns>A string that describes the status of the response.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string StatusDescription
		{
			get
			{
				this.CheckDisposed();
				return this.statusDescription;
			}
		}

		/// <summary>Gets the contents of a header that was returned with the response.</summary>
		/// <returns>The contents of the specified header.</returns>
		/// <param name="headerName">The header value to return. </param>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		public string GetResponseHeader(string headerName)
		{
			this.CheckDisposed();
			string text = this.webHeaders[headerName];
			return (text == null) ? string.Empty : text;
		}

		internal void ReadAll()
		{
			WebConnectionStream webConnectionStream = this.stream as WebConnectionStream;
			if (webConnectionStream == null)
			{
				return;
			}
			try
			{
				webConnectionStream.ReadAll();
			}
			catch
			{
			}
		}

		/// <summary>Gets the stream that is used to read the body of the response from the server.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> containing the body of the response.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">There is no response stream. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The current instance has been disposed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override Stream GetResponseStream()
		{
			this.CheckDisposed();
			if (this.stream == null)
			{
				return Stream.Null;
			}
			if (string.Compare(this.method, "HEAD", true) == 0)
			{
				return Stream.Null;
			}
			return this.stream;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("uri", this.uri);
			serializationInfo.AddValue("contentLength", this.contentLength);
			serializationInfo.AddValue("contentType", this.contentType);
			serializationInfo.AddValue("method", this.method);
			serializationInfo.AddValue("statusDescription", this.statusDescription);
			serializationInfo.AddValue("cookieCollection", this.cookieCollection);
			serializationInfo.AddValue("version", this.version);
			serializationInfo.AddValue("statusCode", this.statusCode);
		}

		/// <summary>Closes the response stream.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Close()
		{
			((IDisposable)this).Dispose();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.HttpWebResponse" />, and optionally disposes of the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to releases only unmanaged resources. </param>
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (disposing)
			{
				this.uri = null;
				this.cookieCollection = null;
				this.method = null;
				this.version = null;
				this.statusDescription = null;
			}
			Stream stream = this.stream;
			this.stream = null;
			if (stream != null)
			{
				stream.Close();
			}
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		private void FillCookies()
		{
			if (this.webHeaders == null)
			{
				return;
			}
			string[] values = this.webHeaders.GetValues("Set-Cookie");
			if (values != null)
			{
				foreach (string cookie in values)
				{
					this.SetCookie(cookie);
				}
			}
			values = this.webHeaders.GetValues("Set-Cookie2");
			if (values != null)
			{
				foreach (string cookie2 in values)
				{
					this.SetCookie2(cookie2);
				}
			}
		}

		private void SetCookie(string header)
		{
			Cookie cookie = null;
			CookieParser cookieParser = new CookieParser(header);
			string text;
			string text2;
			while (cookieParser.GetNextNameValue(out text, out text2))
			{
				if ((text != null && !(text == string.Empty)) || cookie != null)
				{
					if (cookie == null)
					{
						cookie = new Cookie(text, text2);
					}
					else
					{
						text = text.ToUpper();
						string text3 = text;
						switch (text3)
						{
						case "COMMENT":
							if (cookie.Comment == null)
							{
								cookie.Comment = text2;
							}
							break;
						case "COMMENTURL":
							if (cookie.CommentUri == null)
							{
								cookie.CommentUri = new System.Uri(text2);
							}
							break;
						case "DISCARD":
							cookie.Discard = true;
							break;
						case "DOMAIN":
							if (cookie.Domain == string.Empty)
							{
								cookie.Domain = text2;
							}
							break;
						case "HTTPONLY":
							cookie.HttpOnly = true;
							break;
						case "MAX-AGE":
							if (cookie.Expires == DateTime.MinValue)
							{
								try
								{
									cookie.Expires = cookie.TimeStamp.AddSeconds(uint.Parse(text2));
								}
								catch
								{
								}
							}
							break;
						case "EXPIRES":
							if (!(cookie.Expires != DateTime.MinValue))
							{
								cookie.Expires = this.TryParseCookieExpires(text2);
							}
							break;
						case "PATH":
							cookie.Path = text2;
							break;
						case "PORT":
							if (cookie.Port == null)
							{
								cookie.Port = text2;
							}
							break;
						case "SECURE":
							cookie.Secure = true;
							break;
						case "VERSION":
							try
							{
								cookie.Version = (int)uint.Parse(text2);
							}
							catch
							{
							}
							break;
						}
					}
				}
			}
			if (cookie == null)
			{
				return;
			}
			if (this.cookieCollection == null)
			{
				this.cookieCollection = new CookieCollection();
			}
			if (cookie.Domain == string.Empty)
			{
				cookie.Domain = this.uri.Host;
			}
			this.cookieCollection.Add(cookie);
			if (this.cookie_container != null)
			{
				this.cookie_container.Add(this.uri, cookie);
			}
		}

		private void SetCookie2(string cookies_str)
		{
			string[] array = cookies_str.Split(new char[]
			{
				','
			});
			foreach (string cookie in array)
			{
				this.SetCookie(cookie);
			}
		}

		private DateTime TryParseCookieExpires(string value)
		{
			if (value == null || value.Length == 0)
			{
				return DateTime.MinValue;
			}
			for (int i = 0; i < this.cookieExpiresFormats.Length; i++)
			{
				try
				{
					DateTime dateTime = DateTime.ParseExact(value, this.cookieExpiresFormats[i], CultureInfo.InvariantCulture);
					dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
					return TimeZone.CurrentTimeZone.ToLocalTime(dateTime);
				}
				catch
				{
				}
			}
			return DateTime.MinValue;
		}
	}
}
