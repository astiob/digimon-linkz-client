using System;

namespace FacebookGames
{
	public class FeedShareRequest : PipePacketRequest
	{
		public string ToId { get; set; }

		public string Link { get; set; }

		public string LinkName { get; set; }

		public string LinkCaption { get; set; }

		public string LinkDescription { get; set; }

		public string PictureLink { get; set; }

		public string MediaSource { get; set; }

		public FeedShareRequest()
		{
		}

		public FeedShareRequest(string appId, string toId, string link, string linkName, string linkCaption, string linkDescription, string pictureLink, string mediaSource) : base(appId)
		{
			this.ToId = toId;
			this.Link = link;
			this.LinkName = linkName;
			this.LinkCaption = linkCaption;
			this.LinkDescription = linkDescription;
			this.PictureLink = pictureLink;
			this.MediaSource = mediaSource;
		}
	}
}
