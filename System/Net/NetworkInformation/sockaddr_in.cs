using System;

namespace System.Net.NetworkInformation
{
	internal struct sockaddr_in
	{
		public ushort sin_family;

		public ushort sin_port;

		public uint sin_addr;
	}
}
