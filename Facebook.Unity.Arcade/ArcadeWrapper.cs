using FacebookGames;
using FacebookPlatformServiceClient;
using System;
using System.Collections.Generic;

namespace Facebook.Unity.Arcade
{
	internal class ArcadeWrapper : IArcadeWrapper
	{
		private const string PipeErrorMessage = "Pipe name not passed to application on start.\n Make sure you are running inside the facebook games client.";

		private FacebookNamedPipeClient clientPipe;

		private ArcadeFacebookGameObject facebookGameObject;

		public ArcadeWrapper()
		{
			string text;
			Utilities.CommandLineArguments.TryGetValue("/pn", out text);
			if (text == null)
			{
				throw new InvalidOperationException("Pipe name not passed to application on start.\n Make sure you are running inside the facebook games client.");
			}
			this.clientPipe = new FacebookNamedPipeClient(text);
			this.facebookGameObject = ComponentFactory.GetComponent<ArcadeFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
		}

		public IDictionary<string, object> PipeResponse
		{
			get
			{
				PipePacketResponse pipeResponse = this.clientPipe.PipeResponse;
				if (pipeResponse == null)
				{
					return null;
				}
				return pipeResponse.ToDictionary();
			}
			set
			{
				if (value == null)
				{
					this.clientPipe.PipeResponse = null;
					return;
				}
				throw new NotSupportedException("Can only set pipe response to null");
			}
		}

		public void DoLoginRequest(string appID, string permissions, string callbackId, ArcadeFacebook.OnComplete completeDelegate)
		{
			LoginRequest request = new LoginRequest(appID, permissions);
			this.HandleRequest<LoginRequest, LoginResponse>(request, callbackId, completeDelegate);
		}

		public void DoPayRequest(string appId, string method, string action, string product, string productId, string quantity, string quantityMin, string quantityMax, string requestId, string pricepointId, string testCurrency, string callbackId, ArcadeFacebook.OnComplete completeDelegate)
		{
			PayRequest request = new PayRequest(appId, method, action, product, productId, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency);
			this.HandleRequest<PayRequest, PayResponse>(request, callbackId, completeDelegate);
		}

		public void DoFeedShareRequest(string appId, string toId, string link, string linkName, string linkCaption, string linkDescription, string pictureLink, string mediaSource, string callbackId, ArcadeFacebook.OnComplete completeDelegate)
		{
			FeedShareRequest request = new FeedShareRequest(appId, toId, link, linkName, linkCaption, linkDescription, pictureLink, mediaSource);
			this.HandleRequest<FeedShareRequest, FeedShareResponse>(request, callbackId, completeDelegate);
		}

		public void DoAppRequestRequest(string appId, string message, string actionType, string objectId, string to, string filters, string excludeIDs, string maxRecipients, string data, string title, string callbackId, ArcadeFacebook.OnComplete completeDelegate)
		{
			AppRequestRequest request = new AppRequestRequest(appId, message, actionType, objectId, to, filters, excludeIDs, maxRecipients, data, title);
			this.HandleRequest<AppRequestRequest, AppRequestResponse>(request, callbackId, completeDelegate);
		}

		public void SendRequest<T>(T request) where T : PipePacketResponse
		{
			this.clientPipe.SendRequest<T>(request);
		}

		private void HandleRequest<T, R>(T request, string callbackId, ArcadeFacebook.OnComplete completeDelegate) where T : PipePacketRequest where R : PipePacketResponse
		{
			this.clientPipe.SendRequest<R>(request);
			this.facebookGameObject.WaitForResponse(completeDelegate, callbackId);
		}
	}
}
