using System;

namespace System.Net
{
	/// <summary>Provides the interface for retrieving credentials for a host, port, and authentication type.</summary>
	public interface ICredentialsByHost
	{
		/// <summary>Returns the credential for the specified host, port, and authentication protocol.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkCredential" /> for the specified host, port, and authentication protocol, or null if there are no credentials available for the specified host, port, and authentication protocol.</returns>
		/// <param name="host">The host computer that is authenticating the client.</param>
		/// <param name="port">The port on <paramref name="host " />that the client will communicate with.</param>
		/// <param name="authenticationType">The authentication protocol.</param>
		NetworkCredential GetCredential(string host, int port, string authType);
	}
}
