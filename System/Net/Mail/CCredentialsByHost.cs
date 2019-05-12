using System;

namespace System.Net.Mail
{
	internal class CCredentialsByHost : ICredentialsByHost
	{
		private string userName;

		private string password;

		public CCredentialsByHost(string userName, string password)
		{
			this.userName = userName;
			this.password = password;
		}

		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			return new NetworkCredential(this.userName, this.password);
		}
	}
}
