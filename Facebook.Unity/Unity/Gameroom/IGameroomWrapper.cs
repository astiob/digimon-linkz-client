using System;
using System.Collections.Generic;

namespace Facebook.Unity.Gameroom
{
	internal interface IGameroomWrapper
	{
		IDictionary<string, object> PipeResponse { get; set; }

		void Init(GameroomFacebook.OnComplete completeDelegate);

		void DoLoginRequest(string appID, string permissions, string callbackID, GameroomFacebook.OnComplete completeDelegate);

		void DoPayRequest(string appId, string method, string action, string product, string productId, string quantity, string quantityMin, string quantityMax, string requestId, string pricepointId, string testCurrency, string developerPayload, string callbackID, GameroomFacebook.OnComplete completeDelegate);

		void DoFeedShareRequest(string appId, string toId, string link, string linkName, string linkCaption, string linkDescription, string pictureLink, string mediaSource, string callbackID, GameroomFacebook.OnComplete completeDelegate);

		void DoAppRequestRequest(string appId, string message, string actionType, string objectId, string to, string filters, string excludeIDs, string maxRecipients, string data, string title, string callbackID, GameroomFacebook.OnComplete completeDelegate);

		void DoPayPremiumRequest(string appId, string callbackID, GameroomFacebook.OnComplete completeDelegate);

		void DoHasLicenseRequest(string appId, string callbackID, GameroomFacebook.OnComplete completeDelegate);
	}
}
