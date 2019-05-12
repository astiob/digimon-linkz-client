using System;

namespace System.Security.Authentication
{
	/// <summary>Defines the possible versions of <see cref="T:System.Security.Authentication.SslProtocols" />.</summary>
	[Flags]
	public enum SslProtocols
	{
		/// <summary>No SSL protocol is specified.</summary>
		None = 0,
		/// <summary>Specifies the SSL 2.0 protocol. SSL 2.0 has been superseded by the TLS protocol and is provided for backward compatibility only.</summary>
		Ssl2 = 12,
		/// <summary>Specifies the SSL 3.0 protocol. SSL 3.0 has been superseded by the TLS protocol and is provided for backward compatibility only.</summary>
		Ssl3 = 48,
		/// <summary>Specifies the TLS 1.0 security protocol. The TLS protocol is defined in IETF RFC 2246.</summary>
		Tls = 192,
		/// <summary>Specifies that either Secure Sockets Layer (SSL) 3.0 or Transport Layer Security (TLS) 1.0 are acceptable for secure communications</summary>
		Default = 240
	}
}
