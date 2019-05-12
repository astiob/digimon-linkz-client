using SimpleJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Advertisements
{
	internal class UnityAdsEditor : UnityAdsPlatform
	{
		private bool m_HasDefaultZone;

		private ICollection<string> m_ZoneIds;

		private bool m_Ready;

		private UnityAdsEditorPlaceholder m_Placeholder;

		private string m_CampaignDataUrl = "https://impact.applifier.com/mobile/campaigns";

		public override void RegisterNative(string extensionPath)
		{
			GameObject gameObject = new GameObject("Unity Ads Editor Placeholder Object")
			{
				hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset)
			};
			Object.DontDestroyOnLoad(gameObject);
			this.m_Placeholder = gameObject.AddComponent<UnityAdsEditorPlaceholder>();
			this.m_Placeholder.Load(extensionPath);
		}

		public override void Init(string gameId, bool testModeEnabled)
		{
			Debug.Log(string.Concat(new object[]
			{
				"UnityAdsEditor: Initialize(",
				gameId,
				", ",
				testModeEnabled,
				");"
			}));
			string url = string.Concat(new string[]
			{
				this.m_CampaignDataUrl,
				"?platform=editor&gameId=",
				WWW.EscapeURL(gameId),
				"&unityVersion=",
				WWW.EscapeURL(Application.unityVersion)
			});
			AsyncExec.StartCoroutine(this.GetAdPlan(url, new Action<WWW>(this.HandleResponse)));
		}

		private IEnumerator GetAdPlan(string URL, Action<WWW> callback)
		{
			WWW www = new WWW(URL);
			yield return www;
			callback(www);
			yield break;
		}

		private void HandleResponse(WWW www)
		{
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError("UnityAdsEditor: Failed to contact server.");
			}
			else
			{
				string @string = Encoding.UTF8.GetString(www.bytes, 0, www.bytes.Length);
				object obj = SimpleJson.DeserializeObject(@string);
				if (obj is IDictionary<string, object>)
				{
					IDictionary<string, object> dictionary = (IDictionary<string, object>)obj;
					if (dictionary.ContainsKey("status"))
					{
						string text = (string)dictionary["status"];
						if (text.Equals("ok"))
						{
							if (dictionary.ContainsKey("data") && dictionary["data"] is IDictionary<string, object>)
							{
								this.m_ZoneIds = this.CollectZoneIds((IDictionary<string, object>)dictionary["data"]);
							}
							this.m_Ready = true;
						}
						else if (dictionary.ContainsKey("errorMessage"))
						{
							Debug.LogError("UnityAdsEditor: Server returned error message: " + (string)dictionary["errorMessage"]);
						}
					}
					else
					{
						Debug.LogError("UnityAdsEditor: JSON response does not have status field: " + @string);
					}
				}
				else
				{
					Debug.LogError("UnityAdsEditor: Unable to parse JSON: " + @string);
				}
				if (!this.m_Ready)
				{
					Debug.LogError("UnityAdsEditor: Failed to fetch campaigns.");
				}
			}
		}

		private ICollection<string> CollectZoneIds(IDictionary<string, object> data)
		{
			HashSet<string> hashSet = new HashSet<string>();
			if (data.ContainsKey("zones") && data["zones"] is IList<object>)
			{
				IList<object> list = (IList<object>)data["zones"];
				foreach (object obj in list)
				{
					IDictionary<string, object> dictionary = (IDictionary<string, object>)obj;
					if (dictionary.ContainsKey("id") && dictionary["id"] is string)
					{
						hashSet.Add((string)dictionary["id"]);
					}
					if (dictionary.ContainsKey("default") && dictionary["default"] is bool && (bool)dictionary["default"])
					{
						this.m_HasDefaultZone = true;
					}
				}
			}
			return hashSet;
		}

		public override bool CanShowAds(string zoneId)
		{
			if (zoneId != null && zoneId.Length > 0)
			{
				if (this.m_ZoneIds != null && this.m_ZoneIds.Contains(zoneId))
				{
					return this.m_Ready;
				}
			}
			else if (this.m_HasDefaultZone)
			{
				return this.m_Ready;
			}
			return false;
		}

		public override void SetLogLevel(Advertisement.DebugLevel logLevel)
		{
			Debug.Log("UnityAdsEditor: SetLogLevel(" + logLevel + ");");
		}

		public override void SetCampaignDataURL(string url)
		{
			this.m_CampaignDataUrl = url;
		}

		public override bool Show(string zoneId, string gamerSid)
		{
			Debug.Log(string.Concat(new string[]
			{
				"UnityAdsEditor: Show(",
				zoneId,
				", ",
				gamerSid,
				");"
			}));
			if (!this.m_Ready)
			{
				return false;
			}
			if (zoneId != null && zoneId.Length > 0)
			{
				if (this.m_ZoneIds != null && !this.m_ZoneIds.Contains(zoneId))
				{
					return false;
				}
			}
			else if (!this.m_HasDefaultZone)
			{
				return false;
			}
			this.m_Placeholder.Show();
			return true;
		}
	}
}
