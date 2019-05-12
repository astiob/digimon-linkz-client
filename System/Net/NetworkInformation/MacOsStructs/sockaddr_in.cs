using System;

namespace System.Net.NetworkInformation.MacOsStructs
{
	internal struct sockaddr_in
	{
		public byte sin_len;

		public byte sin_family;

		public ushort sin_port;

		public uint sin_addr;
	}
}
