using System;

namespace System
{
	/// <summary>Specifies options for a <see cref="T:System.UriParser" />.</summary>
	[Flags]
	public enum GenericUriParserOptions
	{
		/// <summary>The parser:</summary>
		Default = 0,
		/// <summary>The parser allows a registry-based authority.</summary>
		GenericAuthority = 1,
		/// <summary>The parser allows a URI with no authority.</summary>
		AllowEmptyAuthority = 2,
		/// <summary>The scheme does not define a user information part.</summary>
		NoUserInfo = 4,
		/// <summary>The scheme does not define a port.</summary>
		NoPort = 8,
		/// <summary>The scheme does not define a query part.</summary>
		NoQuery = 16,
		/// <summary>The scheme does not define a fragment part.</summary>
		NoFragment = 32,
		/// <summary>The parser does not convert back slashes into forward slashes.</summary>
		DontConvertPathBackslashes = 64,
		/// <summary>The parser does not canonicalize the URI.</summary>
		DontCompressPath = 128,
		/// <summary>The parser does not unescape path dots, forward slashes, or back slashes.</summary>
		DontUnescapePathDotsAndSlashes = 256,
		/// <summary>The parser supports Internationalized Domain Name (IDN) parsing (IDN) of host names. Whether IDN is used is dictated by configuration values. See the Remarks for more information.</summary>
		Idn = 512,
		/// <summary>The parser supports the parsing rules specified in RFC 3987 for International Resource Identifiers (IRI). Whether IRI is used is dictated by configuration values. See the Remarks for more information.</summary>
		IriParsing = 1024
	}
}
