using System;

namespace System.Net.NetworkInformation
{
	internal class Win32TcpStatistics : TcpStatistics
	{
		private Win32_MIB_TCPSTATS info;

		public Win32TcpStatistics(Win32_MIB_TCPSTATS info)
		{
			this.info = info;
		}

		public override long ConnectionsAccepted
		{
			get
			{
				return (long)((ulong)this.info.PassiveOpens);
			}
		}

		public override long ConnectionsInitiated
		{
			get
			{
				return (long)((ulong)this.info.ActiveOpens);
			}
		}

		public override long CumulativeConnections
		{
			get
			{
				return (long)((ulong)this.info.NumConns);
			}
		}

		public override long CurrentConnections
		{
			get
			{
				return (long)((ulong)this.info.CurrEstab);
			}
		}

		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.info.InErrs);
			}
		}

		public override long FailedConnectionAttempts
		{
			get
			{
				return (long)((ulong)this.info.AttemptFails);
			}
		}

		public override long MaximumConnections
		{
			get
			{
				return (long)((ulong)this.info.MaxConn);
			}
		}

		public override long MaximumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.info.RtoMax);
			}
		}

		public override long MinimumTransmissionTimeout
		{
			get
			{
				return (long)((ulong)this.info.RtoMin);
			}
		}

		public override long ResetConnections
		{
			get
			{
				return (long)((ulong)this.info.EstabResets);
			}
		}

		public override long ResetsSent
		{
			get
			{
				return (long)((ulong)this.info.OutRsts);
			}
		}

		public override long SegmentsReceived
		{
			get
			{
				return (long)((ulong)this.info.InSegs);
			}
		}

		public override long SegmentsResent
		{
			get
			{
				return (long)((ulong)this.info.RetransSegs);
			}
		}

		public override long SegmentsSent
		{
			get
			{
				return (long)((ulong)this.info.OutSegs);
			}
		}
	}
}
