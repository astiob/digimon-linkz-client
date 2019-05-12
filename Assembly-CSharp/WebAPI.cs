using LitJson;
using Neptune.OAuth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebAPI : MonoBehaviour
{
	[SerializeField]
	protected float timeout = 20f;

	protected string[] disableVCList;

	private string masterDataVersion;

	public void SetTimeout(float seconds)
	{
		this.timeout = seconds;
	}

	public float GetTimeout()
	{
		return this.timeout;
	}

	public void SetMasterDataVersion(string version)
	{
		this.masterDataVersion = version;
	}

	private WebAPI.DisableVersionCheckCode GetDisableVersionCheckCode(string apiId)
	{
		if (!string.IsNullOrEmpty(apiId))
		{
			for (int i = 0; i < this.disableVCList.Length; i++)
			{
				if (this.disableVCList[i] == apiId)
				{
					return WebAPI.DisableVersionCheckCode.CHECK_DISABLE;
				}
			}
		}
		return WebAPI.DisableVersionCheckCode.CHECK_ENABLE;
	}

	public IEnumerator StartAPIRequest(string apiId, string json, WWWResponse response)
	{
		this.DEBUG_LOG("Request[" + apiId + "] : " + json);
		yield return base.StartCoroutine(this.WWWRequest(WebAPIPlatformValue.GetHttpActiveController(), "POST", this.GetDisableVersionCheckCode(apiId), json.Trim(), response));
		yield break;
	}

	private IEnumerator WWWRequest(string url, string method, WebAPI.DisableVersionCheckCode disableVersionCheck, string json, WWWResponse response)
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		Dictionary<string, string> headers = new Dictionary<string, string>();
		WWWForm form = new WWWForm();
		int code = (int)disableVersionCheck;
		if (string.IsNullOrEmpty(this.masterDataVersion))
		{
			code = 1;
		}
		else
		{
			parameters["dataVersion"] = this.masterDataVersion;
			form.AddField("dataVersion", this.masterDataVersion);
		}
		parameters["activityList"] = json;
		parameters["disabledVC"] = code.ToString();
		parameters["appVersion"] = WebAPIPlatformValue.GetAppVersion();
		parameters["assetVersion"] = AssetDataMng.assetVersion.ToString();
		headers = NpOAuth.Instance.RequestHeaderDic(method, url, parameters);
		headers["X-AppVer"] = WebAPIPlatformValue.GetAppVersion();
		form.AddField("activityList", json);
		form.AddField("disabledVC", code);
		form.AddField("appVersion", WebAPIPlatformValue.GetAppVersion());
		form.AddField("assetVersion", AssetDataMng.assetVersion.ToString());
		WWWHelper www = new WWWHelper(url, form.data, headers, this.timeout);
		IEnumerator request = www.StartRequest(delegate(string responseText, string errorText, WWWHelper.TimeOut isTimeout)
		{
			response.responseJson = responseText;
			response.errorText = errorText;
			response.errorStatus = ((isTimeout != WWWHelper.TimeOut.YES) ? ((!string.IsNullOrEmpty(errorText)) ? WWWResponse.LocalErrorStatus.LOCAL_ERROR_WWW : WWWResponse.LocalErrorStatus.NONE) : WWWResponse.LocalErrorStatus.LOCAL_ERROR_TIMEOUT);
		});
		yield return base.StartCoroutine(request);
		yield break;
	}

	public string GetResponseJson(WWWResponse request_result)
	{
		if (request_result.errorStatus != WWWResponse.LocalErrorStatus.NONE)
		{
			throw new WebAPIException(request_result.errorStatus, request_result.errorText);
		}
		try
		{
			this.DEBUG_LOG("==================================== API RESPONCE JSON = " + request_result.responseJson);
			int @int = WebAPIJsonParse.GetInt(request_result.responseJson, "venus_status");
			if (@int == -1)
			{
				throw new WebAPIException(WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE, "Not Found venus_status");
			}
			switch (@int)
			{
			case 1:
				return request_result.responseJson;
			case 2:
			case 6:
			case 11:
				throw new WebAPIException(this.GetResponseError(request_result.responseJson));
			}
			throw new WebAPIException(this.GetResponseError(request_result.responseJson));
		}
		catch (JsonException ex)
		{
			throw new WebAPIException(WWWResponse.LocalErrorStatus.LOCAL_ERROR_JSONPARSE, ex.Message);
		}
		string result;
		return result;
	}

	private WebAPI.ResponseDataErr GetResponseError(string json)
	{
		WebAPI.ResponseDataErr responseDataErr = new WebAPI.ResponseDataErr();
		try
		{
			responseDataErr = JsonMapper.ToObject<WebAPI.ResponseDataErr>(json);
			this.DEBUG_LOG("====================================API MESSAGE = " + responseDataErr.message);
		}
		catch (JsonException ex)
		{
			throw ex;
		}
		return responseDataErr;
	}

	protected void DEBUG_LOG(string str)
	{
	}

	public enum RequestStatus
	{
		NORMAL,
		RETRY
	}

	private enum DisableVersionCheckCode
	{
		CHECK_ENABLE,
		CHECK_DISABLE
	}

	public class SendBaseData
	{
		public int uniqueRequestId;

		public int requestStatus;
	}

	[Serializable]
	public class RecvBaseData
	{
		[NonSerialized]
		public string sequentialId;

		[NonSerialized]
		public int status;

		[NonSerialized]
		public int venus_status;

		[NonSerialized]
		public string message;

		[NonSerialized]
		public string subject;
	}

	[Serializable]
	public class ResponseData : WebAPI.RecvBaseData
	{
		public virtual void SetParam(string json)
		{
		}
	}

	public sealed class ResponseDataErr : WebAPI.RecvBaseData
	{
		public string e;

		public string display;

		public WebAPI.ResponseDataErr.UpdateVersionManager updateVersionManager;

		public WebAPI.ResponseDataErr.Maintenance maintenance;

		public string GetAPPVer()
		{
			if (this.updateVersionManager != null && !string.IsNullOrEmpty(this.updateVersionManager.appVersion))
			{
				return this.updateVersionManager.appVersion;
			}
			return null;
		}

		public string GetDATAVer()
		{
			if (this.updateVersionManager != null && !string.IsNullOrEmpty(this.updateVersionManager.dataVersion))
			{
				return this.updateVersionManager.dataVersion;
			}
			return null;
		}

		public string GetASSETVer()
		{
			if (this.updateVersionManager != null && !string.IsNullOrEmpty(this.updateVersionManager.assetVersion))
			{
				return this.updateVersionManager.assetVersion;
			}
			return null;
		}

		public string GetPolicyVersion()
		{
			if (this.updateVersionManager != null && !string.IsNullOrEmpty(this.updateVersionManager.policyVersion))
			{
				return this.updateVersionManager.policyVersion;
			}
			return string.Empty;
		}

		public sealed class UpdateVersionManager
		{
			public string appVersion;

			public string dataVersion;

			public string assetVersion;

			public string policyVersion;
		}

		public sealed class Maintenance
		{
			public string type;

			public string detailUrl;

			public string userCode;
		}
	}
}
