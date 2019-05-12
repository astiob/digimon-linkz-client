using System;

namespace System.Net.NetworkInformation
{
	internal struct Win32_MIB_TCPSTATS
	{
		public uint RtoAlgorithm;

		public uint RtoMin;

		public uint RtoMax;

		public uint MaxConn;

		public uint ActiveOpens;

		public uint PassiveOpens;

		public uint AttemptFails;

		public uint EstabResets;

		public uint CurrEstab;

		public uint InSegs;

		public uint OutSegs;

		public uint RetransSegs;

		public uint InErrs;

		public uint OutRsts;

		public uint NumConns;
	}
}
