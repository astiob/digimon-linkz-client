using System;

namespace FacebookGames
{
	public class AppRequestRequest : PipePacketRequest
	{
		public string Message { get; set; }

		public string ActionType { get; set; }

		public string ObjectId { get; set; }

		public string To { get; set; }

		public string Filters { get; set; }

		public string ExcludeIDs { get; set; }

		public string MaxRecipients { get; set; }

		public string Data { get; set; }

		public string Title { get; set; }

		public AppRequestRequest()
		{
		}

		public AppRequestRequest(string appId, string message, string actionType, string objectId, string to, string filters, string excludeIDs, string maxRecipients, string data, string title) : base(appId)
		{
			if (objectId != null && actionType == null)
			{
				throw new ArgumentNullException("actionType", "actionType must not be null if objectID has been set.");
			}
			if ((actionType == "send" || actionType == "askfor") && objectId == null)
			{
				throw new ArgumentNullException("objectId", "objectID must not be null if actionType is set to send or askfor");
			}
			this.Message = message;
			this.ActionType = actionType;
			this.ObjectId = objectId;
			this.To = to;
			this.Filters = filters;
			this.ExcludeIDs = excludeIDs;
			this.MaxRecipients = maxRecipients;
			this.Data = data;
			this.Title = title;
		}
	}
}
