using System;
using System.IO;
using System.Net.Cache;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net
{
	/// <summary>Provides an HTTP-specific implementation of the <see cref="T:System.Net.WebRequest" /> class.</summary>
	[Serializable]
	public class HttpWebRequest : WebRequest, ISerializable
	{
		private System.Uri requestUri;

		private System.Uri actualUri;

		private bool hostChanged;

		private bool allowAutoRedirect = true;

		private bool allowBuffering = true;

		private System.Security.Cryptography.X509Certificates.X509CertificateCollection certificates;

		private string connectionGroup;

		private long contentLength = -1L;

		private HttpContinueDelegate continueDelegate;

		private CookieContainer cookieContainer;

		private ICredentials credentials;

		private bool haveResponse;

		private bool haveRequest;

		private bool requestSent;

		private WebHeaderCollection webHeaders = new WebHeaderCollection(true);

		private bool keepAlive = true;

		private int maxAutoRedirect = 50;

		private string mediaType = string.Empty;

		private string method = "GET";

		private string initialMethod = "GET";

		private bool pipelined = true;

		private bool preAuthenticate;

		private bool usedPreAuth;

		private Version version = HttpVersion.Version11;

		private Version actualVersion;

		private IWebProxy proxy;

		private bool sendChunked;

		private ServicePoint servicePoint;

		private int timeout = 100000;

		private WebConnectionStream writeStream;

		private HttpWebResponse webResponse;

		private WebAsyncResult asyncWrite;

		private WebAsyncResult asyncRead;

		private EventHandler abortHandler;

		private int aborted;

		private bool gotRequestStream;

		private int redirects;

		private bool expectContinue;

		private bool authCompleted;

		private byte[] bodyBuffer;

		private int bodyBufferLength;

		private bool getResponseCalled;

		private Exception saved_exc;

		private object locker = new object();

		private bool is_ntlm_auth;

		private bool finished_reading;

		internal WebConnection WebConnection;

		private DecompressionMethods auto_decomp;

		private int maxResponseHeadersLength;

		private static int defaultMaxResponseHeadersLength = 65536;

		private int readWriteTimeout = 300000;

		private bool unsafe_auth_blah;

		public HttpWebRequest(System.Uri uri)
		{
			this.requestUri = uri;
			this.actualUri = uri;
			this.proxy = GlobalProxySelection.Select;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpWebRequest" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the new <see cref="T:System.Net.HttpWebRequest" /> object. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Net.HttpWebRequest" /> object. </param>
		[Obsolete("Serialization is obsoleted for this type", false)]
		protected HttpWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.requestUri = (System.Uri)serializationInfo.GetValue("requestUri", typeof(System.Uri));
			this.actualUri = (System.Uri)serializationInfo.GetValue("actualUri", typeof(System.Uri));
			this.allowAutoRedirect = serializationInfo.GetBoolean("allowAutoRedirect");
			this.allowBuffering = serializationInfo.GetBoolean("allowBuffering");
			this.certificates = (System.Security.Cryptography.X509Certificates.X509CertificateCollection)serializationInfo.GetValue("certificates", typeof(System.Security.Cryptography.X509Certificates.X509CertificateCollection));
			this.connectionGroup = serializationInfo.GetString("connectionGroup");
			this.contentLength = serializationInfo.GetInt64("contentLength");
			this.webHeaders = (WebHeaderCollection)serializationInfo.GetValue("webHeaders", typeof(WebHeaderCollection));
			this.keepAlive = serializationInfo.GetBoolean("keepAlive");
			this.maxAutoRedirect = serializationInfo.GetInt32("maxAutoRedirect");
			this.mediaType = serializationInfo.GetString("mediaType");
			this.method = serializationInfo.GetString("method");
			this.initialMethod = serializationInfo.GetString("initialMethod");
			this.pipelined = serializationInfo.GetBoolean("pipelined");
			this.version = (Version)serializationInfo.GetValue("version", typeof(Version));
			this.proxy = (IWebProxy)serializationInfo.GetValue("proxy", typeof(IWebProxy));
			this.sendChunked = serializationInfo.GetBoolean("sendChunked");
			this.timeout = serializationInfo.GetInt32("timeout");
			this.redirects = serializationInfo.GetInt32("redirects");
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		internal bool UsesNtlmAuthentication
		{
			get
			{
				return this.is_ntlm_auth;
			}
		}

		/// <summary>Gets or sets the value of the Accept HTTP header.</summary>
		/// <returns>The value of the Accept HTTP header. The default value is null.</returns>
		public string Accept
		{
			get
			{
				return this.webHeaders["Accept"];
			}
			set
			{
				this.CheckRequestStarted();
				this.webHeaders.RemoveAndAdd("Accept", value);
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the Internet resource that actually responds to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that identifies the Internet resource that actually responds to the request. The default is the URI used by the <see cref="M:System.Net.WebRequest.Create(System.String)" /> method to initialize the request.</returns>
		public System.Uri Address
		{
			get
			{
				return this.actualUri;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the request should follow redirection responses.</summary>
		/// <returns>true if the request should automatically follow redirection responses from the Internet resource; otherwise, false. The default value is true.</returns>
		public bool AllowAutoRedirect
		{
			get
			{
				return this.allowAutoRedirect;
			}
			set
			{
				this.allowAutoRedirect = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to buffer the data sent to the Internet resource.</summary>
		/// <returns>true to enable buffering of the data sent to the Internet resource; false to disable buffering. The default is true.</returns>
		public bool AllowWriteStreamBuffering
		{
			get
			{
				return this.allowBuffering;
			}
			set
			{
				this.allowBuffering = value;
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets or sets the type of decompression that is used.</summary>
		/// <returns>A T:System.Net.DecompressionMethods object that indicates the type of decompression that is used. </returns>
		/// <exception cref="T:System.InvalidOperationException">The object's current state does not allow this property to be set.</exception>
		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return this.auto_decomp;
			}
			set
			{
				this.CheckRequestStarted();
				this.auto_decomp = value;
			}
		}

		internal bool InternalAllowBuffering
		{
			get
			{
				return this.allowBuffering && (this.method != "HEAD" && this.method != "GET" && this.method != "MKCOL" && this.method != "CONNECT" && this.method != "DELETE") && this.method != "TRACE";
			}
		}

		/// <summary>Gets or sets the collection of security certificates that are associated with this request.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> that contains the security certificates associated with this request.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null. </exception>
		public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.certificates == null)
				{
					this.certificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
				}
				return this.certificates;
			}
			[MonoTODO]
			set
			{
				throw HttpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the value of the Connection HTTP header.</summary>
		/// <returns>The value of the Connection HTTP header. The default value is null.</returns>
		/// <exception cref="T:System.ArgumentException">The value of <see cref="P:System.Net.HttpWebRequest.Connection" /> is set to Keep-alive or Close. </exception>
		public string Connection
		{
			get
			{
				return this.webHeaders["Connection"];
			}
			set
			{
				this.CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					this.webHeaders.RemoveInternal("Connection");
					return;
				}
				if (text == "keep-alive" || text == "close")
				{
					throw new ArgumentException("Keep-Alive and Close may not be set with this property");
				}
				if (this.keepAlive && text.IndexOf("keep-alive") == -1)
				{
					value += ", Keep-Alive";
				}
				this.webHeaders.RemoveAndAdd("Connection", value);
			}
		}

		/// <summary>Gets or sets the name of the connection group for the request.</summary>
		/// <returns>The name of the connection group for this request. The default value is null.</returns>
		public override string ConnectionGroupName
		{
			get
			{
				return this.connectionGroup;
			}
			set
			{
				this.connectionGroup = value;
			}
		}

		/// <summary>Gets or sets the Content-length HTTP header.</summary>
		/// <returns>The number of bytes of data to send to the Internet resource. The default is -1, which indicates the property has not been set and that there is no request data to send.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The new <see cref="P:System.Net.HttpWebRequest.ContentLength" /> value is less than 0. </exception>
		public override long ContentLength
		{
			get
			{
				return this.contentLength;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Content-Length must be >= 0");
				}
				this.contentLength = value;
			}
		}

		internal long InternalContentLength
		{
			set
			{
				this.contentLength = value;
			}
		}

		/// <summary>Gets or sets the value of the Content-type HTTP header.</summary>
		/// <returns>The value of the Content-type HTTP header. The default value is null.</returns>
		public override string ContentType
		{
			get
			{
				return this.webHeaders["Content-Type"];
			}
			set
			{
				if (value == null || value.Trim().Length == 0)
				{
					this.webHeaders.RemoveInternal("Content-Type");
					return;
				}
				this.webHeaders.RemoveAndAdd("Content-Type", value);
			}
		}

		/// <summary>Gets or sets the delegate method called when an HTTP 100-continue response is received from the Internet resource.</summary>
		/// <returns>A delegate that implements the callback method that executes when an HTTP Continue response is returned from the Internet resource. The default value is null.</returns>
		public HttpContinueDelegate ContinueDelegate
		{
			get
			{
				return this.continueDelegate;
			}
			set
			{
				this.continueDelegate = value;
			}
		}

		/// <summary>Gets or sets the cookies associated with the request.</summary>
		/// <returns>A <see cref="T:System.Net.CookieContainer" /> that contains the cookies associated with this request.</returns>
		public CookieContainer CookieContainer
		{
			get
			{
				return this.cookieContainer;
			}
			set
			{
				this.cookieContainer = value;
			}
		}

		/// <summary>Gets or sets authentication information for the request.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that contains the authentication credentials associated with the request. The default is null.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		/// <summary>Gets or sets the default cache policy for this request.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> that specifies the cache policy in effect for this request when no other policy is applicable.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO]
		public new static System.Net.Cache.RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				throw HttpWebRequest.GetMustImplement();
			}
			set
			{
				throw HttpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the default maximum length of an HTTP error response.</summary>
		/// <returns>An integer that represents the default maximum length of an HTTP error response.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 and is not equal to -1. </exception>
		[MonoTODO]
		public static int DefaultMaximumErrorResponseLength
		{
			get
			{
				throw HttpWebRequest.GetMustImplement();
			}
			set
			{
				throw HttpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the value of the Expect HTTP header.</summary>
		/// <returns>The contents of the Expect HTTP header. The default value is null.Note:The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection" />. If WebHeaderCollection is set, the property value is lost.</returns>
		/// <exception cref="T:System.ArgumentException">Expect is set to a string that contains "100-continue" as a substring. </exception>
		public string Expect
		{
			get
			{
				return this.webHeaders["Expect"];
			}
			set
			{
				this.CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					this.webHeaders.RemoveInternal("Expect");
					return;
				}
				if (text == "100-continue")
				{
					throw new ArgumentException("100-Continue cannot be set with this property.", "value");
				}
				this.webHeaders.RemoveAndAdd("Expect", value);
			}
		}

		/// <summary>Gets a value that indicates whether a response has been received from an Internet resource.</summary>
		/// <returns>true if a response has been received; otherwise, false.</returns>
		public bool HaveResponse
		{
			get
			{
				return this.haveResponse;
			}
		}

		/// <summary>Specifies a collection of the name/value pairs that make up the HTTP headers.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the name/value pairs that make up the headers for the HTTP request.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.webHeaders;
			}
			set
			{
				this.CheckRequestStarted();
				WebHeaderCollection webHeaderCollection = new WebHeaderCollection(true);
				int count = value.Count;
				for (int i = 0; i < count; i++)
				{
					webHeaderCollection.Add(value.GetKey(i), value.Get(i));
				}
				this.webHeaders = webHeaderCollection;
			}
		}

		/// <summary>Gets or sets the value of the If-Modified-Since HTTP header.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the contents of the If-Modified-Since HTTP header. The default value is the current date and time.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public DateTime IfModifiedSince
		{
			get
			{
				string text = this.webHeaders["If-Modified-Since"];
				if (text == null)
				{
					return DateTime.Now;
				}
				DateTime result;
				try
				{
					result = MonoHttpDate.Parse(text);
				}
				catch (Exception)
				{
					result = DateTime.Now;
				}
				return result;
			}
			set
			{
				this.CheckRequestStarted();
				this.webHeaders.SetInternal("If-Modified-Since", value.ToUniversalTime().ToString("r", null));
			}
		}

		/// <summary>Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.</summary>
		/// <returns>true if the request to the Internet resource should contain a Connection HTTP header with the value Keep-alive; otherwise, false. The default is true.</returns>
		public bool KeepAlive
		{
			get
			{
				return this.keepAlive;
			}
			set
			{
				this.keepAlive = value;
			}
		}

		/// <summary>Gets or sets the maximum number of redirects that the request follows.</summary>
		/// <returns>The maximum number of redirection responses that the request follows. The default value is 50.</returns>
		/// <exception cref="T:System.ArgumentException">The value is set to 0 or less. </exception>
		public int MaximumAutomaticRedirections
		{
			get
			{
				return this.maxAutoRedirect;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Must be > 0", "value");
				}
				this.maxAutoRedirect = value;
			}
		}

		/// <summary>Gets or sets the maximum allowed length of the response headers.</summary>
		/// <returns>The length, in kilobytes (1024 bytes), of the response headers.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is set after the request has already been submitted. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 and is not equal to -1. </exception>
		[MonoTODO("Use this")]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return this.maxResponseHeadersLength;
			}
			set
			{
				this.maxResponseHeadersLength = value;
			}
		}

		/// <summary>Gets or sets the default for the <see cref="P:System.Net.HttpWebRequest.MaximumResponseHeadersLength" /> property.</summary>
		/// <returns>The length, in kilobytes (1024 bytes), of the default maximum for response headers received. The default configuration file sets this value to 64 kilobytes.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is not equal to -1 and is less than zero. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO("Use this")]
		public static int DefaultMaximumResponseHeadersLength
		{
			get
			{
				return HttpWebRequest.defaultMaxResponseHeadersLength;
			}
			set
			{
				HttpWebRequest.defaultMaxResponseHeadersLength = value;
			}
		}

		/// <summary>Gets or sets a time-out in milliseconds when writing to or reading from a stream.</summary>
		/// <returns>The number of milliseconds before the writing or reading times out. The default value is 300,000 milliseconds (5 minutes).</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has already been sent. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /></exception>
		public int ReadWriteTimeout
		{
			get
			{
				return this.readWriteTimeout;
			}
			set
			{
				if (this.requestSent)
				{
					throw new InvalidOperationException("The request has already been sent.");
				}
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value", "Must be >= -1");
				}
				this.readWriteTimeout = value;
			}
		}

		/// <summary>Gets or sets the media type of the request.</summary>
		/// <returns>The media type of the request. The default value is null.</returns>
		public string MediaType
		{
			get
			{
				return this.mediaType;
			}
			set
			{
				this.mediaType = value;
			}
		}

		/// <summary>Gets or sets the method for the request.</summary>
		/// <returns>The request method to use to contact the Internet resource. The default value is GET.</returns>
		/// <exception cref="T:System.ArgumentException">No method is supplied.-or- The method string contains invalid characters. </exception>
		public override string Method
		{
			get
			{
				return this.method;
			}
			set
			{
				if (value == null || value.Trim() == string.Empty)
				{
					throw new ArgumentException("not a valid method");
				}
				this.method = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to pipeline the request to the Internet resource.</summary>
		/// <returns>true if the request should be pipelined; otherwise, false. The default is true.</returns>
		public bool Pipelined
		{
			get
			{
				return this.pipelined;
			}
			set
			{
				this.pipelined = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to send an Authorization header with the request.</summary>
		/// <returns>true to send an HTTP Authorization header with requests after authentication has taken place; otherwise, false. The default is false.</returns>
		public override bool PreAuthenticate
		{
			get
			{
				return this.preAuthenticate;
			}
			set
			{
				this.preAuthenticate = value;
			}
		}

		/// <summary>Gets or sets the version of HTTP to use for the request.</summary>
		/// <returns>The HTTP version to use for the request. The default is <see cref="F:System.Net.HttpVersion.Version11" />.</returns>
		/// <exception cref="T:System.ArgumentException">The HTTP version is set to a value other than 1.0 or 1.1. </exception>
		public Version ProtocolVersion
		{
			get
			{
				return this.version;
			}
			set
			{
				if (value != HttpVersion.Version10 && value != HttpVersion.Version11)
				{
					throw new ArgumentException("value");
				}
				this.version = value;
			}
		}

		/// <summary>Gets or sets proxy information for the request.</summary>
		/// <returns>The <see cref="T:System.Net.IWebProxy" /> object to use to proxy the request. The default value is set by calling the <see cref="P:System.Net.GlobalProxySelection.Select" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.HttpWebRequest.Proxy" /> is set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have permission for the requested operation. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.CheckRequestStarted();
				this.proxy = value;
				this.servicePoint = null;
			}
		}

		/// <summary>Gets or sets the value of the Referer HTTP header.</summary>
		/// <returns>The value of the Referer HTTP header. The default value is null.</returns>
		public string Referer
		{
			get
			{
				return this.webHeaders["Referer"];
			}
			set
			{
				this.CheckRequestStarted();
				if (value == null || value.Trim().Length == 0)
				{
					this.webHeaders.RemoveInternal("Referer");
					return;
				}
				this.webHeaders.SetInternal("Referer", value);
			}
		}

		/// <summary>Gets the original Uniform Resource Identifier (URI) of the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the Internet resource passed to the <see cref="M:System.Net.WebRequest.Create(System.String)" /> method.</returns>
		public override System.Uri RequestUri
		{
			get
			{
				return this.requestUri;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to send data in segments to the Internet resource.</summary>
		/// <returns>true to send data to the Internet resource in segments; otherwise, false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method. </exception>
		public bool SendChunked
		{
			get
			{
				return this.sendChunked;
			}
			set
			{
				this.CheckRequestStarted();
				this.sendChunked = value;
			}
		}

		/// <summary>Gets the service point to use for the request.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> that represents the network connection to the Internet resource.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public ServicePoint ServicePoint
		{
			get
			{
				return this.GetServicePoint();
			}
		}

		/// <summary>Gets or sets the time-out value in milliseconds for the <see cref="M:System.Net.HttpWebRequest.GetResponse" /> and <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> methods.</summary>
		/// <returns>The number of milliseconds to wait before the request times out. The default value is 100,000 milliseconds (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than zero and is not <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
		public override int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
			}
		}

		/// <summary>Gets or sets the value of the Transfer-encoding HTTP header.</summary>
		/// <returns>The value of the Transfer-encoding HTTP header. The default value is null.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set when <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to the value "Chunked". </exception>
		public string TransferEncoding
		{
			get
			{
				return this.webHeaders["Transfer-Encoding"];
			}
			set
			{
				this.CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					this.webHeaders.RemoveInternal("Transfer-Encoding");
					return;
				}
				if (text == "chunked")
				{
					throw new ArgumentException("Chunked encoding must be set with the SendChunked property");
				}
				if (!this.sendChunked)
				{
					throw new ArgumentException("SendChunked must be True", "value");
				}
				this.webHeaders.RemoveAndAdd("Transfer-Encoding", value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether default credentials are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">You attempted to set this property after the request was sent.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		public override bool UseDefaultCredentials
		{
			get
			{
				return CredentialCache.DefaultCredentials == this.Credentials;
			}
			set
			{
				ICredentials credentials;
				if (value)
				{
					ICredentials defaultCredentials = CredentialCache.DefaultCredentials;
					credentials = defaultCredentials;
				}
				else
				{
					credentials = null;
				}
				this.Credentials = credentials;
			}
		}

		/// <summary>Gets or sets the value of the User-agent HTTP header.</summary>
		/// <returns>The value of the User-agent HTTP header. The default value is null.Note:The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection" />. If WebHeaderCollection is set, the property value is lost.</returns>
		public string UserAgent
		{
			get
			{
				return this.webHeaders["User-Agent"];
			}
			set
			{
				this.webHeaders.SetInternal("User-Agent", value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to allow high-speed NTLM-authenticated connection sharing.</summary>
		/// <returns>true to keep the authenticated connection open; otherwise, false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return this.unsafe_auth_blah;
			}
			set
			{
				this.unsafe_auth_blah = value;
			}
		}

		internal bool GotRequestStream
		{
			get
			{
				return this.gotRequestStream;
			}
		}

		internal bool ExpectContinue
		{
			get
			{
				return this.expectContinue;
			}
			set
			{
				this.expectContinue = value;
			}
		}

		internal System.Uri AuthUri
		{
			get
			{
				return this.actualUri;
			}
		}

		internal bool ProxyQuery
		{
			get
			{
				return this.servicePoint.UsesProxy && !this.servicePoint.UseConnect;
			}
		}

		internal ServicePoint GetServicePoint()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.hostChanged || this.servicePoint == null)
				{
					this.servicePoint = ServicePointManager.FindServicePoint(this.actualUri, this.proxy);
					this.hostChanged = false;
				}
			}
			return this.servicePoint;
		}

		/// <summary>Adds a byte range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="range">The starting or ending point of the range. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(int range)
		{
			this.AddRange("bytes", range);
		}

		/// <summary>Adds a byte range header to the request for a specified range.</summary>
		/// <param name="from">The position at which to start sending data. </param>
		/// <param name="to">The position at which to stop sending data. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />-or- <paramref name="from" /> or <paramref name="to" /> is less than 0. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(int from, int to)
		{
			this.AddRange("bytes", from, to);
		}

		/// <summary>Adds a Range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="rangeSpecifier">The description of the range. </param>
		/// <param name="range">The starting or ending point of the range. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(string rangeSpecifier, int range)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			string text = this.webHeaders["Range"];
			if (text == null || text.Length == 0)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				if (!text.ToLower().StartsWith(rangeSpecifier.ToLower() + "="))
				{
					throw new InvalidOperationException("rangeSpecifier");
				}
				text += ",";
			}
			this.webHeaders.RemoveAndAdd("Range", text + range + "-");
		}

		/// <summary>Adds a range header to a request for a specified range.</summary>
		/// <param name="rangeSpecifier">The description of the range. </param>
		/// <param name="from">The position at which to start sending data. </param>
		/// <param name="to">The position at which to stop sending data. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />-or- <paramref name="from" /> or <paramref name="to" /> is less than 0. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid. </exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added. </exception>
		public void AddRange(string rangeSpecifier, int from, int to)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			if (from < 0 || to < 0 || from > to)
			{
				throw new ArgumentOutOfRangeException();
			}
			string text = this.webHeaders["Range"];
			if (text == null || text.Length == 0)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				if (!text.ToLower().StartsWith(rangeSpecifier.ToLower() + "="))
				{
					throw new InvalidOperationException("rangeSpecifier");
				}
				text += ",";
			}
			this.webHeaders.RemoveAndAdd("Range", string.Concat(new object[]
			{
				text,
				from,
				"-",
				to
			}));
		}

		/// <summary>Begins an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">The state object for this request. </param>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is being used by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false.-or- The thread pool is running out of threads. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called. </exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Compact Framework application, a request stream with zero content length was not obtained and closed correctly. For more information about handling zero content length requests, see Network Programming in the .NET Compact Framework.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (this.Aborted)
			{
				throw new WebException("The request was canceled.", WebExceptionStatus.RequestCanceled);
			}
			bool flag = !(this.method == "GET") && !(this.method == "CONNECT") && !(this.method == "HEAD") && !(this.method == "TRACE") && !(this.method == "DELETE");
			if (this.method == null || !flag)
			{
				throw new ProtocolViolationException("Cannot send data when method is: " + this.method);
			}
			if (this.contentLength == -1L && !this.sendChunked && !this.allowBuffering && this.KeepAlive)
			{
				throw new ProtocolViolationException("Content-Length not set");
			}
			string transferEncoding = this.TransferEncoding;
			if (!this.sendChunked && transferEncoding != null && transferEncoding.Trim() != string.Empty)
			{
				throw new ProtocolViolationException("SendChunked should be true.");
			}
			object obj = this.locker;
			IAsyncResult result;
			lock (obj)
			{
				if (this.asyncWrite != null)
				{
					throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
				}
				this.asyncWrite = new WebAsyncResult(this, callback, state);
				this.initialMethod = this.method;
				if (this.haveRequest && this.writeStream != null)
				{
					this.asyncWrite.SetCompleted(true, this.writeStream);
					this.asyncWrite.DoCallback();
					result = this.asyncWrite;
				}
				else
				{
					this.gotRequestStream = true;
					WebAsyncResult webAsyncResult = this.asyncWrite;
					if (!this.requestSent)
					{
						this.requestSent = true;
						this.redirects = 0;
						this.servicePoint = this.GetServicePoint();
						this.abortHandler = this.servicePoint.SendRequest(this, this.connectionGroup);
					}
					result = webAsyncResult;
				}
			}
			return result;
		}

		/// <summary>Ends an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <param name="asyncResult">The pending request for a stream. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.IO.IOException">The request did not complete, and no stream is available. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult" />. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- An error occurred while processing the request. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WebAsyncResult webAsyncResult = asyncResult as WebAsyncResult;
			if (webAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult");
			}
			this.asyncWrite = webAsyncResult;
			webAsyncResult.WaitUntilComplete();
			Exception exception = webAsyncResult.Exception;
			if (exception != null)
			{
				throw exception;
			}
			return webAsyncResult.WriteStream;
		}

		/// <summary>Gets a <see cref="T:System.IO.Stream" /> object to use to write request data.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> method is called more than once.-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- The time-out period for the request expired.-or- An error occurred while processing the request. </exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Compact Framework application, a request stream with zero content length was not obtained and closed correctly. For more information about handling zero content length requests, see Network Programming in the .NET Compact Framework.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override Stream GetRequestStream()
		{
			IAsyncResult asyncResult = this.asyncWrite;
			if (asyncResult == null)
			{
				asyncResult = this.BeginGetRequestStream(null, null);
				this.asyncWrite = (WebAsyncResult)asyncResult;
			}
			if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(this.timeout, false))
			{
				this.Abort();
				throw new WebException("The request timed out", WebExceptionStatus.Timeout);
			}
			return this.EndGetRequestStream(asyncResult);
		}

		private void CheckIfForceWrite()
		{
			if (this.writeStream == null || this.writeStream.RequestWritten || this.contentLength < 0L || !this.InternalAllowBuffering)
			{
				return;
			}
			if ((long)this.writeStream.WriteBufferLength == this.contentLength)
			{
				this.writeStream.WriteRequest();
			}
		}

		/// <summary>Begins an asynchronous request to an Internet resource.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request for a response.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate </param>
		/// <param name="state">The state object for this request. </param>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false.-or- The thread pool is running out of threads. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		///   <see cref="P:System.Net.HttpWebRequest.Method" /> is GET or HEAD, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater than zero or <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is true.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			if (this.Aborted)
			{
				throw new WebException("The request was canceled.", WebExceptionStatus.RequestCanceled);
			}
			if (this.method == null)
			{
				throw new ProtocolViolationException("Method is null.");
			}
			string transferEncoding = this.TransferEncoding;
			if (!this.sendChunked && transferEncoding != null && transferEncoding.Trim() != string.Empty)
			{
				throw new ProtocolViolationException("SendChunked should be true.");
			}
			Monitor.Enter(this.locker);
			this.getResponseCalled = true;
			if (this.asyncRead != null && !this.haveResponse)
			{
				Monitor.Exit(this.locker);
				throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
			}
			this.CheckIfForceWrite();
			this.asyncRead = new WebAsyncResult(this, callback, state);
			WebAsyncResult webAsyncResult = this.asyncRead;
			this.initialMethod = this.method;
			if (this.haveResponse)
			{
				Exception ex = this.saved_exc;
				if (this.webResponse != null)
				{
					Monitor.Exit(this.locker);
					if (ex == null)
					{
						webAsyncResult.SetCompleted(true, this.webResponse);
					}
					else
					{
						webAsyncResult.SetCompleted(true, ex);
					}
					webAsyncResult.DoCallback();
					return webAsyncResult;
				}
				if (ex != null)
				{
					Monitor.Exit(this.locker);
					webAsyncResult.SetCompleted(true, ex);
					webAsyncResult.DoCallback();
					return webAsyncResult;
				}
			}
			if (!this.requestSent)
			{
				this.requestSent = true;
				this.redirects = 0;
				this.servicePoint = this.GetServicePoint();
				this.abortHandler = this.servicePoint.SendRequest(this, this.connectionGroup);
			}
			Monitor.Exit(this.locker);
			return webAsyncResult;
		}

		/// <summary>Ends an asynchronous request to an Internet resource.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the Internet resource.</returns>
		/// <param name="asyncResult">The pending request for a response. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult." />-or- The <see cref="P:System.Net.HttpWebRequest.ContentLength" /> property is greater than 0 but the data has not been written to the request stream. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- An error occurred while processing the request. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WebAsyncResult webAsyncResult = asyncResult as WebAsyncResult;
			if (webAsyncResult == null)
			{
				throw new ArgumentException("Invalid IAsyncResult", "asyncResult");
			}
			if (!webAsyncResult.WaitUntilComplete(this.timeout, false))
			{
				this.Abort();
				throw new WebException("The request timed out", WebExceptionStatus.Timeout);
			}
			if (webAsyncResult.GotException)
			{
				throw webAsyncResult.Exception;
			}
			return webAsyncResult.Response;
		}

		/// <summary>Returns a response from an Internet resource.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the Internet resource.</returns>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.-or- <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		///   <see cref="P:System.Net.HttpWebRequest.Method" /> is GET or HEAD, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater or equal to zero or <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is true.-or- <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is true, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is false, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is false, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT. </exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, this request includes data to be sent to the server. Requests that send data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented. </exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.-or- The time-out period for the request expired.-or- An error occurred while processing the request. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override WebResponse GetResponse()
		{
			WebAsyncResult asyncResult = (WebAsyncResult)this.BeginGetResponse(null, null);
			return this.EndGetResponse(asyncResult);
		}

		internal bool FinishedReading
		{
			get
			{
				return this.finished_reading;
			}
			set
			{
				this.finished_reading = value;
			}
		}

		internal bool Aborted
		{
			get
			{
				return Interlocked.CompareExchange(ref this.aborted, 0, 0) == 1;
			}
		}

		/// <summary>Cancels a request to an Internet resource.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Abort()
		{
			if (Interlocked.CompareExchange(ref this.aborted, 1, 0) == 1)
			{
				return;
			}
			if (this.haveResponse && this.finished_reading)
			{
				return;
			}
			this.haveResponse = true;
			if (this.abortHandler != null)
			{
				try
				{
					this.abortHandler(this, EventArgs.Empty);
				}
				catch (Exception)
				{
				}
				this.abortHandler = null;
			}
			if (this.asyncWrite != null)
			{
				WebAsyncResult webAsyncResult = this.asyncWrite;
				if (!webAsyncResult.IsCompleted)
				{
					try
					{
						WebException e = new WebException("Aborted.", WebExceptionStatus.RequestCanceled);
						webAsyncResult.SetCompleted(false, e);
						webAsyncResult.DoCallback();
					}
					catch
					{
					}
				}
				this.asyncWrite = null;
			}
			if (this.asyncRead != null)
			{
				WebAsyncResult webAsyncResult2 = this.asyncRead;
				if (!webAsyncResult2.IsCompleted)
				{
					try
					{
						WebException e2 = new WebException("Aborted.", WebExceptionStatus.RequestCanceled);
						webAsyncResult2.SetCompleted(false, e2);
						webAsyncResult2.DoCallback();
					}
					catch
					{
					}
				}
				this.asyncRead = null;
			}
			if (this.writeStream != null)
			{
				try
				{
					this.writeStream.Close();
					this.writeStream = null;
				}
				catch
				{
				}
			}
			if (this.webResponse != null)
			{
				try
				{
					this.webResponse.Close();
					this.webResponse = null;
				}
				catch
				{
				}
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data required to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("requestUri", this.requestUri, typeof(System.Uri));
			serializationInfo.AddValue("actualUri", this.actualUri, typeof(System.Uri));
			serializationInfo.AddValue("allowAutoRedirect", this.allowAutoRedirect);
			serializationInfo.AddValue("allowBuffering", this.allowBuffering);
			serializationInfo.AddValue("certificates", this.certificates, typeof(System.Security.Cryptography.X509Certificates.X509CertificateCollection));
			serializationInfo.AddValue("connectionGroup", this.connectionGroup);
			serializationInfo.AddValue("contentLength", this.contentLength);
			serializationInfo.AddValue("webHeaders", this.webHeaders, typeof(WebHeaderCollection));
			serializationInfo.AddValue("keepAlive", this.keepAlive);
			serializationInfo.AddValue("maxAutoRedirect", this.maxAutoRedirect);
			serializationInfo.AddValue("mediaType", this.mediaType);
			serializationInfo.AddValue("method", this.method);
			serializationInfo.AddValue("initialMethod", this.initialMethod);
			serializationInfo.AddValue("pipelined", this.pipelined);
			serializationInfo.AddValue("version", this.version, typeof(Version));
			serializationInfo.AddValue("proxy", this.proxy, typeof(IWebProxy));
			serializationInfo.AddValue("sendChunked", this.sendChunked);
			serializationInfo.AddValue("timeout", this.timeout);
			serializationInfo.AddValue("redirects", this.redirects);
		}

		private void CheckRequestStarted()
		{
			if (this.requestSent)
			{
				throw new InvalidOperationException("request started");
			}
		}

		internal void DoContinueDelegate(int statusCode, WebHeaderCollection headers)
		{
			if (this.continueDelegate != null)
			{
				this.continueDelegate(statusCode, headers);
			}
		}

		private bool Redirect(WebAsyncResult result, HttpStatusCode code)
		{
			this.redirects++;
			Exception ex = null;
			string text = null;
			switch (code)
			{
			case HttpStatusCode.MultipleChoices:
				ex = new WebException("Ambiguous redirect.");
				goto IL_E4;
			case HttpStatusCode.MovedPermanently:
			case HttpStatusCode.Found:
			case HttpStatusCode.TemporaryRedirect:
				this.contentLength = -1L;
				this.bodyBufferLength = 0;
				this.bodyBuffer = null;
				this.method = "GET";
				text = this.webResponse.Headers["Location"];
				goto IL_E4;
			case HttpStatusCode.SeeOther:
				this.method = "GET";
				text = this.webResponse.Headers["Location"];
				goto IL_E4;
			case HttpStatusCode.NotModified:
				return false;
			case HttpStatusCode.UseProxy:
				ex = new NotImplementedException("Proxy support not available.");
				goto IL_E4;
			}
			ex = new ProtocolViolationException("Invalid status code: " + (int)code);
			IL_E4:
			if (ex != null)
			{
				throw ex;
			}
			if (text == null)
			{
				throw new WebException("No Location header found for " + (int)code, WebExceptionStatus.ProtocolError);
			}
			System.Uri uri = this.actualUri;
			try
			{
				this.actualUri = new System.Uri(this.actualUri, text);
			}
			catch (Exception)
			{
				throw new WebException(string.Format("Invalid URL ({0}) for {1}", text, (int)code), WebExceptionStatus.ProtocolError);
			}
			this.hostChanged = (this.actualUri.Scheme != uri.Scheme || this.actualUri.Host != uri.Host || this.actualUri.Port != uri.Port);
			return true;
		}

		private string GetHeaders()
		{
			bool flag = false;
			if (this.sendChunked)
			{
				flag = true;
				this.webHeaders.RemoveAndAdd("Transfer-Encoding", "chunked");
				this.webHeaders.RemoveInternal("Content-Length");
			}
			else if (this.contentLength != -1L)
			{
				if (this.contentLength > 0L)
				{
					flag = true;
				}
				this.webHeaders.SetInternal("Content-Length", this.contentLength.ToString());
				this.webHeaders.RemoveInternal("Transfer-Encoding");
			}
			if (this.actualVersion == HttpVersion.Version11 && flag && this.servicePoint.SendContinue)
			{
				this.webHeaders.RemoveAndAdd("Expect", "100-continue");
				this.expectContinue = true;
			}
			else
			{
				this.webHeaders.RemoveInternal("Expect");
				this.expectContinue = false;
			}
			bool proxyQuery = this.ProxyQuery;
			string name = (!proxyQuery) ? "Connection" : "Proxy-Connection";
			this.webHeaders.RemoveInternal(proxyQuery ? "Connection" : "Proxy-Connection");
			Version protocolVersion = this.servicePoint.ProtocolVersion;
			bool flag2 = protocolVersion == null || protocolVersion == HttpVersion.Version10;
			if (this.keepAlive && (this.version == HttpVersion.Version10 || flag2))
			{
				this.webHeaders.RemoveAndAdd(name, "keep-alive");
			}
			else if (!this.keepAlive && this.version == HttpVersion.Version11)
			{
				this.webHeaders.RemoveAndAdd(name, "close");
			}
			this.webHeaders.SetInternal("Host", this.actualUri.Authority);
			if (this.cookieContainer != null)
			{
				string cookieHeader = this.cookieContainer.GetCookieHeader(this.actualUri);
				if (cookieHeader != string.Empty)
				{
					this.webHeaders.SetInternal("Cookie", cookieHeader);
				}
			}
			string text = null;
			if ((this.auto_decomp & DecompressionMethods.GZip) != DecompressionMethods.None)
			{
				text = "gzip";
			}
			if ((this.auto_decomp & DecompressionMethods.Deflate) != DecompressionMethods.None)
			{
				text = ((text == null) ? "deflate" : "gzip, deflate");
			}
			if (text != null)
			{
				this.webHeaders.RemoveAndAdd("Accept-Encoding", text);
			}
			if (!this.usedPreAuth && this.preAuthenticate)
			{
				this.DoPreAuthenticate();
			}
			return this.webHeaders.ToString();
		}

		private void DoPreAuthenticate()
		{
			bool flag = this.proxy != null && !this.proxy.IsBypassed(this.actualUri);
			ICredentials credentials2;
			if (!flag || this.credentials != null)
			{
				ICredentials credentials = this.credentials;
				credentials2 = credentials;
			}
			else
			{
				credentials2 = this.proxy.Credentials;
			}
			ICredentials credentials3 = credentials2;
			Authorization authorization = AuthenticationManager.PreAuthenticate(this, credentials3);
			if (authorization == null)
			{
				return;
			}
			this.webHeaders.RemoveInternal("Proxy-Authorization");
			this.webHeaders.RemoveInternal("Authorization");
			string name = (!flag || this.credentials != null) ? "Authorization" : "Proxy-Authorization";
			this.webHeaders[name] = authorization.Message;
			this.usedPreAuth = true;
		}

		internal void SetWriteStreamError(WebExceptionStatus status, Exception exc)
		{
			if (this.Aborted)
			{
				return;
			}
			WebAsyncResult webAsyncResult = this.asyncWrite;
			if (webAsyncResult == null)
			{
				webAsyncResult = this.asyncRead;
			}
			if (webAsyncResult != null)
			{
				WebException e;
				if (exc == null)
				{
					string message = "Error: " + status;
					e = new WebException(message, status);
				}
				else
				{
					string message = string.Format("Error: {0} ({1})", status, exc.Message);
					e = new WebException(message, exc, status);
				}
				webAsyncResult.SetCompleted(false, e);
				webAsyncResult.DoCallback();
			}
		}

		internal void SendRequestHeaders(bool propagate_error)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text;
			if (!this.ProxyQuery)
			{
				text = this.actualUri.PathAndQuery;
			}
			else if (this.actualUri.IsDefaultPort)
			{
				text = string.Format("{0}://{1}{2}", this.actualUri.Scheme, this.actualUri.Host, this.actualUri.PathAndQuery);
			}
			else
			{
				text = string.Format("{0}://{1}:{2}{3}", new object[]
				{
					this.actualUri.Scheme,
					this.actualUri.Host,
					this.actualUri.Port,
					this.actualUri.PathAndQuery
				});
			}
			if (this.servicePoint.ProtocolVersion != null && this.servicePoint.ProtocolVersion < this.version)
			{
				this.actualVersion = this.servicePoint.ProtocolVersion;
			}
			else
			{
				this.actualVersion = this.version;
			}
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}.{3}\r\n", new object[]
			{
				this.method,
				text,
				this.actualVersion.Major,
				this.actualVersion.Minor
			});
			stringBuilder.Append(this.GetHeaders());
			string s = stringBuilder.ToString();
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			try
			{
				this.writeStream.SetHeaders(bytes);
			}
			catch (WebException ex)
			{
				this.SetWriteStreamError(ex.Status, ex);
				if (propagate_error)
				{
					throw;
				}
			}
			catch (Exception exc)
			{
				this.SetWriteStreamError(WebExceptionStatus.SendFailure, exc);
				if (propagate_error)
				{
					throw;
				}
			}
		}

		internal void SetWriteStream(WebConnectionStream stream)
		{
			if (this.Aborted)
			{
				return;
			}
			this.writeStream = stream;
			if (this.bodyBuffer != null)
			{
				this.webHeaders.RemoveInternal("Transfer-Encoding");
				this.contentLength = (long)this.bodyBufferLength;
				this.writeStream.SendChunked = false;
			}
			this.SendRequestHeaders(false);
			this.haveRequest = true;
			if (this.bodyBuffer != null)
			{
				this.writeStream.Write(this.bodyBuffer, 0, this.bodyBufferLength);
				this.bodyBuffer = null;
				this.writeStream.Close();
			}
			else if (this.method != "HEAD" && this.method != "GET" && this.method != "MKCOL" && this.method != "CONNECT" && this.method != "DELETE" && this.method != "TRACE" && this.getResponseCalled && !this.writeStream.RequestWritten)
			{
				this.writeStream.WriteRequest();
			}
			if (this.asyncWrite != null)
			{
				this.asyncWrite.SetCompleted(false, stream);
				this.asyncWrite.DoCallback();
				this.asyncWrite = null;
			}
		}

		internal void SetResponseError(WebExceptionStatus status, Exception e, string where)
		{
			if (this.Aborted)
			{
				return;
			}
			object obj = this.locker;
			lock (obj)
			{
				string message = string.Format("Error getting response stream ({0}): {1}", where, status);
				WebAsyncResult webAsyncResult = this.asyncRead;
				if (webAsyncResult == null)
				{
					webAsyncResult = this.asyncWrite;
				}
				WebException e2;
				if (e is WebException)
				{
					e2 = (WebException)e;
				}
				else
				{
					e2 = new WebException(message, e, status, null);
				}
				if (webAsyncResult != null)
				{
					if (!webAsyncResult.IsCompleted)
					{
						webAsyncResult.SetCompleted(false, e2);
						webAsyncResult.DoCallback();
					}
					else if (webAsyncResult == this.asyncWrite)
					{
						this.saved_exc = e2;
					}
					this.haveResponse = true;
					this.asyncRead = null;
					this.asyncWrite = null;
				}
				else
				{
					this.haveResponse = true;
					this.saved_exc = e2;
				}
			}
		}

		private void CheckSendError(WebConnectionData data)
		{
			int statusCode = data.StatusCode;
			if (statusCode < 400 || statusCode == 401 || statusCode == 407)
			{
				return;
			}
			if (this.writeStream != null && this.asyncRead == null && !this.writeStream.CompleteRequestWritten)
			{
				this.saved_exc = new WebException(data.StatusDescription, null, WebExceptionStatus.ProtocolError, this.webResponse);
				this.webResponse.ReadAll();
			}
		}

		private void HandleNtlmAuth(WebAsyncResult r)
		{
			WebConnectionStream webConnectionStream = this.webResponse.GetResponseStream() as WebConnectionStream;
			if (webConnectionStream != null)
			{
				WebConnection connection = webConnectionStream.Connection;
				connection.PriorityRequest = this;
				ICredentials credentials2;
				if (this.proxy == null || this.proxy.IsBypassed(this.actualUri))
				{
					ICredentials credentials = this.credentials;
					credentials2 = credentials;
				}
				else
				{
					credentials2 = this.proxy.Credentials;
				}
				ICredentials credentials3 = credentials2;
				if (credentials3 != null)
				{
					connection.NtlmCredential = credentials3.GetCredential(this.requestUri, "NTLM");
					connection.UnsafeAuthenticatedConnectionSharing = this.unsafe_auth_blah;
				}
			}
			r.Reset();
			this.haveResponse = false;
			this.webResponse.ReadAll();
			this.webResponse = null;
		}

		internal void SetResponseData(WebConnectionData data)
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.Aborted)
				{
					if (data.stream != null)
					{
						data.stream.Close();
					}
				}
				else
				{
					WebException ex = null;
					try
					{
						this.webResponse = new HttpWebResponse(this.actualUri, this.method, data, this.cookieContainer);
					}
					catch (Exception ex2)
					{
						ex = new WebException(ex2.Message, ex2, WebExceptionStatus.ProtocolError, null);
						if (data.stream != null)
						{
							data.stream.Close();
						}
					}
					if (ex == null && (this.method == "POST" || this.method == "PUT"))
					{
						object obj2 = this.locker;
						lock (obj2)
						{
							this.CheckSendError(data);
							if (this.saved_exc != null)
							{
								ex = (WebException)this.saved_exc;
							}
						}
					}
					WebAsyncResult webAsyncResult = this.asyncRead;
					bool flag = false;
					if (webAsyncResult == null && this.webResponse != null)
					{
						flag = true;
						webAsyncResult = new WebAsyncResult(null, null);
						webAsyncResult.SetCompleted(false, this.webResponse);
					}
					if (webAsyncResult != null)
					{
						if (ex != null)
						{
							webAsyncResult.SetCompleted(false, ex);
							webAsyncResult.DoCallback();
						}
						else
						{
							try
							{
								if (!this.CheckFinalStatus(webAsyncResult))
								{
									if (this.is_ntlm_auth && this.authCompleted && this.webResponse != null && this.webResponse.StatusCode < HttpStatusCode.BadRequest)
									{
										WebConnectionStream webConnectionStream = this.webResponse.GetResponseStream() as WebConnectionStream;
										if (webConnectionStream != null)
										{
											WebConnection connection = webConnectionStream.Connection;
											connection.NtlmAuthenticated = true;
										}
									}
									if (this.writeStream != null)
									{
										this.writeStream.KillBuffer();
									}
									this.haveResponse = true;
									webAsyncResult.SetCompleted(false, this.webResponse);
									webAsyncResult.DoCallback();
								}
								else
								{
									if (this.webResponse != null)
									{
										if (this.is_ntlm_auth)
										{
											this.HandleNtlmAuth(webAsyncResult);
											return;
										}
										this.webResponse.Close();
									}
									this.finished_reading = false;
									this.haveResponse = false;
									this.webResponse = null;
									webAsyncResult.Reset();
									this.servicePoint = this.GetServicePoint();
									this.abortHandler = this.servicePoint.SendRequest(this, this.connectionGroup);
								}
							}
							catch (WebException e)
							{
								if (flag)
								{
									this.saved_exc = e;
									this.haveResponse = true;
								}
								webAsyncResult.SetCompleted(false, e);
								webAsyncResult.DoCallback();
							}
							catch (Exception ex3)
							{
								ex = new WebException(ex3.Message, ex3, WebExceptionStatus.ProtocolError, null);
								if (flag)
								{
									this.saved_exc = ex;
									this.haveResponse = true;
								}
								webAsyncResult.SetCompleted(false, ex);
								webAsyncResult.DoCallback();
							}
						}
					}
				}
			}
		}

		private bool CheckAuthorization(WebResponse response, HttpStatusCode code)
		{
			this.authCompleted = false;
			if (code == HttpStatusCode.Unauthorized && this.credentials == null)
			{
				return false;
			}
			bool flag = code == HttpStatusCode.ProxyAuthenticationRequired;
			if (flag && (this.proxy == null || this.proxy.Credentials == null))
			{
				return false;
			}
			string[] values = response.Headers.GetValues((!flag) ? "WWW-Authenticate" : "Proxy-Authenticate");
			if (values == null || values.Length == 0)
			{
				return false;
			}
			ICredentials credentials2;
			if (!flag)
			{
				ICredentials credentials = this.credentials;
				credentials2 = credentials;
			}
			else
			{
				credentials2 = this.proxy.Credentials;
			}
			ICredentials credentials3 = credentials2;
			Authorization authorization = null;
			foreach (string challenge in values)
			{
				authorization = AuthenticationManager.Authenticate(challenge, this, credentials3);
				if (authorization != null)
				{
					break;
				}
			}
			if (authorization == null)
			{
				return false;
			}
			this.webHeaders[(!flag) ? "Authorization" : "Proxy-Authorization"] = authorization.Message;
			this.authCompleted = authorization.Complete;
			this.is_ntlm_auth = (authorization.Module.AuthenticationType == "NTLM");
			return true;
		}

		private bool CheckFinalStatus(WebAsyncResult result)
		{
			if (result.GotException)
			{
				throw result.Exception;
			}
			Exception ex = result.Exception;
			this.bodyBuffer = null;
			HttpWebResponse response = result.Response;
			WebExceptionStatus status = WebExceptionStatus.ProtocolError;
			HttpStatusCode httpStatusCode = (HttpStatusCode)0;
			if (ex == null && this.webResponse != null)
			{
				httpStatusCode = this.webResponse.StatusCode;
				if (!this.authCompleted && ((httpStatusCode == HttpStatusCode.Unauthorized && this.credentials != null) || (this.ProxyQuery && httpStatusCode == HttpStatusCode.ProxyAuthenticationRequired)) && !this.usedPreAuth && this.CheckAuthorization(this.webResponse, httpStatusCode))
				{
					if (this.InternalAllowBuffering)
					{
						this.bodyBuffer = this.writeStream.WriteBuffer;
						this.bodyBufferLength = this.writeStream.WriteBufferLength;
						return true;
					}
					if (this.method != "PUT" && this.method != "POST")
					{
						return true;
					}
					this.writeStream.InternalClose();
					this.writeStream = null;
					this.webResponse.Close();
					this.webResponse = null;
					throw new WebException("This request requires buffering of data for authentication or redirection to be sucessful.");
				}
				else if (httpStatusCode >= HttpStatusCode.BadRequest)
				{
					string message = string.Format("The remote server returned an error: ({0}) {1}.", (int)httpStatusCode, this.webResponse.StatusDescription);
					ex = new WebException(message, null, status, this.webResponse);
					this.webResponse.ReadAll();
				}
				else if (httpStatusCode == HttpStatusCode.NotModified && this.allowAutoRedirect)
				{
					string message2 = string.Format("The remote server returned an error: ({0}) {1}.", (int)httpStatusCode, this.webResponse.StatusDescription);
					ex = new WebException(message2, null, status, this.webResponse);
				}
				else if (httpStatusCode >= HttpStatusCode.MultipleChoices && this.allowAutoRedirect && this.redirects >= this.maxAutoRedirect)
				{
					ex = new WebException("Max. redirections exceeded.", null, status, this.webResponse);
					this.webResponse.ReadAll();
				}
			}
			if (ex == null)
			{
				bool result2 = false;
				int num = (int)httpStatusCode;
				if (this.allowAutoRedirect && num >= 300)
				{
					if (this.InternalAllowBuffering && this.writeStream.WriteBufferLength > 0)
					{
						this.bodyBuffer = this.writeStream.WriteBuffer;
						this.bodyBufferLength = this.writeStream.WriteBufferLength;
					}
					result2 = this.Redirect(result, httpStatusCode);
				}
				if (response != null && num >= 300 && num != 304)
				{
					response.ReadAll();
				}
				return result2;
			}
			if (this.writeStream != null)
			{
				this.writeStream.InternalClose();
				this.writeStream = null;
			}
			this.webResponse = null;
			throw ex;
		}
	}
}
