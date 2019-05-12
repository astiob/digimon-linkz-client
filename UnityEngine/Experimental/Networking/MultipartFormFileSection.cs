using System;
using System.Text;

namespace UnityEngine.Experimental.Networking
{
	/// <summary>
	///   <para>A helper object for adding file uploads to multipart forms via the [IMultipartFormSection] API.</para>
	/// </summary>
	public class MultipartFormFileSection : IMultipartFormSection
	{
		private string name;

		private byte[] data;

		private string file;

		private string content;

		/// <summary>
		///   <para>Contains a named file section based on the raw bytes from data, with a custom Content-Type and file name.</para>
		/// </summary>
		/// <param name="name">Name of this form section.</param>
		/// <param name="data">Raw contents of the file to upload.</param>
		/// <param name="fileName">Name of the file uploaded by this form section.</param>
		/// <param name="contentType">The value for this section's Content-Type header.</param>
		public MultipartFormFileSection(string name, byte[] data, string fileName, string contentType)
		{
			if (data == null || data.Length < 1)
			{
				throw new ArgumentException("Cannot create a multipart form file section without body data");
			}
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = "file.dat";
			}
			if (string.IsNullOrEmpty(contentType))
			{
				contentType = "application/octet-stream";
			}
			this.Init(name, data, fileName, contentType);
		}

		/// <summary>
		///   <para>Contains an anonymous file section based on the raw bytes from data, assigns a default Content-Type and file name.</para>
		/// </summary>
		/// <param name="data">Raw contents of the file to upload.</param>
		public MultipartFormFileSection(byte[] data) : this(null, data, null, null)
		{
		}

		/// <summary>
		///   <para>Contains an anonymous file section based on the raw bytes from data with a specific file name. Assigns a default Content-Type.</para>
		/// </summary>
		/// <param name="data">Raw contents of the file to upload.</param>
		/// <param name="fileName">Name of the file uploaded by this form section.</param>
		public MultipartFormFileSection(string fileName, byte[] data) : this(null, data, fileName, null)
		{
		}

		/// <summary>
		///   <para>Contains a named file section with data drawn from data, as marshaled by dataEncoding. Assigns a specific file name from fileName and a default Content-Type.</para>
		/// </summary>
		/// <param name="name">Name of this form section.</param>
		/// <param name="data">Contents of the file to upload.</param>
		/// <param name="dataEncoding">A string encoding.</param>
		/// <param name="fileName">Name of the file uploaded by this form section.</param>
		public MultipartFormFileSection(string name, string data, Encoding dataEncoding, string fileName)
		{
			if (data == null || data.Length < 1)
			{
				throw new ArgumentException("Cannot create a multipart form file section without body data");
			}
			if (dataEncoding == null)
			{
				dataEncoding = Encoding.UTF8;
			}
			byte[] bytes = dataEncoding.GetBytes(data);
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = "file.txt";
			}
			if (string.IsNullOrEmpty(this.content))
			{
				this.content = "text/plain; charset=" + dataEncoding.WebName;
			}
			this.Init(name, bytes, fileName, this.content);
		}

		/// <summary>
		///   <para>An anonymous file section with data drawn from data, as marshaled by dataEncoding. Assigns a specific file name from fileName and a default Content-Type.</para>
		/// </summary>
		/// <param name="data">Contents of the file to upload.</param>
		/// <param name="dataEncoding">A string encoding.</param>
		/// <param name="fileName">Name of the file uploaded by this form section.</param>
		public MultipartFormFileSection(string data, Encoding dataEncoding, string fileName) : this(null, data, dataEncoding, fileName)
		{
		}

		/// <summary>
		///   <para>An anonymous file section with data drawn from the UTF8 string data. Assigns a specific file name from fileName and a default Content-Type.</para>
		/// </summary>
		/// <param name="data">Contents of the file to upload.</param>
		/// <param name="fileName">Name of the file uploaded by this form section.</param>
		public MultipartFormFileSection(string data, string fileName) : this(data, null, fileName)
		{
		}

		private void Init(string name, byte[] data, string fileName, string contentType)
		{
			this.name = name;
			this.data = data;
			this.file = fileName;
			this.content = contentType;
		}

		/// <summary>
		///   <para>Returns the name of this section, if any.</para>
		/// </summary>
		/// <returns>
		///   <para>The section's name, or null.</para>
		/// </returns>
		public string sectionName
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		///   <para>Returns the raw binary data contained in this section. Will not return null or a zero-length array.</para>
		/// </summary>
		/// <returns>
		///   <para>The raw binary data contained in this section. Will not be null or empty.</para>
		/// </returns>
		public byte[] sectionData
		{
			get
			{
				return this.data;
			}
		}

		/// <summary>
		///   <para>Returns a string denoting the desired filename of this section on the destination server.</para>
		/// </summary>
		/// <returns>
		///   <para>The desired file name of this section, or null if this is not a file section.</para>
		/// </returns>
		public string fileName
		{
			get
			{
				return this.file;
			}
		}

		/// <summary>
		///   <para>Returns the value of the section's Content-Type header.</para>
		/// </summary>
		/// <returns>
		///   <para>The Content-Type header for this section, or null.</para>
		/// </returns>
		public string contentType
		{
			get
			{
				return this.content;
			}
		}
	}
}
