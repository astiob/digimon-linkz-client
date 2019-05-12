using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation
{
	internal class MibTcpStatistics : TcpStatistics
	{
		private System.Collections.Specialized.StringDictionary dic;

		public MibTcpStatistics(System.Collections.Specialized.StringDictionary dic)
		{
			this.dic = dic;
		}

		private long Get(string name)
		{
			return (this.dic[name] == null) ? 0L : long.Parse(this.dic[name], NumberFormatInfo.InvariantInfo);
		}

		public override long ConnectionsAccepted
		{
			get
			{
				return this.Get("PassiveOpens");
			}
		}

		public override long ConnectionsInitiated
		{
			get
			{
				return this.Get("ActiveOpens");
			}
		}

		public override long CumulativeConnections
		{
			get
			{
				return this.Get("NumConns");
			}
		}

		public override long CurrentConnections
		{
			get
			{
				return this.Get("CurrEstab");
			}
		}

		public override long ErrorsReceived
		{
			get
			{
				return this.Get("InErrs");
			}
		}

		public override long FailedConnectionAttempts
		{
			get
			{
				return this.Get("AttemptFails");
			}
		}

		public override long MaximumConnections
		{
			get
			{
				return this.Get("MaxConn");
			}
		}

		public override long MaximumTransmissionTimeout
		{
			get
			{
				return this.Get("RtoMax");
			}
		}

		public override long MinimumTransmissionTimeout
		{
			get
			{
				return this.Get("RtoMin");
			}
		}

		public override long ResetConnections
		{
			get
			{
				return this.Get("EstabResets");
			}
		}

		public override long ResetsSent
		{
			get
			{
				return this.Get("OutRsts");
			}
		}

		public override long SegmentsReceived
		{
			get
			{
				return this.Get("InSegs");
			}
		}

		public override long SegmentsResent
		{
			get
			{
				return this.Get("RetransSegs");
			}
		}

		public override long SegmentsSent
		{
			get
			{
				return this.Get("OutSegs");
			}
		}
	}
}
