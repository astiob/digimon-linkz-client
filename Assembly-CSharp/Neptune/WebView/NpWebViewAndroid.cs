using JsonFx.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune.WebView
{
	public class NpWebViewAndroid
	{
		public static bool IsIndicator
		{
			get
			{
				bool result;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
				{
					result = androidJavaClass.CallStatic<bool>("isIndicator", new object[0]);
				}
				return result;
			}
			set
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
				{
					androidJavaClass.CallStatic("setIndicator", new object[]
					{
						value
					});
				}
			}
		}

		public static void Open(string path, Dictionary<string, string> header, string param, string gameObjectName)
		{
			string text = JsonWriter.Serialize(header);
			global::Debug.Log("#=#=# JsonFx " + text);
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("openView", new object[]
						{
							path,
							text,
							param,
							gameObjectName,
							@static
						});
					}
				}
			}
		}

		public static void Close()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("closeView", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void SetActive(bool active)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("setActive", new object[]
						{
							active,
							@static
						});
					}
				}
			}
		}

		public static void Destroy()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("closeView", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void ReSize(float x, float y, float width, float height)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("reSize", new object[]
						{
							(int)x,
							(int)y,
							(int)width,
							(int)height,
							@static
						});
					}
				}
			}
		}

		public static void ReLoad()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("reLoad", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void NextPage()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("nextPage", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static void BackPage()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("backPage", new object[]
						{
							@static
						});
					}
				}
			}
		}

		public static bool IsAccess(int type)
		{
			bool result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						result = androidJavaClass2.CallStatic<bool>("isAccess", new object[]
						{
							type,
							@static
						});
					}
				}
			}
			return result;
		}

		public static void SetBackgroundColor(float r, float g, float b, float a)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
					{
						androidJavaClass2.CallStatic("setBackgroundColor", new object[]
						{
							(int)a,
							(int)r,
							(int)g,
							(int)b,
							@static
						});
					}
				}
			}
		}

		public static void SetExternalBrowserUrl(List<string> externalBrowserUrlList)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
			{
				foreach (string text in externalBrowserUrlList)
				{
					androidJavaClass.CallStatic("setExternalBrowserUrl", new object[]
					{
						text
					});
				}
			}
		}

		public static void ClearExternalBrowserUrl()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
			{
				androidJavaClass.CallStatic("clearExternalBrowserUrl", new object[0]);
			}
		}

		public static void SetHardwareAccelerated(bool enabled)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("jp.crooz.neptune.plugin.web.NPWebView"))
			{
				androidJavaClass.CallStatic("setHardwareAccelerated", new object[]
				{
					enabled
				});
			}
		}
	}
}
