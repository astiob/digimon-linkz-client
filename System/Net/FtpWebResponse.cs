using System;
using System.IO;

namespace System.Net
{
	/// <summary>Encapsulates a File Transfer Protocol (FTP) server's response to a request.</summary>
	public class FtpWebResponse : WebResponse
	{
		private Stream stream;

		private System.Uri uri;

		private FtpStatusCode statusCode;

		private DateTime lastModified = DateTime.MinValue;

		private string bannerMessage = string.Empty;

		private string welcomeMessage = string.Empty;

		private string exitMessage = string.Empty;

		private string statusDescription;

		private string method;

		private bool disposed;

		private FtpWebRequest request;

		internal long contentLength = -1L;

		internal FtpWebResponse(FtpWebRequest request, System.Uri uri, string method, bool keepAlive)
		{
			this.request = request;
			this.uri = uri;
			this.method = method;
		}

		internal FtpWebResponse(FtpWebRequest request, System.Uri uri, string method, FtpStatusCode statusCode, string statusDescription)
		{
			this.request = request;
			this.uri = uri;
			this.method = method;
			this.statusCode = statusCode;
			this.statusDescription = statusDescription;
		}

		internal FtpWebResponse(FtpWebRequest request, System.Uri uri, string method, FtpStatus status) : this(request, uri, method, status.StatusCode, status.StatusDescription)
		{
		}

		/// <summary>Gets the length of the data received from the FTP server.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that contains the number of bytes of data received from the FTP server. </returns>
		public override long ContentLength
		{
			get
			{
				return this.contentLength;
			}
		}

		/// <summary>Gets an empty <see cref="T:System.Net.WebHeaderCollection" /> object.</summary>
		/// <returns>An empty <see cref="T:System.Net.WebHeaderCollection" /> object.</returns>
		public override WebHeaderCollection Headers
		{
			get
			{
				return new WebHeaderCollection();
			}
		}

		/// <summary>Gets the URI that sent the response to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> instance that identifies the resource associated with this response.</returns>
		public override System.Uri ResponseUri
		{
			get
			{
				return this.uri;
			}
		}

		/// <summary>Gets the date and time that a file on an FTP server was last modified.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the last modified date and time for a file.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
			internal set
			{
				this.lastModified = value;
			}
		}

		/// <summary>Gets the message sent by the FTP server when a connection is established prior to logon.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the banner message sent by the server; otherwise, <see cref="F:System.String.Empty" /> if no message is sent.</returns>
		public string BannerMessage
		{
			get
			{
				return this.bannerMessage;
			}
			internal set
			{
				this.bannerMessage = value;
			}
		}

		/// <summary>Gets the message sent by the FTP server when authentication is complete.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the welcome message sent by the server; otherwise, <see cref="F:System.String.Empty" /> if no message is sent.</returns>
		public string WelcomeMessage
		{
			get
			{
				return this.welcomeMessage;
			}
			internal set
			{
				this.welcomeMessage = value;
			}
		}

		/// <summary>Gets the message sent by the server when the FTP session is ending.</summary>
		/// <returns>A <see cref="T:System.String" /> that contains the exit message sent by the server; otherwise, <see cref="F:System.String.Empty" /> if no message is sent.</returns>
		public string ExitMessage
		{
			get
			{
				return this.exitMessage;
			}
			internal set
			{
				this.exitMessage = value;
			}
		}

		/// <summary>Gets the most recent status code sent from the FTP server.</summary>
		/// <returns>An <see cref="T:System.Net.FtpStatusCode" /> value that indicates the most recent status code returned with this response.</returns>
		public FtpStatusCode StatusCode
		{
			get
			{
				return this.statusCode;
			}
			private set
			{
				this.statusCode = value;
			}
		}

		/// <summary>Gets text that describes a status code sent from the FTP server.</summary>
		/// <returns>A <see cref="T:System.String" /> instance that contains the status code and message returned with this response.</returns>
		public string StatusDescription
		{
			get
			{
				return this.statusDescription;
			}
			private set
			{
				this.statusDescription = value;
			}
		}

		/// <summary>Frees the resources held by the response.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override void Close()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (this.stream != null)
			{
				this.stream.Close();
				if (this.stream == Stream.Null)
				{
					this.request.OperationCompleted();
				}
			}
			this.stream = null;
		}

		/// <summary>Retrieves the stream that contains response data sent from an FTP server.</summary>
		/// <returns>A readable <see cref="T:System.IO.Stream" /> instance that contains data returned with the response; otherwise, <see cref="F:System.IO.Stream.Null" /> if no response data was returned by the server.</returns>
		/// <exception cref="T:System.InvalidOperationException">The response did not return a data stream. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override Stream GetResponseStream()
		{
			if (this.stream == null)
			{
				return Stream.Null;
			}
			if (this.method != "RETR" && this.method != "NLST")
			{
				this.CheckDisposed();
			}
			return this.stream;
		}

		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
			set
			{
				this.stream = value;
			}
		}

		internal void UpdateStatus(FtpStatus status)
		{
			this.statusCode = status.StatusCode;
			this.statusDescription = status.StatusDescription;
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		internal bool IsFinal()
		{
			return this.statusCode >= FtpStatusCode.CommandOK;
		}
	}
}
