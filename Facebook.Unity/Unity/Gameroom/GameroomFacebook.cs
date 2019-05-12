using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Facebook.Unity.Gameroom
{
	internal sealed class GameroomFacebook : FacebookBase, IGameroomFacebookImplementation, IGameroomFacebook, IPayFacebook, IFacebook, IGameroomFacebookResultHandler, IFacebookResultHandler
	{
		private string appId;

		private IGameroomWrapper gameroomWrapper;

		public GameroomFacebook() : this(GameroomFacebook.GetGameroomWrapper(), new CallbackManager())
		{
		}

		public GameroomFacebook(IGameroomWrapper gameroomWrapper, CallbackManager callbackManager) : base(callbackManager)
		{
			this.gameroomWrapper = gameroomWrapper;
		}

		public override bool LimitEventUsage { get; set; }

		public override string SDKName
		{
			get
			{
				return "FBGameroomSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return "0.0.1";
			}
		}

		public void Init(string appId, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			base.Init(onInitComplete);
			this.appId = appId;
			this.gameroomWrapper.Init(new GameroomFacebook.OnComplete(this.OnInitComplete));
		}

		public override void ActivateApp(string appId = null)
		{
			this.AppEventsLogEvent("fb_mobile_activate_app", null, new Dictionary<string, object>());
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			Dictionary<string, object> dictionary = (parameters != null) ? parameters : new Dictionary<string, object>();
			dictionary.Add("_eventName", logEvent);
			if (valueToSum != null)
			{
				dictionary.Add("_valueToSum", valueToSum.Value);
			}
			Dictionary<string, string> formData = new Dictionary<string, string>
			{
				{
					"event",
					"CUSTOM_APP_EVENTS"
				},
				{
					"application_tracking_enabled",
					"0"
				},
				{
					"advertiser_tracking_enabled",
					"0"
				},
				{
					"custom_events",
					string.Format("[{0}]", Json.Serialize(dictionary))
				}
			};
			FB.API(string.Format("{0}/activities", this.appId), HttpMethod.POST, null, formData);
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			if (parameters == null)
			{
				parameters = new Dictionary<string, object>();
			}
			parameters.Add("currency", currency);
			this.AppEventsLogEvent("fb_mobile_purchase", new float?(logPurchase), parameters);
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			string filters2 = null;
			if (filters != null)
			{
				using (IEnumerator<object> enumerator = filters.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						filters2 = (enumerator.Current as string);
					}
				}
			}
			this.gameroomWrapper.DoAppRequestRequest(this.appId, message, (actionType != null) ? actionType.ToString() : null, objectId, to.ToCommaSeparateList(), filters2, excludeIds.ToCommaSeparateList(), (maxRecipients != null) ? maxRecipients.Value.ToString() : null, data, title, base.CallbackManager.AddFacebookDelegate<IAppRequestResult>(callback), new GameroomFacebook.OnComplete(this.OnAppRequestsComplete));
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			this.gameroomWrapper.DoFeedShareRequest(this.appId, toId, (link != null) ? link.ToString() : null, linkName, linkCaption, linkDescription, (picture != null) ? picture.ToString() : null, mediaSource, base.CallbackManager.AddFacebookDelegate<IShareResult>(callback), new GameroomFacebook.OnComplete(this.OnShareLinkComplete));
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.FeedShare(null, contentURL, contentTitle, null, contentDescription, photoURL, null, callback);
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.PayImpl(product, null, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, null, callback);
		}

		public void PayWithProductId(string productId, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.PayImpl(null, productId, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, null, callback);
		}

		public void PayWithProductId(string productId, string action, string developerPayload, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.PayImpl(null, productId, action, 1, null, null, null, null, testCurrency, developerPayload, callback);
		}

		public void PayPremium(FacebookDelegate<IPayResult> callback)
		{
			this.gameroomWrapper.DoPayPremiumRequest(this.appId, base.CallbackManager.AddFacebookDelegate<IPayResult>(callback), new GameroomFacebook.OnComplete(this.OnPayComplete));
		}

		public void HasLicense(FacebookDelegate<IHasLicenseResult> callback)
		{
			this.gameroomWrapper.DoHasLicenseRequest(this.appId, base.CallbackManager.AddFacebookDelegate<IHasLicenseResult>(callback), new GameroomFacebook.OnComplete(this.OnHasLicenseComplete));
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			throw new NotSupportedException();
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback)
		{
			this.LoginWithPermissions(scope, callback);
		}

		public override void LogInWithReadPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback)
		{
			this.LoginWithPermissions(scope, callback);
		}

		public override void OnAppRequestsComplete(ResultContainer resultContainer)
		{
			base.CallbackManager.OnFacebookResponse(new AppRequestResult(resultContainer));
		}

		public override void OnGetAppLinkComplete(ResultContainer resultContainer)
		{
			throw new NotSupportedException();
		}

		public override void OnLoginComplete(ResultContainer resultContainer)
		{
			this.OnAuthResponse(new LoginResult(resultContainer));
		}

		public override void OnShareLinkComplete(ResultContainer resultContainer)
		{
			base.CallbackManager.OnFacebookResponse(new ShareResult(resultContainer));
		}

		public void OnPayComplete(ResultContainer resultContainer)
		{
			base.CallbackManager.OnFacebookResponse(new PayResult(resultContainer));
		}

		public void OnHasLicenseComplete(ResultContainer resultContainer)
		{
			base.CallbackManager.OnFacebookResponse(new HasLicenseResult(resultContainer));
		}

		public bool HaveReceivedPipeResponse()
		{
			return this.gameroomWrapper.PipeResponse != null;
		}

		public string GetPipeResponse(string callbackId)
		{
			IDictionary<string, object> pipeResponse = this.gameroomWrapper.PipeResponse;
			this.gameroomWrapper.PipeResponse = null;
			pipeResponse.Add("callback_id", callbackId);
			return pipeResponse.ToJson();
		}

		private static IGameroomWrapper GetGameroomWrapper()
		{
			return (IGameroomWrapper)Activator.CreateInstance(Assembly.Load("Facebook.Unity.Gameroom").GetType("Facebook.Unity.Gameroom.GameroomWrapper"));
		}

		private void PayImpl(string product, string productId, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, string developerPayload, FacebookDelegate<IPayResult> callback)
		{
			this.gameroomWrapper.DoPayRequest(this.appId, "pay", action, product, productId, quantity.ToString(), (quantityMin != null) ? quantityMin.Value.ToString() : null, (quantityMax != null) ? quantityMax.Value.ToString() : null, requestId, pricepointId, testCurrency, developerPayload, base.CallbackManager.AddFacebookDelegate<IPayResult>(callback), new GameroomFacebook.OnComplete(this.OnPayComplete));
		}

		private void LoginWithPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback)
		{
			this.gameroomWrapper.DoLoginRequest(this.appId, scope.ToCommaSeparateList(), base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback), new GameroomFacebook.OnComplete(this.OnLoginComplete));
		}

		public delegate void OnComplete(ResultContainer resultContainer);
	}
}
