using System;
using System.IO;
using System.Net;

namespace System.Xml
{
	/// <summary>Resolves external XML resources named by a Uniform Resource Identifier (URI).</summary>
	public class XmlUrlResolver : XmlResolver
	{
		private ICredentials credential;

		/// <summary>Sets credentials used to authenticate Web requests.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> object. If this property is not set, the value defaults to null; that is, the XmlUrlResolver has no user credentials.</returns>
		public override ICredentials Credentials
		{
			set
			{
				this.credential = value;
			}
		}

		/// <summary>Maps a URI to an object containing the actual resource.</summary>
		/// <returns>A System.IO.Stream object or null if a type other than stream is specified.</returns>
		/// <param name="absoluteUri">The URI returned from <see cref="M:System.Xml.XmlResolver.ResolveUri(System.Uri,System.String)" /></param>
		/// <param name="role">The current implementation does not use this parameter when resolving URIs. This is provided for future extensibility purposes. For example, this can be mapped to the xlink:role and used as an implementation specific argument in other scenarios. </param>
		/// <param name="ofObjectToReturn">The type of object to return. The current implementation only returns System.IO.Stream objects. </param>
		/// <exception cref="T:System.Xml.XmlException">
		///   <paramref name="ofObjectToReturn" /> is neither null nor a Stream type. </exception>
		/// <exception cref="T:System.UriFormatException">The specified URI is not an absolute URI. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="absoluteUri" /> is null. </exception>
		/// <exception cref="T:System.Exception">There is a runtime error (for example, an interrupted server connection). </exception>
		public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{
			if (ofObjectToReturn == null)
			{
				ofObjectToReturn = typeof(Stream);
			}
			if (ofObjectToReturn != typeof(Stream))
			{
				throw new XmlException("This object type is not supported.");
			}
			if (!absoluteUri.IsAbsoluteUri)
			{
				throw new ArgumentException("uri must be absolute.", "absoluteUri");
			}
			if (!(absoluteUri.Scheme == "file"))
			{
				WebRequest webRequest = WebRequest.Create(absoluteUri);
				if (this.credential != null)
				{
					webRequest.Credentials = this.credential;
				}
				return webRequest.GetResponse().GetResponseStream();
			}
			if (absoluteUri.AbsolutePath == string.Empty)
			{
				throw new ArgumentException("uri must be absolute.", "absoluteUri");
			}
			return new FileStream(this.UnescapeRelativeUriBody(absoluteUri.LocalPath), FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		/// <summary>Resolves the absolute URI from the base and relative URIs.</summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the absolute URI, or null if the relative URI cannot be resolved.</returns>
		/// <param name="baseUri">The base URI used to resolve the relative URI.</param>
		/// <param name="relativeUri">The URI to resolve. The URI can be absolute or relative. If absolute, this value effectively replaces the <paramref name="baseUri" /> value. If relative, it combines with the <paramref name="baseUri" /> to make an absolute URI.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="baseUri " />is null or <paramref name="relativeUri" /> is null</exception>
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			return base.ResolveUri(baseUri, relativeUri);
		}

		private string UnescapeRelativeUriBody(string src)
		{
			return src.Replace("%3C", "<").Replace("%3E", ">").Replace("%23", "#").Replace("%22", "\"").Replace("%20", " ").Replace("%25", "%");
		}
	}
}
