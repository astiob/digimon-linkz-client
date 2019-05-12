using System;

namespace System.Net.Mime
{
	/// <summary>Specifies the media type information for an e-mail message attachment.</summary>
	public static class MediaTypeNames
	{
		/// <summary>Specifies the kind of application data in an e-mail message attachment.</summary>
		public static class Application
		{
			private const string prefix = "application/";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is not interpreted.</summary>
			public const string Octet = "application/octet-stream";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is in Portable Document Format (PDF).</summary>
			public const string Pdf = "application/pdf";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is in Rich Text Format (RTF).</summary>
			public const string Rtf = "application/rtf";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is a SOAP document.</summary>
			public const string Soap = "application/soap+xml";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Application" /> data is compressed.</summary>
			public const string Zip = "application/zip";
		}

		/// <summary>Specifies the type of image data in an e-mail message attachment.</summary>
		public static class Image
		{
			private const string prefix = "image/";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Image" /> data is in Graphics Interchange Format (GIF).</summary>
			public const string Gif = "image/gif";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Image" /> data is in Joint Photographic Experts Group (JPEG) format.</summary>
			public const string Jpeg = "image/jpeg";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Image" /> data is in Tagged Image File Format (TIFF).</summary>
			public const string Tiff = "image/tiff";
		}

		/// <summary>Specifies the type of text data in an e-mail message attachment.</summary>
		public static class Text
		{
			private const string prefix = "text/";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in HTML format.</summary>
			public const string Html = "text/html";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in plain text format.</summary>
			public const string Plain = "text/plain";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in Rich Text Format (RTF).</summary>
			public const string RichText = "text/richtext";

			/// <summary>Specifies that the <see cref="T:System.Net.Mime.MediaTypeNames.Text" /> data is in XML format.</summary>
			public const string Xml = "text/xml";
		}
	}
}
