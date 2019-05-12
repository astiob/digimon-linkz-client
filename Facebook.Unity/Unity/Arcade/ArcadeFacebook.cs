using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Facebook.Unity.Arcade
{
	internal sealed class ArcadeFacebook : FacebookBase, IArcadeFacebookImplementation, IArcadeFacebook, IArcadeFacebookResultHandler, IPayFacebook, IFacebook, IFacebookResultHandler
	{
		private string appId;

		private IArcadeWrapper arcadeWrapper;

		public ArcadeFacebook() : this(ArcadeFacebook.GetArcadeWrapper(), new CallbackManager())
		{
		}

		public ArcadeFacebook(IArcadeWrapper arcadeWrapper, CallbackManager callbackManager) : base(callbackManager)
		{
			this.arcadeWrapper = arcadeWrapper;
		}

		public override bool LimitEventUsage { get; set; }

		public override string SDKName
		{
			get
			{
				return "FBArcadeSDK";
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
			string @string;
			Utilities.CommandLineArguments.TryGetValue("/access_token", out @string);
			if (@string != null)
			{
				@string = Encoding.UTF8.GetString(Convert.FromBase64String(@string));
				this.OnInitComplete(new ResultContainer(@string));
			}
			else
			{
				this.OnInitComplete(new ResultContainer(string.Empty));
			}
		}

		public override void ActivateApp(string appId = null)
		{
			this.AppEventsLogEvent("fb_mobile_activate_app", null, new Dictionary<string, object>());
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			Dictionary<string, object> dictionary = (parameters == null) ? new Dictionary<string, object>() : parameters;
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
			this.arcadeWrapper.DoAppRequestRequest(this.appId, message, (actionType == null) ? null : actionType.ToString(), objectId, to.ToCommaSeparateList(), filters2, excludeIds.ToCommaSeparateList(), (maxRecipients == null) ? null : maxRecipients.Value.ToString(), data, title, base.CallbackManager.AddFacebookDelegate<IAppRequestResult>(callback), new ArcadeFacebook.OnComplete(this.OnAppRequestsComplete));
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			this.arcadeWrapper.DoFeedShareRequest(this.appId, toId, (!(link != null)) ? null : link.ToString(), linkName, linkCaption, linkDescription, (!(picture != null)) ? null : picture.ToString(), mediaSource, base.CallbackManager.AddFacebookDelegate<IShareResult>(callback), new ArcadeFacebook.OnComplete(this.OnShareLinkComplete));
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.FeedShare(null, contentURL, contentTitle, null, contentDescription, photoURL, null, callback);
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.PayImpl(product, null, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
		}

		public void PayWithProductId(string productId, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.PayImpl(null, productId, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
		}

		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			throw new NotSupportedException();
		}

		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			throw new NotSupportedException();
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

		public override void OnGroupCreateComplete(ResultContainer resultContainer)
		{
			throw new NotSupportedException();
		}

		public override void OnGroupJoinComplete(ResultContainer resultContainer)
		{
			throw new NotSupportedException();
		}

		public override void OnLoginComplete(ResultContainer resultContainer)
		{
			base.OnAuthResponse(new LoginResult(resultContainer));
		}

		public override void OnShareLinkComplete(ResultContainer resultContainer)
		{
			base.CallbackManager.OnFacebookResponse(new ShareResult(resultContainer));
		}

		public void OnPayComplete(ResultContainer resultContainer)
		{
			base.CallbackManager.OnFacebookResponse(new PayResult(resultContainer));
		}

		public bool HaveReceivedPipeResponse()
		{
			return this.arcadeWrapper.PipeResponse != null;
		}

		public string GetPipeResponse(string callbackId)
		{
			IDictionary<string, object> pipeResponse = this.arcadeWrapper.PipeResponse;
			this.arcadeWrapper.PipeResponse = null;
			pipeResponse.Add("callback_id", callbackId);
			return pipeResponse.ToJson();
		}

		private static IArcadeWrapper GetArcadeWrapper()
		{
			Assembly assembly = Assembly.Load("Facebook.Unity.Arcade");
			Type type = assembly.GetType("Facebook.Unity.Arcade.ArcadeWrapper");
			return (IArcadeWrapper)Activator.CreateInstance(type);
		}

		private void PayImpl(string product, string productId, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.arcadeWrapper.DoPayRequest(this.appId, "pay", action, product, productId, quantity.ToString(), (quantityMin == null) ? null : quantityMin.Value.ToString(), (quantityMax == null) ? null : quantityMax.Value.ToString(), requestId, pricepointId, testCurrency, base.CallbackManager.AddFacebookDelegate<IPayResult>(callback), new ArcadeFacebook.OnComplete(this.OnPayComplete));
		}

		private void LoginWithPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback)
		{
			this.arcadeWrapper.DoLoginRequest(this.appId, scope.ToCommaSeparateList(), base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback), new ArcadeFacebook.OnComplete(this.OnLoginComplete));
		}

		public delegate void OnComplete(ResultContainer resultContainer);
	}
}
