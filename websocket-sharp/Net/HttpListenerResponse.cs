using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace WebSocketSharp.Net
{
	public sealed class HttpListenerResponse : IDisposable
	{
		private bool _chunked;

		private Encoding _contentEncoding;

		private long _contentLength;

		private bool _contentLengthSet;

		private string _contentType;

		private HttpListenerContext _context;

		private CookieCollection _cookies;

		private bool _disposed;

		private bool _forceCloseChunked;

		private WebHeaderCollection _headers;

		private bool _keepAlive;

		private string _location;

		private ResponseStream _outputStream;

		private int _statusCode;

		private string _statusDescription;

		private Version _version;

		internal bool HeadersSent;

		internal HttpListenerResponse(HttpListenerContext context)
		{
			this._context = context;
			this._headers = new WebHeaderCollection();
			this._keepAlive = true;
			this._statusCode = 200;
			this._statusDescription = "OK";
			this._version = HttpVersion.Version11;
		}

		void IDisposable.Dispose()
		{
			this.close(true);
		}

		internal bool ForceCloseChunked
		{
			get
			{
				return this._forceCloseChunked;
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
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				this._contentEncoding = value;
			}
		}

		public long ContentLength64
		{
			get
			{
				return this._contentLength;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("Must not be less than zero.", "value");
				}
				this._contentLengthSet = true;
				this._contentLength = value;
			}
		}

		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException("Must not be empty.", "value");
				}
				this._contentType = value;
			}
		}

		public CookieCollection Cookies
		{
			get
			{
				CookieCollection result;
				if ((result = this._cookies) == null)
				{
					result = (this._cookies = new CookieCollection());
				}
				return result;
			}
			set
			{
				this._cookies = value;
			}
		}

		public WebHeaderCollection Headers
		{
			get
			{
				return this._headers;
			}
			set
			{
				this._headers = value;
			}
		}

		public bool KeepAlive
		{
			get
			{
				return this._keepAlive;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				this._keepAlive = value;
			}
		}

		public Stream OutputStream
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				ResponseStream result;
				if ((result = this._outputStream) == null)
				{
					result = (this._outputStream = this._context.Connection.GetResponseStream());
				}
				return result;
			}
		}

		public Version ProtocolVersion
		{
			get
			{
				return this._version;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Major != 1 || (value.Minor != 0 && value.Minor != 1))
				{
					throw new ArgumentException("Must be 1.0 or 1.1.", "value");
				}
				this._version = value;
			}
		}

		public string RedirectLocation
		{
			get
			{
				return this._location;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException("Must not be empty.", "value");
				}
				this._location = value;
			}
		}

		public bool SendChunked
		{
			get
			{
				return this._chunked;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				this._chunked = value;
			}
		}

		public int StatusCode
		{
			get
			{
				return this._statusCode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException(base.GetType().ToString());
				}
				if (this.HeadersSent)
				{
					throw new InvalidOperationException("Cannot be changed after headers are sent.");
				}
				if (value < 100 || value > 999)
				{
					throw new ProtocolViolationException("StatusCode must be between 100 and 999.");
				}
				this._statusCode = value;
				this._statusDescription = value.GetStatusDescription();
			}
		}

		public string StatusDescription
		{
			get
			{
				return this._statusDescription;
			}
			set
			{
				this._statusDescription = ((value != null && value.Length != 0) ? value : this._statusCode.GetStatusDescription());
			}
		}

		private bool canAddOrUpdate(Cookie cookie)
		{
			if (this.Cookies.Count == 0)
			{
				return true;
			}
			IEnumerable<Cookie> enumerable = this.findCookie(cookie);
			if (enumerable.Count<Cookie>() == 0)
			{
				return true;
			}
			foreach (Cookie cookie2 in enumerable)
			{
				if (cookie2.Version == cookie.Version)
				{
					return true;
				}
			}
			return false;
		}

		private void close(bool force)
		{
			this._disposed = true;
			this._context.Connection.Close(force);
		}

		private IEnumerable<Cookie> findCookie(Cookie cookie)
		{
			string name = cookie.Name;
			string domain = cookie.Domain;
			string path = cookie.Path;
			return this.Cookies.Cast<Cookie>().Where((Cookie c) => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase) && c.Path.Equals(path, StringComparison.Ordinal));
		}

		internal void SendHeaders(bool closing, MemoryStream stream)
		{
			if (this._contentType != null)
			{
				if (this._contentEncoding != null && this._contentType.IndexOf("charset=", StringComparison.Ordinal) == -1)
				{
					string webName = this._contentEncoding.WebName;
					this._headers.SetInternal("Content-Type", this._contentType + "; charset=" + webName, true);
				}
				else
				{
					this._headers.SetInternal("Content-Type", this._contentType, true);
				}
			}
			if (this._headers["Server"] == null)
			{
				this._headers.SetInternal("Server", "websocket-sharp/1.0", true);
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			if (this._headers["Date"] == null)
			{
				this._headers.SetInternal("Date", DateTime.UtcNow.ToString("r", invariantCulture), true);
			}
			if (!this._chunked)
			{
				if (!this._contentLengthSet && closing)
				{
					this._contentLengthSet = true;
					this._contentLength = 0L;
				}
				if (this._contentLengthSet)
				{
					this._headers.SetInternal("Content-Length", this._contentLength.ToString(invariantCulture), true);
				}
			}
			Version protocolVersion = this._context.Request.ProtocolVersion;
			if (!this._contentLengthSet && !this._chunked && protocolVersion >= HttpVersion.Version11)
			{
				this._chunked = true;
			}
			bool flag = this._statusCode == 400 || this._statusCode == 408 || this._statusCode == 411 || this._statusCode == 413 || this._statusCode == 414 || this._statusCode == 500 || this._statusCode == 503;
			if (!flag)
			{
				flag = !this._context.Request.KeepAlive;
			}
			if (!this._keepAlive || flag)
			{
				this._headers.SetInternal("Connection", "close", true);
				flag = true;
			}
			if (this._chunked)
			{
				this._headers.SetInternal("Transfer-Encoding", "chunked", true);
			}
			int reuses = this._context.Connection.Reuses;
			if (reuses >= 100)
			{
				this._forceCloseChunked = true;
				if (!flag)
				{
					this._headers.SetInternal("Connection", "close", true);
					flag = true;
				}
			}
			if (!flag)
			{
				this._headers.SetInternal("Keep-Alive", string.Format("timeout=15,max={0}", 100 - reuses), true);
				if (this._context.Request.ProtocolVersion <= HttpVersion.Version10)
				{
					this._headers.SetInternal("Connection", "keep-alive", true);
				}
			}
			if (this._location != null)
			{
				this._headers.SetInternal("Location", this._location, true);
			}
			if (this._cookies != null)
			{
				IEnumerator enumerator = this._cookies.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Cookie cookie = (Cookie)obj;
						this._headers.SetInternal("Set-Cookie", cookie.ToResponseString(), true);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			Encoding encoding = this._contentEncoding ?? Encoding.Default;
			StreamWriter streamWriter = new StreamWriter(stream, encoding, 256);
			streamWriter.Write("HTTP/{0} {1} {2}\r\n", this._version, this._statusCode, this._statusDescription);
			string value = this._headers.ToStringMultiValue(true);
			streamWriter.Write(value);
			streamWriter.Flush();
			int num = (encoding.CodePage != 65001) ? encoding.GetPreamble().Length : 3;
			if (this._outputStream == null)
			{
				this._outputStream = this._context.Connection.GetResponseStream();
			}
			stream.Position = (long)num;
			this.HeadersSent = true;
		}

		public void Abort()
		{
			if (this._disposed)
			{
				return;
			}
			this.close(true);
		}

		public void AddHeader(string name, string value)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentNullException("name");
			}
			if (value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException("value", "Greater than 65,535 characters.");
			}
			this._headers.Set(name, value);
		}

		public void AppendCookie(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			this.Cookies.Add(cookie);
		}

		public void AppendHeader(string name, string value)
		{
			if (name == null || name.Length == 0)
			{
				throw new ArgumentException("Must not be null or empty.", "name");
			}
			if (value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException("value", "Greater than 65,535 characters.");
			}
			this._headers.Add(name, value);
		}

		public void Close()
		{
			if (this._disposed)
			{
				return;
			}
			this.close(false);
		}

		public void Close(byte[] responseEntity, bool willBlock)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			if (responseEntity == null)
			{
				throw new ArgumentNullException("responseEntity");
			}
			this.ContentLength64 = (long)responseEntity.Length;
			this.OutputStream.Write(responseEntity, 0, (int)this._contentLength);
			this.close(false);
		}

		public void CopyFrom(HttpListenerResponse templateResponse)
		{
			this._headers.Clear();
			this._headers.Add(templateResponse._headers);
			this._contentLength = templateResponse._contentLength;
			this._statusCode = templateResponse._statusCode;
			this._statusDescription = templateResponse._statusDescription;
			this._keepAlive = templateResponse._keepAlive;
			this._version = templateResponse._version;
		}

		public void Redirect(string url)
		{
			this.StatusCode = 302;
			this._location = url;
		}

		public void SetCookie(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (!this.canAddOrUpdate(cookie))
			{
				throw new ArgumentException("Cannot be replaced.", "cookie");
			}
			this.Cookies.Add(cookie);
		}
	}
}
