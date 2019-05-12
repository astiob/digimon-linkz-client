using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation
{
	internal class MibIcmpV6Statistics : IcmpV6Statistics
	{
		private System.Collections.Specialized.StringDictionary dic;

		public MibIcmpV6Statistics(System.Collections.Specialized.StringDictionary dic)
		{
			this.dic = dic;
		}

		private long Get(string name)
		{
			return (this.dic[name] == null) ? 0L : long.Parse(this.dic[name], NumberFormatInfo.InvariantInfo);
		}

		public override long DestinationUnreachableMessagesReceived
		{
			get
			{
				return this.Get("InDestUnreachs");
			}
		}

		public override long DestinationUnreachableMessagesSent
		{
			get
			{
				return this.Get("OutDestUnreachs");
			}
		}

		public override long EchoRepliesReceived
		{
			get
			{
				return this.Get("InEchoReplies");
			}
		}

		public override long EchoRepliesSent
		{
			get
			{
				return this.Get("OutEchoReplies");
			}
		}

		public override long EchoRequestsReceived
		{
			get
			{
				return this.Get("InEchos");
			}
		}

		public override long EchoRequestsSent
		{
			get
			{
				return this.Get("OutEchos");
			}
		}

		public override long ErrorsReceived
		{
			get
			{
				return this.Get("InErrors");
			}
		}

		public override long ErrorsSent
		{
			get
			{
				return this.Get("OutErrors");
			}
		}

		public override long MembershipQueriesReceived
		{
			get
			{
				return this.Get("InGroupMembQueries");
			}
		}

		public override long MembershipQueriesSent
		{
			get
			{
				return this.Get("OutGroupMembQueries");
			}
		}

		public override long MembershipReductionsReceived
		{
			get
			{
				return this.Get("InGroupMembReductiions");
			}
		}

		public override long MembershipReductionsSent
		{
			get
			{
				return this.Get("OutGroupMembReductiions");
			}
		}

		public override long MembershipReportsReceived
		{
			get
			{
				return this.Get("InGroupMembRespons");
			}
		}

		public override long MembershipReportsSent
		{
			get
			{
				return this.Get("OutGroupMembRespons");
			}
		}

		public override long MessagesReceived
		{
			get
			{
				return this.Get("InMsgs");
			}
		}

		public override long MessagesSent
		{
			get
			{
				return this.Get("OutMsgs");
			}
		}

		public override long NeighborAdvertisementsReceived
		{
			get
			{
				return this.Get("InNeighborAdvertisements");
			}
		}

		public override long NeighborAdvertisementsSent
		{
			get
			{
				return this.Get("OutNeighborAdvertisements");
			}
		}

		public override long NeighborSolicitsReceived
		{
			get
			{
				return this.Get("InNeighborSolicits");
			}
		}

		public override long NeighborSolicitsSent
		{
			get
			{
				return this.Get("OutNeighborSolicits");
			}
		}

		public override long PacketTooBigMessagesReceived
		{
			get
			{
				return this.Get("InPktTooBigs");
			}
		}

		public override long PacketTooBigMessagesSent
		{
			get
			{
				return this.Get("OutPktTooBigs");
			}
		}

		public override long ParameterProblemsReceived
		{
			get
			{
				return this.Get("InParmProblems");
			}
		}

		public override long ParameterProblemsSent
		{
			get
			{
				return this.Get("OutParmProblems");
			}
		}

		public override long RedirectsReceived
		{
			get
			{
				return this.Get("InRedirects");
			}
		}

		public override long RedirectsSent
		{
			get
			{
				return this.Get("OutRedirects");
			}
		}

		public override long RouterAdvertisementsReceived
		{
			get
			{
				return this.Get("InRouterAdvertisements");
			}
		}

		public override long RouterAdvertisementsSent
		{
			get
			{
				return this.Get("OutRouterAdvertisements");
			}
		}

		public override long RouterSolicitsReceived
		{
			get
			{
				return this.Get("InRouterSolicits");
			}
		}

		public override long RouterSolicitsSent
		{
			get
			{
				return this.Get("OutRouterSolicits");
			}
		}

		public override long TimeExceededMessagesReceived
		{
			get
			{
				return this.Get("InTimeExcds");
			}
		}

		public override long TimeExceededMessagesSent
		{
			get
			{
				return this.Get("OutTimeExcds");
			}
		}
	}
}
