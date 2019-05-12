using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation
{
	internal class MibIcmpV4Statistics : IcmpV4Statistics
	{
		private System.Collections.Specialized.StringDictionary dic;

		public MibIcmpV4Statistics(System.Collections.Specialized.StringDictionary dic)
		{
			this.dic = dic;
		}

		private long Get(string name)
		{
			return (this.dic[name] == null) ? 0L : long.Parse(this.dic[name], NumberFormatInfo.InvariantInfo);
		}

		public override long AddressMaskRepliesReceived
		{
			get
			{
				return this.Get("InAddrMaskReps");
			}
		}

		public override long AddressMaskRepliesSent
		{
			get
			{
				return this.Get("OutAddrMaskReps");
			}
		}

		public override long AddressMaskRequestsReceived
		{
			get
			{
				return this.Get("InAddrMasks");
			}
		}

		public override long AddressMaskRequestsSent
		{
			get
			{
				return this.Get("OutAddrMasks");
			}
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
				return this.Get("InEchoReps");
			}
		}

		public override long EchoRepliesSent
		{
			get
			{
				return this.Get("OutEchoReps");
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

		public override long ParameterProblemsReceived
		{
			get
			{
				return this.Get("InParmProbs");
			}
		}

		public override long ParameterProblemsSent
		{
			get
			{
				return this.Get("OutParmProbs");
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

		public override long SourceQuenchesReceived
		{
			get
			{
				return this.Get("InSrcQuenchs");
			}
		}

		public override long SourceQuenchesSent
		{
			get
			{
				return this.Get("OutSrcQuenchs");
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

		public override long TimestampRepliesReceived
		{
			get
			{
				return this.Get("InTimestampReps");
			}
		}

		public override long TimestampRepliesSent
		{
			get
			{
				return this.Get("OutTimestampReps");
			}
		}

		public override long TimestampRequestsReceived
		{
			get
			{
				return this.Get("InTimestamps");
			}
		}

		public override long TimestampRequestsSent
		{
			get
			{
				return this.Get("OutTimestamps");
			}
		}
	}
}
