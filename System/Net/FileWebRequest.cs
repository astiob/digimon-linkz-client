using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Net
{
	/// <summary>Provides a file system implementation of the <see cref="T:System.Net.WebRequest" /> class.</summary>
	[Serializable]
	public class FileWebRequest : WebRequest, ISerializable
	{
		private System.Uri uri;

		private WebHeaderCollection webHeaders;

		private ICredentials credentials;

		private string connectionGroup;

		private long contentLength;

		private FileAccess fileAccess = FileAccess.Read;

		private string method = "GET";

		private IWebProxy proxy;

		private bool preAuthenticate;

		private int timeout = 100000;

		private Stream requestStream;

		private FileWebResponse webResponse;

		private AutoResetEvent requestEndEvent;

		private bool requesting;

		private bool asyncResponding;

		internal FileWebRequest(System.Uri uri)
		{
			this.uri = uri;
			this.webHeaders = new WebHeaderCollection();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.FileWebRequest" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information that is required to serialize the new <see cref="T:System.Net.FileWebRequest" /> object. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.FileWebRequest" /> object. </param>
		[Obsolete("Serialization is obsoleted for this type", false)]
		protected FileWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.webHeaders = (WebHeaderCollection)serializationInfo.GetValue("headers", typeof(WebHeaderCollection));
			this.proxy = (IWebProxy)serializationInfo.GetValue("proxy", typeof(IWebProxy));
			this.uri = (System.Uri)serializationInfo.GetValue("uri", typeof(System.Uri));
			this.connectionGroup = serializationInfo.GetString("connectionGroupName");
			this.method = serializationInfo.GetString("method");
			this.contentLength = serializationInfo.GetInt64("contentLength");
			this.timeout = serializationInfo.GetInt32("timeout");
			this.fileAccess = (FileAccess)((int)serializationInfo.GetValue("fileAccess", typeof(FileAccess)));
			this.preAuthenticate = serializationInfo.GetBoolean("preauthenticate");
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the required data to serialize the <see cref="T:System.Net.FileWebRequest" />.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized data for the <see cref="T:System.Net.FileWebRequest" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the destination of the serialized stream that is associated with the new <see cref="T:System.Net.FileWebRequest" />. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Gets or sets the name of the connection group for the request. This property is reserved for future use.</summary>
		/// <returns>The name of the connection group for the request.</returns>
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

		/// <summary>Gets or sets the content length of the data being sent.</summary>
		/// <returns>The number of bytes of request data being sent.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.FileWebRequest.ContentLength" /> is less than 0. </exception>
		public override long ContentLength
		{
			get
			{
				return this.contentLength;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentException("The Content-Length value must be greater than or equal to zero.", "value");
				}
				this.contentLength = value;
			}
		}

		/// <summary>Gets or sets the content type of the data being sent. This property is reserved for future use.</summary>
		/// <returns>The content type of the data being sent.</returns>
		public override string ContentType
		{
			get
			{
				return this.webHeaders["Content-Type"];
			}
			set
			{
				this.webHeaders["Content-Type"] = value;
			}
		}

		/// <summary>Gets or sets the credentials that are associated with this request. This property is reserved for future use.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that contains the authentication credentials that are associated with this request. The default is null.</returns>
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

		/// <summary>Gets a collection of the name/value pairs that are associated with the request. This property is reserved for future use.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains header name/value pairs associated with this request.</returns>
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.webHeaders;
			}
		}

		/// <summary>Gets or sets the protocol method used for the request. This property is reserved for future use.</summary>
		/// <returns>The protocol method to use in this request.</returns>
		public override string Method
		{
			get
			{
				return this.method;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					throw new ArgumentException("Cannot set null or blank methods on request.", "value");
				}
				this.method = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to preauthenticate a request. This property is reserved for future use.</summary>
		/// <returns>true to preauthenticate; otherwise, false.</returns>
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

		/// <summary>Gets or sets the network proxy to use for this request. This property is reserved for future use.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> that indicates the network proxy to use for this request.</returns>
		public override IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.proxy = value;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the request.</returns>
		public override System.Uri RequestUri
		{
			get
			{
				return this.uri;
			}
		}

		/// <summary>Gets or sets the length of time until the request times out.</summary>
		/// <returns>The time, in milliseconds, until the request times out, or the value <see cref="F:System.Threading.Timeout.Infinite" /> to indicate that the request does not time out.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than or equal to zero and is not <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
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
					throw new ArgumentOutOfRangeException("Timeout can be only set to 'System.Threading.Timeout.Infinite' or a value >= 0.");
				}
				this.timeout = value;
			}
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Default credentials are not supported for file Uniform Resource Identifiers (URIs).</exception>
		public override bool UseDefaultCredentials
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

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Cancels a request to an Internet resource.</summary>
		[MonoTODO]
		public override void Abort()
		{
			throw FileWebRequest.GetMustImplement();
		}

		/// <summary>Begins an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.FileWebRequest.Method" /> property is GET and the application writes to the stream. </exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is being used by a previous call to <see cref="M:System.Net.FileWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />. </exception>
		/// <exception cref="T:System.ApplicationException">No write stream is available. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			if (string.Compare("GET", this.method, true) == 0 || string.Compare("HEAD", this.method, true) == 0 || string.Compare("CONNECT", this.method, true) == 0)
			{
				throw new ProtocolViolationException("Cannot send a content-body with this verb-type.");
			}
			lock (this)
			{
				if (this.asyncResponding || this.webResponse != null)
				{
					throw new InvalidOperationException("This operation cannot be performed after the request has been submitted.");
				}
				if (this.requesting)
				{
					throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
				}
				this.requesting = true;
			}
			FileWebRequest.GetRequestStreamCallback getRequestStreamCallback = new FileWebRequest.GetRequestStreamCallback(this.GetRequestStreamInternal);
			return getRequestStreamCallback.BeginInvoke(callback, state);
		}

		/// <summary>Ends an asynchronous request for a <see cref="T:System.IO.Stream" /> instance that the application uses to write data.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> object that the application uses to write data.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that references the pending request for a stream. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			AsyncResult asyncResult2 = (AsyncResult)asyncResult;
			FileWebRequest.GetRequestStreamCallback getRequestStreamCallback = (FileWebRequest.GetRequestStreamCallback)asyncResult2.AsyncDelegate;
			return getRequestStreamCallback.EndInvoke(asyncResult);
		}

		/// <summary>Returns a <see cref="T:System.IO.Stream" /> object for writing data to the file system resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> for writing data to the file system resource.</returns>
		/// <exception cref="T:System.Net.WebException">The request times out. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override Stream GetRequestStream()
		{
			IAsyncResult asyncResult = this.BeginGetRequestStream(null, null);
			if (!asyncResult.AsyncWaitHandle.WaitOne(this.timeout, false))
			{
				throw new WebException("The request timed out", WebExceptionStatus.Timeout);
			}
			return this.EndGetRequestStream(asyncResult);
		}

		internal Stream GetRequestStreamInternal()
		{
			this.requestStream = new FileWebRequest.FileWebStream(this, FileMode.Create, FileAccess.Write, FileShare.Read);
			return this.requestStream;
		}

		/// <summary>Begins an asynchronous request for a file system resource.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate. </param>
		/// <param name="state">An object that contains state information for this request. </param>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.FileWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			lock (this)
			{
				if (this.asyncResponding)
				{
					throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
				}
				this.asyncResponding = true;
			}
			FileWebRequest.GetResponseCallback getResponseCallback = new FileWebRequest.GetResponseCallback(this.GetResponseInternal);
			return getResponseCallback.BeginInvoke(callback, state);
		}

		/// <summary>Ends an asynchronous request for a file system resource.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the file system resource.</returns>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that references the pending request for a response. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is null. </exception>
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			AsyncResult asyncResult2 = (AsyncResult)asyncResult;
			FileWebRequest.GetResponseCallback getResponseCallback = (FileWebRequest.GetResponseCallback)asyncResult2.AsyncDelegate;
			WebResponse result = getResponseCallback.EndInvoke(asyncResult);
			this.asyncResponding = false;
			return result;
		}

		/// <summary>Returns a response to a file system request.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the file system resource.</returns>
		/// <exception cref="T:System.Net.WebException">The request timed out. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override WebResponse GetResponse()
		{
			IAsyncResult asyncResult = this.BeginGetResponse(null, null);
			if (!asyncResult.AsyncWaitHandle.WaitOne(this.timeout, false))
			{
				throw new WebException("The request timed out", WebExceptionStatus.Timeout);
			}
			return this.EndGetResponse(asyncResult);
		}

		private WebResponse GetResponseInternal()
		{
			if (this.webResponse != null)
			{
				return this.webResponse;
			}
			lock (this)
			{
				if (this.requesting)
				{
					this.requestEndEvent = new AutoResetEvent(false);
				}
			}
			if (this.requestEndEvent != null)
			{
				this.requestEndEvent.WaitOne();
			}
			FileStream fileStream = null;
			try
			{
				fileStream = new FileWebRequest.FileWebStream(this, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			catch (Exception ex)
			{
				throw new WebException(ex.Message, ex);
			}
			this.webResponse = new FileWebResponse(this.uri, fileStream);
			return this.webResponse;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" />  that specifies the destination for this serialization. </param>
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("headers", this.webHeaders, typeof(WebHeaderCollection));
			serializationInfo.AddValue("proxy", this.proxy, typeof(IWebProxy));
			serializationInfo.AddValue("uri", this.uri, typeof(System.Uri));
			serializationInfo.AddValue("connectionGroupName", this.connectionGroup);
			serializationInfo.AddValue("method", this.method);
			serializationInfo.AddValue("contentLength", this.contentLength);
			serializationInfo.AddValue("timeout", this.timeout);
			serializationInfo.AddValue("fileAccess", this.fileAccess);
			serializationInfo.AddValue("preauthenticate", false);
		}

		internal void Close()
		{
			lock (this)
			{
				this.requesting = false;
				if (this.requestEndEvent != null)
				{
					this.requestEndEvent.Set();
				}
			}
		}

		internal class FileWebStream : FileStream
		{
			private FileWebRequest webRequest;

			internal FileWebStream(FileWebRequest webRequest, FileMode mode, FileAccess access, FileShare share) : base(webRequest.RequestUri.LocalPath, mode, access, share)
			{
				this.webRequest = webRequest;
			}

			public override void Close()
			{
				base.Close();
				FileWebRequest fileWebRequest = this.webRequest;
				this.webRequest = null;
				if (fileWebRequest != null)
				{
					fileWebRequest.Close();
				}
			}
		}

		private delegate Stream GetRequestStreamCallback();

		private delegate WebResponse GetResponseCallback();
	}
}
