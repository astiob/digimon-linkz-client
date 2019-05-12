using System;

namespace System.Net.NetworkInformation.MacOsStructs
{
	internal struct sockaddr_in6
	{
		public byte sin6_len;

		public byte sin6_family;

		public ushort sin6_port;

		public uint sin6_flowinfo;

		public in6_addr sin6_addr;

		public uint sin6_scope_id;
	}
}
