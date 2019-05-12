using System;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Represents an embedded external resource in an email attachment, such as an image in an HTML attachment.</summary>
	public class LinkedResource : AttachmentBase
	{
		private System.Uri contentLink;

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.LinkedResource" /> using the specified file name.</summary>
		/// <param name="fileName">The file name holding the content for this embedded resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		public LinkedResource(string fileName) : base(fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.LinkedResource" /> with the specified file name and content type.</summary>
		/// <param name="fileName">The file name that holds the content for this embedded resource.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		public LinkedResource(string fileName, System.Net.Mime.ContentType contentType) : base(fileName, contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.LinkedResource" /> with the specified file name and media type.</summary>
		/// <param name="fileName">The file name that holds the content for this embedded resource.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		public LinkedResource(string fileName, string mediaType) : base(fileName, mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.LinkedResource" /> using the supplied <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="contentStream">A stream that contains the content for this embedded resource.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		public LinkedResource(Stream contentStream) : base(contentStream)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.LinkedResource" /> with the values supplied by <see cref="T:System.IO.Stream" /> and <see cref="T:System.Net.Mime.ContentType" />.</summary>
		/// <param name="contentStream">A stream that contains the content for this embedded resource.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		public LinkedResource(Stream contentStream, System.Net.Mime.ContentType contentType) : base(contentStream, contentType)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Net.Mail.LinkedResource" /> with the specified <see cref="T:System.IO.Stream" /> and media type.</summary>
		/// <param name="contentStream">A stream that contains the content for this embedded resource.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		public LinkedResource(Stream contentStream, string mediaType) : base(contentStream, mediaType)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException();
			}
		}

		/// <summary>Gets or sets a URI that the resource must match.</summary>
		/// <returns>If <see cref="P:System.Net.Mail.LinkedResource.ContentLink" /> is a relative URI, the recipient of the message must resolve it.</returns>
		public System.Uri ContentLink
		{
			get
			{
				return this.contentLink;
			}
			set
			{
				this.contentLink = value;
			}
		}

		/// <summary>Creates a <see cref="T:System.Net.Mail.LinkedResource" /> object from a string to be included in an email attachment as an embedded resource. The default media type is plain text, and the default content type is ASCII.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.LinkedResource" /> object that contains the embedded resource to be included in the email attachment.</returns>
		/// <param name="content">A string that contains the embedded resource to be included in the email attachment.</param>
		/// <exception cref="T:System.ArgumentNullException">The specified content string is null.</exception>
		public static LinkedResource CreateLinkedResourceFromString(string content)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			MemoryStream contentStream = new MemoryStream(Encoding.Default.GetBytes(content));
			return new LinkedResource(contentStream)
			{
				TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable
			};
		}

		/// <summary>Creates a <see cref="T:System.Net.Mail.LinkedResource" /> object from a string to be included in an email attachment as an embedded resource, with the specified content type, and media type as plain text.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.LinkedResource" /> object that contains the embedded resource to be included in the email attachment.</returns>
		/// <param name="content">A string that contains the embedded resource to be included in the email attachment.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">The specified content string is null.</exception>
		public static LinkedResource CreateLinkedResourceFromString(string content, System.Net.Mime.ContentType contentType)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			MemoryStream contentStream = new MemoryStream(Encoding.Default.GetBytes(content));
			return new LinkedResource(contentStream, contentType)
			{
				TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable
			};
		}

		/// <summary>Creates a <see cref="T:System.Net.Mail.LinkedResource" /> object from a string to be included in an email attachment as an embedded resource, with the specified content type, and media type.</summary>
		/// <returns>A <see cref="T:System.Net.Mail.LinkedResource" /> object that contains the embedded resource to be included in the email attachment.</returns>
		/// <param name="content">A string that contains the embedded resource to be included in the email attachment.</param>
		/// <param name="contentEncoding">The type of the content.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">The specified content string is null.</exception>
		public static LinkedResource CreateLinkedResourceFromString(string content, Encoding contentEncoding, string mediaType)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			MemoryStream contentStream = new MemoryStream(contentEncoding.GetBytes(content));
			return new LinkedResource(contentStream, mediaType)
			{
				TransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable
			};
		}
	}
}
