using System;

namespace System.Net
{
	/// <summary>Provides the base authentication interface for retrieving credentials for Web client authentication.</summary>
	public interface ICredentials
	{
		/// <summary>Returns a <see cref="T:System.Net.NetworkCredential" /> object that is associated with the specified URI, and authentication type.</summary>
		/// <returns>The <see cref="T:System.Net.NetworkCredential" /> that is associated with the specified URI and authentication type, or, if no credentials are available, null.</returns>
		/// <param name="uri">The <see cref="T:System.Uri" /> that the client is providing authentication for. </param>
		/// <param name="authType">The type of authentication, as defined in the <see cref="P:System.Net.IAuthenticationModule.AuthenticationType" /> property. </param>
		NetworkCredential GetCredential(System.Uri uri, string authType);
	}
}
