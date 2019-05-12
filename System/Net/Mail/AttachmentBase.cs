using System;
using System.Collections;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	/// <summary>Base class that represents an email attachment. Classes <see cref="T:System.Net.Mail.Attachment" />, <see cref="T:System.Net.Mail.AlternateView" />, and <see cref="T:System.Net.Mail.LinkedResource" /> derive from this class.</summary>
	public abstract class AttachmentBase : IDisposable
	{
		private string id;

		private System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();

		private Stream contentStream;

		private System.Net.Mime.TransferEncoding transferEncoding = System.Net.Mime.TransferEncoding.Base64;

		/// <summary>Instantiates an <see cref="T:System.Net.Mail.AttachmentBase" /> with the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="contentStream">A stream containing the content for this attachment.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		protected AttachmentBase(Stream contentStream)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException();
			}
			this.contentStream = contentStream;
			this.contentType.MediaType = "application/octet-stream";
			this.contentType.CharSet = null;
		}

		/// <summary>Instantiates an <see cref="T:System.Net.Mail.AttachmentBase" /> with the specified <see cref="T:System.IO.Stream" /> and <see cref="T:System.Net.Mime.ContentType" />.</summary>
		/// <param name="contentStream">A stream containing the content for this attachment.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		protected AttachmentBase(Stream contentStream, System.Net.Mime.ContentType contentType)
		{
			if (contentStream == null || contentType == null)
			{
				throw new ArgumentNullException();
			}
			this.contentStream = contentStream;
			this.contentType = contentType;
		}

		/// <summary>Instantiates an <see cref="T:System.Net.Mail.AttachmentBase" /> with the specified <see cref="T:System.IO.Stream" /> and media type.</summary>
		/// <param name="contentStream">A stream containing the content for this attachment.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="contentStream" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		protected AttachmentBase(Stream contentStream, string mediaType)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException();
			}
			this.contentStream = contentStream;
			this.contentType.MediaType = mediaType;
		}

		/// <summary>Instantiates an <see cref="T:System.Net.Mail.AttachmentBase" /> with the specified file name.</summary>
		/// <param name="fileName">The file name holding the content for this attachment.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		protected AttachmentBase(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
			this.contentStream = File.OpenRead(fileName);
			this.contentType = new System.Net.Mime.ContentType(AttachmentBase.MimeTypes.GetMimeType(fileName));
		}

		/// <summary>Instantiates an <see cref="T:System.Net.Mail.AttachmentBase" /> with the specified file name and content type.</summary>
		/// <param name="fileName">The file name holding the content for this attachment.</param>
		/// <param name="contentType">The type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="contentType" /> is not a valid value.</exception>
		protected AttachmentBase(string fileName, System.Net.Mime.ContentType contentType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
			this.contentStream = File.OpenRead(fileName);
			this.contentType = contentType;
		}

		/// <summary>Instantiates an <see cref="T:System.Net.Mail.AttachmentBase" /> with the specified file name and media type.</summary>
		/// <param name="fileName">The file name holding the content for this attachment.</param>
		/// <param name="mediaType">The MIME media type of the content.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="fileName" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="mediaType" /> is not a valid value.</exception>
		protected AttachmentBase(string fileName, string mediaType)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException();
			}
			this.contentStream = File.OpenRead(fileName);
			this.contentType.MediaType = mediaType;
		}

		/// <summary>Gets or sets the MIME content ID for this attachment.</summary>
		/// <returns>A <see cref="T:System.String" /> holding the content ID.</returns>
		/// <exception cref="T:System.ArgumentNullException">Attempted to set <see cref="P:System.Net.Mail.AttachmentBase.ContentId" /> to null.</exception>
		public string ContentId
		{
			get
			{
				return this.id;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.id = value;
			}
		}

		/// <summary>Gets the content stream of this attachment.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" />.</returns>
		public Stream ContentStream
		{
			get
			{
				return this.contentStream;
			}
		}

		/// <summary>Gets the content type of this attachment.</summary>
		/// <returns>A <see cref="T:System.Net.Mime.ContentType" />.</returns>
		public System.Net.Mime.ContentType ContentType
		{
			get
			{
				return this.contentType;
			}
			set
			{
				this.contentType = value;
			}
		}

		/// <summary>Gets or sets the encoding of this attachment.</summary>
		/// <returns>A <see cref="T:System.Net.Mime.TransferEncoding" />.</returns>
		public System.Net.Mime.TransferEncoding TransferEncoding
		{
			get
			{
				return this.transferEncoding;
			}
			set
			{
				this.transferEncoding = value;
			}
		}

		/// <summary>Releases the resources used by the <see cref="T:System.Net.Mail.AttachmentBase" />. </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Mail.AttachmentBase" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.contentStream.Close();
			}
		}

		private class MimeTypes
		{
			private static Hashtable mimeTypes = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

			static MimeTypes()
			{
				AttachmentBase.MimeTypes.mimeTypes.Add("3dm", "x-world/x-3dmf");
				AttachmentBase.MimeTypes.mimeTypes.Add("3dmf", "x-world/x-3dmf");
				AttachmentBase.MimeTypes.mimeTypes.Add("aab", "application/x-authorware-bin");
				AttachmentBase.MimeTypes.mimeTypes.Add("aam", "application/x-authorware-map");
				AttachmentBase.MimeTypes.mimeTypes.Add("aas", "application/x-authorware-seg");
				AttachmentBase.MimeTypes.mimeTypes.Add("abc", "text/vnd.abc");
				AttachmentBase.MimeTypes.mimeTypes.Add("acgi", "text/html");
				AttachmentBase.MimeTypes.mimeTypes.Add("afl", "video/animaflex");
				AttachmentBase.MimeTypes.mimeTypes.Add("ai", "application/postscript");
				AttachmentBase.MimeTypes.mimeTypes.Add("aif", "audio/aiff");
				AttachmentBase.MimeTypes.mimeTypes.Add("aifc", "audio/aiff");
				AttachmentBase.MimeTypes.mimeTypes.Add("aiff", "audio/aiff");
				AttachmentBase.MimeTypes.mimeTypes.Add("aim", "application/x-aim");
				AttachmentBase.MimeTypes.mimeTypes.Add("aip", "text/x-audiosoft-intra");
				AttachmentBase.MimeTypes.mimeTypes.Add("ani", "application/x-navi-animation");
				AttachmentBase.MimeTypes.mimeTypes.Add("aos", "application/x-nokia-9000-communicator-add-on-software");
				AttachmentBase.MimeTypes.mimeTypes.Add("aps", "application/mime");
				AttachmentBase.MimeTypes.mimeTypes.Add("art", "image/x-jg");
				AttachmentBase.MimeTypes.mimeTypes.Add("asf", "video/x-ms-asf");
				AttachmentBase.MimeTypes.mimeTypes.Add("asm", "text/x-asm");
				AttachmentBase.MimeTypes.mimeTypes.Add("asp", "text/asp");
				AttachmentBase.MimeTypes.mimeTypes.Add("asx", "application/x-mplayer2");
				AttachmentBase.MimeTypes.mimeTypes.Add("au", "audio/x-au");
				AttachmentBase.MimeTypes.mimeTypes.Add("avi", "video/avi");
				AttachmentBase.MimeTypes.mimeTypes.Add("avs", "video/avs-video");
				AttachmentBase.MimeTypes.mimeTypes.Add("bcpio", "application/x-bcpio");
				AttachmentBase.MimeTypes.mimeTypes.Add("bm", "image/bmp");
				AttachmentBase.MimeTypes.mimeTypes.Add("bmp", "image/bmp");
				AttachmentBase.MimeTypes.mimeTypes.Add("boo", "application/book");
				AttachmentBase.MimeTypes.mimeTypes.Add("book", "application/book");
				AttachmentBase.MimeTypes.mimeTypes.Add("boz", "application/x-bzip2");
				AttachmentBase.MimeTypes.mimeTypes.Add("bsh", "application/x-bsh");
				AttachmentBase.MimeTypes.mimeTypes.Add("bz", "application/x-bzip");
				AttachmentBase.MimeTypes.mimeTypes.Add("bz2", "application/x-bzip2");
				AttachmentBase.MimeTypes.mimeTypes.Add("c", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("c++", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("cat", "application/vnd.ms-pki.seccat");
				AttachmentBase.MimeTypes.mimeTypes.Add("cc", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("ccad", "application/clariscad");
				AttachmentBase.MimeTypes.mimeTypes.Add("cco", "application/x-cocoa");
				AttachmentBase.MimeTypes.mimeTypes.Add("cdf", "application/cdf");
				AttachmentBase.MimeTypes.mimeTypes.Add("cer", "application/pkix-cert");
				AttachmentBase.MimeTypes.mimeTypes.Add("cha", "application/x-chat");
				AttachmentBase.MimeTypes.mimeTypes.Add("chat", "application/x-chat");
				AttachmentBase.MimeTypes.mimeTypes.Add("class", "application/java");
				AttachmentBase.MimeTypes.mimeTypes.Add("conf", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("cpio", "application/x-cpio");
				AttachmentBase.MimeTypes.mimeTypes.Add("cpp", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("cpt", "application/x-cpt");
				AttachmentBase.MimeTypes.mimeTypes.Add("crl", "application/pkix-crl");
				AttachmentBase.MimeTypes.mimeTypes.Add("crt", "application/pkix-cert");
				AttachmentBase.MimeTypes.mimeTypes.Add("csh", "application/x-csh");
				AttachmentBase.MimeTypes.mimeTypes.Add("css", "text/css");
				AttachmentBase.MimeTypes.mimeTypes.Add("cxx", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("dcr", "application/x-director");
				AttachmentBase.MimeTypes.mimeTypes.Add("deepv", "application/x-deepv");
				AttachmentBase.MimeTypes.mimeTypes.Add("def", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("der", "application/x-x509-ca-cert");
				AttachmentBase.MimeTypes.mimeTypes.Add("dif", "video/x-dv");
				AttachmentBase.MimeTypes.mimeTypes.Add("dir", "application/x-director");
				AttachmentBase.MimeTypes.mimeTypes.Add("dl", "video/dl");
				AttachmentBase.MimeTypes.mimeTypes.Add("doc", "application/msword");
				AttachmentBase.MimeTypes.mimeTypes.Add("dot", "application/msword");
				AttachmentBase.MimeTypes.mimeTypes.Add("dp", "application/commonground");
				AttachmentBase.MimeTypes.mimeTypes.Add("drw", "application/drafting");
				AttachmentBase.MimeTypes.mimeTypes.Add("dv", "video/x-dv");
				AttachmentBase.MimeTypes.mimeTypes.Add("dvi", "application/x-dvi");
				AttachmentBase.MimeTypes.mimeTypes.Add("dwf", "drawing/x-dwf (old)");
				AttachmentBase.MimeTypes.mimeTypes.Add("dwg", "application/acad");
				AttachmentBase.MimeTypes.mimeTypes.Add("dxf", "application/dxf");
				AttachmentBase.MimeTypes.mimeTypes.Add("dxr", "application/x-director");
				AttachmentBase.MimeTypes.mimeTypes.Add("el", "text/x-script.elisp");
				AttachmentBase.MimeTypes.mimeTypes.Add("elc", "application/x-elc");
				AttachmentBase.MimeTypes.mimeTypes.Add("eps", "application/postscript");
				AttachmentBase.MimeTypes.mimeTypes.Add("es", "application/x-esrehber");
				AttachmentBase.MimeTypes.mimeTypes.Add("etx", "text/x-setext");
				AttachmentBase.MimeTypes.mimeTypes.Add("evy", "application/envoy");
				AttachmentBase.MimeTypes.mimeTypes.Add("f", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("f77", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("f90", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("fdf", "application/vnd.fdf");
				AttachmentBase.MimeTypes.mimeTypes.Add("fif", "image/fif");
				AttachmentBase.MimeTypes.mimeTypes.Add("fli", "video/fli");
				AttachmentBase.MimeTypes.mimeTypes.Add("flo", "image/florian");
				AttachmentBase.MimeTypes.mimeTypes.Add("flx", "text/vnd.fmi.flexstor");
				AttachmentBase.MimeTypes.mimeTypes.Add("fmf", "video/x-atomic3d-feature");
				AttachmentBase.MimeTypes.mimeTypes.Add("for", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("fpx", "image/vnd.fpx");
				AttachmentBase.MimeTypes.mimeTypes.Add("frl", "application/freeloader");
				AttachmentBase.MimeTypes.mimeTypes.Add("funk", "audio/make");
				AttachmentBase.MimeTypes.mimeTypes.Add("g", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("g3", "image/g3fax");
				AttachmentBase.MimeTypes.mimeTypes.Add("gif", "image/gif");
				AttachmentBase.MimeTypes.mimeTypes.Add("gl", "video/gl");
				AttachmentBase.MimeTypes.mimeTypes.Add("gsd", "audio/x-gsm");
				AttachmentBase.MimeTypes.mimeTypes.Add("gsm", "audio/x-gsm");
				AttachmentBase.MimeTypes.mimeTypes.Add("gsp", "application/x-gsp");
				AttachmentBase.MimeTypes.mimeTypes.Add("gss", "application/x-gss");
				AttachmentBase.MimeTypes.mimeTypes.Add("gtar", "application/x-gtar");
				AttachmentBase.MimeTypes.mimeTypes.Add("gz", "application/x-gzip");
				AttachmentBase.MimeTypes.mimeTypes.Add("gzip", "application/x-gzip");
				AttachmentBase.MimeTypes.mimeTypes.Add("h", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("hdf", "application/x-hdf");
				AttachmentBase.MimeTypes.mimeTypes.Add("help", "application/x-helpfile");
				AttachmentBase.MimeTypes.mimeTypes.Add("hgl", "application/vnd.hp-HPGL");
				AttachmentBase.MimeTypes.mimeTypes.Add("hh", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("hlb", "text/x-script");
				AttachmentBase.MimeTypes.mimeTypes.Add("hlp", "application/x-helpfile");
				AttachmentBase.MimeTypes.mimeTypes.Add("hpg", "application/vnd.hp-HPGL");
				AttachmentBase.MimeTypes.mimeTypes.Add("hpgl", "application/vnd.hp-HPGL");
				AttachmentBase.MimeTypes.mimeTypes.Add("hqx", "application/binhex");
				AttachmentBase.MimeTypes.mimeTypes.Add("hta", "application/hta");
				AttachmentBase.MimeTypes.mimeTypes.Add("htc", "text/x-component");
				AttachmentBase.MimeTypes.mimeTypes.Add("htm", "text/html");
				AttachmentBase.MimeTypes.mimeTypes.Add("html", "text/html");
				AttachmentBase.MimeTypes.mimeTypes.Add("htmls", "text/html");
				AttachmentBase.MimeTypes.mimeTypes.Add("htt", "text/webviewhtml");
				AttachmentBase.MimeTypes.mimeTypes.Add("htx", "text/html");
				AttachmentBase.MimeTypes.mimeTypes.Add("ice", "x-conference/x-cooltalk");
				AttachmentBase.MimeTypes.mimeTypes.Add("ico", "image/x-icon");
				AttachmentBase.MimeTypes.mimeTypes.Add("idc", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("ief", "image/ief");
				AttachmentBase.MimeTypes.mimeTypes.Add("iefs", "image/ief");
				AttachmentBase.MimeTypes.mimeTypes.Add("iges", "application/iges");
				AttachmentBase.MimeTypes.mimeTypes.Add("igs", "application/iges");
				AttachmentBase.MimeTypes.mimeTypes.Add("ima", "application/x-ima");
				AttachmentBase.MimeTypes.mimeTypes.Add("imap", "application/x-httpd-imap");
				AttachmentBase.MimeTypes.mimeTypes.Add("inf", "application/inf");
				AttachmentBase.MimeTypes.mimeTypes.Add("ins", "application/x-internett-signup");
				AttachmentBase.MimeTypes.mimeTypes.Add("ip", "application/x-ip2");
				AttachmentBase.MimeTypes.mimeTypes.Add("isu", "video/x-isvideo");
				AttachmentBase.MimeTypes.mimeTypes.Add("it", "audio/it");
				AttachmentBase.MimeTypes.mimeTypes.Add("iv", "application/x-inventor");
				AttachmentBase.MimeTypes.mimeTypes.Add("ivr", "i-world/i-vrml");
				AttachmentBase.MimeTypes.mimeTypes.Add("ivy", "application/x-livescreen");
				AttachmentBase.MimeTypes.mimeTypes.Add("jam", "audio/x-jam");
				AttachmentBase.MimeTypes.mimeTypes.Add("jav", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("java", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("jcm", "application/x-java-commerce");
				AttachmentBase.MimeTypes.mimeTypes.Add("jfif", "image/jpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("jfif-tbnl", "image/jpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("jpe", "image/jpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("jpeg", "image/jpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("jpg", "image/jpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("jps", "image/x-jps");
				AttachmentBase.MimeTypes.mimeTypes.Add("js", "application/x-javascript");
				AttachmentBase.MimeTypes.mimeTypes.Add("jut", "image/jutvision");
				AttachmentBase.MimeTypes.mimeTypes.Add("kar", "audio/midi");
				AttachmentBase.MimeTypes.mimeTypes.Add("ksh", "text/x-script.ksh");
				AttachmentBase.MimeTypes.mimeTypes.Add("la", "audio/nspaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("lam", "audio/x-liveaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("latex", "application/x-latex");
				AttachmentBase.MimeTypes.mimeTypes.Add("list", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("lma", "audio/nspaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("log", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("lsp", "application/x-lisp");
				AttachmentBase.MimeTypes.mimeTypes.Add("lst", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("lsx", "text/x-la-asf");
				AttachmentBase.MimeTypes.mimeTypes.Add("ltx", "application/x-latex");
				AttachmentBase.MimeTypes.mimeTypes.Add("m", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("m1v", "video/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("m2a", "audio/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("m2v", "video/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("m3u", "audio/x-mpequrl");
				AttachmentBase.MimeTypes.mimeTypes.Add("man", "application/x-troff-man");
				AttachmentBase.MimeTypes.mimeTypes.Add("map", "application/x-navimap");
				AttachmentBase.MimeTypes.mimeTypes.Add("mar", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("mbd", "application/mbedlet");
				AttachmentBase.MimeTypes.mimeTypes.Add("mc$", "application/x-magic-cap-package-1.0");
				AttachmentBase.MimeTypes.mimeTypes.Add("mcd", "application/mcad");
				AttachmentBase.MimeTypes.mimeTypes.Add("mcf", "image/vasa");
				AttachmentBase.MimeTypes.mimeTypes.Add("mcp", "application/netmc");
				AttachmentBase.MimeTypes.mimeTypes.Add("me", "application/x-troff-me");
				AttachmentBase.MimeTypes.mimeTypes.Add("mht", "message/rfc822");
				AttachmentBase.MimeTypes.mimeTypes.Add("mhtml", "message/rfc822");
				AttachmentBase.MimeTypes.mimeTypes.Add("mid", "audio/midi");
				AttachmentBase.MimeTypes.mimeTypes.Add("midi", "audio/midi");
				AttachmentBase.MimeTypes.mimeTypes.Add("mif", "application/x-mif");
				AttachmentBase.MimeTypes.mimeTypes.Add("mime", "message/rfc822");
				AttachmentBase.MimeTypes.mimeTypes.Add("mjf", "audio/x-vnd.AudioExplosion.MjuiceMediaFile");
				AttachmentBase.MimeTypes.mimeTypes.Add("mjpg", "video/x-motion-jpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mm", "application/base64");
				AttachmentBase.MimeTypes.mimeTypes.Add("mme", "application/base64");
				AttachmentBase.MimeTypes.mimeTypes.Add("mod", "audio/mod");
				AttachmentBase.MimeTypes.mimeTypes.Add("moov", "video/quicktime");
				AttachmentBase.MimeTypes.mimeTypes.Add("mov", "video/quicktime");
				AttachmentBase.MimeTypes.mimeTypes.Add("movie", "video/x-sgi-movie");
				AttachmentBase.MimeTypes.mimeTypes.Add("mp2", "video/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mp3", "audio/mpeg3");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpa", "audio/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpc", "application/x-project");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpe", "video/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpeg", "video/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpg", "video/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpga", "audio/mpeg");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpp", "application/vnd.ms-project");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpt", "application/x-project");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpv", "application/x-project");
				AttachmentBase.MimeTypes.mimeTypes.Add("mpx", "application/x-project");
				AttachmentBase.MimeTypes.mimeTypes.Add("mrc", "application/marc");
				AttachmentBase.MimeTypes.mimeTypes.Add("ms", "application/x-troff-ms");
				AttachmentBase.MimeTypes.mimeTypes.Add("mv", "video/x-sgi-movie");
				AttachmentBase.MimeTypes.mimeTypes.Add("my", "audio/make");
				AttachmentBase.MimeTypes.mimeTypes.Add("mzz", "application/x-vnd.AudioExplosion.mzz");
				AttachmentBase.MimeTypes.mimeTypes.Add("nap", "image/naplps");
				AttachmentBase.MimeTypes.mimeTypes.Add("naplps", "image/naplps");
				AttachmentBase.MimeTypes.mimeTypes.Add("nc", "application/x-netcdf");
				AttachmentBase.MimeTypes.mimeTypes.Add("ncm", "application/vnd.nokia.configuration-message");
				AttachmentBase.MimeTypes.mimeTypes.Add("nif", "image/x-niff");
				AttachmentBase.MimeTypes.mimeTypes.Add("niff", "image/x-niff");
				AttachmentBase.MimeTypes.mimeTypes.Add("nix", "application/x-mix-transfer");
				AttachmentBase.MimeTypes.mimeTypes.Add("nsc", "application/x-conference");
				AttachmentBase.MimeTypes.mimeTypes.Add("nvd", "application/x-navidoc");
				AttachmentBase.MimeTypes.mimeTypes.Add("oda", "application/oda");
				AttachmentBase.MimeTypes.mimeTypes.Add("omc", "application/x-omc");
				AttachmentBase.MimeTypes.mimeTypes.Add("omcd", "application/x-omcdatamaker");
				AttachmentBase.MimeTypes.mimeTypes.Add("omcr", "application/x-omcregerator");
				AttachmentBase.MimeTypes.mimeTypes.Add("p", "text/x-pascal");
				AttachmentBase.MimeTypes.mimeTypes.Add("p10", "application/pkcs10");
				AttachmentBase.MimeTypes.mimeTypes.Add("p12", "application/pkcs-12");
				AttachmentBase.MimeTypes.mimeTypes.Add("p7a", "application/x-pkcs7-signature");
				AttachmentBase.MimeTypes.mimeTypes.Add("p7c", "application/pkcs7-mime");
				AttachmentBase.MimeTypes.mimeTypes.Add("p7m", "application/pkcs7-mime");
				AttachmentBase.MimeTypes.mimeTypes.Add("p7r", "application/x-pkcs7-certreqresp");
				AttachmentBase.MimeTypes.mimeTypes.Add("p7s", "application/pkcs7-signature");
				AttachmentBase.MimeTypes.mimeTypes.Add("part", "application/pro_eng");
				AttachmentBase.MimeTypes.mimeTypes.Add("pas", "text/pascal");
				AttachmentBase.MimeTypes.mimeTypes.Add("pbm", "image/x-portable-bitmap");
				AttachmentBase.MimeTypes.mimeTypes.Add("pcl", "application/x-pcl");
				AttachmentBase.MimeTypes.mimeTypes.Add("pct", "image/x-pict");
				AttachmentBase.MimeTypes.mimeTypes.Add("pcx", "image/x-pcx");
				AttachmentBase.MimeTypes.mimeTypes.Add("pdb", "chemical/x-pdb");
				AttachmentBase.MimeTypes.mimeTypes.Add("pdf", "application/pdf");
				AttachmentBase.MimeTypes.mimeTypes.Add("pfunk", "audio/make");
				AttachmentBase.MimeTypes.mimeTypes.Add("pgm", "image/x-portable-graymap");
				AttachmentBase.MimeTypes.mimeTypes.Add("pic", "image/pict");
				AttachmentBase.MimeTypes.mimeTypes.Add("pict", "image/pict");
				AttachmentBase.MimeTypes.mimeTypes.Add("pkg", "application/x-newton-compatible-pkg");
				AttachmentBase.MimeTypes.mimeTypes.Add("pko", "application/vnd.ms-pki.pko");
				AttachmentBase.MimeTypes.mimeTypes.Add("pl", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("plx", "application/x-PiXCLscript");
				AttachmentBase.MimeTypes.mimeTypes.Add("pm", "image/x-xpixmap");
				AttachmentBase.MimeTypes.mimeTypes.Add("pm4", "application/x-pagemaker");
				AttachmentBase.MimeTypes.mimeTypes.Add("pm5", "application/x-pagemaker");
				AttachmentBase.MimeTypes.mimeTypes.Add("png", "image/png");
				AttachmentBase.MimeTypes.mimeTypes.Add("pnm", "application/x-portable-anymap");
				AttachmentBase.MimeTypes.mimeTypes.Add("pot", "application/mspowerpoint");
				AttachmentBase.MimeTypes.mimeTypes.Add("pov", "model/x-pov");
				AttachmentBase.MimeTypes.mimeTypes.Add("ppa", "application/vnd.ms-powerpoint");
				AttachmentBase.MimeTypes.mimeTypes.Add("ppm", "image/x-portable-pixmap");
				AttachmentBase.MimeTypes.mimeTypes.Add("pps", "application/mspowerpoint");
				AttachmentBase.MimeTypes.mimeTypes.Add("ppt", "application/mspowerpoint");
				AttachmentBase.MimeTypes.mimeTypes.Add("ppz", "application/mspowerpoint");
				AttachmentBase.MimeTypes.mimeTypes.Add("pre", "application/x-freelance");
				AttachmentBase.MimeTypes.mimeTypes.Add("prt", "application/pro_eng");
				AttachmentBase.MimeTypes.mimeTypes.Add("ps", "application/postscript");
				AttachmentBase.MimeTypes.mimeTypes.Add("pvu", "paleovu/x-pv");
				AttachmentBase.MimeTypes.mimeTypes.Add("pwz", "application/vnd.ms-powerpoint");
				AttachmentBase.MimeTypes.mimeTypes.Add("py", "text/x-script.phyton");
				AttachmentBase.MimeTypes.mimeTypes.Add("pyc", "applicaiton/x-bytecode.python");
				AttachmentBase.MimeTypes.mimeTypes.Add("qcp", "audio/vnd.qcelp");
				AttachmentBase.MimeTypes.mimeTypes.Add("qd3", "x-world/x-3dmf");
				AttachmentBase.MimeTypes.mimeTypes.Add("qd3d", "x-world/x-3dmf");
				AttachmentBase.MimeTypes.mimeTypes.Add("qif", "image/x-quicktime");
				AttachmentBase.MimeTypes.mimeTypes.Add("qt", "video/quicktime");
				AttachmentBase.MimeTypes.mimeTypes.Add("qtc", "video/x-qtc");
				AttachmentBase.MimeTypes.mimeTypes.Add("qti", "image/x-quicktime");
				AttachmentBase.MimeTypes.mimeTypes.Add("qtif", "image/x-quicktime");
				AttachmentBase.MimeTypes.mimeTypes.Add("ra", "audio/x-pn-realaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("ram", "audio/x-pn-realaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("ras", "application/x-cmu-raster");
				AttachmentBase.MimeTypes.mimeTypes.Add("rast", "image/cmu-raster");
				AttachmentBase.MimeTypes.mimeTypes.Add("rexx", "text/x-script.rexx");
				AttachmentBase.MimeTypes.mimeTypes.Add("rf", "image/vnd.rn-realflash");
				AttachmentBase.MimeTypes.mimeTypes.Add("rgb", "image/x-rgb");
				AttachmentBase.MimeTypes.mimeTypes.Add("rm", "application/vnd.rn-realmedia");
				AttachmentBase.MimeTypes.mimeTypes.Add("rmi", "audio/mid");
				AttachmentBase.MimeTypes.mimeTypes.Add("rmm", "audio/x-pn-realaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("rmp", "audio/x-pn-realaudio");
				AttachmentBase.MimeTypes.mimeTypes.Add("rng", "application/ringing-tones");
				AttachmentBase.MimeTypes.mimeTypes.Add("rnx", "application/vnd.rn-realplayer");
				AttachmentBase.MimeTypes.mimeTypes.Add("roff", "application/x-troff");
				AttachmentBase.MimeTypes.mimeTypes.Add("rp", "image/vnd.rn-realpix");
				AttachmentBase.MimeTypes.mimeTypes.Add("rpm", "audio/x-pn-realaudio-plugin");
				AttachmentBase.MimeTypes.mimeTypes.Add("rss", "text/xml");
				AttachmentBase.MimeTypes.mimeTypes.Add("rt", "text/richtext");
				AttachmentBase.MimeTypes.mimeTypes.Add("rtf", "text/richtext");
				AttachmentBase.MimeTypes.mimeTypes.Add("rtx", "text/richtext");
				AttachmentBase.MimeTypes.mimeTypes.Add("rv", "video/vnd.rn-realvideo");
				AttachmentBase.MimeTypes.mimeTypes.Add("s", "text/x-asm");
				AttachmentBase.MimeTypes.mimeTypes.Add("s3m", "audio/s3m");
				AttachmentBase.MimeTypes.mimeTypes.Add("sbk", "application/x-tbook");
				AttachmentBase.MimeTypes.mimeTypes.Add("scm", "application/x-lotusscreencam");
				AttachmentBase.MimeTypes.mimeTypes.Add("sdml", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("sdp", "application/sdp");
				AttachmentBase.MimeTypes.mimeTypes.Add("sdr", "application/sounder");
				AttachmentBase.MimeTypes.mimeTypes.Add("sea", "application/sea");
				AttachmentBase.MimeTypes.mimeTypes.Add("set", "application/set");
				AttachmentBase.MimeTypes.mimeTypes.Add("sgm", "text/sgml");
				AttachmentBase.MimeTypes.mimeTypes.Add("sgml", "text/sgml");
				AttachmentBase.MimeTypes.mimeTypes.Add("sh", "text/x-script.sh");
				AttachmentBase.MimeTypes.mimeTypes.Add("shar", "application/x-bsh");
				AttachmentBase.MimeTypes.mimeTypes.Add("shtml", "text/html");
				AttachmentBase.MimeTypes.mimeTypes.Add("sid", "audio/x-psid");
				AttachmentBase.MimeTypes.mimeTypes.Add("sit", "application/x-sit");
				AttachmentBase.MimeTypes.mimeTypes.Add("skd", "application/x-koan");
				AttachmentBase.MimeTypes.mimeTypes.Add("skm", "application/x-koan");
				AttachmentBase.MimeTypes.mimeTypes.Add("skp", "application/x-koan");
				AttachmentBase.MimeTypes.mimeTypes.Add("skt", "application/x-koan");
				AttachmentBase.MimeTypes.mimeTypes.Add("sl", "application/x-seelogo");
				AttachmentBase.MimeTypes.mimeTypes.Add("smi", "application/smil");
				AttachmentBase.MimeTypes.mimeTypes.Add("smil", "application/smil");
				AttachmentBase.MimeTypes.mimeTypes.Add("snd", "audio/basic");
				AttachmentBase.MimeTypes.mimeTypes.Add("sol", "application/solids");
				AttachmentBase.MimeTypes.mimeTypes.Add("spc", "application/x-pkcs7-certificates");
				AttachmentBase.MimeTypes.mimeTypes.Add("spl", "application/futuresplash");
				AttachmentBase.MimeTypes.mimeTypes.Add("spr", "application/x-sprite");
				AttachmentBase.MimeTypes.mimeTypes.Add("sprite", "application/x-sprite");
				AttachmentBase.MimeTypes.mimeTypes.Add("src", "application/x-wais-source");
				AttachmentBase.MimeTypes.mimeTypes.Add("ssi", "text/x-server-parsed-html");
				AttachmentBase.MimeTypes.mimeTypes.Add("ssm", "application/streamingmedia");
				AttachmentBase.MimeTypes.mimeTypes.Add("sst", "application/vnd.ms-pki.certstore");
				AttachmentBase.MimeTypes.mimeTypes.Add("step", "application/step");
				AttachmentBase.MimeTypes.mimeTypes.Add("stl", "application/sla");
				AttachmentBase.MimeTypes.mimeTypes.Add("stp", "application/step");
				AttachmentBase.MimeTypes.mimeTypes.Add("sv4cpio", "application/x-sv4cpio");
				AttachmentBase.MimeTypes.mimeTypes.Add("sv4crc", "application/x-sv4crc");
				AttachmentBase.MimeTypes.mimeTypes.Add("svf", "image/x-dwg");
				AttachmentBase.MimeTypes.mimeTypes.Add("svr", "application/x-world");
				AttachmentBase.MimeTypes.mimeTypes.Add("swf", "application/x-shockwave-flash");
				AttachmentBase.MimeTypes.mimeTypes.Add("t", "application/x-troff");
				AttachmentBase.MimeTypes.mimeTypes.Add("talk", "text/x-speech");
				AttachmentBase.MimeTypes.mimeTypes.Add("tar", "application/x-tar");
				AttachmentBase.MimeTypes.mimeTypes.Add("tbk", "application/toolbook");
				AttachmentBase.MimeTypes.mimeTypes.Add("tcl", "text/x-script.tcl");
				AttachmentBase.MimeTypes.mimeTypes.Add("tcsh", "text/x-script.tcsh");
				AttachmentBase.MimeTypes.mimeTypes.Add("tex", "application/x-tex");
				AttachmentBase.MimeTypes.mimeTypes.Add("texi", "application/x-texinfo");
				AttachmentBase.MimeTypes.mimeTypes.Add("texinfo", "application/x-texinfo");
				AttachmentBase.MimeTypes.mimeTypes.Add("text", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("tgz", "application/x-compressed");
				AttachmentBase.MimeTypes.mimeTypes.Add("tif", "image/tiff");
				AttachmentBase.MimeTypes.mimeTypes.Add("tiff", "image/tiff");
				AttachmentBase.MimeTypes.mimeTypes.Add("tr", "application/x-troff");
				AttachmentBase.MimeTypes.mimeTypes.Add("tsi", "audio/tsp-audio");
				AttachmentBase.MimeTypes.mimeTypes.Add("tsp", "audio/tsplayer");
				AttachmentBase.MimeTypes.mimeTypes.Add("tsv", "text/tab-separated-values");
				AttachmentBase.MimeTypes.mimeTypes.Add("turbot", "image/florian");
				AttachmentBase.MimeTypes.mimeTypes.Add("txt", "text/plain");
				AttachmentBase.MimeTypes.mimeTypes.Add("uil", "text/x-uil");
				AttachmentBase.MimeTypes.mimeTypes.Add("uni", "text/uri-list");
				AttachmentBase.MimeTypes.mimeTypes.Add("unis", "text/uri-list");
				AttachmentBase.MimeTypes.mimeTypes.Add("unv", "application/i-deas");
				AttachmentBase.MimeTypes.mimeTypes.Add("uri", "text/uri-list");
				AttachmentBase.MimeTypes.mimeTypes.Add("uris", "text/uri-list");
				AttachmentBase.MimeTypes.mimeTypes.Add("ustar", "multipart/x-ustar");
				AttachmentBase.MimeTypes.mimeTypes.Add("uu", "text/x-uuencode");
				AttachmentBase.MimeTypes.mimeTypes.Add("uue", "text/x-uuencode");
				AttachmentBase.MimeTypes.mimeTypes.Add("vcd", "application/x-cdlink");
				AttachmentBase.MimeTypes.mimeTypes.Add("vcs", "text/x-vCalendar");
				AttachmentBase.MimeTypes.mimeTypes.Add("vda", "application/vda");
				AttachmentBase.MimeTypes.mimeTypes.Add("vdo", "video/vdo");
				AttachmentBase.MimeTypes.mimeTypes.Add("vew", "application/groupwise");
				AttachmentBase.MimeTypes.mimeTypes.Add("viv", "video/vivo");
				AttachmentBase.MimeTypes.mimeTypes.Add("vivo", "video/vivo");
				AttachmentBase.MimeTypes.mimeTypes.Add("vmd", "application/vocaltec-media-desc");
				AttachmentBase.MimeTypes.mimeTypes.Add("vmf", "application/vocaltec-media-file");
				AttachmentBase.MimeTypes.mimeTypes.Add("voc", "audio/voc");
				AttachmentBase.MimeTypes.mimeTypes.Add("vos", "video/vosaic");
				AttachmentBase.MimeTypes.mimeTypes.Add("vox", "audio/voxware");
				AttachmentBase.MimeTypes.mimeTypes.Add("vqe", "audio/x-twinvq-plugin");
				AttachmentBase.MimeTypes.mimeTypes.Add("vqf", "audio/x-twinvq");
				AttachmentBase.MimeTypes.mimeTypes.Add("vql", "audio/x-twinvq-plugin");
				AttachmentBase.MimeTypes.mimeTypes.Add("vrml", "application/x-vrml");
				AttachmentBase.MimeTypes.mimeTypes.Add("vrt", "x-world/x-vrt");
				AttachmentBase.MimeTypes.mimeTypes.Add("vsd", "application/x-visio");
				AttachmentBase.MimeTypes.mimeTypes.Add("vst", "application/x-visio");
				AttachmentBase.MimeTypes.mimeTypes.Add("vsw", "application/x-visio");
				AttachmentBase.MimeTypes.mimeTypes.Add("w60", "application/wordperfect6.0");
				AttachmentBase.MimeTypes.mimeTypes.Add("w61", "application/wordperfect6.1");
				AttachmentBase.MimeTypes.mimeTypes.Add("w6w", "application/msword");
				AttachmentBase.MimeTypes.mimeTypes.Add("wav", "audio/wav");
				AttachmentBase.MimeTypes.mimeTypes.Add("wb1", "application/x-qpro");
				AttachmentBase.MimeTypes.mimeTypes.Add("wbmp", "image/vnd.wap.wbmp");
				AttachmentBase.MimeTypes.mimeTypes.Add("web", "application/vnd.xara");
				AttachmentBase.MimeTypes.mimeTypes.Add("wiz", "application/msword");
				AttachmentBase.MimeTypes.mimeTypes.Add("wk1", "application/x-123");
				AttachmentBase.MimeTypes.mimeTypes.Add("wmf", "windows/metafile");
				AttachmentBase.MimeTypes.mimeTypes.Add("wml", "text/vnd.wap.wml");
				AttachmentBase.MimeTypes.mimeTypes.Add("wmlc", "application/vnd.wap.wmlc");
				AttachmentBase.MimeTypes.mimeTypes.Add("wmls", "text/vnd.wap.wmlscript");
				AttachmentBase.MimeTypes.mimeTypes.Add("wmlsc", "application/vnd.wap.wmlscriptc");
				AttachmentBase.MimeTypes.mimeTypes.Add("word", "application/msword");
				AttachmentBase.MimeTypes.mimeTypes.Add("wp", "application/wordperfect");
				AttachmentBase.MimeTypes.mimeTypes.Add("wp5", "application/wordperfect");
				AttachmentBase.MimeTypes.mimeTypes.Add("wp6", "application/wordperfect");
				AttachmentBase.MimeTypes.mimeTypes.Add("wpd", "application/wordperfect");
				AttachmentBase.MimeTypes.mimeTypes.Add("wq1", "application/x-lotus");
				AttachmentBase.MimeTypes.mimeTypes.Add("wri", "application/mswrite");
				AttachmentBase.MimeTypes.mimeTypes.Add("wrl", "application/x-world");
				AttachmentBase.MimeTypes.mimeTypes.Add("wrz", "model/vrml");
				AttachmentBase.MimeTypes.mimeTypes.Add("wsc", "text/scriplet");
				AttachmentBase.MimeTypes.mimeTypes.Add("wsrc", "application/x-wais-source");
				AttachmentBase.MimeTypes.mimeTypes.Add("wtk", "application/x-wintalk");
				AttachmentBase.MimeTypes.mimeTypes.Add("xbm", "image/x-xbitmap");
				AttachmentBase.MimeTypes.mimeTypes.Add("xdr", "video/x-amt-demorun");
				AttachmentBase.MimeTypes.mimeTypes.Add("xgz", "xgl/drawing");
				AttachmentBase.MimeTypes.mimeTypes.Add("xif", "image/vnd.xiff");
				AttachmentBase.MimeTypes.mimeTypes.Add("xl", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xla", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlb", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlc", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xld", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlk", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xll", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlm", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xls", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlt", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlv", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xlw", "application/excel");
				AttachmentBase.MimeTypes.mimeTypes.Add("xm", "audio/xm");
				AttachmentBase.MimeTypes.mimeTypes.Add("xml", "text/xml");
				AttachmentBase.MimeTypes.mimeTypes.Add("xmz", "xgl/movie");
				AttachmentBase.MimeTypes.mimeTypes.Add("xpix", "application/x-vnd.ls-xpix");
				AttachmentBase.MimeTypes.mimeTypes.Add("xpm", "image/xpm");
				AttachmentBase.MimeTypes.mimeTypes.Add("x-png", "image/png");
				AttachmentBase.MimeTypes.mimeTypes.Add("xsr", "video/x-amt-showrun");
				AttachmentBase.MimeTypes.mimeTypes.Add("xwd", "image/x-xwd");
				AttachmentBase.MimeTypes.mimeTypes.Add("xyz", "chemical/x-pdb");
				AttachmentBase.MimeTypes.mimeTypes.Add("z", "application/x-compressed");
				AttachmentBase.MimeTypes.mimeTypes.Add("zip", "application/zip");
				AttachmentBase.MimeTypes.mimeTypes.Add("zsh", "text/x-script.zsh");
			}

			public static string GetMimeType(string fileName)
			{
				string text = null;
				int num = fileName.LastIndexOf('.');
				if (num != -1 && fileName.Length > num + 1)
				{
					text = (AttachmentBase.MimeTypes.mimeTypes[fileName.Substring(num + 1)] as string);
				}
				if (text == null)
				{
					text = "application/octet-stream";
				}
				return text;
			}
		}
	}
}
