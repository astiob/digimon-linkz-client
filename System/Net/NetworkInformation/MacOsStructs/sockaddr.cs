using System;

namespace System.Net.NetworkInformation.MacOsStructs
{
	internal struct sockaddr
	{
		public byte sa_len;

		public byte sa_family;
	}
}
