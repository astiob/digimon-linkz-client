using System;
using System.Collections.Generic;

namespace FacebookGames
{
	public class FeedShareResponse : PipePacketResponse
	{
		public string PostId { get; set; }

		public FeedShareResponse()
		{
		}

		public FeedShareResponse(string postId, string error = null, bool cancelled = false) : base(error, cancelled)
		{
			this.PostId = postId;
		}

		public override IDictionary<string, object> ToDictionary()
		{
			IDictionary<string, object> dictionary = base.ToDictionary();
			dictionary.Add("id", this.PostId);
			return dictionary;
		}
	}
}
