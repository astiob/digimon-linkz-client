using System;

namespace UnityEngine.Analytics
{
	internal class BasePlatformWrapper : IPlatformWrapper
	{
		private Random m_Random;

		public BasePlatformWrapper()
		{
			this.m_Random = new Random();
		}

		public string appVersion
		{
			get
			{
				return Application.version;
			}
		}

		public string appBundleIdentifier
		{
			get
			{
				return Application.bundleIdentifier;
			}
		}

		public string appInstallMode
		{
			get
			{
				switch (Application.installMode)
				{
				case ApplicationInstallMode.Store:
					return "store";
				case ApplicationInstallMode.DeveloperBuild:
					return "dev_release";
				case ApplicationInstallMode.Adhoc:
					return "adhoc";
				case ApplicationInstallMode.Enterprise:
					return "enterprise";
				case ApplicationInstallMode.Editor:
					return "editor";
				default:
					return null;
				}
			}
		}

		public string licenseType
		{
			get
			{
				if (Application.HasAdvancedLicense() && Application.HasProLicense())
				{
					return "advanced_pro";
				}
				if (Application.HasAdvancedLicense())
				{
					return "advanced";
				}
				if (Application.HasProLicense())
				{
					return "pro";
				}
				return "personal";
			}
		}

		public bool isRootedOrJailbroken
		{
			get
			{
				return Application.sandboxType == ApplicationSandboxType.SandboxBroken;
			}
		}

		public bool isNetworkReachable
		{
			get
			{
				return Application.internetReachability != NetworkReachability.NotReachable;
			}
		}

		public bool isWebPlayer
		{
			get
			{
				return Application.isWebPlayer;
			}
		}

		public bool isEditor
		{
			get
			{
				return Application.isEditor;
			}
		}

		public bool isAndroid
		{
			get
			{
				return Application.platform == RuntimePlatform.Android;
			}
		}

		public int levelCount
		{
			get
			{
				return Application.levelCount;
			}
		}

		public int loadedLevel
		{
			get
			{
				return Application.loadedLevel;
			}
		}

		public string loadedLevelName
		{
			get
			{
				return Application.loadedLevelName;
			}
		}

		public string persistentDataPath
		{
			get
			{
				return Application.persistentDataPath;
			}
		}

		public string platformName
		{
			get
			{
				return Application.platform.ToString();
			}
		}

		public string unityVersion
		{
			get
			{
				return Application.unityVersion;
			}
		}

		public string deviceModel
		{
			get
			{
				return SystemInfo.deviceModel;
			}
		}

		public string deviceUniqueIdentifier
		{
			get
			{
				return UnityAnalyticsManager.deviceUniqueIdentifier;
			}
		}

		public string operatingSystem
		{
			get
			{
				return SystemInfo.operatingSystem;
			}
		}

		public string processorType
		{
			get
			{
				return SystemInfo.processorType;
			}
		}

		public int systemMemorySize
		{
			get
			{
				return SystemInfo.systemMemorySize;
			}
		}

		public long randomNumber
		{
			get
			{
				return Mathf.RandomToLong(this.m_Random);
			}
		}
	}
}
