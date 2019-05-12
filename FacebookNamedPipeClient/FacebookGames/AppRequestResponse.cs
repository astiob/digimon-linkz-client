using System;
using System.Collections.Generic;

namespace FacebookGames
{
	public class AppRequestResponse : PipePacketResponse
	{
		public string RequestObjectId { get; set; }

		public string To { get; set; }

		public AppRequestResponse()
		{
		}

		public AppRequestResponse(string requestObjectId, string to, string error = null, bool cancelled = false) : base(error, cancelled)
		{
			this.RequestObjectId = requestObjectId;
			this.To = to;
		}

		public override IDictionary<string, object> ToDictionary()
		{
			IDictionary<string, object> dictionary = base.ToDictionary();
			dictionary.Add("request", this.RequestObjectId);
			dictionary.Add("to", this.To);
			return dictionary;
		}
	}
}
