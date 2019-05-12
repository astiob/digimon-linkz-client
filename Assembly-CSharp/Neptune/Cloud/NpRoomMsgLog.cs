using System;

namespace Neptune.Cloud
{
	public class NpRoomMsgLog
	{
		public NpRoomMsgLog()
		{
			this.Sender = string.Empty;
			this.Sendtime = default(DateTime);
			this.Message = string.Empty;
		}

		public NpRoomMsgLog(string sender, DateTime sendtime, string message)
		{
			this.Sender = sender;
			this.Sendtime = sendtime;
			this.Message = message;
		}

		public string Sender { get; set; }

		public DateTime Sendtime { get; set; }

		public string Message { get; set; }
	}
}
