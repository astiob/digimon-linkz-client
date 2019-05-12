using System;
using System.IO;
using System.Runtime.Serialization;

namespace System.Net
{
	/// <summary>Provides a file system implementation of the <see cref="T:System.Net.WebResponse" /> class.</summary>
	[Serializable]
	public class FileWebResponse : WebResponse, IDisposable, ISerializable
	{
		private System.Uri responseUri;

		private FileStream fileStream;

		private long contentLength;

		private WebHeaderCollection webHeaders;

		private bool disposed;

		internal FileWebResponse(System.Uri responseUri, FileStream fileStream)
		{
			try
			{
				this.responseUri = responseUri;
				this.fileStream = fileStream;
				this.contentLength = fileStream.Length;
				this.webHeaders = new WebHeaderCollection();
				this.webHeaders.Add("Content-Length", Convert.ToString(this.contentLength));
				this.webHeaders.Add("Content-Type", "application/octet-stream");
			}
			catch (Exception ex)
			{
				throw new WebException(ex.Message, ex);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.FileWebResponse" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance that contains the information required to serialize the new <see cref="T:System.Net.FileWebResponse" /> instance. </param>
		/// <param name="streamingContext">An instance of the <see cref="T:System.Runtime.Serialization.StreamingContext" /> class that contains the source of the serialized stream associated with the new <see cref="T:System.Net.FileWebResponse" /> instance. </param>
		[Obsolete("Serialization is obsoleted for this type", false)]
		protected FileWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.responseUri = (System.Uri)serializationInfo.GetValue("responseUri", typeof(System.Uri));
			this.contentLength = serializationInfo.GetInt64("contentLength");
			this.webHeaders = (WebHeaderCollection)serializationInfo.GetValue("webHeaders", typeof(WebHeaderCollection));
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data needed to serialize the <see cref="T:System.Net.FileWebResponse" />.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> , which will hold the serialized data for the <see cref="T:System.Net.FileWebResponse" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> containing the destination of the serialized stream associated with the new <see cref="T:System.Net.FileWebResponse" />. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.FileWebResponse" />.</summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Gets the length of the content in the file system resource.</summary>
		/// <returns>The number of bytes returned from the file system resource.</returns>
		public override long ContentLength
		{
			get
			{
				this.CheckDisposed();
				return this.contentLength;
			}
		}

		/// <summary>Gets the content type of the file system resource.</summary>
		/// <returns>The value "binary/octet-stream".</returns>
		public override string ContentType
		{
			get
			{
				this.CheckDisposed();
				return "application/octet-stream";
			}
		}

		/// <summary>Gets a collection of header name/value pairs associated with the response.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the header name/value pairs associated with the response.</returns>
		public override WebHeaderCollection Headers
		{
			get
			{
				this.CheckDisposed();
				return this.webHeaders;
			}
		}

		/// <summary>Gets the URI of the file system resource that provided the response.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the file system resource that provided the response.</returns>
		public override System.Uri ResponseUri
		{
			get
			{
				this.CheckDisposed();
				return this.responseUri;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("responseUri", this.responseUri, typeof(System.Uri));
			serializationInfo.AddValue("contentLength", this.contentLength);
			serializationInfo.AddValue("webHeaders", this.webHeaders, typeof(WebHeaderCollection));
		}

		/// <summary>Returns the data stream from the file system resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> for reading data from the file system resource.</returns>
		public override Stream GetResponseStream()
		{
			this.CheckDisposed();
			return this.fileStream;
		}

		~FileWebResponse()
		{
			this.Dispose(false);
		}

		/// <summary>Closes the response stream.</summary>
		public override void Close()
		{
			((IDisposable)this).Dispose();
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.FileWebResponse" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		private void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (disposing)
			{
				this.responseUri = null;
				this.webHeaders = null;
			}
			FileStream fileStream = this.fileStream;
			this.fileStream = null;
			if (fileStream != null)
			{
				fileStream.Close();
			}
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}
	}
}
