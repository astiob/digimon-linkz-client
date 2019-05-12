using System;

namespace System.Net.Mail
{
	/// <summary>Specifies the level of access allowed to a Simple Mail Transport Protocol (SMTP) server.</summary>
	public enum SmtpAccess
	{
		/// <summary>No access to an SMTP host.</summary>
		None,
		/// <summary>Connection to an SMTP host on the default port (port 25).</summary>
		Connect,
		/// <summary>Connection to an SMTP host on any port.</summary>
		ConnectToUnrestrictedPort
	}
}
