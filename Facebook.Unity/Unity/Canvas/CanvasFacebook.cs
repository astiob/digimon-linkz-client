using Facebook.MiniJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Facebook.Unity.Canvas
{
	internal sealed class CanvasFacebook : FacebookBase, ICanvasFacebookImplementation, ICanvasFacebook, IPayFacebook, IFacebook, ICanvasFacebookResultHandler, IFacebookResultHandler
	{
		internal const string MethodAppRequests = "apprequests";

		internal const string MethodFeed = "feed";

		internal const string MethodPay = "pay";

		internal const string CancelledResponse = "{\"cancelled\":true}";

		internal const string FacebookConnectURL = "https://connect.facebook.net";

		private const string AuthResponseKey = "authResponse";

		private string appId;

		private string appLinkUrl;

		private ICanvasJSWrapper canvasJSWrapper;

		private HideUnityDelegate onHideUnityDelegate;

		public CanvasFacebook() : this(CanvasFacebook.GetCanvasJSWrapper(), new CallbackManager())
		{
		}

		public CanvasFacebook(ICanvasJSWrapper canvasJSWrapper, CallbackManager callbackManager) : base(callbackManager)
		{
			this.canvasJSWrapper = canvasJSWrapper;
		}

		private static ICanvasJSWrapper GetCanvasJSWrapper()
		{
			return (ICanvasJSWrapper)Activator.CreateInstance(Assembly.Load("Facebook.Unity.Canvas").GetType("Facebook.Unity.Canvas.CanvasJSWrapper"));
		}

		public override bool LimitEventUsage { get; set; }

		public override string SDKName
		{
			get
			{
				return "FBJSSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return this.canvasJSWrapper.GetSDKVersion();
			}
		}

		public override string SDKUserAgent
		{
			get
			{
				FacebookUnityPlatform currentPlatform = Constants.CurrentPlatform;
				string productName;
				if (currentPlatform == FacebookUnityPlatform.WebGL)
				{
					productName = string.Format(CultureInfo.InvariantCulture, "FBUnity{0}", new object[]
					{
						Constants.CurrentPlatform.ToString()
					});
				}
				else
				{
					FacebookLogger.Warn("Currently running on uknown web platform");
					productName = "FBUnityWebUnknown";
				}
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[]
				{
					base.SDKUserAgent,
					Utilities.GetUserAgent(productName, FacebookSdkVersion.Build)
				});
			}
		}

		public void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, string javascriptSDKLocale, bool loadDebugJSSDK, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			base.Init(onInitComplete);
			this.canvasJSWrapper.InitScreenPosition();
			this.appId = appId;
			this.onHideUnityDelegate = hideUnityDelegate;
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("appId", appId);
			methodArguments.AddPrimative<bool>("cookie", cookie);
			methodArguments.AddPrimative<bool>("logging", logging);
			methodArguments.AddPrimative<bool>("status", status);
			methodArguments.AddPrimative<bool>("xfbml", xfbml);
			methodArguments.AddString("channelUrl", channelUrl);
			methodArguments.AddString("authResponse", authResponse);
			methodArguments.AddPrimative<bool>("frictionlessRequests", frictionlessRequests);
			methodArguments.AddString("version", FB.GraphApiVersion);
			this.canvasJSWrapper.Init("https://connect.facebook.net", javascriptSDKLocale, loadDebugJSSDK ? 1 : 0, methodArguments.ToJsonString(), status ? 1 : 0);
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.canvasJSWrapper.DisableFullScreen();
			this.canvasJSWrapper.Login(permissions, base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback));
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.canvasJSWrapper.DisableFullScreen();
			this.canvasJSWrapper.Login(permissions, base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback));
		}

		public override void LogOut()
		{
			base.LogOut();
			this.canvasJSWrapper.Logout();
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("message", message);
			methodArguments.AddCommaSeparatedList("to", to);
			methodArguments.AddString("action_type", (actionType != null) ? actionType.ToString() : null);
			methodArguments.AddString("object_id", objectId);
			methodArguments.AddList<object>("filters", filters);
			methodArguments.AddList<string>("exclude_ids", excludeIds);
			methodArguments.AddNullablePrimitive<int>("max_recipients", maxRecipients);
			methodArguments.AddString("data", data);
			methodArguments.AddString("title", title);
			new CanvasFacebook.CanvasUIMethodCall<IAppRequestResult>(this, "apprequests", "OnAppRequestsComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		public override void ActivateApp(string appId)
		{
			this.canvasJSWrapper.ActivateApp();
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddUri("link", contentURL);
			methodArguments.AddString("name", contentTitle);
			methodArguments.AddString("description", contentDescription);
			methodArguments.AddUri("picture", photoURL);
			new CanvasFacebook.CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("to", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("name", linkName);
			methodArguments.AddString("caption", linkCaption);
			methodArguments.AddString("description", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("source", mediaSource);
			new CanvasFacebook.CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete")
			{
				Callback = callback
			}.Call(methodArguments);
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

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"url",
					this.appLinkUrl
				}
			};
			callback(new AppLinkResult(new ResultContainer(dictionary)));
			this.appLinkUrl = string.Empty;
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			this.canvasJSWrapper.LogAppEvent(logEvent, valueToSum, Json.Serialize(parameters));
		}

		public override void AppEventsLogPurchase(float purchaseAmount, string currency, Dictionary<string, object> parameters)
		{
			this.canvasJSWrapper.LogPurchase(purchaseAmount, currency, Json.Serialize(parameters));
		}

		public override void OnLoginComplete(ResultContainer result)
		{
			CanvasFacebook.FormatAuthResponse(result, delegate(ResultContainer formattedResponse)
			{
				this.OnAuthResponse(new LoginResult(formattedResponse));
			});
		}

		public override void OnGetAppLinkComplete(ResultContainer message)
		{
			throw new NotImplementedException();
		}

		public void OnFacebookAuthResponseChange(string responseJsonData)
		{
			this.OnFacebookAuthResponseChange(new ResultContainer(responseJsonData));
		}

		public void OnFacebookAuthResponseChange(ResultContainer resultContainer)
		{
			CanvasFacebook.FormatAuthResponse(resultContainer, delegate(ResultContainer formattedResponse)
			{
				AccessToken.CurrentAccessToken = new LoginResult(formattedResponse).AccessToken;
			});
		}

		public void OnPayComplete(string responseJsonData)
		{
			this.OnPayComplete(new ResultContainer(responseJsonData));
		}

		public void OnPayComplete(ResultContainer resultContainer)
		{
			PayResult result = new PayResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnAppRequestsComplete(ResultContainer resultContainer)
		{
			AppRequestResult result = new AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnShareLinkComplete(ResultContainer resultContainer)
		{
			ShareResult result = new ShareResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnUrlResponse(string url)
		{
			this.appLinkUrl = url;
		}

		public void OnHideUnity(bool isGameShown)
		{
			if (this.onHideUnityDelegate != null)
			{
				this.onHideUnityDelegate(isGameShown);
			}
		}

		private static void FormatAuthResponse(ResultContainer result, Utilities.Callback<ResultContainer> callback)
		{
			if (result.ResultDictionary == null)
			{
				callback(result);
				return;
			}
			IDictionary<string, object> dictionary;
			if (result.ResultDictionary.TryGetValue("authResponse", out dictionary))
			{
				result.ResultDictionary.Remove("authResponse");
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					result.ResultDictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			if (result.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey) && !result.ResultDictionary.ContainsKey(LoginResult.PermissionsKey))
			{
				Dictionary<string, string> formData = new Dictionary<string, string>
				{
					{
						"fields",
						"permissions"
					},
					{
						"access_token",
						(string)result.ResultDictionary[LoginResult.AccessTokenKey]
					}
				};
				FacebookDelegate<IGraphResult> callback2 = delegate(IGraphResult r)
				{
					IDictionary<string, object> dictionary2;
					if (r.ResultDictionary != null && r.ResultDictionary.TryGetValue("permissions", out dictionary2))
					{
						IList<string> list = new List<string>();
						IList<object> list2;
						if (dictionary2.TryGetValue("data", out list2))
						{
							using (IEnumerator<object> enumerator2 = list2.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj = enumerator2.Current;
									IDictionary<string, object> dictionary3 = obj as IDictionary<string, object>;
									if (dictionary3 != null)
									{
										string text;
										if (dictionary3.TryGetValue("status", out text) && text.Equals("granted", StringComparison.InvariantCultureIgnoreCase))
										{
											string item;
											if (dictionary3.TryGetValue("permission", out item))
											{
												list.Add(item);
											}
											else
											{
												FacebookLogger.Warn("Didn't find permission name");
											}
										}
										else
										{
											FacebookLogger.Warn("Didn't find status in permissions result");
										}
									}
									else
									{
										FacebookLogger.Warn("Failed to case permission dictionary");
									}
								}
								goto IL_CD;
							}
						}
						FacebookLogger.Warn("Failed to extract data from permissions");
						IL_CD:
						result.ResultDictionary[LoginResult.PermissionsKey] = list.ToCommaSeparateList();
					}
					else
					{
						FacebookLogger.Warn("Failed to load permissions for access token");
					}
					callback(result);
				};
				FB.API("me", HttpMethod.GET, callback2, formData);
				return;
			}
			callback(result);
		}

		private void PayImpl(string product, string productId, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, string developerPayload, FacebookDelegate<IPayResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("product", product);
			methodArguments.AddString("product_id", productId);
			methodArguments.AddString("action", action);
			methodArguments.AddPrimative<int>("quantity", quantity);
			methodArguments.AddNullablePrimitive<int>("quantity_min", quantityMin);
			methodArguments.AddNullablePrimitive<int>("quantity_max", quantityMax);
			methodArguments.AddString("request_id", requestId);
			methodArguments.AddString("pricepoint_id", pricepointId);
			methodArguments.AddString("test_currency", testCurrency);
			methodArguments.AddString("developer_payload", developerPayload);
			new CanvasFacebook.CanvasUIMethodCall<IPayResult>(this, "pay", "OnPayComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		private class CanvasUIMethodCall<T> : MethodCall<T> where T : IResult
		{
			private CanvasFacebook canvasImpl;

			private string callbackMethod;

			public CanvasUIMethodCall(CanvasFacebook canvasImpl, string methodName, string callbackMethod) : base(canvasImpl, methodName)
			{
				this.canvasImpl = canvasImpl;
				this.callbackMethod = callbackMethod;
			}

			public override void Call(MethodArguments args)
			{
				this.UI(base.MethodName, args, base.Callback);
			}

			private void UI(string method, MethodArguments args, FacebookDelegate<T> callback = null)
			{
				this.canvasImpl.canvasJSWrapper.DisableFullScreen();
				MethodArguments methodArguments = new MethodArguments(args);
				methodArguments.AddString("app_id", this.canvasImpl.appId);
				methodArguments.AddString("method", method);
				string uid = this.canvasImpl.CallbackManager.AddFacebookDelegate<T>(callback);
				this.canvasImpl.canvasJSWrapper.Ui(methodArguments.ToJsonString(), uid, this.callbackMethod);
			}
		}
	}
}
