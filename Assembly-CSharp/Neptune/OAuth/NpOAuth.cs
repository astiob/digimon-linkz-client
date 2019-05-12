using JsonFx.Json;
using Neptune.Common;
using OAuth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace Neptune.OAuth
{
	public class NpOAuth
	{
		private readonly string OS_TYPE_ANDROID = "2";

		public string initURL = string.Empty;

		public string counsumerKey = string.Empty;

		public string counsumerSecret = string.Empty;

		private float timeOut;

		public NpOAuthType type;

		public string X_AppVer = string.Empty;

		private string tokenKey = string.Empty;

		private string tokenSecret = string.Empty;

		private int userId = -1;

		private string guid = string.Empty;

		private string countryCode = string.Empty;

		private string uuid = string.Empty;

		private string adid = string.Empty;

		private bool isInitialized;

		private bool isInitializedError;

		private NpOAuthErrData mErrData = new NpOAuthErrData();

		private static NpOAuth instance;

		private NpOAuth()
		{
			this.initURL = string.Empty;
			this.counsumerKey = string.Empty;
			this.counsumerSecret = string.Empty;
			this.timeOut = 10f;
			this.type = NpOAuthType.Json;
			this.X_AppVer = string.Empty;
			this.tokenKey = string.Empty;
			this.tokenSecret = string.Empty;
			this.userId = -1;
			this.guid = string.Empty;
			this.countryCode = string.Empty;
			this.uuid = string.Empty;
			this.isInitialized = false;
			this.isInitializedError = false;
			this.mErrData = new NpOAuthErrData();
		}

		public float TimeOut { get; set; }

		public static NpOAuth Instance
		{
			get
			{
				if (NpOAuth.instance == null)
				{
					NpOAuth.instance = new NpOAuth();
				}
				return NpOAuth.instance;
			}
		}

		public void Clear()
		{
			NpOAuth.instance = null;
		}

		public bool IsInitialized
		{
			get
			{
				return this.isInitialized;
			}
		}

		public int UserId
		{
			get
			{
				return this.userId;
			}
		}

		public void Init(MonoBehaviour behaviour, Action successAction, Action<NpOAuthErrData> faildAction, NpAes aes)
		{
			if (this.initURL.Equals(string.Empty) || this.counsumerKey.Equals(string.Empty) || this.counsumerSecret.Equals(string.Empty))
			{
				if (faildAction != null)
				{
					this.mErrData.FailedCode = NpOatuhFailedCodeE.InitFaield;
					this.mErrData.NativeErrLog = "Not OAuthData";
					faildAction(this.mErrData);
				}
				return;
			}
			behaviour.StartCoroutine(this.LoadOAuth(behaviour, successAction, faildAction, aes));
		}

		private IEnumerator LoadOAuth(MonoBehaviour _behaviour, Action successAction, Action<NpOAuthErrData> faildAction, NpAes aes)
		{
			bool isNew = false;
			string baseUUIDData = PlayerPrefs.GetString("uuid");
			if (baseUUIDData.Equals(string.Empty))
			{
				isNew = true;
				this.uuid = NpDeviceInfo.GenerateUUID();
				NpDebugLog.Log("create uuid " + this.uuid);
			}
			else
			{
				this.uuid = aes.DecryptString(baseUUIDData);
				NpDebugLog.Log("load uuid " + this.uuid);
			}
			if (!this.isInitialized)
			{
				yield return _behaviour.StartCoroutine(this.OauthInitialize(this.uuid, _behaviour));
			}
			if (isNew && this.isInitialized)
			{
				PlayerPrefs.SetString("uuid", aes.EncryptString(this.uuid));
				PlayerPrefs.Save();
			}
			if (this.isInitializedError)
			{
				if (faildAction != null)
				{
					faildAction(this.mErrData);
				}
			}
			else if (this.isInitialized)
			{
				if (successAction != null)
				{
					successAction();
				}
			}
			else if (faildAction != null)
			{
				this.mErrData.FailedCode = NpOatuhFailedCodeE.OtherException;
				this.mErrData.NativeErrLog = "No isInitialized";
				faildAction(this.mErrData);
			}
			yield break;
		}

		private IEnumerator OauthInitialize(string uuid, MonoBehaviour _behaviour)
		{
			this.isInitializedError = false;
			this.mErrData = new NpOAuthErrData();
			this.guid = NpDeviceInfo.GetGuid();
			this.adid = NpDeviceInfo.GetAdid();
			this.countryCode = NpDeviceInfo.GetLocaleLanguageAndCountry();
			string authHeader = this.InitialRequestHeaderString(uuid, "json");
			Dictionary<string, string> headersDic = this.SetHeaderCommon(authHeader);
			headersDic.Add("X-AppVer", this.X_AppVer);
			NpDebugLog.Log(this.X_AppVer);
			headersDic.Add("X-TimeZone", global::TimeZone.GetTimezoneName());
			headersDic.Add("X-Lang", CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			Dictionary<string, object> postDic = new Dictionary<string, object>();
			postDic.Add("x_uuid", uuid);
			OAuthConnect oAuthConnect = new OAuthConnect(_behaviour, new Action<WWW>(this.OnHttpResponse), this.timeOut, new Action(this.OnTimeOutErr));
			yield return _behaviour.StartCoroutine(oAuthConnect.OAuthPostRequest(this.initURL, postDic, headersDic));
			yield break;
		}

		private string InitialRequestHeaderString(string uuid, string formatType)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("x_uuid", uuid);
			int num = this.initURL.IndexOf('?');
			if (num != -1)
			{
				string text = this.initURL.Substring(num + 1);
				string[] array = text.Split(new char[]
				{
					'&'
				});
				foreach (string text2 in array)
				{
					string[] array3 = text2.Split(new char[]
					{
						'='
					});
					nameValueCollection.Add(array3[0], array3[1]);
				}
			}
			OAuthRequest oauthRequest = new OAuthRequest
			{
				Method = "POST",
				ConsumerKey = this.counsumerKey,
				ConsumerSecret = this.counsumerSecret,
				RequestUrl = string.Format(this.initURL, "request_token")
			};
			return oauthRequest.GetAuthorizationHeader(nameValueCollection);
		}

		private void messagePack(byte[] data)
		{
			Dictionary<string, object> dictionary = NpMessagePack.Unpack<Dictionary<string, object>>(data);
			if (dictionary.ContainsKey("response"))
			{
				Dictionary<string, object> ret = (Dictionary<string, object>)dictionary["response"];
				this.SetOAuthResultData(ret, string.Empty);
			}
			else
			{
				this.SetOAuthResultData(dictionary, string.Empty);
			}
		}

		private void json(string response)
		{
			Dictionary<string, object> dictionary = JsonReader.Deserialize<Dictionary<string, object>>(response);
			if (dictionary.ContainsKey("response"))
			{
				Dictionary<string, object> ret = (Dictionary<string, object>)dictionary["response"];
				this.SetOAuthResultData(ret, response);
			}
			else
			{
				this.SetOAuthResultData(dictionary, response);
			}
		}

		private void SetOAuthResultData(Dictionary<string, object> ret, string responseJson = "")
		{
			int num = Convert.ToInt32(ret["venus_status"]);
			VenusStatusE venusStatusE = (VenusStatusE)num;
			if (venusStatusE == VenusStatusE.RESPONSE_SUCCESS)
			{
				this.tokenKey = (ret["token_key"] as string);
				this.tokenSecret = (ret["token_secret"] as string);
				this.userId = Convert.ToInt32(ret["x_user_id"]);
				this.uuid = (ret["x_uuid"] as string);
			}
			else
			{
				this.mErrData = new NpOAuthErrData(NpOatuhFailedCodeE.ServerFailed, "サーバーエラー", new NpOAuthVenusRespones
				{
					VenusStatus = venusStatusE,
					Message = (ret["message"] as string),
					Subject = (ret["subject"] as string),
					VenusErrLog = (ret["e"] as string),
					ResJson = responseJson
				});
				this.isInitializedError = true;
			}
		}

		public Dictionary<string, string> RequestHeaderDic(string requestMethod, string url, Dictionary<string, object> parameters = null)
		{
			if (!this.isInitialized)
			{
				return null;
			}
			string headerCommon = this.RequestHeaderString(requestMethod, url, parameters);
			this.guid = NpDeviceInfo.GetGuid();
			this.adid = NpDeviceInfo.GetAdid();
			this.countryCode = NpDeviceInfo.GetLocaleLanguageAndCountry();
			return this.SetHeaderCommon(headerCommon);
		}

		private string RequestHeaderString(string requestMethod, string url, Dictionary<string, object> parameters = null)
		{
			if (!this.isInitialized)
			{
				throw new Exception();
			}
			string authorizationHeader;
			if (requestMethod == "GET")
			{
				WebParameterCollection webParameterCollection = new WebParameterCollection();
				int num = url.IndexOf('?');
				if (num != -1)
				{
					string text = url.Substring(num + 1);
					string[] array = text.Split(new char[]
					{
						'&'
					});
					foreach (string text2 in array)
					{
						string[] array3 = text2.Split(new char[]
						{
							'='
						});
						webParameterCollection.Add(array3[0], array3[1]);
					}
				}
				OAuthRequest oauthRequest = new OAuthRequest
				{
					Method = "GET",
					ConsumerKey = this.counsumerKey,
					ConsumerSecret = this.counsumerSecret,
					RequestUrl = string.Format(url, "request_token"),
					Token = this.tokenKey,
					TokenSecret = this.tokenSecret
				};
				authorizationHeader = oauthRequest.GetAuthorizationHeader(webParameterCollection);
			}
			else
			{
				WebParameterCollection webParameterCollection2 = new WebParameterCollection();
				int num2 = url.IndexOf('?');
				if (num2 != -1)
				{
					string text3 = url.Substring(num2 + 1);
					string[] array4 = text3.Split(new char[]
					{
						'&'
					});
					foreach (string text4 in array4)
					{
						string[] array6 = text4.Split(new char[]
						{
							'='
						});
						webParameterCollection2.Add(array6[0], array6[1]);
					}
				}
				if (parameters != null)
				{
					foreach (KeyValuePair<string, object> keyValuePair in parameters)
					{
						webParameterCollection2.Add(Uri.EscapeUriString(keyValuePair.Key), Uri.EscapeUriString(keyValuePair.Value.ToString()));
					}
				}
				OAuthRequest oauthRequest2 = new OAuthRequest
				{
					Method = "POST",
					ConsumerKey = this.counsumerKey,
					ConsumerSecret = this.counsumerSecret,
					RequestUrl = string.Format(url, "request_token"),
					Token = this.tokenKey,
					TokenSecret = this.tokenSecret
				};
				authorizationHeader = oauthRequest2.GetAuthorizationHeader(webParameterCollection2);
			}
			return authorizationHeader;
		}

		private Dictionary<string, string> SetHeaderCommon(string authorization)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("Authorization", authorization);
			if (this.guid != null || this.guid.Length != 0)
			{
				dictionary.Add("x-guid", this.guid);
			}
			if (this.uuid != null || this.uuid.Length != 0)
			{
				dictionary["x-uuid"] = this.uuid;
			}
			if (this.adid != null || this.adid.Length != 0)
			{
				dictionary["x-adid"] = this.adid;
			}
			if (this.countryCode != null || this.countryCode.Length != 0)
			{
				dictionary.Add("country-code", this.countryCode);
			}
			dictionary.Add("x-device", SystemInfo.deviceModel);
			dictionary.Add("x-osver", NpDeviceInfo.GetOSVersion());
			dictionary.Add("x-ostype", this.OS_TYPE_ANDROID);
			return dictionary;
		}

		private void OnHttpResponse(WWW www)
		{
			if (www.error == null)
			{
				try
				{
					this.tokenKey = string.Empty;
					this.tokenSecret = string.Empty;
					this.userId = -1;
					this.uuid = string.Empty;
					NpOAuthType npOAuthType = this.type;
					if (npOAuthType != NpOAuthType.Json)
					{
						if (npOAuthType == NpOAuthType.MessagePack)
						{
							this.messagePack(www.bytes);
						}
					}
					else
					{
						this.json(www.text);
					}
					if (this.userId != -1)
					{
						this.isInitialized = true;
						this.isInitializedError = false;
						this.mErrData = new NpOAuthErrData();
					}
					else if (this.isInitializedError)
					{
						this.isInitialized = false;
					}
					else
					{
						this.isInitialized = false;
						this.isInitializedError = true;
						this.mErrData.FailedCode = NpOatuhFailedCodeE.NoneUserID;
						this.mErrData.NativeErrLog = "no user_id";
					}
				}
				catch (Exception ex)
				{
					this.isInitialized = false;
					this.isInitializedError = true;
					this.mErrData.FailedCode = NpOatuhFailedCodeE.OtherException;
					this.mErrData.NativeErrLog = ex.Message;
				}
			}
			else
			{
				this.isInitialized = false;
				this.isInitializedError = true;
				this.mErrData.FailedCode = NpOatuhFailedCodeE.WWWFaield;
				this.mErrData.NativeErrLog = www.error;
			}
		}

		private void OnTimeOutErr()
		{
			this.isInitialized = false;
			this.isInitializedError = true;
			this.mErrData.FailedCode = NpOatuhFailedCodeE.TimeOut;
			this.mErrData.NativeErrLog = "NpOAuth TimeOut Err!!";
		}
	}
}
