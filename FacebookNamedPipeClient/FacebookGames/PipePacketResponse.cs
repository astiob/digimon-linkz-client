using System;
using System.Collections.Generic;

namespace FacebookGames
{
	public class PipePacketResponse : PipePacket
	{
		public string Error { get; set; }

		public bool Cancelled { get; set; }

		public PipePacketResponse()
		{
			this.Cancelled = true;
		}

		public PipePacketResponse(string error, bool cancelled)
		{
			this.Error = error;
			this.Cancelled = cancelled;
		}

		public virtual IDictionary<string, object> ToDictionary()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (!string.IsNullOrEmpty(this.Error))
			{
				dictionary.Add("error", this.Error);
			}
			if (this.Cancelled)
			{
				dictionary.Add("cancelled", this.Cancelled);
			}
			return dictionary;
		}
	}
}
