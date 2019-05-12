using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation.MacOsStructs
{
	internal struct sockaddr_dl
	{
		public byte sdl_len;

		public byte sdl_family;

		public ushort sdl_index;

		public byte sdl_type;

		public byte sdl_nlen;

		public byte sdl_alen;

		public byte sdl_slen;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
		public byte[] sdl_data;
	}
}
