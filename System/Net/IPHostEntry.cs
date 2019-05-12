using System;

namespace System.Net
{
	/// <summary>Provides a container class for Internet host address information.</summary>
	public class IPHostEntry
	{
		private IPAddress[] addressList;

		private string[] aliases;

		private string hostName;

		/// <summary>Gets or sets a list of IP addresses that are associated with a host.</summary>
		/// <returns>An array of type <see cref="T:System.Net.IPAddress" /> that contains IP addresses that resolve to the host names that are contained in the <see cref="P:System.Net.IPHostEntry.Aliases" /> property.</returns>
		public IPAddress[] AddressList
		{
			get
			{
				return this.addressList;
			}
			set
			{
				this.addressList = value;
			}
		}

		/// <summary>Gets or sets a list of aliases that are associated with a host.</summary>
		/// <returns>An array of strings that contain DNS names that resolve to the IP addresses in the <see cref="P:System.Net.IPHostEntry.AddressList" /> property.</returns>
		public string[] Aliases
		{
			get
			{
				return this.aliases;
			}
			set
			{
				this.aliases = value;
			}
		}

		/// <summary>Gets or sets the DNS name of the host.</summary>
		/// <returns>A string that contains the primary host name for the server.</returns>
		public string HostName
		{
			get
			{
				return this.hostName;
			}
			set
			{
				this.hostName = value;
			}
		}
	}
}
