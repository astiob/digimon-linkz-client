using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace System.Net
{
	/// <summary>Provides common methods for sending data to and receiving data from a resource identified by a URI.</summary>
	[ComVisible(true)]
	public class WebClient : System.ComponentModel.Component
	{
		private static readonly string urlEncodedCType = "application/x-www-form-urlencoded";

		private static byte[] hexBytes = new byte[16];

		private ICredentials credentials;

		private WebHeaderCollection headers;

		private WebHeaderCollection responseHeaders;

		private System.Uri baseAddress;

		private string baseString;

		private System.Collections.Specialized.NameValueCollection queryString;

		private bool is_busy;

		private bool async;

		private Thread async_thread;

		private Encoding encoding = Encoding.Default;

		private IWebProxy proxy;

		static WebClient()
		{
			int num = 0;
			int i = 48;
			while (i <= 57)
			{
				WebClient.hexBytes[num] = (byte)i;
				i++;
				num++;
			}
			int j = 97;
			while (j <= 102)
			{
				WebClient.hexBytes[num] = (byte)j;
				j++;
				num++;
			}
		}

		/// <summary>Occurs when an asynchronous data download operation completes.</summary>
		public event DownloadDataCompletedEventHandler DownloadDataCompleted;

		/// <summary>Occurs when an asynchronous file download operation completes.</summary>
		public event System.ComponentModel.AsyncCompletedEventHandler DownloadFileCompleted;

		/// <summary>Occurs when an asynchronous download operation successfully transfers some or all of the data.</summary>
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		/// <summary>Occurs when an asynchronous resource-download operation completes.</summary>
		public event DownloadStringCompletedEventHandler DownloadStringCompleted;

		/// <summary>Occurs when an asynchronous operation to open a stream containing a resource completes.</summary>
		public event OpenReadCompletedEventHandler OpenReadCompleted;

		/// <summary>Occurs when an asynchronous operation to open a stream to write data to a resource completes.</summary>
		public event OpenWriteCompletedEventHandler OpenWriteCompleted;

		/// <summary>Occurs when an asynchronous data-upload operation completes.</summary>
		public event UploadDataCompletedEventHandler UploadDataCompleted;

		/// <summary>Occurs when an asynchronous file-upload operation completes.</summary>
		public event UploadFileCompletedEventHandler UploadFileCompleted;

		/// <summary>Occurs when an asynchronous upload operation successfully transfers some or all of the data.</summary>
		public event UploadProgressChangedEventHandler UploadProgressChanged;

		/// <summary>Occurs when an asynchronous string-upload operation completes.</summary>
		public event UploadStringCompletedEventHandler UploadStringCompleted;

		/// <summary>Occurs when an asynchronous upload of a name/value collection completes.</summary>
		public event UploadValuesCompletedEventHandler UploadValuesCompleted;

		/// <summary>Gets or sets the base URI for requests made by a <see cref="T:System.Net.WebClient" />.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the base URI for requests made by a <see cref="T:System.Net.WebClient" /> or <see cref="F:System.String.Empty" /> if no base address has been specified.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.WebClient.BaseAddress" /> is set to an invalid URI. The inner exception may contain information that will help you locate the error.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public string BaseAddress
		{
			get
			{
				if (this.baseString == null && this.baseAddress == null)
				{
					return string.Empty;
				}
				this.baseString = this.baseAddress.ToString();
				return this.baseString;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.baseAddress = null;
				}
				else
				{
					this.baseAddress = new System.Uri(value);
				}
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets or sets the application's cache policy for any resources obtained by this WebClient instance using <see cref="T:System.Net.WebRequest" /> objects.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.RequestCachePolicy" /> object that represents the application's caching requirements.</returns>
		[MonoTODO]
		public System.Net.Cache.RequestCachePolicy CachePolicy
		{
			get
			{
				throw WebClient.GetMustImplement();
			}
			set
			{
				throw WebClient.GetMustImplement();
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether the <see cref="P:System.Net.CredentialCache.DefaultCredentials" /> are sent with requests.</summary>
		/// <returns>true if the default credentials are used; otherwise false. The default value is false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		[MonoTODO]
		public bool UseDefaultCredentials
		{
			get
			{
				throw WebClient.GetMustImplement();
			}
			set
			{
				throw WebClient.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the network credentials that are sent to the host and used to authenticate the request.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> containing the authentication credentials for the request. The default is null.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public ICredentials Credentials
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

		/// <summary>Gets or sets a collection of header name/value pairs associated with the request.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> containing header name/value pairs associated with this request.</returns>
		public WebHeaderCollection Headers
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new WebHeaderCollection();
				}
				return this.headers;
			}
			set
			{
				this.headers = value;
			}
		}

		/// <summary>Gets or sets a collection of query name/value pairs associated with the request.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> that contains query name/value pairs associated with the request. If no pairs are associated with the request, the value is an empty <see cref="T:System.Collections.Specialized.NameValueCollection" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public System.Collections.Specialized.NameValueCollection QueryString
		{
			get
			{
				if (this.queryString == null)
				{
					this.queryString = new System.Collections.Specialized.NameValueCollection();
				}
				return this.queryString;
			}
			set
			{
				this.queryString = value;
			}
		}

		/// <summary>Gets a collection of header name/value pairs associated with the response.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> containing header name/value pairs associated with the response, or null if no response has been received.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public WebHeaderCollection ResponseHeaders
		{
			get
			{
				return this.responseHeaders;
			}
		}

		/// <summary>Gets and sets the <see cref="T:System.Text.Encoding" /> used to upload and download strings.</summary>
		/// <returns>A <see cref="T:System.Text.Encoding" /> that is used to encode strings. The default value of this property is the encoding returned by <see cref="P:System.Text.Encoding.Default" />.</returns>
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Encoding");
				}
				this.encoding = value;
			}
		}

		/// <summary>Gets or sets the proxy used by this <see cref="T:System.Net.WebClient" /> object.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> instance used to send requests.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.WebClient.Proxy" /> is set to null. </exception>
		public IWebProxy Proxy
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

		/// <summary>Gets whether a Web request is in progress.</summary>
		/// <returns>true if the Web request is still in progress; otherwise false.</returns>
		public bool IsBusy
		{
			get
			{
				return this.is_busy;
			}
		}

		private void CheckBusy()
		{
			if (this.IsBusy)
			{
				throw new NotSupportedException("WebClient does not support conccurent I/O operations.");
			}
		}

		private void SetBusy()
		{
			lock (this)
			{
				this.CheckBusy();
				this.is_busy = true;
			}
		}

		/// <summary>Downloads the resource with the specified URI as a <see cref="T:System.Byte" /> array.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <param name="address">The URI from which to download data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading data. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] DownloadData(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.DownloadData(this.CreateUri(address));
		}

		/// <summary>Downloads the resource with the specified URI as a <see cref="T:System.Byte" /> array.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <param name="address">The URI represented by the <see cref="T:System.Uri" />  object, from which to download data.</param>
		public byte[] DownloadData(System.Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			byte[] result;
			try
			{
				this.SetBusy();
				this.async = false;
				result = this.DownloadDataCore(address, null);
			}
			finally
			{
				this.is_busy = false;
			}
			return result;
		}

		private byte[] DownloadDataCore(System.Uri address, object userToken)
		{
			WebRequest webRequest = null;
			byte[] result;
			try
			{
				webRequest = this.SetupRequest(address);
				WebResponse webResponse = this.GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				result = this.ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
			}
			catch (ThreadInterruptedException)
			{
				if (webRequest != null)
				{
					webRequest.Abort();
				}
				throw;
			}
			catch (WebException ex)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			return result;
		}

		/// <summary>Downloads the resource with the specified URI to a local file.</summary>
		/// <param name="address">The URI from which to download data. </param>
		/// <param name="fileName">The name of the local file that is to receive the data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="filename" /> is null or <see cref="F:System.String.Empty" />.-or-The file does not exist.-or- An error occurred while downloading data. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void DownloadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.DownloadFile(this.CreateUri(address), fileName);
		}

		/// <summary>Downloads the resource with the specified URI to a local file.</summary>
		/// <param name="address">The URI specified as a <see cref="T:System.String" />, from which to download data. </param>
		/// <param name="fileName">The name of the local file that is to receive the data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="filename" /> is null or <see cref="F:System.String.Empty" />.-or- The file does not exist. -or- An error occurred while downloading data. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		public void DownloadFile(System.Uri address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			try
			{
				this.SetBusy();
				this.async = false;
				this.DownloadFileCore(address, fileName, null);
			}
			catch (WebException ex)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			finally
			{
				this.is_busy = false;
			}
		}

		private void DownloadFileCore(System.Uri address, string fileName, object userToken)
		{
			WebRequest webRequest = null;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				try
				{
					webRequest = this.SetupRequest(address);
					WebResponse webResponse = this.GetWebResponse(webRequest);
					Stream responseStream = webResponse.GetResponseStream();
					int num = (int)webResponse.ContentLength;
					int num2 = (num > -1 && num <= 32768) ? num : 32768;
					byte[] array = new byte[num2];
					long num3 = 0L;
					int num4;
					while ((num4 = responseStream.Read(array, 0, num2)) != 0)
					{
						if (this.async)
						{
							num3 += (long)num4;
							this.OnDownloadProgressChanged(new DownloadProgressChangedEventArgs(num3, webResponse.ContentLength, userToken));
						}
						fileStream.Write(array, 0, num4);
					}
				}
				catch (ThreadInterruptedException)
				{
					if (webRequest != null)
					{
						webRequest.Abort();
					}
					throw;
				}
			}
		}

		/// <summary>Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <param name="address">The URI specified as a <see cref="T:System.String" /> from which to download data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, <paramref name="address" /> is invalid.-or- An error occurred while downloading data. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream OpenRead(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenRead(this.CreateUri(address));
		}

		/// <summary>Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.Uri" /></summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <param name="address">The URI specified as a <see cref="T:System.Uri" /> from which to download data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, <paramref name="address" /> is invalid.-or- An error occurred while downloading data. </exception>
		public Stream OpenRead(System.Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			Stream responseStream;
			try
			{
				this.SetBusy();
				this.async = false;
				WebRequest request = this.SetupRequest(address);
				WebResponse webResponse = this.GetWebResponse(request);
				responseStream = webResponse.GetResponseStream();
			}
			catch (WebException ex)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			finally
			{
				this.is_busy = false;
			}
			return responseStream;
		}

		/// <summary>Opens a stream for writing data to the specified resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream OpenWrite(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenWrite(this.CreateUri(address));
		}

		/// <summary>Opens a stream for writing data to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public Stream OpenWrite(string address, string method)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.OpenWrite(this.CreateUri(address), method);
		}

		/// <summary>Opens a stream for writing data to the specified resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		public Stream OpenWrite(System.Uri address)
		{
			return this.OpenWrite(address, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource, by using the specified method.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		public Stream OpenWrite(System.Uri address, string method)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			Stream requestStream;
			try
			{
				this.SetBusy();
				this.async = false;
				WebRequest webRequest = this.SetupRequest(address, method, true);
				requestStream = webRequest.GetRequestStream();
			}
			catch (WebException ex)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			finally
			{
				this.is_busy = false;
			}
			return requestStream;
		}

		private string DetermineMethod(System.Uri address, string method, bool is_upload)
		{
			if (method != null)
			{
				return method;
			}
			if (address.Scheme == System.Uri.UriSchemeFtp)
			{
				return (!is_upload) ? "RETR" : "STOR";
			}
			return (!is_upload) ? "GET" : "POST";
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null. -or-An error occurred while sending the data.-or- There was no response from the server hosting the resource. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadData(string address, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadData(this.CreateUri(address), data);
		}

		/// <summary>Uploads a data buffer to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while uploading the data.-or- There was no response from the server hosting the resource. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadData(string address, string method, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadData(this.CreateUri(address), method, data);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null. -or-An error occurred while sending the data.-or- There was no response from the server hosting the resource. </exception>
		public byte[] UploadData(System.Uri address, byte[] data)
		{
			return this.UploadData(address, null, data);
		}

		/// <summary>Uploads a data buffer to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while uploading the data.-or- There was no response from the server hosting the resource. </exception>
		public byte[] UploadData(System.Uri address, string method, byte[] data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] result;
			try
			{
				this.SetBusy();
				this.async = false;
				result = this.UploadDataCore(address, method, data, null);
			}
			catch (WebException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			finally
			{
				this.is_busy = false;
			}
			return result;
		}

		private byte[] UploadDataCore(System.Uri address, string method, byte[] data, object userToken)
		{
			WebRequest webRequest = this.SetupRequest(address, method, true);
			byte[] result;
			try
			{
				int num = data.Length;
				webRequest.ContentLength = (long)num;
				using (Stream requestStream = webRequest.GetRequestStream())
				{
					requestStream.Write(data, 0, num);
				}
				WebResponse webResponse = this.GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				result = this.ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
			}
			catch (ThreadInterruptedException)
			{
				if (webRequest != null)
				{
					webRequest.Abort();
				}
				throw;
			}
			return result;
		}

		/// <summary>Uploads the specified local file to a resource with the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file. For example, ftp://localhost/samplefile.txt.</param>
		/// <param name="fileName">The file to send to the resource. For example, "samplefile.txt".</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadFile(string address, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadFile(this.CreateUri(address), fileName);
		}

		/// <summary>Uploads the specified local file to a resource with the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file. For example, ftp://localhost/samplefile.txt.</param>
		/// <param name="fileName">The file to send to the resource. For example, "samplefile.txt".</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public byte[] UploadFile(System.Uri address, string fileName)
		{
			return this.UploadFile(address, null, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadFile(string address, string method, string fileName)
		{
			return this.UploadFile(this.CreateUri(address), method, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.-or- An error occurred while uploading the file.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public byte[] UploadFile(System.Uri address, string method, string fileName)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			byte[] result;
			try
			{
				this.SetBusy();
				this.async = false;
				result = this.UploadFileCore(address, method, fileName, null);
			}
			catch (WebException ex)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			finally
			{
				this.is_busy = false;
			}
			return result;
		}

		private byte[] UploadFileCore(System.Uri address, string method, string fileName, object userToken)
		{
			string text = this.Headers["Content-Type"];
			if (text != null)
			{
				string text2 = text.ToLower();
				if (text2.StartsWith("multipart/"))
				{
					throw new WebException("Content-Type cannot be set to a multipart type for this request.");
				}
			}
			else
			{
				text = "application/octet-stream";
			}
			string text3 = "------------" + DateTime.Now.Ticks.ToString("x");
			this.Headers["Content-Type"] = string.Format("multipart/form-data; boundary={0}", text3);
			Stream stream = null;
			Stream stream2 = null;
			byte[] result = null;
			fileName = Path.GetFullPath(fileName);
			WebRequest webRequest = null;
			try
			{
				stream2 = File.OpenRead(fileName);
				webRequest = this.SetupRequest(address, method, true);
				stream = webRequest.GetRequestStream();
				byte[] bytes = Encoding.ASCII.GetBytes("--" + text3 + "\r\n");
				stream.Write(bytes, 0, bytes.Length);
				string s = string.Format("Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\nContent-Type: {1}\r\n\r\n", Path.GetFileName(fileName), text);
				byte[] bytes2 = Encoding.UTF8.GetBytes(s);
				stream.Write(bytes2, 0, bytes2.Length);
				byte[] buffer = new byte[4096];
				int count;
				while ((count = stream2.Read(buffer, 0, 4096)) != 0)
				{
					stream.Write(buffer, 0, count);
				}
				stream.WriteByte(13);
				stream.WriteByte(10);
				stream.Write(bytes, 0, bytes.Length);
				stream.Close();
				stream = null;
				WebResponse webResponse = this.GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				result = this.ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
			}
			catch (ThreadInterruptedException)
			{
				if (webRequest != null)
				{
					webRequest.Abort();
				}
				throw;
			}
			finally
			{
				if (stream2 != null)
				{
					stream2.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
			}
			return result;
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- There was no response from the server hosting the resource.-or- An error occurred while opening the stream.-or- The Content-type header is not null or "application/x-www-form-urlencoded". </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadValues(string address, System.Collections.Specialized.NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadValues(this.CreateUri(address), data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header value is not null and is not application/x-www-form-urlencoded. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public byte[] UploadValues(string address, string method, System.Collections.Specialized.NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.UploadValues(this.CreateUri(address), method, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- There was no response from the server hosting the resource.-or- An error occurred while opening the stream.-or- The Content-type header is not null or "application/x-www-form-urlencoded". </exception>
		public byte[] UploadValues(System.Uri address, System.Collections.Specialized.NameValueCollection data)
		{
			return this.UploadValues(address, null, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.</summary>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <param name="address">The URI of the resource to receive the collection. </param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.-or- <paramref name="data" /> is null.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header value is not null and is not application/x-www-form-urlencoded. </exception>
		public byte[] UploadValues(System.Uri address, string method, System.Collections.Specialized.NameValueCollection data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] result;
			try
			{
				this.SetBusy();
				this.async = false;
				result = this.UploadValuesCore(address, method, data, null);
			}
			catch (WebException ex)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new WebException("An error occurred performing a WebClient request.", innerException);
			}
			finally
			{
				this.is_busy = false;
			}
			return result;
		}

		private byte[] UploadValuesCore(System.Uri uri, string method, System.Collections.Specialized.NameValueCollection data, object userToken)
		{
			string text = this.Headers["Content-Type"];
			if (text != null && string.Compare(text, WebClient.urlEncodedCType, true) != 0)
			{
				throw new WebException("Content-Type header cannot be changed from its default value for this request.");
			}
			this.Headers["Content-Type"] = WebClient.urlEncodedCType;
			WebRequest webRequest = this.SetupRequest(uri, method, true);
			byte[] result;
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				foreach (object obj in data)
				{
					string text2 = (string)obj;
					byte[] bytes = Encoding.UTF8.GetBytes(text2);
					WebClient.UrlEncodeAndWrite(memoryStream, bytes);
					memoryStream.WriteByte(61);
					bytes = Encoding.UTF8.GetBytes(data[text2]);
					WebClient.UrlEncodeAndWrite(memoryStream, bytes);
					memoryStream.WriteByte(38);
				}
				int num = (int)memoryStream.Length;
				if (num > 0)
				{
					memoryStream.SetLength((long)(--num));
				}
				byte[] buffer = memoryStream.GetBuffer();
				webRequest.ContentLength = (long)num;
				using (Stream requestStream = webRequest.GetRequestStream())
				{
					requestStream.Write(buffer, 0, num);
				}
				memoryStream.Close();
				WebResponse webResponse = this.GetWebResponse(webRequest);
				Stream responseStream = webResponse.GetResponseStream();
				result = this.ReadAll(responseStream, (int)webResponse.ContentLength, userToken);
			}
			catch (ThreadInterruptedException)
			{
				webRequest.Abort();
				throw;
			}
			return result;
		}

		/// <summary>Downloads the requested resource as a <see cref="T:System.String" />. The resource to download is specified as a <see cref="T:System.String" /> containing the URI.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the requested resource.</returns>
		/// <param name="address">A <see cref="T:System.String" /> containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string DownloadString(string address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.encoding.GetString(this.DownloadData(this.CreateUri(address)));
		}

		/// <summary>Downloads the requested resource as a <see cref="T:System.String" />. The resource to download is specified as a <see cref="T:System.Uri" />.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the requested resource.</returns>
		/// <param name="address">A <see cref="T:System.Uri" /> object containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		public string DownloadString(System.Uri address)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			return this.encoding.GetString(this.DownloadData(this.CreateUri(address)));
		}

		/// <summary>Uploads the specified string to the specified resource, using the POST method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the string. For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string UploadString(string address, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = this.UploadData(address, this.encoding.GetBytes(data));
			return this.encoding.GetString(bytes);
		}

		/// <summary>Uploads the specified string to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the file. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method. </param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public string UploadString(string address, string method, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = this.UploadData(address, method, this.encoding.GetBytes(data));
			return this.encoding.GetString(bytes);
		}

		/// <summary>Uploads the specified string to the specified resource, using the POST method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the string. For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public string UploadString(System.Uri address, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = this.UploadData(address, this.encoding.GetBytes(data));
			return this.encoding.GetString(bytes);
		}

		/// <summary>Uploads the specified string to the specified resource, using the specified method.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <param name="address">The URI of the resource to receive the file. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method. </param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		public string UploadString(System.Uri address, string method, string data)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			byte[] bytes = this.UploadData(address, method, this.encoding.GetBytes(data));
			return this.encoding.GetString(bytes);
		}

		private System.Uri CreateUri(string address)
		{
			return this.MakeUri(address);
		}

		private System.Uri CreateUri(System.Uri address)
		{
			string query = address.Query;
			if (string.IsNullOrEmpty(query))
			{
				query = this.GetQueryString(true);
			}
			if (this.baseAddress == null && query == null)
			{
				return address;
			}
			if (this.baseAddress == null)
			{
				return new System.Uri(address.ToString() + query, query != null);
			}
			if (query == null)
			{
				return new System.Uri(this.baseAddress, address.ToString());
			}
			return new System.Uri(this.baseAddress, address.ToString() + query, query != null);
		}

		private string GetQueryString(bool add_qmark)
		{
			if (this.queryString == null || this.queryString.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (add_qmark)
			{
				stringBuilder.Append('?');
			}
			foreach (object obj in this.queryString)
			{
				string text = (string)obj;
				stringBuilder.AppendFormat("{0}={1}&", text, this.UrlEncode(this.queryString[text]));
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Length--;
			}
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		private System.Uri MakeUri(string path)
		{
			string text = this.GetQueryString(true);
			if (this.baseAddress == null && text == null)
			{
				try
				{
					return new System.Uri(path);
				}
				catch (ArgumentNullException)
				{
					if (Environment.UnityWebSecurityEnabled)
					{
						throw;
					}
					path = Path.GetFullPath(path);
					return new System.Uri("file://" + path);
				}
				catch (System.UriFormatException)
				{
					if (Environment.UnityWebSecurityEnabled)
					{
						throw;
					}
					path = Path.GetFullPath(path);
					return new System.Uri("file://" + path);
				}
			}
			if (this.baseAddress == null)
			{
				return new System.Uri(path + text, text != null);
			}
			if (text == null)
			{
				return new System.Uri(this.baseAddress, path);
			}
			return new System.Uri(this.baseAddress, path + text, text != null);
		}

		private WebRequest SetupRequest(System.Uri uri)
		{
			WebRequest webRequest = this.GetWebRequest(uri);
			if (this.Proxy != null)
			{
				webRequest.Proxy = this.Proxy;
			}
			webRequest.Credentials = this.credentials;
			if (this.headers != null && this.headers.Count != 0 && webRequest is HttpWebRequest)
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)webRequest;
				string text = this.headers["Expect"];
				string text2 = this.headers["Content-Type"];
				string text3 = this.headers["Accept"];
				string text4 = this.headers["Connection"];
				string text5 = this.headers["User-Agent"];
				string text6 = this.headers["Referer"];
				this.headers.RemoveInternal("Expect");
				this.headers.RemoveInternal("Content-Type");
				this.headers.RemoveInternal("Accept");
				this.headers.RemoveInternal("Connection");
				this.headers.RemoveInternal("Referer");
				this.headers.RemoveInternal("User-Agent");
				webRequest.Headers = this.headers;
				if (text != null && text.Length > 0)
				{
					httpWebRequest.Expect = text;
				}
				if (text3 != null && text3.Length > 0)
				{
					httpWebRequest.Accept = text3;
				}
				if (text2 != null && text2.Length > 0)
				{
					httpWebRequest.ContentType = text2;
				}
				if (text4 != null && text4.Length > 0)
				{
					httpWebRequest.Connection = text4;
				}
				if (text5 != null && text5.Length > 0)
				{
					httpWebRequest.UserAgent = text5;
				}
				if (text6 != null && text6.Length > 0)
				{
					httpWebRequest.Referer = text6;
				}
			}
			this.responseHeaders = null;
			return webRequest;
		}

		private WebRequest SetupRequest(System.Uri uri, string method, bool is_upload)
		{
			WebRequest webRequest = this.SetupRequest(uri);
			webRequest.Method = this.DetermineMethod(uri, method, is_upload);
			return webRequest;
		}

		private byte[] ReadAll(Stream stream, int length, object userToken)
		{
			MemoryStream memoryStream = null;
			bool flag = length == -1;
			int num = (!flag) ? length : 8192;
			if (flag)
			{
				memoryStream = new MemoryStream();
			}
			int num2 = 0;
			byte[] array = new byte[num];
			int num3;
			while ((num3 = stream.Read(array, num2, num)) != 0)
			{
				if (flag)
				{
					memoryStream.Write(array, 0, num3);
				}
				else
				{
					num2 += num3;
					num -= num3;
				}
				if (this.async)
				{
					this.OnDownloadProgressChanged(new DownloadProgressChangedEventArgs((long)num3, (long)length, userToken));
				}
			}
			if (flag)
			{
				return memoryStream.ToArray();
			}
			return array;
		}

		private string UrlEncode(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int length = str.Length;
			for (int i = 0; i < length; i++)
			{
				char c = str[i];
				if (c == ' ')
				{
					stringBuilder.Append('+');
				}
				else if ((c < '0' && c != '-' && c != '.') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a' && c != '_') || c > 'z')
				{
					stringBuilder.Append('%');
					int num = (int)(c >> 4);
					stringBuilder.Append((char)WebClient.hexBytes[num]);
					num = (int)(c & '\u000f');
					stringBuilder.Append((char)WebClient.hexBytes[num]);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		private static void UrlEncodeAndWrite(Stream stream, byte[] bytes)
		{
			if (bytes == null)
			{
				return;
			}
			int num = bytes.Length;
			if (num == 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				char c = (char)bytes[i];
				if (c == ' ')
				{
					stream.WriteByte(43);
				}
				else if ((c < '0' && c != '-' && c != '.') || (c < 'A' && c > '9') || (c > 'Z' && c < 'a' && c != '_') || c > 'z')
				{
					stream.WriteByte(37);
					int num2 = (int)(c >> 4);
					stream.WriteByte(WebClient.hexBytes[num2]);
					num2 = (int)(c & '\u000f');
					stream.WriteByte(WebClient.hexBytes[num2]);
				}
				else
				{
					stream.WriteByte((byte)c);
				}
			}
		}

		/// <summary>Cancels a pending asynchronous operation.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void CancelAsync()
		{
			lock (this)
			{
				if (this.async_thread != null)
				{
					Thread thread = this.async_thread;
					this.CompleteAsync();
					thread.Interrupt();
				}
			}
		}

		private void CompleteAsync()
		{
			lock (this)
			{
				this.is_busy = false;
				this.async_thread = null;
			}
		}

		/// <summary>Downloads the specified resource as a <see cref="T:System.Byte" /> array. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadDataAsync(System.Uri address)
		{
			this.DownloadDataAsync(address, null);
		}

		/// <summary>Downloads the specified resource as a <see cref="T:System.Byte" /> array. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadDataAsync(System.Uri address, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = this.DownloadDataCore((System.Uri)array[0], array[1]);
						this.OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(result, null, false, array[1]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(null, null, true, array[1]));
						throw;
					}
					catch (Exception error)
					{
						this.OnDownloadDataCompleted(new DownloadDataCompletedEventArgs(null, error, false, array[1]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Downloads, to a local file, the resource with the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to download. </param>
		/// <param name="fileName">The name of the file to be placed on the local computer. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		public void DownloadFileAsync(System.Uri address, string fileName)
		{
			this.DownloadFileAsync(address, fileName, null);
		}

		/// <summary>Downloads, to a local file, the resource with the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to download. </param>
		/// <param name="fileName">The name of the file to be placed on the local computer. </param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		public void DownloadFileAsync(System.Uri address, string fileName, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						this.DownloadFileCore((System.Uri)array[0], (string)array[1], array[2]);
						this.OnDownloadFileCompleted(new System.ComponentModel.AsyncCompletedEventArgs(null, false, array[2]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnDownloadFileCompleted(new System.ComponentModel.AsyncCompletedEventArgs(null, true, array[2]));
					}
					catch (Exception error)
					{
						this.OnDownloadFileCompleted(new System.ComponentModel.AsyncCompletedEventArgs(error, false, array[2]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					fileName,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Downloads the resource specified as a <see cref="T:System.Uri" />. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadStringAsync(System.Uri address)
		{
			this.DownloadStringAsync(address, null);
		}

		/// <summary>Downloads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while downloading the resource. </exception>
		public void DownloadStringAsync(System.Uri address, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						string @string = this.encoding.GetString(this.DownloadDataCore((System.Uri)array[0], array[1]));
						this.OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(@string, null, false, array[1]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(null, null, true, array[1]));
					}
					catch (Exception error)
					{
						this.OnDownloadStringCompleted(new DownloadStringCompletedEventArgs(null, error, false, array[1]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Opens a readable stream containing the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.-or- An error occurred while downloading the resource. -or- An error occurred while opening the stream.</exception>
		public void OpenReadAsync(System.Uri address)
		{
			this.OpenReadAsync(address, null);
		}

		/// <summary>Opens a readable stream containing the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.-or- An error occurred while downloading the resource. -or- An error occurred while opening the stream.</exception>
		public void OpenReadAsync(System.Uri address, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					WebRequest webRequest = null;
					try
					{
						webRequest = this.SetupRequest((System.Uri)array[0]);
						WebResponse webResponse = this.GetWebResponse(webRequest);
						Stream responseStream = webResponse.GetResponseStream();
						this.OnOpenReadCompleted(new OpenReadCompletedEventArgs(responseStream, null, false, array[1]));
					}
					catch (ThreadInterruptedException)
					{
						if (webRequest != null)
						{
							webRequest.Abort();
						}
						this.OnOpenReadCompleted(new OpenReadCompletedEventArgs(null, null, true, array[1]));
					}
					catch (Exception error)
					{
						this.OnOpenReadCompleted(new OpenReadCompletedEventArgs(null, error, false, array[1]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Opens a stream for writing data to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data. </param>
		public void OpenWriteAsync(System.Uri address)
		{
			this.OpenWriteAsync(address, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		public void OpenWriteAsync(System.Uri address, string method)
		{
			this.OpenWriteAsync(address, method, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream. </exception>
		public void OpenWriteAsync(System.Uri address, string method, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					WebRequest webRequest = null;
					try
					{
						webRequest = this.SetupRequest((System.Uri)array[0], (string)array[1], true);
						Stream requestStream = webRequest.GetRequestStream();
						this.OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(requestStream, null, false, array[2]));
					}
					catch (ThreadInterruptedException)
					{
						if (webRequest != null)
						{
							webRequest.Abort();
						}
						this.OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(null, null, true, array[2]));
					}
					catch (Exception error)
					{
						this.OnOpenWriteCompleted(new OpenWriteCompletedEventArgs(null, error, false, array[2]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					method,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data. </param>
		/// <param name="data">The data buffer to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource. </exception>
		public void UploadDataAsync(System.Uri address, byte[] data)
		{
			this.UploadDataAsync(address, null, data);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource. </exception>
		public void UploadDataAsync(System.Uri address, string method, byte[] data)
		{
			this.UploadDataAsync(address, method, data, null);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the specified method and identifying token.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource. </exception>
		public void UploadDataAsync(System.Uri address, string method, byte[] data, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = this.UploadDataCore((System.Uri)array[0], (string)array[1], (byte[])array[2], array[3]);
						this.OnUploadDataCompleted(new UploadDataCompletedEventArgs(result, null, false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnUploadDataCompleted(new UploadDataCompletedEventArgs(null, null, true, array[3]));
					}
					catch (Exception error)
					{
						this.OnUploadDataCompleted(new UploadDataCompletedEventArgs(null, error, false, array[3]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					method,
					data,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public void UploadFileAsync(System.Uri address, string fileName)
		{
			this.UploadFileAsync(address, null, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource. </param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public void UploadFileAsync(System.Uri address, string method, string fileName)
		{
			this.UploadFileAsync(address, method, fileName, null);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- <paramref name="fileName" /> is null, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.-or- An error occurred while opening the stream.-or- There was no response from the server hosting the resource.-or- The Content-type header begins with multipart. </exception>
		public void UploadFileAsync(System.Uri address, string method, string fileName, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			lock (this)
			{
				this.SetBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = this.UploadFileCore((System.Uri)array[0], (string)array[1], (string)array[2], array[3]);
						this.OnUploadFileCompleted(new UploadFileCompletedEventArgs(result, null, false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnUploadFileCompleted(new UploadFileCompletedEventArgs(null, null, true, array[3]));
					}
					catch (Exception error)
					{
						this.OnUploadFileCompleted(new UploadFileCompletedEventArgs(null, error, false, array[3]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					method,
					fileName,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page. </param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadStringAsync(System.Uri address, string data)
		{
			this.UploadStringAsync(address, null, data);
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadStringAsync(System.Uri address, string method, string data)
		{
			this.UploadStringAsync(address, method, data, null);
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadStringAsync(System.Uri address, string method, string data, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			lock (this)
			{
				this.CheckBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						string result = this.UploadString((System.Uri)array[0], (string)array[1], (string)array[2]);
						this.OnUploadStringCompleted(new UploadStringCompletedEventArgs(result, null, false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnUploadStringCompleted(new UploadStringCompletedEventArgs(null, null, true, array[3]));
					}
					catch (Exception error)
					{
						this.OnUploadStringCompleted(new UploadStringCompletedEventArgs(null, error, false, array[3]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					method,
					data,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the default method. See remarks.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.</exception>
		public void UploadValuesAsync(System.Uri address, System.Collections.Specialized.NameValueCollection values)
		{
			this.UploadValuesAsync(address, null, values);
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null. -or- <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		public void UploadValuesAsync(System.Uri address, string method, System.Collections.Specialized.NameValueCollection values)
		{
			this.UploadValuesAsync(address, method, values, null);
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method. This method does not block the calling thread, and allows the caller to pass an object to the method that is invoked when the operation completes.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="data" /> is null. -or- <paramref name="address" /> is null.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.-or- There was no response from the server hosting the resource.-or-<paramref name="method" /> cannot be used to send content.</exception>
		public void UploadValuesAsync(System.Uri address, string method, System.Collections.Specialized.NameValueCollection values, object userToken)
		{
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			lock (this)
			{
				this.CheckBusy();
				this.async = true;
				this.async_thread = new Thread(delegate(object state)
				{
					object[] array = (object[])state;
					try
					{
						byte[] result = this.UploadValuesCore((System.Uri)array[0], (string)array[1], (System.Collections.Specialized.NameValueCollection)array[2], array[3]);
						this.OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(result, null, false, array[3]));
					}
					catch (ThreadInterruptedException)
					{
						this.OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(null, null, true, array[3]));
					}
					catch (Exception error)
					{
						this.OnUploadValuesCompleted(new UploadValuesCompletedEventArgs(null, error, false, array[3]));
					}
				});
				object[] parameter = new object[]
				{
					address,
					method,
					values,
					userToken
				};
				this.async_thread.Start(parameter);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadDataCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadDataCompletedEventArgs" /> object that contains event data.</param>
		protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.DownloadDataCompleted != null)
			{
				this.DownloadDataCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadFileCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnDownloadFileCompleted(System.ComponentModel.AsyncCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.DownloadFileCompleted != null)
			{
				this.DownloadFileCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadProgressChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadProgressChangedEventArgs" /> object containing event data.</param>
		protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
		{
			if (this.DownloadProgressChanged != null)
			{
				this.DownloadProgressChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadStringCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadStringCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.DownloadStringCompleted != null)
			{
				this.DownloadStringCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.OpenReadCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.OpenReadCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.OpenReadCompleted != null)
			{
				this.OpenReadCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.OpenWriteCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.OpenWriteCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.OpenWriteCompleted != null)
			{
				this.OpenWriteCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadDataCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.UploadDataCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.UploadDataCompleted != null)
			{
				this.UploadDataCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadFileCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadFileCompletedEventArgs" /> object containing event data.</param>
		protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.UploadFileCompleted != null)
			{
				this.UploadFileCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadProgressChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadProgressChangedEventArgs" /> object containing event data.</param>
		protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
		{
			if (this.UploadProgressChanged != null)
			{
				this.UploadProgressChanged(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadStringCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadStringCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.UploadStringCompleted != null)
			{
				this.UploadStringCompleted(this, args);
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadValuesCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.UploadValuesCompletedEventArgs" />  object containing event data.</param>
		protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs args)
		{
			this.CompleteAsync();
			if (this.UploadValuesCompleted != null)
			{
				this.UploadValuesCompleted(this, args);
			}
		}

		/// <summary>Returns the <see cref="T:System.Net.WebResponse" /> for the specified <see cref="T:System.Net.WebRequest" /> using the specified <see cref="T:System.IAsyncResult" />.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> containing the response for the specified <see cref="T:System.Net.WebRequest" />.</returns>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> that is used to obtain the response.</param>
		/// <param name="result">An <see cref="T:System.IAsyncResult" /> object obtained from a previous call to <see cref="M:System.Net.WebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> .</param>
		protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = request.EndGetResponse(result);
			this.responseHeaders = webResponse.Headers;
			return webResponse;
		}

		/// <summary>Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.</summary>
		/// <returns>A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.</returns>
		/// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
		protected virtual WebRequest GetWebRequest(System.Uri address)
		{
			return WebRequest.Create(address);
		}

		/// <summary>Returns the <see cref="T:System.Net.WebResponse" /> for the specified <see cref="T:System.Net.WebRequest" />.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> containing the response for the specified <see cref="T:System.Net.WebRequest" />.</returns>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> that is used to obtain the response. </param>
		protected virtual WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = request.GetResponse();
			this.responseHeaders = response.Headers;
			return response;
		}
	}
}
