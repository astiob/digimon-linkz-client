using System;

namespace System
{
	/// <summary>Specifies the parts of a <see cref="T:System.Uri" />.</summary>
	/// <filterpriority>1</filterpriority>
	[Flags]
	public enum UriComponents
	{
		/// <summary>The <see cref="P:System.Uri.Scheme" /> data.</summary>
		Scheme = 1,
		/// <summary>The <see cref="P:System.Uri.UserInfo" /> data.</summary>
		UserInfo = 2,
		/// <summary>The <see cref="P:System.Uri.Host" /> data.</summary>
		Host = 4,
		/// <summary>The <see cref="P:System.Uri.Port" /> data.</summary>
		Port = 8,
		/// <summary>The <see cref="P:System.Uri.LocalPath" /> data.</summary>
		Path = 16,
		/// <summary>The <see cref="P:System.Uri.Query" /> data.</summary>
		Query = 32,
		/// <summary>The <see cref="P:System.Uri.Fragment" /> data.</summary>
		Fragment = 64,
		/// <summary>The <see cref="P:System.Uri.Port" /> data. If no port data is in the <see cref="T:System.Uri" /> and a default port has been assigned to the <see cref="P:System.Uri.Scheme" />, the default port is returned. If there is no default port, -1 is returned.</summary>
		StrongPort = 128,
		/// <summary>Specifies that the delimiter should be included.</summary>
		KeepDelimiter = 1073741824,
		/// <summary>The <see cref="P:System.Uri.Host" /> and <see cref="P:System.Uri.Port" /> data. If no port data is in the Uri and a default port has been assigned to the <see cref="P:System.Uri.Scheme" />, the default port is returned. If there is no default port, -1 is returned.</summary>
		HostAndPort = 132,
		/// <summary>The <see cref="P:System.Uri.UserInfo" />, <see cref="P:System.Uri.Host" />, and <see cref="P:System.Uri.Port" /> data. If no port data is in the <see cref="T:System.Uri" /> and a default port has been assigned to the <see cref="P:System.Uri.Scheme" />, the default port is returned. If there is no default port, -1 is returned.</summary>
		StrongAuthority = 134,
		/// <summary>The <see cref="P:System.Uri.Scheme" />, <see cref="P:System.Uri.UserInfo" />, <see cref="P:System.Uri.Host" />, <see cref="P:System.Uri.Port" />, <see cref="P:System.Uri.LocalPath" />, <see cref="P:System.Uri.Query" />, and <see cref="P:System.Uri.Fragment" /> data.</summary>
		AbsoluteUri = 127,
		/// <summary>The <see cref="P:System.Uri.LocalPath" /> and <see cref="P:System.Uri.Query" /> data. Also see <see cref="P:System.Uri.PathAndQuery" />. </summary>
		PathAndQuery = 48,
		/// <summary>The <see cref="P:System.Uri.Scheme" />, <see cref="P:System.Uri.Host" />, <see cref="P:System.Uri.Port" />, <see cref="P:System.Uri.LocalPath" />, and <see cref="P:System.Uri.Query" /> data.</summary>
		HttpRequestUrl = 61,
		/// <summary>The <see cref="P:System.Uri.Scheme" />, <see cref="P:System.Uri.Host" />, and <see cref="P:System.Uri.Port" /> data.</summary>
		SchemeAndServer = 13,
		/// <summary>The complete <see cref="T:System.Uri" /> context that is needed for Uri Serializers. The context includes the IPv6 scope.</summary>
		SerializationInfoString = -2147483648
	}
}
