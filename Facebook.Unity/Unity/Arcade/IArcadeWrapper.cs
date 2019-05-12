using System;
using System.Collections.Generic;

namespace Facebook.Unity.Arcade
{
	internal interface IArcadeWrapper
	{
		IDictionary<string, object> PipeResponse { get; set; }

		void DoLoginRequest(string appID, string permissions, string callbackID, ArcadeFacebook.OnComplete completeDelegate);

		void DoPayRequest(string appId, string method, string action, string product, string productId, string quantity, string quantityMin, string quantityMax, string requestId, string pricepointId, string testCurrency, string callbackID, ArcadeFacebook.OnComplete completeDelegate);

		void DoFeedShareRequest(string appId, string toId, string link, string linkName, string linkCaption, string linkDescription, string pictureLink, string mediaSource, string callbackID, ArcadeFacebook.OnComplete completeDelegate);

		void DoAppRequestRequest(string appId, string message, string actionType, string objectId, string to, string filters, string excludeIDs, string maxRecipients, string data, string title, string callbackID, ArcadeFacebook.OnComplete completeDelegate);
	}
}
