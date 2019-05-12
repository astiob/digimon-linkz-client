using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIBICMPSTATS
	{
		public uint Msgs;

		public uint Errors;

		public uint DestUnreachs;

		public uint TimeExcds;

		public uint ParmProbs;

		public uint SrcQuenchs;

		public uint Redirects;

		public uint Echos;

		public uint EchoReps;

		public uint Timestamps;

		public uint TimestampReps;

		public uint AddrMasks;

		public uint AddrMaskReps;
	}
}
