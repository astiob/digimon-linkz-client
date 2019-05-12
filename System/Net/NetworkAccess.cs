using System;

namespace System.Net
{
	/// <summary>Specifies network access permissions.</summary>
	[Flags]
	public enum NetworkAccess
	{
		/// <summary>Indicates that the application is allowed to accept connections from the Internet on a local resource. Notice that this is a protection for the local host that uses Accept to grant access to a local resource (address/port). At the time a socket tries to bind to this local resource a permission check is performed to see if an Accept exists on that resource.</summary>
		Accept = 128,
		/// <summary>Indicates that the application is allowed to connect to specific Internet resources. Notice that, in the case of remote host resource, no check is performed to see that Connect permissions exist. This is because the port of a connecting remote host is unknown and not suitable permissions can be built in advance. It is the application responsibility to check the permissions of the remote host trying to connect to a listening socket.</summary>
		Connect = 64
	}
}
