using System;

namespace System.Net
{
	/// <summary>Specifies protocols for authentication.</summary>
	[Flags]
	public enum AuthenticationSchemes
	{
		/// <summary>No authentication is allowed. A client requesting an <see cref="T:System.Net.HttpListener" /> object with this flag set will always receive a 403 Forbidden status. Use this flag when a resource should never be served to a client.</summary>
		None = 0,
		/// <summary>Specifies digest authentication.</summary>
		Digest = 1,
		/// <summary>Negotiates with the client to determine the authentication scheme. If both client and server support Kerberos, it is used; otherwise, NTLM is used.</summary>
		Negotiate = 2,
		/// <summary>Specifies NTLM authentication.</summary>
		Ntlm = 4,
		/// <summary>Specifies Windows authentication.</summary>
		IntegratedWindowsAuthentication = 6,
		/// <summary>Specifies basic authentication. </summary>
		Basic = 8,
		/// <summary>Specifies anonymous authentication.</summary>
		Anonymous = 32768
	}
}
