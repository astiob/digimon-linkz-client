using System;

namespace FacebookGames
{
	public class PipePacketRequest : PipePacket
	{
		public string AppId { get; set; }

		public PipePacketRequest()
		{
		}

		public PipePacketRequest(string appId)
		{
			if (appId == null)
			{
				throw new ArgumentNullException("appId");
			}
			this.AppId = appId;
		}
	}
}
