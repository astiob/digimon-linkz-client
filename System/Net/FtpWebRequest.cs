using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Cache;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net
{
	/// <summary>Implements a File Transfer Protocol (FTP) client.</summary>
	public sealed class FtpWebRequest : WebRequest
	{
		private const string ChangeDir = "CWD";

		private const string UserCommand = "USER";

		private const string PasswordCommand = "PASS";

		private const string TypeCommand = "TYPE";

		private const string PassiveCommand = "PASV";

		private const string PortCommand = "PORT";

		private const string AbortCommand = "ABOR";

		private const string AuthCommand = "AUTH";

		private const string RestCommand = "REST";

		private const string RenameFromCommand = "RNFR";

		private const string RenameToCommand = "RNTO";

		private const string QuitCommand = "QUIT";

		private const string EOL = "\r\n";

		private System.Uri requestUri;

		private string file_name;

		private ServicePoint servicePoint;

		private Stream origDataStream;

		private Stream dataStream;

		private Stream controlStream;

		private StreamReader controlReader;

		private NetworkCredential credentials;

		private IPHostEntry hostEntry;

		private IPEndPoint localEndPoint;

		private IWebProxy proxy;

		private int timeout = 100000;

		private int rwTimeout = 300000;

		private long offset;

		private bool binary = true;

		private bool enableSsl;

		private bool usePassive = true;

		private bool keepAlive;

		private string method = "RETR";

		private string renameTo;

		private object locker = new object();

		private FtpWebRequest.RequestState requestState;

		private FtpAsyncResult asyncResult;

		private FtpWebResponse ftpResponse;

		private Stream requestStream;

		private string initial_path;

		private static readonly string[] supportedCommands = new string[]
		{
			"APPE",
			"DELE",
			"LIST",
			"MDTM",
			"MKD",
			"NLST",
			"PWD",
			"RENAME",
			"RETR",
			"RMD",
			"SIZE",
			"STOR",
			"STOU"
		};

		private System.Net.Security.RemoteCertificateValidationCallback callback = delegate(object sender, X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			if (ServicePointManager.ServerCertificateValidationCallback != null)
			{
				return ServicePointManager.ServerCertificateValidationCallback(sender, certificate, chain, sslPolicyErrors);
			}
			if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
			{
				throw new InvalidOperationException("SSL authentication error: " + sslPolicyErrors);
			}
			return true;
		};

		internal FtpWebRequest(System.Uri uri)
		{
			this.requestUri = uri;
			this.proxy = GlobalProxySelection.Select;
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets the certificates used for establishing an encrypted connection to the FTP server.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> object that contains the client certificates.</returns>
		[MonoTODO]
		public System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates
		{
			get
			{
				throw FtpWebRequest.GetMustImplement();
			}
			set
			{
				throw FtpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the name of the connection group that contains the service point used to send the current request.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains a connection group name.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		[MonoTODO]
		public override string ConnectionGroupName
		{
			get
			{
				throw FtpWebRequest.GetMustImplement();
			}
			set
			{
				throw FtpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Content type information is not supported for FTP.</exception>
		public override string ContentType
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets or sets a value that is ignored by the <see cref="T:System.Net.FtpWebRequest" /> class.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that should be ignored.</returns>
		public override long ContentLength
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		/// <summary>Gets or sets a byte offset into the file being downloaded by this request.</summary>
		/// <returns>An <see cref="T:System.Int64" /> instance that specifies the file offset, in bytes. The default value is zero.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for this property is less than zero. </exception>
		public long ContentOffset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.offset = value;
			}
		}

		/// <summary>Gets or sets the credentials used to communicate with the FTP server.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> instance; otherwise, null if the property has not been set.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is null.</exception>
		/// <exception cref="T:System.ArgumentException">An <see cref="T:System.Net.ICredentials" /> of a type other than <see cref="T:System.Net.NetworkCredential" /> was specified for a set operation.</exception>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.CheckRequestStarted();
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (!(value is NetworkCredential))
				{
					throw new ArgumentException();
				}
				this.credentials = (value as NetworkCredential);
			}
		}

		/// <summary>Defines the default cache policy for all FTP requests.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.RequestCachePolicy" /> that defines the cache policy for FTP requests.</returns>
		/// <exception cref="T:System.ArgumentNullException">The caller tried to set this property to null.</exception>
		[MonoTODO]
		public new static System.Net.Cache.RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				throw FtpWebRequest.GetMustImplement();
			}
			set
			{
				throw FtpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> that specifies that an SSL connection should be used.</summary>
		/// <returns>true if control and data transmissions are encrypted; otherwise, false. The default value is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The connection to the FTP server has already been established.</exception>
		public bool EnableSsl
		{
			get
			{
				return this.enableSsl;
			}
			set
			{
				this.CheckRequestStarted();
				this.enableSsl = value;
			}
		}

		/// <summary>Gets an empty <see cref="T:System.Net.WebHeaderCollection" /> object.</summary>
		/// <returns>An empty <see cref="T:System.Net.WebHeaderCollection" /> object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[MonoTODO]
		public override WebHeaderCollection Headers
		{
			get
			{
				throw FtpWebRequest.GetMustImplement();
			}
			set
			{
				throw FtpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the control connection to the FTP server is closed after the request completes.</summary>
		/// <returns>true if the connection to the server should not be destroyed; otherwise, false. The default value is true.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		[MonoTODO("We don't support KeepAlive = true")]
		public bool KeepAlive
		{
			get
			{
				return this.keepAlive;
			}
			set
			{
				this.CheckRequestStarted();
			}
		}

		/// <summary>Gets or sets the command to send to the FTP server.</summary>
		/// <returns>A <see cref="T:System.String" /> value that contains the FTP command to send to the server. The default value is <see cref="F:System.Net.WebRequestMethods.Ftp.DownloadFile" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <exception cref="T:System.ArgumentException">The method is invalid.- or -The method is not supported.- or -Multiple methods were specified.</exception>
		public override string Method
		{
			get
			{
				return this.method;
			}
			set
			{
				this.CheckRequestStarted();
				if (value == null)
				{
					throw new ArgumentNullException("Method string cannot be null");
				}
				if (value.Length == 0 || Array.BinarySearch<string>(FtpWebRequest.supportedCommands, value) < 0)
				{
					throw new ArgumentException("Method not supported", "value");
				}
				this.method = value;
			}
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Preauthentication is not supported for FTP.</exception>
		public override bool PreAuthenticate
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets or sets the proxy used to communicate with the FTP server.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> instance responsible for communicating with the FTP server.</returns>
		/// <exception cref="T:System.ArgumentNullException">This property cannot be set to null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		/// <PermissionSet>
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
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.proxy = value;
			}
		}

		/// <summary>Gets or sets a time-out when reading from or writing to a stream.</summary>
		/// <returns>The number of milliseconds before the reading or writing times out. The default value is 300,000 milliseconds (5 minutes).</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has already been sent. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero and is not equal to <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
		public int ReadWriteTimeout
		{
			get
			{
				return this.rwTimeout;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.rwTimeout = value;
			}
		}

		/// <summary>Gets or sets the new name of a file being renamed.</summary>
		/// <returns>The new name of the file being renamed.</returns>
		public string RenameTo
		{
			get
			{
				return this.renameTo;
			}
			set
			{
				this.CheckRequestStarted();
				if (value == null || value.Length == 0)
				{
					throw new ArgumentException("RenameTo value can't be null or empty", "RenameTo");
				}
				this.renameTo = value;
			}
		}

		/// <summary>Gets the URI requested by this instance.</summary>
		/// <returns>A <see cref="T:System.Uri" /> instance that identifies a resource that is accessed using the File Transfer Protocol.</returns>
		public override System.Uri RequestUri
		{
			get
			{
				return this.requestUri;
			}
		}

		/// <summary>Gets the <see cref="T:System.Net.ServicePoint" /> object used to connect to the FTP server.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> object that can be used to customize connection behavior.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public ServicePoint ServicePoint
		{
			get
			{
				return this.GetServicePoint();
			}
		}

		/// <summary>Gets or sets the behavior of a client application's data transfer process.</summary>
		/// <returns>false if the client application's data transfer process listens for a connection on the data port; otherwise, true if the client should initiate a connection on the data port. The default value is true.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		public bool UsePassive
		{
			get
			{
				return this.usePassive;
			}
			set
			{
				this.CheckRequestStarted();
				this.usePassive = value;
			}
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Default credentials are not supported for FTP.</exception>
		[MonoTODO]
		public override bool UseDefaultCredentials
		{
			get
			{
				throw FtpWebRequest.GetMustImplement();
			}
			set
			{
				throw FtpWebRequest.GetMustImplement();
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies the data type for file transfers.</summary>
		/// <returns>true to indicate to the server that the data to be transferred is binary; false to indicate that the data is text. The default value is true.</returns>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress.</exception>
		public bool UseBinary
		{
			get
			{
				return this.binary;
			}
			set
			{
				this.CheckRequestStarted();
				this.binary = value;
			}
		}

		/// <summary>Gets or sets the number of milliseconds to wait for a request.</summary>
		/// <returns>An <see cref="T:System.Int32" /> value that contains the number of milliseconds to wait before a request times out. The default value is <see cref="F:System.Threading.Timeout.Infinite" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than zero and is not <see cref="F:System.Threading.Timeout.Infinite" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">A new value was specified for this property for a request that is already in progress. </exception>
		public override int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.timeout = value;
			}
		}

		private string DataType
		{
			get
			{
				return (!this.binary) ? "A" : "I";
			}
		}

		private FtpWebRequest.RequestState State
		{
			get
			{
				object obj = this.locker;
				FtpWebRequest.RequestState result;
				lock (obj)
				{
					result = this.requestState;
				}
				return result;
			}
			set
			{
				object obj = this.locker;
				lock (obj)
				{
					this.CheckIfAborted();
					this.CheckFinalState();
					this.requestState = value;
				}
			}
		}

		/// <summary>Terminates an asynchronous FTP operation.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void Abort()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.State == FtpWebRequest.RequestState.TransferInProgress)
				{
					this.SendCommand(false, "ABOR", new string[0]);
				}
				if (!this.InFinalState())
				{
					this.State = FtpWebRequest.RequestState.Aborted;
					this.ftpResponse = new FtpWebResponse(this, this.requestUri, this.method, FtpStatusCode.FileActionAborted, "Aborted by request");
				}
			}
		}

		/// <summary>Begins sending a request and receiving a response from an FTP server asynchronously.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that indicates the status of the operation.</returns>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="callback" /> delegate when the operation completes. </param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.FtpWebRequest.GetResponse" /> or <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> has already been called for this instance. </exception>
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
			if (this.asyncResult != null && !this.asyncResult.IsCompleted)
			{
				throw new InvalidOperationException("Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress");
			}
			this.CheckIfAborted();
			this.asyncResult = new FtpAsyncResult(callback, state);
			object obj = this.locker;
			lock (obj)
			{
				if (this.InFinalState())
				{
					this.asyncResult.SetCompleted(true, this.ftpResponse);
				}
				else
				{
					if (this.State == FtpWebRequest.RequestState.Before)
					{
						this.State = FtpWebRequest.RequestState.Scheduled;
					}
					Thread thread = new Thread(new ThreadStart(this.ProcessRequest));
					thread.Start();
				}
			}
			return this.asyncResult;
		}

		/// <summary>Ends a pending asynchronous operation started with <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> reference that contains an <see cref="T:System.Net.FtpWebResponse" /> instance. This object contains the FTP server's response to the request.</returns>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> that was returned when the operation started. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not obtained by calling <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was already called for the operation identified by <paramref name="asyncResult" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("AsyncResult cannot be null!");
			}
			if (!(asyncResult is FtpAsyncResult) || asyncResult != this.asyncResult)
			{
				throw new ArgumentException("AsyncResult is from another request!");
			}
			FtpAsyncResult ftpAsyncResult = (FtpAsyncResult)asyncResult;
			if (!ftpAsyncResult.WaitUntilComplete(this.timeout, false))
			{
				this.Abort();
				throw new WebException("Transfer timed out.", WebExceptionStatus.Timeout);
			}
			this.CheckIfAborted();
			asyncResult = null;
			if (ftpAsyncResult.GotException)
			{
				throw ftpAsyncResult.Exception;
			}
			return ftpAsyncResult.Response;
		}

		/// <summary>Returns the FTP server response.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> reference that contains an <see cref="T:System.Net.FtpWebResponse" /> instance. This object contains the FTP server's response to the request.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.FtpWebRequest.GetResponse" /> or <see cref="M:System.Net.FtpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> has already been called for this instance.- or -An HTTP proxy is enabled, and you attempted to use an FTP command other than <see cref="F:System.Net.WebRequestMethods.Ftp.DownloadFile" />, <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectory" />, or <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectoryDetails" />.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="P:System.Net.FtpWebRequest.EnableSsl" /> is set to true, but the server does not support this feature.</exception>
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
			IAsyncResult asyncResult = this.BeginGetResponse(null, null);
			return this.EndGetResponse(asyncResult);
		}

		/// <summary>Begins asynchronously opening a request's content stream for writing.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> instance that indicates the status of the operation.</returns>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="callback" /> delegate when the operation completes. </param>
		/// <exception cref="T:System.InvalidOperationException">A previous call to this method or <see cref="M:System.Net.FtpWebRequest.GetRequestStream" /> has not yet completed. </exception>
		/// <exception cref="T:System.Net.WebException">A connection to the FTP server could not be established. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.FtpWebRequest.Method" /> property is not set to <see cref="F:System.Net.WebRequestMethods.Ftp.UploadFile" />. </exception>
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
			if (this.method != "STOR" && this.method != "STOU" && this.method != "APPE")
			{
				throw new ProtocolViolationException();
			}
			object obj = this.locker;
			lock (obj)
			{
				this.CheckIfAborted();
				if (this.State != FtpWebRequest.RequestState.Before)
				{
					throw new InvalidOperationException("Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress");
				}
				this.State = FtpWebRequest.RequestState.Scheduled;
			}
			this.asyncResult = new FtpAsyncResult(callback, state);
			Thread thread = new Thread(new ThreadStart(this.ProcessRequest));
			thread.Start();
			return this.asyncResult;
		}

		/// <summary>Ends a pending asynchronous operation started with <see cref="M:System.Net.FtpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />.</summary>
		/// <returns>A writable <see cref="T:System.IO.Stream" /> instance associated with this instance.</returns>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> object that was returned when the operation started. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not obtained by calling <see cref="M:System.Net.FtpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">This method was already called for the operation identified by <paramref name="asyncResult" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.WebPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is FtpAsyncResult))
			{
				throw new ArgumentException("asyncResult");
			}
			if (this.State == FtpWebRequest.RequestState.Aborted)
			{
				throw new WebException("Request aborted", WebExceptionStatus.RequestCanceled);
			}
			if (asyncResult != this.asyncResult)
			{
				throw new ArgumentException("AsyncResult is from another request!");
			}
			FtpAsyncResult ftpAsyncResult = (FtpAsyncResult)asyncResult;
			if (!ftpAsyncResult.WaitUntilComplete(this.timeout, false))
			{
				this.Abort();
				throw new WebException("Request timed out");
			}
			if (ftpAsyncResult.GotException)
			{
				throw ftpAsyncResult.Exception;
			}
			return ftpAsyncResult.Stream;
		}

		/// <summary>Retrieves the stream used to upload data to an FTP server.</summary>
		/// <returns>A writable <see cref="T:System.IO.Stream" /> instance used to store data to be sent to the server by the current request.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Net.FtpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" /> has been called and has not completed. - or -An HTTP proxy is enabled, and you attempted to use an FTP command other than <see cref="F:System.Net.WebRequestMethods.Ftp.DownloadFile" />, <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectory" />, or <see cref="F:System.Net.WebRequestMethods.Ftp.ListDirectoryDetails" />.</exception>
		/// <exception cref="T:System.Net.WebException">A connection to the FTP server could not be established. </exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.FtpWebRequest.Method" /> property is not set to <see cref="F:System.Net.WebRequestMethods.Ftp.UploadFile" /> or <see cref="F:System.Net.WebRequestMethods.Ftp.AppendFile" />. </exception>
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
			IAsyncResult asyncResult = this.BeginGetRequestStream(null, null);
			return this.EndGetRequestStream(asyncResult);
		}

		private ServicePoint GetServicePoint()
		{
			if (this.servicePoint == null)
			{
				this.servicePoint = ServicePointManager.FindServicePoint(this.requestUri, this.proxy);
			}
			return this.servicePoint;
		}

		private void ResolveHost()
		{
			this.CheckIfAborted();
			this.hostEntry = this.GetServicePoint().HostEntry;
			if (this.hostEntry == null)
			{
				this.ftpResponse.UpdateStatus(new FtpStatus(FtpStatusCode.ActionAbortedLocalProcessingError, "Cannot resolve server name"));
				throw new WebException("The remote server name could not be resolved: " + this.requestUri, null, WebExceptionStatus.NameResolutionFailure, this.ftpResponse);
			}
		}

		private void ProcessRequest()
		{
			if (this.State == FtpWebRequest.RequestState.Scheduled)
			{
				this.ftpResponse = new FtpWebResponse(this, this.requestUri, this.method, this.keepAlive);
				try
				{
					this.ProcessMethod();
					this.asyncResult.SetCompleted(false, this.ftpResponse);
				}
				catch (Exception completeWithError)
				{
					this.State = FtpWebRequest.RequestState.Error;
					this.SetCompleteWithError(completeWithError);
				}
			}
			else
			{
				if (this.InProgress())
				{
					FtpStatus responseStatus = this.GetResponseStatus();
					this.ftpResponse.UpdateStatus(responseStatus);
					if (this.ftpResponse.IsFinal())
					{
						this.State = FtpWebRequest.RequestState.Finished;
					}
				}
				this.asyncResult.SetCompleted(false, this.ftpResponse);
			}
		}

		private void SetType()
		{
			if (this.binary)
			{
				FtpStatus ftpStatus = this.SendCommand("TYPE", new string[]
				{
					this.DataType
				});
				if (ftpStatus.StatusCode < FtpStatusCode.CommandOK || ftpStatus.StatusCode >= (FtpStatusCode)300)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
			}
		}

		private string GetRemoteFolderPath(System.Uri uri)
		{
			string text = System.Uri.UnescapeDataString(uri.LocalPath);
			string text2;
			if (this.initial_path == null || this.initial_path == "/")
			{
				text2 = text;
			}
			else
			{
				if (text[0] == '/')
				{
					text = text.Substring(1);
				}
				System.Uri baseUri = new System.Uri("ftp://dummy-host" + this.initial_path);
				text2 = new System.Uri(baseUri, text).LocalPath;
			}
			int num = text2.LastIndexOf('/');
			if (num == -1)
			{
				return null;
			}
			return text2.Substring(0, num + 1);
		}

		private void CWDAndSetFileName(System.Uri uri)
		{
			string remoteFolderPath = this.GetRemoteFolderPath(uri);
			if (remoteFolderPath != null)
			{
				FtpStatus ftpStatus = this.SendCommand("CWD", new string[]
				{
					remoteFolderPath
				});
				if (ftpStatus.StatusCode < FtpStatusCode.CommandOK || ftpStatus.StatusCode >= (FtpStatusCode)300)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				int num = uri.LocalPath.LastIndexOf('/');
				if (num >= 0)
				{
					this.file_name = System.Uri.UnescapeDataString(uri.LocalPath.Substring(num + 1));
				}
			}
		}

		private void ProcessMethod()
		{
			this.State = FtpWebRequest.RequestState.Connecting;
			this.ResolveHost();
			this.OpenControlConnection();
			this.CWDAndSetFileName(this.requestUri);
			this.SetType();
			string text = this.method;
			if (text != null)
			{
				if (FtpWebRequest.<>f__switch$map5 == null)
				{
					FtpWebRequest.<>f__switch$map5 = new Dictionary<string, int>(12)
					{
						{
							"RETR",
							0
						},
						{
							"NLST",
							0
						},
						{
							"LIST",
							0
						},
						{
							"APPE",
							1
						},
						{
							"STOR",
							1
						},
						{
							"STOU",
							1
						},
						{
							"SIZE",
							2
						},
						{
							"MDTM",
							2
						},
						{
							"PWD",
							2
						},
						{
							"MKD",
							2
						},
						{
							"RENAME",
							2
						},
						{
							"DELE",
							2
						}
					};
				}
				int num;
				if (FtpWebRequest.<>f__switch$map5.TryGetValue(text, out num))
				{
					switch (num)
					{
					case 0:
						this.DownloadData();
						break;
					case 1:
						this.UploadData();
						break;
					case 2:
						this.ProcessSimpleMethod();
						break;
					default:
						goto IL_124;
					}
					this.CheckIfAborted();
					return;
				}
			}
			IL_124:
			throw new Exception(string.Format("Support for command {0} not implemented yet", this.method));
		}

		private void CloseControlConnection()
		{
			if (this.controlStream != null)
			{
				this.SendCommand("QUIT", new string[0]);
				this.controlStream.Close();
				this.controlStream = null;
			}
		}

		internal void CloseDataConnection()
		{
			if (this.origDataStream != null)
			{
				this.origDataStream.Close();
				this.origDataStream = null;
			}
		}

		private void CloseConnection()
		{
			this.CloseControlConnection();
			this.CloseDataConnection();
		}

		private void ProcessSimpleMethod()
		{
			this.State = FtpWebRequest.RequestState.TransferInProgress;
			if (this.method == "PWD")
			{
				this.method = "PWD";
			}
			if (this.method == "RENAME")
			{
				this.method = "RNFR";
			}
			FtpStatus ftpStatus = this.SendCommand(this.method, new string[]
			{
				this.file_name
			});
			this.ftpResponse.Stream = Stream.Null;
			string statusDescription = ftpStatus.StatusDescription;
			string text = this.method;
			switch (text)
			{
			case "SIZE":
			{
				if (ftpStatus.StatusCode != FtpStatusCode.FileStatus)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				int num2 = 4;
				int num3 = 0;
				while (num2 < statusDescription.Length && char.IsDigit(statusDescription[num2]))
				{
					num2++;
					num3++;
				}
				if (num3 == 0)
				{
					throw new WebException("Bad format for server response in " + this.method);
				}
				long contentLength;
				if (!long.TryParse(statusDescription.Substring(4, num3), out contentLength))
				{
					throw new WebException("Bad format for server response in " + this.method);
				}
				this.ftpResponse.contentLength = contentLength;
				break;
			}
			case "MDTM":
				if (ftpStatus.StatusCode != FtpStatusCode.FileStatus)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				this.ftpResponse.LastModified = DateTime.ParseExact(statusDescription.Substring(4), "yyyyMMddHHmmss", null);
				break;
			case "MKD":
				if (ftpStatus.StatusCode != FtpStatusCode.PathnameCreated)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				break;
			case "CWD":
				this.method = "PWD";
				if (ftpStatus.StatusCode != FtpStatusCode.FileActionOK)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = this.SendCommand(this.method, new string[0]);
				if (ftpStatus.StatusCode != FtpStatusCode.PathnameCreated)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				break;
			case "RNFR":
				this.method = "RENAME";
				if (ftpStatus.StatusCode != FtpStatusCode.FileCommandPending)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = this.SendCommand("RNTO", new string[]
				{
					(this.renameTo == null) ? string.Empty : this.renameTo
				});
				if (ftpStatus.StatusCode != FtpStatusCode.FileActionOK)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				break;
			case "DELE":
				if (ftpStatus.StatusCode != FtpStatusCode.FileActionOK)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				break;
			}
			this.State = FtpWebRequest.RequestState.Finished;
		}

		private void UploadData()
		{
			this.State = FtpWebRequest.RequestState.OpeningData;
			this.OpenDataConnection();
			this.State = FtpWebRequest.RequestState.TransferInProgress;
			this.requestStream = new FtpDataStream(this, this.dataStream, false);
			this.asyncResult.Stream = this.requestStream;
		}

		private void DownloadData()
		{
			this.State = FtpWebRequest.RequestState.OpeningData;
			this.OpenDataConnection();
			this.State = FtpWebRequest.RequestState.TransferInProgress;
			this.ftpResponse.Stream = new FtpDataStream(this, this.dataStream, true);
		}

		private void CheckRequestStarted()
		{
			if (this.State != FtpWebRequest.RequestState.Before)
			{
				throw new InvalidOperationException("There is a request currently in progress");
			}
		}

		private void OpenControlConnection()
		{
			Exception innerException = null;
			System.Net.Sockets.Socket socket = null;
			foreach (IPAddress ipaddress in this.hostEntry.AddressList)
			{
				socket = new System.Net.Sockets.Socket(ipaddress.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
				IPEndPoint ipendPoint = new IPEndPoint(ipaddress, this.requestUri.Port);
				if (!this.ServicePoint.CallEndPointDelegate(socket, ipendPoint))
				{
					socket.Close();
					socket = null;
				}
				else
				{
					try
					{
						socket.Connect(ipendPoint);
						this.localEndPoint = (IPEndPoint)socket.LocalEndPoint;
						break;
					}
					catch (System.Net.Sockets.SocketException ex)
					{
						innerException = ex;
						socket.Close();
						socket = null;
					}
				}
			}
			if (socket == null)
			{
				throw new WebException("Unable to connect to remote server", innerException, WebExceptionStatus.UnknownError, this.ftpResponse);
			}
			this.controlStream = new System.Net.Sockets.NetworkStream(socket);
			this.controlReader = new StreamReader(this.controlStream, Encoding.ASCII);
			this.State = FtpWebRequest.RequestState.Authenticating;
			this.Authenticate();
			FtpStatus status = this.SendCommand("OPTS", new string[]
			{
				"utf8",
				"on"
			});
			status = this.SendCommand("PWD", new string[0]);
			this.initial_path = FtpWebRequest.GetInitialPath(status);
		}

		private static string GetInitialPath(FtpStatus status)
		{
			int statusCode = (int)status.StatusCode;
			if (statusCode < 200 || statusCode > 300 || status.StatusDescription.Length <= 4)
			{
				throw new WebException("Error getting current directory: " + status.StatusDescription, null, WebExceptionStatus.UnknownError, null);
			}
			string text = status.StatusDescription.Substring(4);
			if (text[0] == '"')
			{
				int num = text.IndexOf('"', 1);
				if (num == -1)
				{
					throw new WebException("Error getting current directory: PWD -> " + status.StatusDescription, null, WebExceptionStatus.UnknownError, null);
				}
				text = text.Substring(1, num - 1);
			}
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
			return text;
		}

		private System.Net.Sockets.Socket SetupPassiveConnection(string statusDescription)
		{
			if (statusDescription.Length < 4)
			{
				throw new WebException("Cannot open passive data connection");
			}
			int num = 3;
			while (num < statusDescription.Length && !char.IsDigit(statusDescription[num]))
			{
				num++;
			}
			if (num >= statusDescription.Length)
			{
				throw new WebException("Cannot open passive data connection");
			}
			string[] array = statusDescription.Substring(num).Split(new char[]
			{
				','
			}, 6);
			if (array.Length != 6)
			{
				throw new WebException("Cannot open passive data connection");
			}
			int num2 = array[5].Length - 1;
			while (num2 >= 0 && !char.IsDigit(array[5][num2]))
			{
				num2--;
			}
			if (num2 < 0)
			{
				throw new WebException("Cannot open passive data connection");
			}
			array[5] = array[5].Substring(0, num2 + 1);
			IPAddress address;
			try
			{
				address = IPAddress.Parse(string.Join(".", array, 0, 4));
			}
			catch (FormatException)
			{
				throw new WebException("Cannot open passive data connection");
			}
			int num3;
			int num4;
			if (!int.TryParse(array[4], out num3) || !int.TryParse(array[5], out num4))
			{
				throw new WebException("Cannot open passive data connection");
			}
			int num5 = (num3 << 8) + num4;
			if (num5 < 0 || num5 > 65535)
			{
				throw new WebException("Cannot open passive data connection");
			}
			IPEndPoint ipendPoint = new IPEndPoint(address, num5);
			System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(ipendPoint.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
			try
			{
				socket.Connect(ipendPoint);
			}
			catch (System.Net.Sockets.SocketException)
			{
				socket.Close();
				throw new WebException("Cannot open passive data connection");
			}
			return socket;
		}

		private Exception CreateExceptionFromResponse(FtpStatus status)
		{
			FtpWebResponse response = new FtpWebResponse(this, this.requestUri, this.method, status);
			return new WebException("Server returned an error: " + status.StatusDescription, null, WebExceptionStatus.ProtocolError, response);
		}

		internal void SetTransferCompleted()
		{
			if (this.InFinalState())
			{
				return;
			}
			this.State = FtpWebRequest.RequestState.Finished;
			FtpStatus responseStatus = this.GetResponseStatus();
			this.ftpResponse.UpdateStatus(responseStatus);
			if (!this.keepAlive)
			{
				this.CloseConnection();
			}
		}

		internal void OperationCompleted()
		{
			if (!this.keepAlive)
			{
				this.CloseConnection();
			}
		}

		private void SetCompleteWithError(Exception exc)
		{
			if (this.asyncResult != null)
			{
				this.asyncResult.SetCompleted(false, exc);
			}
		}

		private System.Net.Sockets.Socket InitDataConnection()
		{
			if (this.usePassive)
			{
				FtpStatus ftpStatus = this.SendCommand("PASV", new string[0]);
				if (ftpStatus.StatusCode != FtpStatusCode.EnteringPassive)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				return this.SetupPassiveConnection(ftpStatus.StatusDescription);
			}
			else
			{
				System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
				try
				{
					socket.Bind(new IPEndPoint(this.localEndPoint.Address, 0));
					socket.Listen(1);
				}
				catch (System.Net.Sockets.SocketException innerException)
				{
					socket.Close();
					throw new WebException("Couldn't open listening socket on client", innerException);
				}
				IPEndPoint ipendPoint = (IPEndPoint)socket.LocalEndPoint;
				string text = ipendPoint.Address.ToString().Replace('.', ',');
				int num = ipendPoint.Port >> 8;
				int num2 = ipendPoint.Port % 256;
				string text2 = string.Concat(new object[]
				{
					text,
					",",
					num,
					",",
					num2
				});
				FtpStatus ftpStatus = this.SendCommand("PORT", new string[]
				{
					text2
				});
				if (ftpStatus.StatusCode != FtpStatusCode.CommandOK)
				{
					socket.Close();
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				return socket;
			}
		}

		private void OpenDataConnection()
		{
			System.Net.Sockets.Socket socket = this.InitDataConnection();
			FtpStatus ftpStatus;
			if (this.offset > 0L)
			{
				ftpStatus = this.SendCommand("REST", new string[]
				{
					this.offset.ToString()
				});
				if (ftpStatus.StatusCode != FtpStatusCode.FileCommandPending)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
			}
			if (this.method != "NLST" && this.method != "LIST" && this.method != "STOU")
			{
				ftpStatus = this.SendCommand(this.method, new string[]
				{
					this.file_name
				});
			}
			else
			{
				ftpStatus = this.SendCommand(this.method, new string[0]);
			}
			if (ftpStatus.StatusCode != FtpStatusCode.OpeningData && ftpStatus.StatusCode != FtpStatusCode.DataAlreadyOpen)
			{
				throw this.CreateExceptionFromResponse(ftpStatus);
			}
			if (this.usePassive)
			{
				this.origDataStream = new System.Net.Sockets.NetworkStream(socket, true);
				this.dataStream = this.origDataStream;
				if (this.EnableSsl)
				{
					this.ChangeToSSLSocket(ref this.dataStream);
				}
			}
			else
			{
				System.Net.Sockets.Socket socket2 = null;
				try
				{
					socket2 = socket.Accept();
				}
				catch (System.Net.Sockets.SocketException)
				{
					socket.Close();
					if (socket2 != null)
					{
						socket2.Close();
					}
					throw new ProtocolViolationException("Server commited a protocol violation.");
				}
				socket.Close();
				this.origDataStream = new System.Net.Sockets.NetworkStream(socket, true);
				this.dataStream = this.origDataStream;
				if (this.EnableSsl)
				{
					this.ChangeToSSLSocket(ref this.dataStream);
				}
			}
			this.ftpResponse.UpdateStatus(ftpStatus);
		}

		private void Authenticate()
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			if (this.credentials != null)
			{
				text = this.credentials.UserName;
				text2 = this.credentials.Password;
				text3 = this.credentials.Domain;
			}
			if (text == null)
			{
				text = "anonymous";
			}
			if (text2 == null)
			{
				text2 = "@anonymous";
			}
			if (!string.IsNullOrEmpty(text3))
			{
				text = text3 + '\\' + text;
			}
			FtpStatus ftpStatus = this.GetResponseStatus();
			this.ftpResponse.BannerMessage = ftpStatus.StatusDescription;
			if (this.EnableSsl)
			{
				this.InitiateSecureConnection(ref this.controlStream);
				this.controlReader = new StreamReader(this.controlStream, Encoding.ASCII);
				ftpStatus = this.SendCommand("PBSZ", new string[]
				{
					"0"
				});
				int statusCode = (int)ftpStatus.StatusCode;
				if (statusCode < 200 || statusCode >= 300)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = this.SendCommand("PROT", new string[]
				{
					"P"
				});
				statusCode = (int)ftpStatus.StatusCode;
				if (statusCode < 200 || statusCode >= 300)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = new FtpStatus(FtpStatusCode.SendUserCommand, string.Empty);
			}
			if (ftpStatus.StatusCode != FtpStatusCode.SendUserCommand)
			{
				throw this.CreateExceptionFromResponse(ftpStatus);
			}
			ftpStatus = this.SendCommand("USER", new string[]
			{
				text
			});
			FtpStatusCode statusCode2 = ftpStatus.StatusCode;
			if (statusCode2 != FtpStatusCode.LoggedInProceed)
			{
				if (statusCode2 != FtpStatusCode.SendPasswordCommand)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
				ftpStatus = this.SendCommand("PASS", new string[]
				{
					text2
				});
				if (ftpStatus.StatusCode != FtpStatusCode.LoggedInProceed)
				{
					throw this.CreateExceptionFromResponse(ftpStatus);
				}
			}
			this.ftpResponse.WelcomeMessage = ftpStatus.StatusDescription;
			this.ftpResponse.UpdateStatus(ftpStatus);
		}

		private FtpStatus SendCommand(string command, params string[] parameters)
		{
			return this.SendCommand(true, command, parameters);
		}

		private FtpStatus SendCommand(bool waitResponse, string command, params string[] parameters)
		{
			string text = command;
			if (parameters.Length > 0)
			{
				text = text + " " + string.Join(" ", parameters);
			}
			text += "\r\n";
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			try
			{
				this.controlStream.Write(bytes, 0, bytes.Length);
			}
			catch (IOException)
			{
				return new FtpStatus(FtpStatusCode.ServiceNotAvailable, "Write failed");
			}
			if (!waitResponse)
			{
				return null;
			}
			FtpStatus responseStatus = this.GetResponseStatus();
			if (this.ftpResponse != null)
			{
				this.ftpResponse.UpdateStatus(responseStatus);
			}
			return responseStatus;
		}

		internal static FtpStatus ServiceNotAvailable()
		{
			return new FtpStatus(FtpStatusCode.ServiceNotAvailable, Locale.GetText("Invalid response from server"));
		}

		internal FtpStatus GetResponseStatus()
		{
			string text = null;
			try
			{
				text = this.controlReader.ReadLine();
			}
			catch (IOException)
			{
			}
			if (text == null || text.Length < 3)
			{
				return FtpWebRequest.ServiceNotAvailable();
			}
			int statusCode;
			if (!int.TryParse(text.Substring(0, 3), out statusCode))
			{
				return FtpWebRequest.ServiceNotAvailable();
			}
			if (text.Length > 3 && text[3] == '-')
			{
				string text2 = null;
				string value = statusCode.ToString() + ' ';
				for (;;)
				{
					text2 = null;
					try
					{
						text2 = this.controlReader.ReadLine();
					}
					catch (IOException)
					{
					}
					if (text2 == null)
					{
						break;
					}
					text = text + Environment.NewLine + text2;
					if (text2.StartsWith(value, StringComparison.Ordinal))
					{
						goto Block_8;
					}
				}
				return FtpWebRequest.ServiceNotAvailable();
				Block_8:;
			}
			return new FtpStatus((FtpStatusCode)statusCode, text);
		}

		private void InitiateSecureConnection(ref Stream stream)
		{
			FtpStatus ftpStatus = this.SendCommand("AUTH", new string[]
			{
				"TLS"
			});
			if (ftpStatus.StatusCode != FtpStatusCode.ServerWantsSecureSession)
			{
				throw this.CreateExceptionFromResponse(ftpStatus);
			}
			this.ChangeToSSLSocket(ref stream);
		}

		internal bool ChangeToSSLSocket(ref Stream stream)
		{
			System.Net.Security.SslStream sslStream = new System.Net.Security.SslStream(stream, true, this.callback, null);
			sslStream.AuthenticateAsClient(this.requestUri.Host, null, System.Security.Authentication.SslProtocols.Default, false);
			stream = sslStream;
			return true;
		}

		private bool InFinalState()
		{
			return this.State == FtpWebRequest.RequestState.Aborted || this.State == FtpWebRequest.RequestState.Error || this.State == FtpWebRequest.RequestState.Finished;
		}

		private bool InProgress()
		{
			return this.State != FtpWebRequest.RequestState.Before && !this.InFinalState();
		}

		internal void CheckIfAborted()
		{
			if (this.State == FtpWebRequest.RequestState.Aborted)
			{
				throw new WebException("Request aborted", WebExceptionStatus.RequestCanceled);
			}
		}

		private void CheckFinalState()
		{
			if (this.InFinalState())
			{
				throw new InvalidOperationException("Cannot change final state");
			}
		}

		private enum RequestState
		{
			Before,
			Scheduled,
			Connecting,
			Authenticating,
			OpeningData,
			TransferInProgress,
			Finished,
			Aborted,
			Error
		}
	}
}
