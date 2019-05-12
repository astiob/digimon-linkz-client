using System;
using System.Diagnostics;
using UnityEngine;

namespace SmartBeat
{
	public static class SmartBeat
	{
		private const string SCREENSHOT_FILE_NAME = "/smartbeat_screenshot";

		private const string SCREENSHOT_FILE_EXT = ".png";

		private const string UNITY_PLAYER = "com.unity3d.player.UnityPlayer";

		private const double SKIP_LOGGING_MEANTIME = 2.0;

		private static DateTime mLatestTime;

		public static void init(string appKey)
		{
			SmartBeat.init(appKey, true);
		}

		public static void init(string appKey, bool enable)
		{
			SmartBeat.SingleInstance instance = SmartBeat.SingleInstance.getInstance();
			if (instance.getInitialized())
			{
				return;
			}
			SmartBeat.SingleInstance obj = instance;
			lock (obj)
			{
				if (!instance.getInitialized())
				{
					instance.setInitialized();
					Application.logMessageReceived += SmartBeat.HandleLog;
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getApplication", new object[0]);
					AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.smrtbeat.SmartBeat");
					SmartBeat.SingleInstance.getInstance().setSmartBeatAndroid(androidJavaClass2);
					androidJavaClass2.CallStatic("initAndStartSession", new object[]
					{
						androidJavaObject,
						appKey,
						enable
					});
					androidJavaClass2.CallStatic("notifyOnResume", new object[]
					{
						@static
					});
				}
			}
			SmartBeat.SingleInstance.getInstance().enableSmartBeat(enable);
		}

		public static void leaveBreadcrumb(string breadcrumb)
		{
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("leaveBreadcrumbs", new object[]
				{
					breadcrumb
				});
			}
		}

		public static void setUserId(string userId)
		{
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("setUserId", new object[]
				{
					userId
				});
			}
		}

		public static void enableLog()
		{
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("enableLogCat", new object[0]);
			}
		}

		public static void enable()
		{
			SmartBeat.SingleInstance.getInstance().enableSmartBeat(true);
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("enable", new object[0]);
			}
		}

		public static void disable()
		{
			SmartBeat.SingleInstance.getInstance().enableSmartBeat(false);
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("disable", new object[0]);
			}
		}

		public static void enableLogRedirect(string tag)
		{
			SmartBeat.SingleInstance.getInstance().enableLogRedirect(true, tag);
		}

		public static void disableLogRedirect()
		{
			SmartBeat.SingleInstance.getInstance().enableLogRedirect(false, string.Empty);
		}

		public static void addExtraData(string key, string value)
		{
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("addExtraData", new object[]
				{
					key,
					value
				});
			}
		}

		public static void enableScreenshot()
		{
			SmartBeat.SingleInstance.getInstance().enableScreenshot(true);
		}

		public static void disableScreenshot()
		{
			SmartBeat.SingleInstance.getInstance().enableScreenshot(false);
		}

		public static void onPause(bool pauseStatus)
		{
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				if (pauseStatus)
				{
					smartBeatAndroid.CallStatic("notifyOnPause", new object[]
					{
						@static
					});
				}
				else
				{
					smartBeatAndroid.CallStatic("notifyOnResume", new object[]
					{
						@static
					});
				}
			}
		}

		public static void enableDebugLog(string tag)
		{
			AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
			if (smartBeatAndroid != null)
			{
				smartBeatAndroid.CallStatic("enableDebugLog", new object[]
				{
					tag
				});
			}
		}

		private static void HandleLog(string logString, string stackTrace, LogType type)
		{
			bool flag = SmartBeat.SingleInstance.getInstance().isEnabled();
			if (flag && (type == LogType.Exception || type == LogType.Error))
			{
				DateTime now = DateTime.Now;
				if (now.Subtract(SmartBeat.mLatestTime).TotalSeconds < 2.0)
				{
					return;
				}
				SmartBeat.mLatestTime = now;
				string text = string.Empty;
				bool flag2 = SmartBeat.SingleInstance.getInstance().isEnabledScreenshot();
				if (flag2)
				{
					text = "/smartbeat_screenshot" + string.Format("_{0:00}", SmartBeat.SingleInstance.getInstance().getImageCount()) + ".png";
					Application.CaptureScreenshot(text);
				}
				string text2 = stackTrace;
				if (text2.Length <= 0)
				{
					StackTrace stackTrace2 = new StackTrace(2, true);
					text2 = stackTrace2.ToString();
				}
				AndroidJavaClass smartBeatAndroid = SmartBeat.SingleInstance.getInstance().getSmartBeatAndroid();
				if (smartBeatAndroid != null)
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					if (flag2)
					{
						smartBeatAndroid.CallStatic("logHandleExceptionForUnity", new object[]
						{
							@static,
							logString,
							text2,
							Application.persistentDataPath + text
						});
					}
					else
					{
						smartBeatAndroid.CallStatic("logHandleExceptionForUnity", new object[]
						{
							@static,
							logString,
							text2
						});
					}
				}
			}
			if (SmartBeat.SingleInstance.getInstance().isEnabledLogRedirect())
			{
			}
		}

		private class SingleInstance
		{
			private static SmartBeat.SingleInstance _self;

			private static object _lock = new object();

			private bool initialized;

			private int mImageCount;

			private bool mEnableScreenshot;

			private bool mEnableSmartBeat = true;

			private bool mEnableLogRedirect;

			private string mRedirectLogTag = string.Empty;

			private AndroidJavaClass mSmartBeatAndroid;

			public static SmartBeat.SingleInstance getInstance()
			{
				if (SmartBeat.SingleInstance._self == null)
				{
					object @lock = SmartBeat.SingleInstance._lock;
					lock (@lock)
					{
						if (SmartBeat.SingleInstance._self == null)
						{
							SmartBeat.SingleInstance._self = new SmartBeat.SingleInstance();
						}
					}
				}
				return SmartBeat.SingleInstance._self;
			}

			public bool getInitialized()
			{
				return this.initialized;
			}

			public void setInitialized()
			{
				this.initialized = true;
			}

			public void setSmartBeatAndroid(AndroidJavaClass clz)
			{
				this.mSmartBeatAndroid = clz;
			}

			public AndroidJavaClass getSmartBeatAndroid()
			{
				if (SmartBeat.SingleInstance._self == null)
				{
					return null;
				}
				return this.mSmartBeatAndroid;
			}

			public int getImageCount()
			{
				if (this.mImageCount >= 100)
				{
					this.mImageCount = 0;
				}
				return this.mImageCount++;
			}

			public void enableScreenshot(bool enable)
			{
				this.mEnableScreenshot = enable;
			}

			public bool isEnabledScreenshot()
			{
				return this.mEnableScreenshot;
			}

			public void enableSmartBeat(bool enable)
			{
				this.mEnableSmartBeat = enable;
			}

			public bool isEnabled()
			{
				return this.mEnableSmartBeat;
			}

			public void enableLogRedirect(bool enable, string tag)
			{
				this.mEnableLogRedirect = enable;
				this.mRedirectLogTag = tag;
			}

			public bool isEnabledLogRedirect()
			{
				return this.mEnableLogRedirect;
			}

			public string getTagRedirectLog()
			{
				return this.mRedirectLogTag;
			}
		}
	}
}
