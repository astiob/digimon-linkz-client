using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity.Settings
{
	public class FacebookSettings : ScriptableObject
	{
		public const string FacebookSettingsAssetName = "FacebookSettings";

		public const string FacebookSettingsPath = "FacebookSDK/SDK/Resources";

		public const string FacebookSettingsAssetExtension = ".asset";

		private static List<FacebookSettings.OnChangeCallback> onChangeCallbacks = new List<FacebookSettings.OnChangeCallback>();

		private static FacebookSettings instance;

		[SerializeField]
		private int selectedAppIndex;

		[SerializeField]
		private List<string> clientTokens = new List<string>
		{
			string.Empty
		};

		[SerializeField]
		private List<string> appIds = new List<string>
		{
			"0"
		};

		[SerializeField]
		private List<string> appLabels = new List<string>
		{
			"App Name"
		};

		[SerializeField]
		private bool cookie = true;

		[SerializeField]
		private bool logging = true;

		[SerializeField]
		private bool status = true;

		[SerializeField]
		private bool xfbml;

		[SerializeField]
		private bool frictionlessRequests = true;

		[SerializeField]
		private string iosURLSuffix = string.Empty;

		[SerializeField]
		private List<FacebookSettings.UrlSchemes> appLinkSchemes = new List<FacebookSettings.UrlSchemes>
		{
			new FacebookSettings.UrlSchemes(null)
		};

		[SerializeField]
		private string uploadAccessToken = string.Empty;

		public static int SelectedAppIndex
		{
			get
			{
				return FacebookSettings.Instance.selectedAppIndex;
			}
			set
			{
				if (FacebookSettings.Instance.selectedAppIndex != value)
				{
					FacebookSettings.Instance.selectedAppIndex = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static List<string> AppIds
		{
			get
			{
				return FacebookSettings.Instance.appIds;
			}
			set
			{
				if (FacebookSettings.Instance.appIds != value)
				{
					FacebookSettings.Instance.appIds = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static List<string> AppLabels
		{
			get
			{
				return FacebookSettings.Instance.appLabels;
			}
			set
			{
				if (FacebookSettings.Instance.appLabels != value)
				{
					FacebookSettings.Instance.appLabels = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static List<string> ClientTokens
		{
			get
			{
				return FacebookSettings.Instance.clientTokens;
			}
			set
			{
				if (FacebookSettings.Instance.clientTokens != value)
				{
					FacebookSettings.Instance.clientTokens = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static string AppId
		{
			get
			{
				return FacebookSettings.AppIds[FacebookSettings.SelectedAppIndex];
			}
		}

		public static string ClientToken
		{
			get
			{
				return FacebookSettings.ClientTokens[FacebookSettings.SelectedAppIndex];
			}
		}

		public static bool IsValidAppId
		{
			get
			{
				return FacebookSettings.AppId != null && FacebookSettings.AppId.Length > 0 && !FacebookSettings.AppId.Equals("0");
			}
		}

		public static bool Cookie
		{
			get
			{
				return FacebookSettings.Instance.cookie;
			}
			set
			{
				if (FacebookSettings.Instance.cookie != value)
				{
					FacebookSettings.Instance.cookie = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static bool Logging
		{
			get
			{
				return FacebookSettings.Instance.logging;
			}
			set
			{
				if (FacebookSettings.Instance.logging != value)
				{
					FacebookSettings.Instance.logging = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static bool Status
		{
			get
			{
				return FacebookSettings.Instance.status;
			}
			set
			{
				if (FacebookSettings.Instance.status != value)
				{
					FacebookSettings.Instance.status = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static bool Xfbml
		{
			get
			{
				return FacebookSettings.Instance.xfbml;
			}
			set
			{
				if (FacebookSettings.Instance.xfbml != value)
				{
					FacebookSettings.Instance.xfbml = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static string IosURLSuffix
		{
			get
			{
				return FacebookSettings.Instance.iosURLSuffix;
			}
			set
			{
				if (FacebookSettings.Instance.iosURLSuffix != value)
				{
					FacebookSettings.Instance.iosURLSuffix = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static string ChannelUrl
		{
			get
			{
				return "/channel.html";
			}
		}

		public static bool FrictionlessRequests
		{
			get
			{
				return FacebookSettings.Instance.frictionlessRequests;
			}
			set
			{
				if (FacebookSettings.Instance.frictionlessRequests != value)
				{
					FacebookSettings.Instance.frictionlessRequests = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static List<FacebookSettings.UrlSchemes> AppLinkSchemes
		{
			get
			{
				return FacebookSettings.Instance.appLinkSchemes;
			}
			set
			{
				if (FacebookSettings.Instance.appLinkSchemes != value)
				{
					FacebookSettings.Instance.appLinkSchemes = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static string UploadAccessToken
		{
			get
			{
				return FacebookSettings.Instance.uploadAccessToken;
			}
			set
			{
				if (FacebookSettings.Instance.uploadAccessToken != value)
				{
					FacebookSettings.Instance.uploadAccessToken = value;
					FacebookSettings.SettingsChanged();
				}
			}
		}

		public static FacebookSettings Instance
		{
			get
			{
				FacebookSettings.instance = FacebookSettings.NullableInstance;
				if (FacebookSettings.instance == null)
				{
					FacebookSettings.instance = ScriptableObject.CreateInstance<FacebookSettings>();
				}
				return FacebookSettings.instance;
			}
		}

		public static FacebookSettings NullableInstance
		{
			get
			{
				if (FacebookSettings.instance == null)
				{
					FacebookSettings.instance = (Resources.Load("FacebookSettings") as FacebookSettings);
				}
				return FacebookSettings.instance;
			}
		}

		public static void RegisterChangeEventCallback(FacebookSettings.OnChangeCallback callback)
		{
			FacebookSettings.onChangeCallbacks.Add(callback);
		}

		public static void UnregisterChangeEventCallback(FacebookSettings.OnChangeCallback callback)
		{
			FacebookSettings.onChangeCallbacks.Remove(callback);
		}

		private static void SettingsChanged()
		{
			FacebookSettings.onChangeCallbacks.ForEach(delegate(FacebookSettings.OnChangeCallback callback)
			{
				callback();
			});
		}

		public delegate void OnChangeCallback();

		[Serializable]
		public class UrlSchemes
		{
			[SerializeField]
			private List<string> list;

			public UrlSchemes(List<string> schemes = null)
			{
				this.list = ((schemes == null) ? new List<string>() : schemes);
			}

			public List<string> Schemes
			{
				get
				{
					return this.list;
				}
				set
				{
					this.list = value;
				}
			}
		}
	}
}
