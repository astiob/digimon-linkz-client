using System;

namespace System.Net.NetworkInformation
{
	internal class Win32IcmpV6Statistics : IcmpV6Statistics
	{
		private Win32_MIBICMPSTATS_EX iin;

		private Win32_MIBICMPSTATS_EX iout;

		public Win32IcmpV6Statistics(Win32_MIB_ICMP_EX info)
		{
			this.iin = info.InStats;
			this.iout = info.OutStats;
		}

		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[1]);
			}
		}

		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[1]);
			}
		}

		public override long EchoRepliesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[129]);
			}
		}

		public override long EchoRepliesSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[129]);
			}
		}

		public override long EchoRequestsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[128]);
			}
		}

		public override long EchoRequestsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[128]);
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

		public override long MembershipQueriesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[130]);
			}
		}

		public override long MembershipQueriesSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[130]);
			}
		}

		public override long MembershipReductionsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[132]);
			}
		}

		public override long MembershipReductionsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[132]);
			}
		}

		public override long MembershipReportsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[131]);
			}
		}

		public override long MembershipReportsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[131]);
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

		public override long NeighborAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[136]);
			}
		}

		public override long NeighborAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[136]);
			}
		}

		public override long NeighborSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[135]);
			}
		}

		public override long NeighborSolicitsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[135]);
			}
		}

		public override long PacketTooBigMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[2]);
			}
		}

		public override long PacketTooBigMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[2]);
			}
		}

		public override long ParameterProblemsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[4]);
			}
		}

		public override long ParameterProblemsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[4]);
			}
		}

		public override long RedirectsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[137]);
			}
		}

		public override long RedirectsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[137]);
			}
		}

		public override long RouterAdvertisementsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[134]);
			}
		}

		public override long RouterAdvertisementsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[134]);
			}
		}

		public override long RouterSolicitsReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[133]);
			}
		}

		public override long RouterSolicitsSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[133]);
			}
		}

		public override long TimeExceededMessagesReceived
		{
			get
			{
				return (long)((ulong)this.iin.Counts[3]);
			}
		}

		public override long TimeExceededMessagesSent
		{
			get
			{
				return (long)((ulong)this.iout.Counts[3]);
			}
		}
	}
}
