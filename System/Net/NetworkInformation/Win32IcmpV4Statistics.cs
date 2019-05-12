using System;

namespace System.Net.NetworkInformation
{
	internal class Win32IcmpV4Statistics : IcmpV4Statistics
	{
		private Win32_MIBICMPSTATS iin;

		private Win32_MIBICMPSTATS iout;

		public Win32IcmpV4Statistics(Win32_MIBICMPINFO info)
		{
			this.iin = info.InStats;
			this.iout = info.OutStats;
		}

		public override long AddressMaskRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.AddrMaskReps);
			}
		}

		public override long AddressMaskRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.AddrMaskReps);
			}
		}

		public override long AddressMaskRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.AddrMasks);
			}
		}

		public override long AddressMaskRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.AddrMasks);
			}
		}

		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.DestUnreachs);
			}
		}

		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.DestUnreachs);
			}
		}

		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.EchoReps);
			}
		}

		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.EchoReps);
			}
		}

		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Echos);
			}
		}

		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.Echos);
			}
		}

		public override long ErrorsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Errors);
			}
		}

		public override long ErrorsSent
		{
			get
			{
				return (long)((ulong)this.iout.Errors);
			}
		}

		public override long MessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Msgs);
			}
		}

		public override long MessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.Msgs);
			}
		}

		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.iin.ParmProbs);
			}
		}

		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.iout.ParmProbs);
			}
		}

		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Redirects);
			}
		}

		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.iout.Redirects);
			}
		}

		public override long SourceQuenchesReceived
		{
			get
			{
				return (long)((ulong)this.iin.SrcQuenchs);
			}
		}

		public override long SourceQuenchesSent
		{
			get
			{
				return (long)((ulong)this.iout.SrcQuenchs);
			}
		}

		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.TimeExcds);
			}
		}

		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.TimeExcds);
			}
		}

		public override long TimestampRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.TimestampReps);
			}
		}

		public override long TimestampRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.TimestampReps);
			}
		}

		public override long TimestampRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Timestamps);
			}
		}

		public override long TimestampRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.Timestamps);
			}
		}
	}
}
