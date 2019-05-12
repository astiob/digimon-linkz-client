using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Facebook.Unity.Mobile.IOS
{
	internal class IOSFacebook : MobileFacebook
	{
		private const string CancelledResponse = "{\"cancelled\":true}";

		private bool limitEventUsage;

		private IIOSWrapper iosWrapper;

		public IOSFacebook() : this(IOSFacebook.GetIOSWrapper(), new CallbackManager())
		{
		}

		public IOSFacebook(IIOSWrapper iosWrapper, CallbackManager callbackManager) : base(callbackManager)
		{
			this.iosWrapper = iosWrapper;
		}

		public override bool LimitEventUsage
		{
			get
			{
				return this.limitEventUsage;
			}
			set
			{
				this.limitEventUsage = value;
				this.iosWrapper.FBAppEventsSetLimitEventUsage(value);
			}
		}

		public override string SDKName
		{
			get
			{
				return "FBiOSSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return this.iosWrapper.FBSdkVersion();
			}
		}

		public void Init(string appId, bool frictionlessRequests, string iosURLSuffix, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			base.Init(onInitComplete);
			this.iosWrapper.Init(appId, frictionlessRequests, iosURLSuffix, Constants.UnitySDKUserAgentSuffixLegacy);
		}

		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.iosWrapper.LogInWithReadPermissions(this.AddCallback<ILoginResult>(callback), permissions.ToCommaSeparateList());
		}

		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.iosWrapper.LogInWithPublishPermissions(this.AddCallback<ILoginResult>(callback), permissions.ToCommaSeparateList());
		}

		public override void LogOut()
		{
			base.LogOut();
			this.iosWrapper.LogOut();
		}

		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			string text = null;
			if (filters != null && filters.Any<object>())
			{
				text = (filters.First<object>() as string);
			}
			this.iosWrapper.AppRequest(this.AddCallback<IAppRequestResult>(callback), message, (actionType != null) ? actionType.ToString() : string.Empty, (objectId != null) ? objectId : string.Empty, (to != null) ? to.ToArray<string>() : null, (to != null) ? to.Count<string>() : 0, (text != null) ? text : string.Empty, (excludeIds != null) ? excludeIds.ToArray<string>() : null, (excludeIds != null) ? excludeIds.Count<string>() : 0, maxRecipients != null, (maxRecipients != null) ? maxRecipients.Value : 0, data, title);
		}

		public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			string appLinkUrl2 = string.Empty;
			string previewImageUrl2 = string.Empty;
			if (appLinkUrl != null && !string.IsNullOrEmpty(appLinkUrl.AbsoluteUri))
			{
				appLinkUrl2 = appLinkUrl.AbsoluteUri;
			}
			if (previewImageUrl != null && !string.IsNullOrEmpty(previewImageUrl.AbsoluteUri))
			{
				previewImageUrl2 = previewImageUrl.AbsoluteUri;
			}
			this.iosWrapper.AppInvite(this.AddCallback<IAppInviteResult>(callback), appLinkUrl2, previewImageUrl2);
		}

		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.iosWrapper.ShareLink(this.AddCallback<IShareResult>(callback), contentURL.AbsoluteUrlOrEmptyString(), contentTitle, contentDescription, photoURL.AbsoluteUrlOrEmptyString());
		}

		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			string link2 = (link != null) ? link.ToString() : string.Empty;
			string picture2 = (picture != null) ? picture.ToString() : string.Empty;
			this.iosWrapper.FeedShare(this.AddCallback<IShareResult>(callback), toId, link2, linkName, linkCaption, linkDescription, picture2, mediaSource);
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			IOSFacebook.NativeDict nativeDict = IOSFacebook.MarshallDict(parameters);
			if (valueToSum != null)
			{
				this.iosWrapper.LogAppEvent(logEvent, (double)valueToSum.Value, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
				return;
			}
			this.iosWrapper.LogAppEvent(logEvent, 0.0, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			IOSFacebook.NativeDict nativeDict = IOSFacebook.MarshallDict(parameters);
			this.iosWrapper.LogPurchaseAppEvent((double)logPurchase, currency, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
		}

		public override bool IsImplicitPurchaseLoggingEnabled()
		{
			return false;
		}

		public override void ActivateApp(string appId)
		{
		}

		public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			this.iosWrapper.FetchDeferredAppLink(this.AddCallback<IAppLinkResult>(callback));
		}

		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			this.iosWrapper.GetAppLink(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback)));
		}

		public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			this.iosWrapper.RefreshCurrentAccessToken(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback)));
		}

		protected override void SetShareDialogMode(ShareDialogMode mode)
		{
			this.iosWrapper.SetShareDialogMode((int)mode);
		}

		private static IIOSWrapper GetIOSWrapper()
		{
			return (IIOSWrapper)Activator.CreateInstance(Assembly.Load("Facebook.Unity.IOS").GetType("Facebook.Unity.IOS.IOSWrapper"));
		}

		private static IOSFacebook.NativeDict MarshallDict(Dictionary<string, object> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (KeyValuePair<string, object> keyValuePair in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = keyValuePair.Key;
					nativeDict.Values[nativeDict.NumEntries] = keyValuePair.Value.ToString();
					IOSFacebook.NativeDict nativeDict2 = nativeDict;
					int numEntries = nativeDict2.NumEntries;
					nativeDict2.NumEntries = numEntries + 1;
				}
			}
			return nativeDict;
		}

		private static IOSFacebook.NativeDict MarshallDict(Dictionary<string, string> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (KeyValuePair<string, string> keyValuePair in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = keyValuePair.Key;
					nativeDict.Values[nativeDict.NumEntries] = keyValuePair.Value;
					IOSFacebook.NativeDict nativeDict2 = nativeDict;
					int numEntries = nativeDict2.NumEntries;
					nativeDict2.NumEntries = numEntries + 1;
				}
			}
			return nativeDict;
		}

		private int AddCallback<T>(FacebookDelegate<T> callback) where T : IResult
		{
			return Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<T>(callback));
		}

		public enum FBInsightsFlushBehavior
		{
			FBInsightsFlushBehaviorAuto,
			FBInsightsFlushBehaviorExplicitOnly
		}

		private class NativeDict
		{
			public NativeDict()
			{
				this.NumEntries = 0;
				this.Keys = null;
				this.Values = null;
			}

			public int NumEntries { get; set; }

			public string[] Keys { get; set; }

			public string[] Values { get; set; }
		}
	}
}
