using System;
using System.Collections;

namespace WebAPIRequest
{
	public abstract class RequestBase
	{
		public string apiId;

		public WebAPI.SendBaseData param;

		public abstract void SetRequestHeader(int requestId);

		public abstract void SetParam();

		public IEnumerator Exceution(WebAPI webApi, WebAPI.RequestStatus requestStatus)
		{
			this.param.requestStatus = (int)requestStatus;
			string sendJson = WebAPIJsonParse.CreateJsonData(this.apiId, this.param);
			WWWResponse response = new WWWResponse();
			yield return webApi.StartCoroutine(webApi.StartAPIRequest(this.apiId, sendJson, response));
			try
			{
				string receiveJson = webApi.GetResponseJson(response);
				this.ToObject(receiveJson, true);
			}
			catch (WebAPIException ex)
			{
				WebAPIException exception = ex;
				if (exception != null)
				{
					exception.apiId = this.apiId;
				}
				throw exception;
			}
			yield break;
		}

		public abstract void ToObject(string json, bool isResData);
	}
}
