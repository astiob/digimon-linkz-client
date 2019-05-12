using System;

namespace System.Net
{
	/// <summary>Defines the HTTP version numbers that are supported by the <see cref="T:System.Net.HttpWebRequest" /> and <see cref="T:System.Net.HttpWebResponse" /> classes.</summary>
	public class HttpVersion
	{
		/// <summary>Defines a <see cref="T:System.Version" /> instance for HTTP 1.0.</summary>
		public static readonly Version Version10 = new Version(1, 0);

		/// <summary>Defines a <see cref="T:System.Version" /> instance for HTTP 1.1.</summary>
		public static readonly Version Version11 = new Version(1, 1);
	}
}
